using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using RetroArchPlaylistFilter.Model;
using SevenZip;

namespace RetroArchPlaylistFilter
{
    public partial class MainForm : Form
    {
        private RetroArchFile? _currentFile;

        private readonly Subject<RetroArchFile?> _currentFileObservable = new Subject<RetroArchFile?>();
        private readonly ObservableCollection<RetroArchEntry> _selectedEntries = new ObservableCollection<RetroArchEntry>();
        
        private IObservable<IList<RetroArchEntry>> SelectedEntriesObservable
        {
            get
            {
                return 
                    Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>
                    (
                        handler => handler.Invoke,
                        h => _selectedEntries.CollectionChanged += h,
                        h => _selectedEntries.CollectionChanged -= h
                    )
                    .Select(pattern => _selectedEntries)
                    .StartWith(new ObservableCollection<RetroArchEntry>());
            }
        }
        
        private readonly CompositeDisposable _disposeBag = new CompositeDisposable();
        
        public MainForm()
        {
            InitializeComponent();

            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName != null)
            {
                var libPath = Path.Combine(directoryName, Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");
                SevenZipBase.SetLibraryPath(libPath);
            }

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        #region Reactive Observers Creation

        private void CreateListViewUpdatingObservers()
        {
            var filterLeftChanged = Observable.FromEventPattern<EventHandler, EventArgs>
            (
                handler => handler.Invoke,
                h => txt_filter.TextChanged += h,
                h => txt_filter.TextChanged -= h
            );
            
            var latestLeftFilter =
                filterLeftChanged
                    .ObserveOn(this)
                    .Select(pattern => txt_filter.Text);

            _currentFileObservable.AsObservable()
                .Select(file => file.HasValue ? file.Value.Path : "<None>")
                .Subscribe(s =>
                {
                    lbl_path.Text = s;
                }).AddToDisposable(_disposeBag);

            _currentFileObservable.AsObservable()
                .Subscribe(file =>
                {
                    _selectedEntries.Clear();
                })
                .AddToDisposable(_disposeBag);
            
            _currentFileObservable.AsObservable()
                .CombineLatest(latestLeftFilter.StartWith(""), (file, text) => (file, text))
                .CombineLatest(SelectedEntriesObservable, (tuple, selected) => (tuple.Item1, tuple.Item2, selected))
                .Sample(TimeSpan.FromSeconds(0.03))
                .Where(tuple => tuple.Item1.HasValue)
                .Select(tuple =>
                {
                    Debug.Assert(tuple.Item1 != null, "tuple.Item1 != null");

                    var filter = tuple.Item2;
                    var file = tuple.Item1.Value;
                    var selected = tuple.Item3;

                    // Strip from this list, the currently selected items
                    var set = new HashSet<RetroArchEntry>(file.Entries);
                    set.ExceptWith(selected);

                    var entries = set.ToList();

                    if (filter != "")
                    {
                        try
                        {
                            var regex = new Regex(filter, RegexOptions.IgnoreCase);

                            return entries.Where(entry => regex.IsMatch(entry.Name)).ToList();
                        }
                        catch (Exception)
                        {
                            return entries;
                        }
                    }

                    return entries;
                })
                .ObserveOn(this)
                .Select(MapEntriesIntoListItems)
                .Subscribe(items =>
                {
                    var itemsArray = items.ToArray();
                    
                    list_results.LockingUpdates(() =>
                    {
                        list_results.ReplaceItems(itemsArray);
                    });

                    lbl_resultsCount.Text = $@"{itemsArray.Length} of {_currentFile?.Entries.Count ?? 0} item(s) visible";
                }).AddToDisposable(_disposeBag);


            var filterRightChanged = Observable.FromEventPattern<EventHandler, EventArgs>
            (
                handler => handler.Invoke,
                h => txt_filterSelected.TextChanged += h,
                h => txt_filterSelected.TextChanged -= h
            );

            var latestRightFilter =
                filterRightChanged
                    .ObserveOn(this)
                    .Select(pattern => txt_filterSelected.Text);

            SelectedEntriesObservable
                .CombineLatest(latestRightFilter.StartWith(""), (list, text) => (list, text))
                .Sample(TimeSpan.FromSeconds(0.03))
                .Select(tuple =>
                {
                    Debug.Assert(tuple.Item1 != null, "tuple.Item1 != null");
                    
                    var entries = tuple.Item1;
                    var filter = tuple.Item2;
                    
                    if (filter != "")
                    {
                        try
                        {
                            var regex = new Regex(filter, RegexOptions.IgnoreCase);

                            return entries.Where(entry => regex.IsMatch(entry.Name)).ToList();
                        }
                        catch (Exception)
                        {
                            return entries;
                        }
                    }

                    return entries;
                })
                .ObserveOn(this)
                .Select(MapEntriesIntoListItems)
                .Subscribe(items =>
                {
                    var itemsArray = items.ToArray();

                    list_selectedGames.LockingUpdates(() =>
                    {
                        list_selectedGames.ReplaceItems(itemsArray);
                    });

                    lbl_selectedCount.Text = $@"{itemsArray.Length} of {_selectedEntries.Count} item(s) visible";
                }).AddToDisposable(_disposeBag);
        }

        private void CreateListViewSelectionObservers()
        {
            var selectedOnLeft = Observable.FromEventPattern<ListViewItemSelectionChangedEventHandler, ListViewItemSelectionChangedEventArgs>
            (
                handler => handler.Invoke,
                h => list_results.ItemSelectionChanged += h,
                h => list_results.ItemSelectionChanged -= h
            );
            
            var selectedOnRight = Observable.FromEventPattern<ListViewItemSelectionChangedEventHandler, ListViewItemSelectionChangedEventArgs>
            (
                handler => handler.Invoke,
                h => list_selectedGames.ItemSelectionChanged += h,
                h => list_selectedGames.ItemSelectionChanged -= h
            );

            selectedOnLeft
                .CombineLatest(SelectedEntriesObservable, (pattern, list) => pattern)
                .Select(pattern => list_results.SelectedIndices)
                .Delay(TimeSpan.FromSeconds(0.05))
                .Sample(TimeSpan.FromSeconds(0.03))
                .ObserveOn(this)
                .Subscribe(indices =>
                {
                    btn_add.Text = indices.Count == 0 ? @"Add All >" : $@"Add {indices.Count} >";
                })
                .AddToDisposable(_disposeBag);

            selectedOnRight
                .CombineLatest(SelectedEntriesObservable, (pattern, list) => pattern)
                .Select(pattern => list_selectedGames.SelectedIndices)
                .Delay(TimeSpan.FromSeconds(0.05))
                .Sample(TimeSpan.FromSeconds(0.03))
                .ObserveOn(this)
                .Subscribe(indices =>
                {
                    btn_remove.Text = indices.Count == 0 ? @"< Remove All" : $@"< Remove {indices.Count}";
                })
                .AddToDisposable(_disposeBag);

            list_results
                .KeyDown += (sender, args) =>
                {
                    if (args.Control && args.KeyCode == Keys.A)
                    {
                        foreach (ListViewItem item in list_results.Items)
                        {
                            item.Selected = true;
                        }
                    }
                };

            list_selectedGames
                .KeyDown += (sender, args) =>
                {
                    if (args.Control && args.KeyCode == Keys.A)
                    {
                        foreach (ListViewItem item in list_selectedGames.Items)
                        {
                            item.Selected = true;
                        }
                    }
                };
        }

        private void CreateSelectionObservers()
        {
            // Save/copy roms buttons can only be enabled when more than one game is currently selected
            SelectedEntriesObservable
                .Sample(TimeSpan.FromSeconds(0.05))
                .ObserveOn(this)
                .Select(items => items.Count > 0)
                .Subscribe(hasSelection =>
                {
                    btn_save.Enabled = hasSelection;
                    btn_copyRomFiles.Enabled = hasSelection;
                })
                .AddToDisposable(_disposeBag);
        }

        private void CreateButtonEventObservers()
        {
            btn_add
                .Click += (sender, args) =>
            {
                var selectedEntries =
                    SelectedEntriesOnListView(list_results).ToArray();

                foreach (var item in selectedEntries)
                {
                    _selectedEntries.Add(item);
                }
            };

            btn_remove
                .Click += (sender, args) =>
            {
                var selectedEntries =
                    SelectedEntriesOnListView(list_selectedGames).ToArray();

                foreach (var item in selectedEntries)
                {
                    _selectedEntries.Remove(item);
                }
            };
        }

        #endregion
        
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            CreateListViewUpdatingObservers();
            CreateListViewSelectionObservers();
            CreateSelectionObservers();
            CreateButtonEventObservers();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _disposeBag.Dispose();
        }

        private void SetCurrentFile(RetroArchFile? file)
        {
            _currentFile = file;

            _currentFileObservable.OnNext(file);
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = @"Playlist files (*.lpl)|*.lpl"
            };
            if (fileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            var file = LoadRetroArchPlaylist(fileDialog.FileName);
            SetCurrentFile(file);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog
            {
                Filter = @"Playlist files (*.lpl)|*.lpl"
            };
            if (fileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            SaveRetroArcPlaylist(_selectedEntries, fileDialog.FileName);
        }

        private void btn_copyRomFiles_Click(object sender, EventArgs e)
        {
            // Select dictionary to export files to
            var dialog = new FolderBrowserDialog { ShowNewFolderButton = true };

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;
            
            // Perform task in background
            var worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            
            void UpdateUiWithState(bool running)
            {
                btn_open.Enabled = !running;
                btn_save.Enabled = !running;
                btn_copyRomFiles.Enabled = !running;
                btn_remove.Enabled = !running;
                btn_add.Enabled = !running;

                txt_filter.Enabled = !running;
                txt_filterSelected.Enabled = !running;

                list_results.Enabled = !running;
                list_selectedGames.Enabled = !running;

                pb_export.Visible = running;
            }

            UpdateUiWithState(true);

            worker.ProgressChanged += (o, args) => { pb_export.Value = args.ProgressPercentage; };
            worker.RunWorkerCompleted += (o, args) =>
            {
                // Re-enable UI
                UpdateUiWithState(false);
            };

            worker.DoWork += (o, args) =>
            {
                worker.ReportProgress(0);

                var progressMutex = new Mutex();
                var sevenZipMutext = new Mutex();
                int completed = 0;
                
                var taskList = 
                    _selectedEntries.Select(entry =>
                    {
                        return
                            new Task(() =>
                            {
                                if (worker.CancellationPending)
                                    return;

                                // Check if file is within a compressed archive
                                var isCompressed = entry.RomPath.Contains("#");
                                var sourcePath =
                                    isCompressed
                                        ? entry.RomPath.Split('#')[0]
                                        : entry.RomPath;

                                var romOriginalFileName = isCompressed ? entry.RomPath.Split('#')[1] : Path.GetFileName(sourcePath);
                                var romTargetFileName = Path.ChangeExtension(entry.Name, ".bin");

                                if (!File.Exists(sourcePath) || romTargetFileName == null)
                                    return;

                                var targetPath = Path.Combine(dialog.SelectedPath, romTargetFileName);

                                // Compressed file - detect compression format and skip if not supported
                                if (isCompressed)
                                {
                                    try
                                    {
                                        sevenZipMutext.WaitOne();

                                        using (var extractor = new SevenZipExtractor(sourcePath))
                                        {
                                            if (!extractor.ArchiveFileNames.Contains(romOriginalFileName))
                                                return;

                                            // Extract to target path
                                            var fileStream = new FileStream(targetPath, FileMode.OpenOrCreate);
                                            fileStream.SetLength(0);

                                            extractor.ExtractFile(romOriginalFileName, fileStream);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        // Unsupported archive
                                    }
                                    finally
                                    {
                                        sevenZipMutext.ReleaseMutex();
                                    }
                                }
                                else
                                {
                                    File.Copy(sourcePath, targetPath, true);
                                }

                                progressMutex.WaitOne();

                                completed += 1;
                                var progress = (int)(completed / (float)_selectedEntries.Count * 100);
                                worker.ReportProgress(progress);

                                progressMutex.ReleaseMutex();
                            });
                    }).ToList();
                
                StartAndWaitAllThrottled(taskList, 10);
            };

            worker.RunWorkerAsync();
        }

        #region Static Helpers

        private static IEnumerable<RetroArchEntry> SelectedEntriesOnListView(ListView listView)
        {
            var itemsToPick =
                listView
                    .SelectedItems.Cast<ListViewItem>().ToArray();

            if (itemsToPick.Length == 0)
                itemsToPick = listView.Items.Cast<ListViewItem>().ToArray();

            var selectedEntries =
                itemsToPick
                    .Select(item => item.Tag as RetroArchEntry?)
                    .Where(item => item.HasValue)
                    .Select(item => item.Value);

            return selectedEntries;
        }

        private static IEnumerable<ListViewItem> MapEntriesIntoListItems(IEnumerable<RetroArchEntry> source)
        {
            return
                source
                    .OrderBy(entry => entry.Name)
                    .Select(entry =>
                    {
                        var item = new ListViewItem(new[] {entry.Name, entry.RomPath})
                        {
                            Tag = entry
                        };

                        return item;
                    });
        }

        #endregion
        
        #region Playlist File Loading/Saving

        private static RetroArchFile LoadRetroArchPlaylist(string filePath)
        {
            // .lpl playlist file reference taken from: https://github.com/libretro/RetroArch/wiki/Manually-Creating-Custom-Playlists

            var list = new List<RetroArchEntry>();

            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    // Read 5 lines:
                    //
                    // ROM path
                    // ROM name
                    // Core path
                    // Core name
                    // CRC
                    // Playlist Name
                    var path = reader.ReadLine();
                    var name = reader.ReadLine();
                    var corePath = reader.ReadLine();
                    var coreName = reader.ReadLine();
                    var crcLine = reader.ReadLine();
                    var playlistName = reader.ReadLine();
                    
                    var archFile = new RetroArchEntry(name, path, corePath, coreName, crcLine, playlistName);
                    list.Add(archFile);
                }
            }

            return new RetroArchFile(list, Path.GetFullPath(filePath));
        }

        private static void SaveRetroArcPlaylist(IEnumerable<RetroArchEntry> entries, string path)
        {
            // .lpl playlist file reference taken from: https://github.com/libretro/RetroArch/wiki/Manually-Creating-Custom-Playlists

            using (var writer = new StreamWriter(path))
            {
                foreach (var entry in entries)
                {
                    // Write 5 lines:
                    //
                    // ROM path
                    // ROM name
                    // Core path
                    // Core name
                    // CRC
                    // Playlist Name
                    writer.WriteLine(entry.RomPath);
                    writer.WriteLine(entry.Name);
                    writer.WriteLine(entry.CorePath);
                    writer.WriteLine(entry.CoreName);
                    writer.WriteLine(entry.Crc);
                    writer.WriteLine(entry.PlaylistName);
                }
            }
        }

        #endregion

        /// <summary>
        /// Starts the given tasks and waits for them to complete. This will run, at most, the specified number of tasks in parallel.
        /// <para>NOTE: If one of the given tasks has already been started, an exception will be thrown.</para>
        /// </summary>
        /// <param name="tasksToRun">The tasks to run.</param>
        /// <param name="maxTasksToRunInParallel">The maximum number of tasks to run in parallel.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static void StartAndWaitAllThrottled(IEnumerable<Task> tasksToRun, int maxTasksToRunInParallel, CancellationToken cancellationToken = new CancellationToken())
        {
            StartAndWaitAllThrottled(tasksToRun, maxTasksToRunInParallel, -1, cancellationToken);
        }

        /// <summary>
        /// Starts the given tasks and waits for them to complete. This will run, at most, the specified number of tasks in parallel.
        /// <para>NOTE: If one of the given tasks has already been started, an exception will be thrown.</para>
        /// </summary>
        /// <param name="tasksToRun">The tasks to run.</param>
        /// <param name="maxTasksToRunInParallel">The maximum number of tasks to run in parallel.</param>
        /// <param name="timeoutInMilliseconds">The maximum milliseconds we should allow the max tasks to run in parallel before allowing another task to start. Specify -1 to wait indefinitely.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static void StartAndWaitAllThrottled(IEnumerable<Task> tasksToRun, int maxTasksToRunInParallel, int timeoutInMilliseconds, CancellationToken cancellationToken = new CancellationToken())
        {
            // Convert to a list of tasks so that we don&#39;t enumerate over it multiple times needlessly.
            var tasks = tasksToRun.ToList();

            using (var throttler = new SemaphoreSlim(maxTasksToRunInParallel))
            {
                var postTaskTasks = new List<Task>();

                // Have each task notify the throttler when it completes so that it decrements the number of tasks currently running.
                tasks.ForEach(t => postTaskTasks.Add(t.ContinueWith(tsk => throttler.Release(), cancellationToken)));

                // Start running each task.
                foreach (var task in tasks)
                {
                    // Increment the number of tasks currently running and wait if too many are running.
                    throttler.Wait(timeoutInMilliseconds, cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();
                    task.Start();
                }

                // Wait for all of the provided tasks to complete.
                // We wait on the list of "post" tasks instead of the original tasks, otherwise there is a potential race condition where the throttler&#39;s using block is exited before some Tasks have had their "post" action completed, which references the throttler, resulting in an exception due to accessing a disposed object.
                Task.WaitAll(postTaskTasks.ToArray(), cancellationToken);
            }
        }
    }

    public static class DisposableExtension
    {
        public static void AddToDisposable<T>(this IDisposable disposable, T target) where T: ICollection<IDisposable>, IDisposable
        {
            target.Add(disposable);
        }
    }

    public static class ListViewExtensions
    {
        /// <summary>
        /// While locking a ListView's updates, perform a set of actions that might potentially alter
        /// the list's view state.
        /// </summary>
        public static void LockingUpdates(this ListView listView, Action performAction)
        {
            listView.SuspendLayout();
            listView.BeginUpdate();
            performAction();
            listView.EndUpdate();
            listView.ResumeLayout(true);
        }

        /// <summary>
        /// Replaces a listview's item set with a new item set.
        /// This method avoids removing all items before replacing so the scoll of the list view
        /// remains the same.
        /// </summary>
        public static void ReplaceItems(this ListView listView, IList<ListViewItem> newItems)
        {
            // Replace items at same indexes
            var sameIndex = Math.Min(listView.Items.Count, newItems.Count);
            for (int i = 0; i < sameIndex; i++)
            {
                listView.Items[i] = newItems[i];
            }

            // Remove extra items
            if (newItems.Count < listView.Items.Count)
            {
                // Go from last to first
                for (int i = listView.Items.Count - 1; i >= newItems.Count; i--)
                {
                    listView.Items.RemoveAt(i);
                }
            }
            // Add extra items
            else if (newItems.Count > listView.Items.Count)
            {
                for (int i = listView.Items.Count; i < newItems.Count; i++)
                {
                    listView.Items.Add(newItems[i]);
                }
            }
        }
    }
}

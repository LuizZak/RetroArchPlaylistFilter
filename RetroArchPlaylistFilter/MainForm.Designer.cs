namespace RetroArchPlaylistFilter
{
    partial class MainForm
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.list_results = new RetroArchPlaylistFilter.ListViewNF();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.btn_open = new System.Windows.Forms.Button();
            this.txt_filter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_path = new System.Windows.Forms.Label();
            this.lbl_resultsCount = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btn_copyRomFiles = new System.Windows.Forms.Button();
            this.txt_filterSelected = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.list_selectedGames = new RetroArchPlaylistFilter.ListViewNF();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbl_selectedCount = new System.Windows.Forms.Label();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_remove = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.pb_export = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // list_results
            // 
            this.list_results.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.list_results.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.list_results.HideSelection = false;
            this.list_results.Location = new System.Drawing.Point(0, 56);
            this.list_results.Name = "list_results";
            this.list_results.Size = new System.Drawing.Size(668, 601);
            this.list_results.TabIndex = 0;
            this.list_results.UseCompatibleStateImageBehavior = false;
            this.list_results.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Rom Name";
            this.columnHeader1.Width = 361;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "File Path";
            this.columnHeader2.Width = 397;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Playlist:";
            // 
            // btn_open
            // 
            this.btn_open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_open.Location = new System.Drawing.Point(1280, 8);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(75, 23);
            this.btn_open.TabIndex = 3;
            this.btn_open.Text = "Open...";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // txt_filter
            // 
            this.txt_filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_filter.Location = new System.Drawing.Point(3, 17);
            this.txt_filter.Name = "txt_filter";
            this.txt_filter.Size = new System.Drawing.Size(662, 20);
            this.txt_filter.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Filtering (regular expression):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Results:";
            // 
            // lbl_path
            // 
            this.lbl_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_path.Location = new System.Drawing.Point(97, 13);
            this.lbl_path.Name = "lbl_path";
            this.lbl_path.Size = new System.Drawing.Size(1177, 13);
            this.lbl_path.TabIndex = 7;
            this.lbl_path.Text = "<None>";
            // 
            // lbl_resultsCount
            // 
            this.lbl_resultsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_resultsCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_resultsCount.Location = new System.Drawing.Point(517, 40);
            this.lbl_resultsCount.Name = "lbl_resultsCount";
            this.lbl_resultsCount.Size = new System.Drawing.Size(148, 13);
            this.lbl_resultsCount.TabIndex = 8;
            this.lbl_resultsCount.Text = "0 of 0 items(s) visible";
            this.lbl_resultsCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 66);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.list_results);
            this.splitContainer1.Panel1.Controls.Add(this.lbl_resultsCount);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.txt_filter);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pb_export);
            this.splitContainer1.Panel2.Controls.Add(this.btn_copyRomFiles);
            this.splitContainer1.Panel2.Controls.Add(this.txt_filterSelected);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.list_selectedGames);
            this.splitContainer1.Panel2.Controls.Add(this.lbl_selectedCount);
            this.splitContainer1.Size = new System.Drawing.Size(1343, 657);
            this.splitContainer1.SplitterDistance = 668;
            this.splitContainer1.TabIndex = 9;
            // 
            // btn_copyRomFiles
            // 
            this.btn_copyRomFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_copyRomFiles.Location = new System.Drawing.Point(559, 631);
            this.btn_copyRomFiles.Name = "btn_copyRomFiles";
            this.btn_copyRomFiles.Size = new System.Drawing.Size(109, 23);
            this.btn_copyRomFiles.TabIndex = 16;
            this.btn_copyRomFiles.Text = "Copy rom files to...";
            this.btn_copyRomFiles.UseVisualStyleBackColor = true;
            this.btn_copyRomFiles.Click += new System.EventHandler(this.btn_copyRomFiles_Click);
            // 
            // txt_filterSelected
            // 
            this.txt_filterSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_filterSelected.Location = new System.Drawing.Point(3, 17);
            this.txt_filterSelected.Name = "txt_filterSelected";
            this.txt_filterSelected.Size = new System.Drawing.Size(665, 20);
            this.txt_filterSelected.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Filtering (regular expression):";
            // 
            // list_selectedGames
            // 
            this.list_selectedGames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.list_selectedGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.list_selectedGames.HideSelection = false;
            this.list_selectedGames.Location = new System.Drawing.Point(0, 56);
            this.list_selectedGames.Name = "list_selectedGames";
            this.list_selectedGames.Size = new System.Drawing.Size(671, 569);
            this.list_selectedGames.TabIndex = 1;
            this.list_selectedGames.UseCompatibleStateImageBehavior = false;
            this.list_selectedGames.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Rom Name";
            this.columnHeader3.Width = 431;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "File Path";
            this.columnHeader4.Width = 397;
            // 
            // lbl_selectedCount
            // 
            this.lbl_selectedCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_selectedCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_selectedCount.Location = new System.Drawing.Point(520, 40);
            this.lbl_selectedCount.Name = "lbl_selectedCount";
            this.lbl_selectedCount.Size = new System.Drawing.Size(148, 13);
            this.lbl_selectedCount.TabIndex = 10;
            this.lbl_selectedCount.Text = "0 of 0 items(s) visible";
            this.lbl_selectedCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_add
            // 
            this.btn_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_add.Location = new System.Drawing.Point(12, 729);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(104, 23);
            this.btn_add.TabIndex = 11;
            this.btn_add.Text = "Add All >";
            this.btn_add.UseVisualStyleBackColor = true;
            // 
            // btn_remove
            // 
            this.btn_remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_remove.Location = new System.Drawing.Point(1243, 729);
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Size = new System.Drawing.Size(112, 23);
            this.btn_remove.TabIndex = 12;
            this.btn_remove.Text = "< Remove All";
            this.btn_remove.UseVisualStyleBackColor = true;
            // 
            // btn_save
            // 
            this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_save.Location = new System.Drawing.Point(1280, 37);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 13;
            this.btn_save.Text = "Save...";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // pb_export
            // 
            this.pb_export.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_export.Location = new System.Drawing.Point(3, 631);
            this.pb_export.Name = "pb_export";
            this.pb_export.Size = new System.Drawing.Size(550, 23);
            this.pb_export.TabIndex = 17;
            this.pb_export.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1367, 764);
            this.Controls.Add(this.btn_remove);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lbl_path);
            this.Controls.Add(this.btn_open);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "RetroArch Playlist Filter";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListViewNF list_results;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.TextBox txt_filter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_path;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label lbl_resultsCount;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ListViewNF list_selectedGames;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label lbl_selectedCount;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_remove;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.TextBox txt_filterSelected;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_copyRomFiles;
        private System.Windows.Forms.ProgressBar pb_export;
    }
}


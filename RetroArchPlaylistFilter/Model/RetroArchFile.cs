using System;
using System.Collections.Generic;

namespace RetroArchPlaylistFilter.Model
{
    public struct RetroArchFile
    {
        public List<RetroArchEntry> Entries;
        public string Path;

        public RetroArchFile(List<RetroArchEntry> entries, string path)
        {
            Entries = entries;
            Path = path;
        }
    }

    public struct RetroArchEntry : IEquatable<RetroArchEntry>
    {
        public readonly string Name;
        public readonly string RomPath;
        public readonly string CorePath;
        public readonly string CoreName;
        public readonly string Crc;
        public readonly string PlaylistName;

        public RetroArchEntry(string name, string romPath, string corePath, string coreName, string crc, string playlistName)
        {
            Name = name;
            RomPath = romPath;
            CorePath = corePath;
            CoreName = coreName;
            Crc = crc;
            PlaylistName = playlistName;
        }
        
        public bool Equals(RetroArchEntry other)
        {
            return
                string.Equals(Name, other.Name) &&
                string.Equals(RomPath, other.RomPath) &&
                string.Equals(CorePath, other.CorePath) &&
                string.Equals(CoreName, other.CoreName) &&
                string.Equals(Crc, other.Crc) &&
                string.Equals(PlaylistName, other.PlaylistName)
                ;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RetroArchEntry && Equals((RetroArchEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RomPath != null ? RomPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CorePath != null ? CorePath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CoreName != null ? CoreName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Crc != null ? Crc.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PlaylistName != null ? PlaylistName.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}

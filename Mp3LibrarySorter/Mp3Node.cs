namespace Mp3LibrarySorter
{
    /// <summary>
    /// Propriatary structure containing data needed to write files out 
    /// </summary>
    public class Mp3Node
    {
        public string FileName { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string TrackName { get; set; }
        public string TrackNumber { get; set; }
    }
}
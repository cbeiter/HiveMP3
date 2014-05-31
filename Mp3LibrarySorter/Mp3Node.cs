namespace HiveOrganizer
{
    /// <summary>
    /// Propriatary structure containing data needed to write files out 
    /// </summary>
    public class Mp3Node
    {
        public string FileName { get; set; }
        public string NewFileName { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string Title { get; set; }
        public string TrackNumber { get; set; }
        public int Bitrate { get; set; }
    }
}
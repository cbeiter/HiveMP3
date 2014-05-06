namespace Mp3LibrarySorter
{
    public interface IMp3Node
    {
        string FileName { get; set; }
        string ArtistName { get; set; }
        string AlbumName { get; set; }
    }

    /// <summary>
    /// Propriatary structure containing data needed to write files out 
    /// </summary>
    public class Mp3Node : IMp3Node
    {
        public string FileName { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
    }
}
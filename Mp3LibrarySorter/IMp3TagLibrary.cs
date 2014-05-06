using System.Collections.Generic;

namespace Mp3LibrarySorter
{
    /// <summary>
    /// Interface shminterface I suppose there must be a reason why these are helpful but I don't
    /// see it yet and hate planning for future requirements
    /// </summary>
    public interface IMp3TagLibrary
    {
        void AddInformation(IMp3Node Mp3Node);
        IList<string> Artists { get; }
        IList<string> GetAlbumsForArtist(string artistName);
        IList<string> GetSongsForAlbumOfArtist(string albumName, string artistName);
    }
}
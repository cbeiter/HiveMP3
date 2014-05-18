using System.Collections.Generic;
using System.Linq;

namespace Mp3LibrarySorter
{
    /// <summary>
    /// Creates a mapping of the source files to their new names in the destination
    /// The mapper is then used by the library generator to move the files with the 
    /// new appropriate names.
    /// </summary>
    public class Mp3FileMapper
    {
        /// <summary>
        /// Core data structure that contains the library heirarchy of information 
        /// 
        /// form:
        /// Dictionary (artist) of dictionary (album) of lists of strings (songs)
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, List<string>>> _artistAlbumSongs;

        /// <summary>
        /// Constructor for library
        /// </summary>
        public Mp3FileMapper()
        {
            // Dictionary Artist string ->  Dictionary Album string -> List fileName string
            _artistAlbumSongs = new Dictionary<string, Dictionary<string, List<string>>>();
        }

        /// <summary>
        /// Checks whether a song is in the heirarchy and adds if necessary
        /// </summary>
        /// <param name="Mp3Node"></param>
        public void AddInformation(Mp3Node Mp3Node)
        {
            var artist = Mp3Node.ArtistName;
            var albumName = Mp3Node.AlbumName;
            var fileName = Mp3Node.FileName;

            // find artist in the library
            if (_artistAlbumSongs.ContainsKey(artist))
            {
                var albumSongs = _artistAlbumSongs[artist];

                // if album exists, add song to the fileName list for that album
                // this is a blind add. we do not check for dupes
                if (albumSongs.ContainsKey(albumName))
                {
                    albumSongs[albumName].Add(fileName);
                }
                else
                {
                    // otherwise, initialize album name and add fileName
                    albumSongs[albumName] = new List<string>{fileName};
                }
            }
            else
            {
                // add Artist, album, and song
                _artistAlbumSongs[artist] = new Dictionary<string, List<string>>();
                _artistAlbumSongs[artist][albumName] = new List<string>{fileName};
            }
        }
    
        /// <summary>
        /// Gets a list of strings representing the artists in the library
        /// </summary>
        public IList<string> Artists
        {
            get
            {
                return _artistAlbumSongs.Keys.ToList();
            }
        }


        /// <summary>
        /// Gets a string list of albums for an artist
        /// </summary>
        /// <param name="artistName">string artist name</param>
        /// <returns>IList of strings</returns>
        public IList<string> GetAlbumsForArtist(string artistName)
        {
            var result = new List<string>();
            if (_artistAlbumSongs.ContainsKey(artistName))
            {
                var albumSongs = _artistAlbumSongs[artistName];
                result.AddRange(albumSongs.Keys);
            }
            return result;
        }

        /// <summary>
        /// Gets songs for an artist album
        /// </summary>
        /// <param name="albumName">string album name</param>
        /// <param name="artistName">string artist name</param>
        /// <returns>IList of string names of songs</returns>
        public IList<string> GetSongsForAlbumOfArtist(string albumName, string artistName)
        {
            var albumSongs = _artistAlbumSongs[artistName];
            return albumSongs[albumName];
        }
    }
}
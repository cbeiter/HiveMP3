using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HiveOrganizer
{
    /// <summary>
    /// Creates a heirarchical data structure of the files mapped to their new names
    /// The mapper is then used by the library generator to move the files with the 
    /// new appropriate names.
    /// </summary>
    public class artistAlbumCatalog
    {
        /// <summary>
        /// Core data structure that contains the library heirarchy of information 
        /// 
        /// form:
        /// Dictionary (artist) of dictionary (album) of lists of strings (songs)
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, List<Mp3Node>>> _artistAlbumMp3Nodes;

        /// <summary>
        /// Constructor for library
        /// </summary>
        public artistAlbumCatalog()
        {
            // Dictionary Artist string ->  Dictionary Album string -> List fileName string
            _artistAlbumMp3Nodes = new Dictionary<string, Dictionary<string, List<Mp3Node>>>();
        }

        /// <summary>
        /// Checks whether a song is in the heirarchy and adds if necessary
        /// </summary>
        /// <param name="Mp3Node"></param>
        public void AddInformation(Mp3Node newNode)
        {
            string separator = " - ";
            bool addCurrentMp3 = true;
            var nodeList = new List<Mp3Node>();

            // TODO: add bitrate to the file name ? 
            // Title: trackNum - artist - album - tracktitle.mp3
            newNode.NewFileName = newNode.TrackNumber + separator + newNode.ArtistName + separator + newNode.AlbumName + separator + newNode.Title + ".mp3";

            if (!_artistAlbumMp3Nodes.ContainsKey(newNode.ArtistName))
            {
                // artist not in catalog
                _artistAlbumMp3Nodes[newNode.ArtistName] = new Dictionary<string, List<Mp3Node>>();
                _artistAlbumMp3Nodes[newNode.ArtistName][newNode.AlbumName] = new List<Mp3Node>();
            }
            else if (!_artistAlbumMp3Nodes[newNode.ArtistName].ContainsKey(newNode.AlbumName))
            {
                // artist in catalog, but album is not
                _artistAlbumMp3Nodes[newNode.ArtistName][newNode.AlbumName] = new List<Mp3Node>();
            }
            
            nodeList = _artistAlbumMp3Nodes[newNode.ArtistName][newNode.AlbumName];

            // go through the nodeList stored for the album
            foreach (Mp3Node existingNode in nodeList)
            {
                // if we find a matching node based on the new filename
                // pick the one with the best bitrate
                if (existingNode.NewFileName == newNode.NewFileName)
                {
                    if (newNode.Bitrate <= existingNode.Bitrate)
                    {
                        addCurrentMp3 = false;
                    }
                    else
                    {
                        nodeList.Remove(existingNode);
                    }

                    break;
                }
            }
            
            if (addCurrentMp3)
            {
                nodeList.Add(newNode);                    
            }
        }
    
        /// <summary>
        /// Gets a list of strings representing the artists in the library
        /// </summary>
        public IList<string> Artists
        {
            get
            {
                return _artistAlbumMp3Nodes.Keys.ToList();
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
            if (_artistAlbumMp3Nodes.ContainsKey(artistName))
            {
                var albumSongs = _artistAlbumMp3Nodes[artistName];
                result.AddRange(albumSongs.Keys);
            }
            return result;
        }

        /// <summary>
        /// Gets songs for an artist album
        /// </summary>
        /// <param name="albumName">string album name</param>
        /// <param name="artistName">string artist name</param>
        /// <returns>IList of Mp3Nodes</returns>
        public IList<Mp3Node> GetSongsForAlbumOfArtist(string albumName, string artistName)
        {
            var albumSongs = _artistAlbumMp3Nodes[artistName];
            return albumSongs[albumName];
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HiveOrganizer
{
    /// <summary>
    /// Creates a library of info about MP3 files found in the provided folder
    /// and subsequently copies the files to a new folder structure based on the
    /// attributes tagged on the MP3 files
    /// </summary>
    public class Mp3LibraryGenerator
    {
        private readonly FileManager _fileManager;
        private readonly string _sourceFolder;
        private readonly string _destinationFolder;
        private readonly artistAlbumCatalog _newFileStructure;
        private readonly Mp3FileReader _mp3FileReader;
        private readonly IList<Mp3Node> _mp3FileList;

        /// <summary>
        /// Constructor that initializes the HiveOrganizer with the list of files at the source
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destinationFolder"></param>
        /// <param name="mp3TagsHierarchy"></param>
        public Mp3LibraryGenerator(string sourceFolder, string destinationFolder, artistAlbumCatalog newFileStructure)
        {
            _fileManager = new FileManager();
            _sourceFolder = sourceFolder;
            _destinationFolder = destinationFolder;
            _newFileStructure = newFileStructure;
            _mp3FileReader = new Mp3FileReader();

            // get all paths to mp3s from the source folder
            var filePaths = _fileManager.GetMp3FilePaths(_sourceFolder, true);

            // generates a flat list of MP3Node files representing the songs in the folder
            _mp3FileList = _mp3FileReader.RetrieveTagsFromMp3Files(filePaths);
            
            foreach (var Mp3Node in _mp3FileList)
            {
                _newFileStructure.AddInformation(Mp3Node);
            }
        }


        /// <summary>
        /// Reads in catalog of information about the mp3 files found at the source location
        /// and uses it move the mp3s to the destination in the folder and 
        /// file name structure as currently implemented here
        /// 
        /// format of:  c:/destinationFolder/artistName/albumName/*.mp3
        /// </summary>
        public void CreateFoldersForArtists(ConsoleWriter consoleWriter)
        {
            foreach (var artist in _newFileStructure.Artists)
            {
                
                string artistFolderName = _destinationFolder + Path.DirectorySeparatorChar + artist;
                consoleWriter.WriteLine(String.Format("Creating folder for artist: {0}", artistFolderName));
               
                _fileManager.CreateDirectory(artistFolderName);

                foreach (var album in _newFileStructure.GetAlbumsForArtist(artist))
                {
                    var albumFolderName = artistFolderName + Path.DirectorySeparatorChar + album;

                    consoleWriter.WriteLine(String.Format("Creating folder for album: {0}", albumFolderName));
                    
                    // TODO: let's think about having a richer album folder name
                    _fileManager.CreateDirectory(albumFolderName);

                    var mp3Nodes = _newFileStructure.GetSongsForAlbumOfArtist(album, artist);
                    int count = 1;

                    foreach (var node in mp3Nodes)
                    {
                        consoleWriter.WriteLine("Count of files moved to album folder... ");
                        consoleWriter.Write(count.ToString());

                        _fileManager.Move(node.FileName, albumFolderName + Path.DirectorySeparatorChar + node.NewFileName);
                        
                        count++;
                    }

                    consoleWriter.Write("\n");
                }
            }
        }
    }
}
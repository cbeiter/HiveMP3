using System.Collections.Generic;
using System.IO;

namespace Mp3LibrarySorter
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
        private readonly Mp3TagLibrary _mp3TagsHierarchy;
        private readonly Mp3FileReader _mp3FileReader;
        private readonly IList<Mp3Node> _mp3FileList;

        /// <summary>
        /// Constructor that initializes the MP3LibrarySorter with the list of files at the source
        /// </summary>
        /// <param name="fileManager"></param>
        /// <param name="sourceFolder"></param>
        /// <param name="destinationFolder"></param>
        /// <param name="mp3TagsHierarchy"></param>
        /// <param name="mp3FileReader"></param>
        public Mp3LibraryGenerator(FileManager fileManager, string sourceFolder, string destinationFolder, 
            Mp3TagLibrary mp3TagsHierarchy, Mp3FileReader mp3FileReader)
        {
            _fileManager = fileManager;
            _sourceFolder = sourceFolder;
            _destinationFolder = destinationFolder;
            _mp3TagsHierarchy = mp3TagsHierarchy;
            _mp3FileReader = mp3FileReader;

            // get all the mp3s from the source folder
            var files = _fileManager.GetMp3FilePaths(_sourceFolder, true);

            // generates a List of MP3Node files representing the songs in the folder
            _mp3FileList = _mp3FileReader.RetrieveTagsFromMp3Files(files);
            
            foreach (var Mp3Node in _mp3FileList)
            {
                _mp3TagsHierarchy.AddInformation(Mp3Node);
            }
        }


        /// <summary>
        /// Reads in catalog of information about the mp3 files found at the source location
        /// and uses it move the mp3s to the destination in the folder and 
        /// file name structure as currently implemented here
        /// 
        /// format of:  c:/destinationFolder/artistName/albumName/*.mp3
        /// </summary>
        public void CreateFoldersForArtists()
        {
            foreach (var artist in _mp3TagsHierarchy.Artists)
            {
                string artistFolderName = _destinationFolder + Path.DirectorySeparatorChar + artist;
                _fileManager.CreateDirectory(artistFolderName);

                foreach (var album in _mp3TagsHierarchy.GetAlbumsForArtist(artist))
                {
                    var albumFolderName = artistFolderName + Path.DirectorySeparatorChar + album;
                    
                    // TODO: let's think about having a richer album folder name
                    _fileManager.CreateDirectory(albumFolderName);

                    var filesNames = _mp3TagsHierarchy.GetSongsForAlbumOfArtist(album, artist);

                    foreach (var fileName in filesNames)
                    {
                        var fileNameWithoutFullPath = Path.GetFileName(fileName);
                        _fileManager.Move(fileName, albumFolderName + Path.DirectorySeparatorChar + fileNameWithoutFullPath);
                    }
                }
            }
        }
    }
}
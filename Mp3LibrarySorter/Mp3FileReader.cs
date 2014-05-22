using System;
using System.Collections.Generic;
using System.IO;
using Raize.CodeSiteLogging;

namespace HiveOrganizer
{
    /// <summary>
    /// Reads tags from MP3 files from file folder
    /// </summary>
    public class Mp3FileReader
    {
        /// <summary>
        /// List of files with missing tags.  Used by unit test only
        /// </summary>
        private readonly IList<string> _filesWithMissingTags = new List<string>(); 

        /// <summary>
        /// Gets tags from the raw MP3 files and generates a List of MP3Node data structures with 
        /// necessary info to construct our MP3 folder structure
        /// 
        /// Not sure whether I understand why we are moving this into a separate structure
        /// </summary>
        /// <param name="mp3Files">List of string paths to the MP3 files</param>
        /// <returns>List of MP3 representations</returns>
        public IList<Mp3Node> RetrieveTagsFromMp3Files(IList<string> mp3FilePaths)
        {
            var mp3FileList = new List<Mp3Node>();
            var count = 0;

            foreach (var mp3File in mp3FilePaths)
            {
                // filtering out these two types.  Don't know why
                string extension = Path.GetExtension(mp3File);
                if ((extension != ".cue") && (extension != ".db"))
                try
                {
                    // Codesite is a logging tool. Where is this going???
                    CodeSite.Send("Processing file " + count++ + " from " + mp3FilePaths.Count);

                    TagLib.File tagLibFile = null;

                    try
                    {
                        // hydrate TagLib File data structure from raw mp3 file
                        tagLibFile = TagLib.File.Create(mp3File);
                    }
                    catch (Exception exception)
                    {
                        _filesWithMissingTags.Add(mp3File);
                        CodeSite.Send(mp3File);
                        CodeSite.SendException(exception);
                        continue;
                    }
                    
                    // if if we have a tag library, we'll go through it and create an MP3Node to represent it
                    // TODO: Not sure the justification of moving from this tagLib format to our own custom format
                    if (tagLibFile != null && tagLibFile.Tag != null)
                    {
                        string artist;
                        string album;

                        // set artist
                        if (tagLibFile.Tag.AlbumArtists.Length > 0)
                        {
                            artist = tagLibFile.Tag.AlbumArtists[0];
                        }
                        // this property is obsoleted so only check as a fallback
                        else if (tagLibFile.Tag.Artists.Length > 0)
                        {
                            artist = tagLibFile.Tag.Artists[0];
                        }
                        else
                        {
                            artist = "unknown artist";
                            _filesWithMissingTags.Add(mp3File);
                        }

                        // set album
                        if (tagLibFile.Tag.Album.Length > 0)
                        {
                            album = tagLibFile.Tag.Album;
                        }
                        else if (tagLibFile.Tag.AlbumArtists.Length > 0)
                        {
                            album = tagLibFile.Tag.AlbumArtists[0];
                        }
                        else
                        {
                            album = "unknown album";
                            _filesWithMissingTags.Add(mp3File);
                        }

                        // create new MP3 Node in the list
                        var Mp3Node1 = new Mp3Node()
                        {
                            AlbumName = album,
                            ArtistName = artist,
                            FileName = mp3File
                        };
                        mp3FileList.Add(Mp3Node1);
                        
                    }
                    else 
                    {
                        // create new MP3 Node in the list
                        var Mp3Node1 = new Mp3Node()
                        {
                            AlbumName = "Untagged MP3 File",
                            ArtistName = "Untagged MP3 File",
                            FileName = mp3File
                        };

                        mp3FileList.Add(Mp3Node1);

                        _filesWithMissingTags.Add(mp3File);
                    }
                } 
                catch (Exception ex) 
                {
                    CodeSite.Send(mp3File);
                    CodeSite.SendException(ex);
                    _filesWithMissingTags.Add(mp3File);
                }
            }

            return mp3FileList;
        }

        public string GetArtistFromTagFile(TagLib.File mp3File)
        {
            string artist = "";

            if (mp3File.Tag.AlbumArtists.Length > 0)
                artist = mp3File.Tag.AlbumArtists[0];
            else if (mp3File.Tag.Artists.Length > 0)
                artist = mp3File.Tag.Artists[0];

            return artist;
        }

        /// <summary>
        /// Appears to be used only by unit tests, not returned to users
        /// </summary>
        public IList<string> FilesWithMissingTags
        {
            get { return _filesWithMissingTags; }
        }
    }
}
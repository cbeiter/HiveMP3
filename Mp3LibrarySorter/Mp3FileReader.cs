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
        /// <param name="mp3FilePaths">List of string paths to the MP3 files</param>
        /// <returns>List of MP3 representations</returns>
        public IList<Mp3Node> RetrieveTagsFromMp3Files(IList<string> mp3FilePaths)
        {
            var mp3FileList = new List<Mp3Node>();
            var count = 0;

            foreach (var currentMp3FilePath in mp3FilePaths)
            {
                // filtering out these two types.  Don't know why
                string extension = Path.GetExtension(currentMp3FilePath);

                if ((extension != ".cue") && (extension != ".db"))
                try
                {
                    // Codesite is a logging tool. Where is this going???
                    CodeSite.Send("Processing file " + count++ + " from " + mp3FilePaths.Count);

                    TagLib.File tagLibFile = null;

                    try
                    {
                        // hydrate TagLib File data structure from raw mp3 file
                        tagLibFile = TagLib.File.Create(currentMp3FilePath);
                    }
                    catch (Exception exception)
                    {
                        _filesWithMissingTags.Add(currentMp3FilePath);
                        CodeSite.Send(currentMp3FilePath);
                        CodeSite.SendException(exception);
                    }

                    string artist = "Unknown artist";
                    string album = "Unknown album";
                    string title = "Unknown title";
                    string trackNumber = "00";
                    int bitrate = 1;

                    // if we have a tag library, we'll go through it and create an MP3Node to represent it
                    // TODO: Not sure the justification of moving from this tagLib format to our own custom format
                    if (tagLibFile != null && tagLibFile.Tag != null)
                    {
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
                            _filesWithMissingTags.Add(currentMp3FilePath);
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
                            _filesWithMissingTags.Add(currentMp3FilePath);
                        }
                        
                        // set trackName
                        if (tagLibFile.Tag.Title.Length > 0)
                        {
                            title = tagLibFile.Tag.Title;
                        }
                        else
                        {
                            title = currentMp3FilePath;
                        }

                        // set track number
                        trackNumber = tagLibFile.Tag.Track.ToString();
                        
                        if (string.IsNullOrEmpty(trackNumber))
                        {
                            trackNumber = "00";
                        }

                        bitrate = tagLibFile.Properties.AudioBitrate;
                    }
                    else 
                    {
                        _filesWithMissingTags.Add(currentMp3FilePath);
                    }

                    // create new MP3 Node in the list
                    var Mp3Node1 = new Mp3Node()
                    {
                        AlbumName = album,
                        ArtistName = artist,
                        FileName = currentMp3FilePath,
                        Title = title,
                        Bitrate = bitrate,
                        TrackNumber = trackNumber
                    };

                    mp3FileList.Add(Mp3Node1);
                } 
                catch (Exception ex) 
                {
                    CodeSite.Send(currentMp3FilePath);
                    CodeSite.SendException(ex);
                    _filesWithMissingTags.Add(currentMp3FilePath);
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
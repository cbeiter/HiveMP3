﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Mp3LibrarySorter
{
    /// <summary>
    /// Interfacd for File System object
    /// 
    /// Not sure why we have this interface
    /// </summary>
    public interface IFileSystem
    {
        void CreateDirectory(string artistName);
        List<string> GetMp3FilePaths(string someStartDirectory);
        void Move(string source, string destination);
    }

    /// <summary>
    /// File System object that wraps System.IO.  Not sure what the purpose of this 
    /// since there really is not much here like error handling. Doesn't make sense
    /// unless the original author had other intentions?
    /// </summary>
    public class FileSystem : IFileSystem
    {
        /// <summary>
        /// Creates a directory with the name provided
        /// </summary>
        /// <param name="folderName"></param>
        public void CreateDirectory(string folderName)
        {
            Raize.CodeSiteLogging.CodeSite.Send(folderName);
            Directory.CreateDirectory(folderName);
        }

        /// <summary>
        /// Gets the string paths to files at the designated location
        /// 
        /// TODO: Looks like this is hard coded to suppport only looking at a single level for mp3 files
        /// TODO: also, does not use the file extensions to filter the Get of files like MP3.
        /// </summary>
        /// <param name="someStartDirectory">Directory from which we are getting the MP3s</param>
        /// <returns>List of strings of names of all the MP3 files</returns>
        public List<string> GetMp3FilePaths(string someStartDirectory)
        {
            // looks like this is cur
            return new List<string>(Directory.GetFiles(someStartDirectory, "*.*", SearchOption.TopDirectoryOnly));
        }

        /// <summary>
        /// Moves a file from one location to another
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void Move(string source, string destination)
        {
            Raize.CodeSiteLogging.CodeSite.Send("Moving " + source + " to " + destination);
            File.Move(source, destination);
        }
    }

}
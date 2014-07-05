using System;
using System.Collections.Generic;
using System.IO;

namespace HiveOrganizer
{
    /// <summary>
    /// FileManager object that wraps System.IO.  Not sure what the purpose of this 
    /// since there really is not much here like error handling. Doesn't make sense
    /// unless the original author had other intentions?
    /// </summary>
    public class FileManager
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
        /// <param name="isRecursive" type="bool">indicates whether a recursive get should be done</param>
        /// <returns>List of strings of names of all the MP3 files</returns>
        public List<string> GetMp3FilePaths(string someStartDirectory, bool isRecursive)
        {
            SearchOption recursiveGet = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            // looks like this is cur
            return new List<string>(Directory.GetFiles(someStartDirectory, "*.*", recursiveGet));
        }

        /// <summary>
        /// Moves a file from one location to another
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void Move(string source, string destination)
        {
            Raize.CodeSiteLogging.CodeSite.Send("Moving " + source + " to " + destination);
            try
            {
                File.Move(source, destination);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            
        }
    }

}

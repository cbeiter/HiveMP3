using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Mp3LibrarySorter;

namespace HiveConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Hive MP3 Library Sorter!");
       //     TagLib.File file = TagLib.File.Create(@"c:\testmp3\01 Egg Raid On Mojo.mp3");
            // Console.WriteLine(file.Tag);
            //var a = new Mp3LibrarySorter.Mp3LibrarySorter(new FileSystem(), @"c:\testmp3", new Mp3TagsHierarchy(), new Mp3FileReader());
            //a.CreateFoldersForArtists();

            /* END OF ORIGINAL FILE */

            // Define some locals
            List<string> allowed = new List<string>();

            allowed.Add(".flac");
            allowed.Add(".mp3");
            allowed.Add(".m4a");

            string startIn = "", destination = "";
            bool errors = false;
            
            // Print welcome message
            Console.WriteLine("This tool indexes your music " + Environment.NewLine +
                              "specified by fileformat. Everything is based on your IDv3 tags. " + Environment.NewLine +
                              "If your tags don't match, the tool will mess up the copying. Be " + Environment.NewLine +
                              "very careful with your indexing scope!");
            Console.WriteLine(Environment.NewLine);

            // Print allowed formats
            Console.WriteLine("Allowed file formats supported are:");

            foreach (string format in allowed)
            {
                Console.WriteLine(string.Format(@" - {0}", format));
            }

            Console.Write(Environment.NewLine);

            // Display target location folder dialog
            Console.WriteLine("Define a target location for the renamed track...");

            // windows form dialog to select Folder destination
            FolderBrowserDialog targetLocationDialog = new FolderBrowserDialog();
            targetLocationDialog.Description = "Select the base folder to copy the renamed tracks to";
            targetLocationDialog.ShowNewFolderButton = true;
            targetLocationDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (targetLocationDialog.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("User abort. Exiting...");
                Environment.Exit(0);
            }

            destination = targetLocationDialog.SelectedPath;

            Console.WriteLine(string.Format(@"Chosen target path is {0}", destination));
            Console.Write(Environment.NewLine);
            
            // Display start location folder dialog
            Console.WriteLine("Where are your music files?  Select the base folder... ");

            FolderBrowserDialog startInDialog = new FolderBrowserDialog();
            startInDialog.Description = "Select the base folder where your music files are located";
            startInDialog.ShowNewFolderButton = false;
            startInDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (startInDialog.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("User abort. Exiting...");
                Environment.Exit(0);
            }

            startIn = startInDialog.SelectedPath;

            Console.WriteLine(string.Format(@"The path to your music files is {0}", startIn));
            Console.Write(Environment.NewLine);
            Console.WriteLine("Start indexing and organizing procedure. Press Y to start, Q to Quit.");
            
            System.ConsoleKeyInfo response;

            do
            {
                response = Console.ReadKey();
                
                if(response.Key == ConsoleKey.Y)
                {
                    break;
                }
                else if(response.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("User abort. Exiting...");
                    Environment.Exit(0);
                }

            } while (true);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Inspect the files and build the Hive metadata library
            Mp3TagLibrary tagLibrary = new Mp3TagLibrary();
            Mp3FileReader fileReader = new Mp3FileReader();
            FileSystem fileSystem = new FileSystem();

            var Hive = new Mp3LibrarySorter.Mp3LibrarySorter(fileSystem, startIn, destination,
                tagLibrary, fileReader);

            Console.WriteLine(string.Format("{0} artists found", tagLibrary.Artists.Count));

            foreach(string artist in tagLibrary.Artists)
            {
                IList<string> albums = tagLibrary.GetAlbumsForArtist(artist);

                Console.WriteLine("Artist: " + artist + "\tAlbum count: " + albums.Count);

                foreach(string album in albums)
                {
                    Console.WriteLine("Artist: " + artist + "\tAlbum: " + album);
                }
            }

            Console.Write(Environment.NewLine);

            try
            {
                // Hive.CreateFoldersForArtists();
    
                //organizer.Index();
                //organizer.Organize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                errors = true;
            }

            Console.Write(Environment.NewLine);

            stopwatch.Stop();

            // Print some nice stats
            if (!errors)
            {
                //Console.WriteLine(string.Format(@"{0} tracks were renamed and moved to the target location.", organizer.Files.Count));
                Console.WriteLine(string.Format(@"All tracks were indexed and organized in {0}.", stopwatch.Elapsed));
                Console.Write(Environment.NewLine);
            }

            // And wait for last user input
            Console.WriteLine("Done! Press any key to continue...");
            Console.ReadKey();
        }
    }
}

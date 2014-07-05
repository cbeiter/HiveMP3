using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using HiveOrganizer;

namespace HiveConsole
{
    class Program
    {
        static string divider = "-----------------\n";

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            ConsoleWriter consoleWriter = new ConsoleWriter();

            consoleWriter.WriteLine("Welcome to the Hive MP3 Library Sorter!");
       //     TagLib.File file = TagLib.File.Create(@"c:\testmp3\01 Egg Raid On Mojo.mp3");
            // consoleWriter.WriteLine(file.Tag);
            //var a = new HiveOrganizer.HiveOrganizer(new fileManager(), @"c:\testmp3", new Mp3TagsHierarchy(), new Mp3FileReader());
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
            consoleWriter.WriteLine("This tool indexes your music " + Environment.NewLine +
                              "specified by fileformat. Everything is based on your IDv3 tags. " + Environment.NewLine +
                              "If your tags don't match, the tool will mess up the copying. Be " + Environment.NewLine +
                              "very careful with your indexing scope!");
            consoleWriter.WriteLine(Environment.NewLine);

            // Print allowed formats
            consoleWriter.WriteLine("Allowed file formats supported are:");

            foreach (string format in allowed)
            {
                consoleWriter.WriteLine(string.Format(@" - {0}", format));
            }

            consoleWriter.WriteLine(Environment.NewLine);

            // Display start location folder dialog
            consoleWriter.WriteLine("Where are your music files?  Select the source folder... ");

            FolderBrowserDialog startInDialog = new FolderBrowserDialog();
            startInDialog.Description = "Select the source folder where your music files are located";
            startInDialog.ShowNewFolderButton = false;
            startInDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (startInDialog.ShowDialog() != DialogResult.OK)
            {
                consoleWriter.WriteLine("User abort. Exiting...");
                Environment.Exit(0);
            }

            startIn = startInDialog.SelectedPath;

            consoleWriter.WriteLine(string.Format(@"The path to your music files is {0}", startIn));
            consoleWriter.Write(Environment.NewLine);


            // Display target location folder dialog
            consoleWriter.WriteLine("Define a destination for the renamed tracks...");

            // windows form dialog to select Folder destination
            FolderBrowserDialog targetLocationDialog = new FolderBrowserDialog();
            targetLocationDialog.Description = "Select the destination folder to copy the files to";
            targetLocationDialog.ShowNewFolderButton = true;
            targetLocationDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (targetLocationDialog.ShowDialog() != DialogResult.OK)
            {
                consoleWriter.WriteLine("User abort. Exiting...");
                Environment.Exit(0);
            }

            destination = targetLocationDialog.SelectedPath;

            consoleWriter.WriteLine(string.Format(@"Chosen destination path is {0}", destination));
            consoleWriter.Write(Environment.NewLine);
            
            consoleWriter.WriteLine("Start indexing and organizing procedure. Press S to start, Q to Quit.");
            
            System.ConsoleKeyInfo response;

            do
            {
                response = Console.ReadKey();
                
                if(response.Key == ConsoleKey.S)
                {
                    break;
                }
                else if(response.Key == ConsoleKey.Q)
                {
                    consoleWriter.WriteLine("User abort. Exiting...");
                    Environment.Exit(0);
                }

            } while (true);

            consoleWriter.Write(Environment.NewLine);
            consoleWriter.Write(Environment.NewLine);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Inspect the files and build the Hive metadata library
            artistAlbumCatalog tagLibrary = new artistAlbumCatalog();

            var Hive = new HiveOrganizer.Mp3LibraryGenerator(startIn, destination, tagLibrary);

            consoleWriter.WriteLine(string.Format("{0} artists found", tagLibrary.Artists.Count));
            consoleWriter.WriteLine(divider);

            foreach(string artist in tagLibrary.Artists)
            {
                IList<string> albums = tagLibrary.GetAlbumsForArtist(artist);

                consoleWriter.WriteLine(divider);

                consoleWriter.WriteLine("Artist: " + artist + "\tAlbum count: " + albums.Count);

                IList<HiveOrganizer.Mp3Node> songs;
                
                foreach(string album in albums)
                {
                    songs = tagLibrary.GetSongsForAlbumOfArtist(album, artist);
                    consoleWriter.WriteLine("\tAlbum: " + album + "\tTracks: " + songs.Count);
                }
            }

            consoleWriter.Write(Environment.NewLine);
            consoleWriter.WriteLine("Press the ENTER key to your library and copy files to their new folder structure.");
            Console.ReadLine();

            try
            {
                Hive.CreateFoldersForArtists(consoleWriter);
            }
            catch (Exception e)
            {
                consoleWriter.WriteLine(e.Message);
                consoleWriter.WriteLine(e.StackTrace);
                errors = true;
            }

            consoleWriter.Write(Environment.NewLine);

            stopwatch.Stop();

            // Print some nice stats
            if (!errors)
            {
                //consoleWriter.WriteLine(string.Format(@"{0} tracks were renamed and moved to the target location.", organizer.Files.Count));
                consoleWriter.WriteLine(string.Format(@"All tracks were indexed and organized in {0}.", stopwatch.Elapsed));
                consoleWriter.Write(Environment.NewLine);
            }

            // And wait for last user input
            consoleWriter.WriteLine("Done! Press the ENTER key to complete...");
            Console.ReadLine();
        }
    }
}

Hive MP3 Library is designed to read in MP3 files from a folder, and copies them
to a new directory with file folder and MP3 file names modified to match
artist data encoded on the files.

Features
* Read files from share location
* Read tags off MP3 files
* Write folder library structure based on /band/album/mp3filename.mp3

Burn down list
* Update library writing to change file name on copy to use tags to construct the file name instead of the original file name
* Current implementation uses the filename as a unique identifier, and does not match files based on tags
* Update MP3 reader to track duplicates
* Read bitrate off files and choose which files should win when dupe based on bitrate
* Special characters in folder names cause exceptions when writing

Current algorithm
* Program collects info from user on where the files are and where they should go
* Program instantiates an Mp3LibraryGenerator
* Mp3LibraryGenerator constructor uses a Mp3FileReader to read tags off files from source directory
* Mp3FileReader creates Mp3Nodes for every file it finds and puts them in a flat list of Mp3Nodes
* Mp3LibraryGenerator goes through Mp3NodeList and tries to add each song to the artistAlbumCatalog
* artistAlbumCatalog reads the Mp3Node and groups songs to albums and albums to artists
* Mp3LibraryGenerator is returned to program with the catalog
* Program confirms Mp3LibraryGenerator to proceed
* Mp3LibraryGenerator creates the new directories based on the catalog then moves files to the destination
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
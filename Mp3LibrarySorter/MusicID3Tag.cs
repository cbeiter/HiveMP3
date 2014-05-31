namespace HiveOrganizer
{
    /// <summary>
    /// This class appears to be unused and have no idea what if anything it was supposed to do.
    /// </summary>
    internal class MusicID3Tag
    {
        public byte[] TAGID = new byte[3];      //  3
        public byte[] Title = new byte[30];     //  30
        public byte[] Artist = new byte[30];    //  30 
        public byte[] Album = new byte[30];     //  30 
        public byte[] Year = new byte[4];       //  4 
        public byte[] Comment = new byte[30];   //  30 
        public byte[] Genre = new byte[1];      //  1
    }
}
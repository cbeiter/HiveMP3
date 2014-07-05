using System;
using System.IO;

namespace HiveOrganizer
{
    /// <summary>
    /// Abstraction so we can write both to the console and to log file at same time
    /// </summary>
    public class ConsoleWriter
    {
        StreamWriter _logFile;

        public ConsoleWriter()
        {
            DateTime currentTime = DateTime.Now;

            _logFile = File.CreateText("HiveLog_" + currentTime.ToFileTime() + ".txt");
        }

        ~ConsoleWriter()
        {
            // not sure I really need to do any cleanup in here
        }

        /// <summary>
        /// Writes a new line to the designated output
        /// </summary>
        /// <param name="input"></param>
        public void WriteLine(string input)
        {
            Console.WriteLine(input);
            _logFile.WriteLine(input);
            _logFile.Flush();
        }

        /// <summary>
        /// Writes a new entry to to the designated output
        /// </summary>
        /// <param name="input"></param>
        public void Write(string input)
        {
            Console.Write(input);
            _logFile.Write(input);
            _logFile.Flush();
        }

        /// <summary>
        /// Write new string to the designated output
        /// </summary>
        /// <param name="input"></param>
        public void WriteString(string input)
        {
            Console.Write(input);
            _logFile.Write(input);
        }
    }
}
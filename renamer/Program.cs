using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Randomiser
{
    class Program
    {
        private static Random rand = new Random();

        //Program is intended for use with the game Half-Life 2 by Valve to randomise texture, sound etc. files and copy into the 'custom' folder for the game to read. Source files must first be extracted from the game's resource archive(s) into a folder for this program to read.
        //This process is simple and does not require much (if any) error checking, so this is mainly for posterity & I will most likely not be updating this repo much.
        //Download and make any changes you wish, I claim no copyrights or trademarks of any kind!
        public static string dirPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Half-Life 2\\hl2\\custom\\";
        public static string sourceFolder = "rootSource\\", resultFolder = "root\\", sourceDir, resultDir, fileType = "*.wav";

        static void Main(string[] args)
        {
            sourceDir = dirPath + sourceFolder;
            resultDir = dirPath + resultFolder;

            Console.WriteLine("File randomiser. Current main directory: " + dirPath + "\nFolder to be taken from: " + sourceFolder + "\nFolder to copy to: " + resultFolder);
            bool asdf = true;



            while (asdf)
            {
                Console.WriteLine("\n\nPlease enter file extension (letters only). Enter 'x' to quit");
                fileType = "*." + Console.ReadLine();

                if (fileType == ".x")
                    asdf = true;

                Console.WriteLine("\nClear or randomise? ('c' or 'r')");

                switch (Console.ReadLine()[0])
                {
                    case 'r':
                        Randomiser();
                        Console.WriteLine("\nRandomisation complete. You can use Clear to remove the files later, or Randomise again if you want.");
                        break;

                    case 'c':
                        Clear();
                        Console.WriteLine("\nAll ." + fileType + " files removed from " + resultFolder + ".\nIf '0 " + fileType + " files' returned, said files were not previously randomised - otherwise check spelling");
                        break;

                    default:
                        break;
                }
            }

            Console.WriteLine("\nSee you later!");
            return;
        }

        private static void Clear()
        {
            List<string> filenameList = Directory.GetFileSystemEntries(resultDir, fileType, SearchOption.AllDirectories).ToList();

            int lenList = filenameList.Count;
            Console.WriteLine("\nRemoving " + lenList + " " + fileType + " files...");

            foreach (string s in filenameList)
                File.Delete(s);
        }

        private static void Randomiser()
        {
            List<string> filenameList = Directory.GetFileSystemEntries(sourceDir, fileType, SearchOption.AllDirectories).ToList(), filenameListShuffled;

            int lenList = filenameList.Count;
            Console.WriteLine("\nNumber of " + fileType + " files: " + lenList);

            filenameListShuffled = new(filenameList);
            filenameListShuffled = ShuffleList(filenameListShuffled, sourceDir);

            string[] subdirectories = Directory.GetDirectories(sourceDir);
            foreach (string s in subdirectories)
            {
                Directory.CreateDirectory(s.Replace(sourceFolder, resultFolder));
                LoadSubs(s);
            }

            for (int i = 0; i < lenList; i++)
                File.Copy(filenameList[i], filenameListShuffled[i], true);
        }

        private static List<string> ShuffleList(List<string> listIn, string sourceDir)
        {
            List<string> listOut = new();
            int lenList = listIn.Count;

            while (lenList > 0)
            {
                int select = rand.Next(lenList);

                listOut.Add(dirPath + resultFolder + listIn[select][sourceDir.Length..]);
                listIn.RemoveAt(select);

                lenList = listIn.Count;
            }

            return listOut;
        }

        private static void LoadSubs(string dir)
        {
            string[] subdirectories = Directory.GetDirectories(dir);
            Directory.CreateDirectory(dir.Replace(sourceFolder, resultFolder));

            foreach (string s in subdirectories)
                LoadSubs(s);
        }
    }
}

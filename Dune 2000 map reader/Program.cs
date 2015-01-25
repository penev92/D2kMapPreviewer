using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dune_2000_map_reader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                ShowUsage();
                return;
            }

            var fileType = args[0].ToLowerInvariant();
            args = args.Skip(1).ToArray();

            if (fileType == "map")
                ReadMapFile.Run(args);
            else
                if (fileType == "mis")
                ReadMisFile.Run(args[0]);
            else
                ShowUsage();
        }

        static void ShowUsage()
        {
            Console.WriteLine("This program can either generate a preview of a Dune 2000 map in a PNG format");
            Console.WriteLine("or read a Dune 2000 MIS file and dump the contents in a text file.");
            Console.WriteLine("Dune 2000 map reader {map <fullPathToMapFile> [<fullPathToTilesetFile>]}|{mis <fullPathToMisFile>}");

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dune_2000_map_reader
{
    public static class ReadMapFile
    {
        static int[] mapInfo;

        public static void Run(string[] args)
        {
            if (!Directory.Exists("Generated"))
                Directory.CreateDirectory("Generated");

            ReadMap(args[0]);

            if (args.Length == 1)
                foreach (var tilesetFile in Directory.GetFiles("..\\..\\..\\Tilesets"))
                    GenerateMapPreview(args[0], tilesetFile);
            else
            if (args.Length == 2)
                GenerateMapPreview(args[0], args[1]);
            else
                Console.WriteLine("Incorrect number of arguments!");
        }

        static void ReadMap(string mapFilePath)
        {
            var mapBytes = File.ReadAllBytes(mapFilePath);
            mapInfo = new int[mapBytes.Length / 2];
            for (int i = 0; i < mapBytes.Length; i += 2)
                mapInfo[i / 2] = mapBytes[i] + 256 * mapBytes[i + 1];
        }

        static void GenerateMapPreview(string mapFilePath, string tileSetFilePath)
        {
            Console.WriteLine("Generating map preview with tileset {0}", Path.GetFileNameWithoutExtension(tileSetFilePath).ToUpper());

            var mapSize = new Size(mapInfo[0], mapInfo[1]);
            var newMap = new Bitmap(mapSize.Width * 32, mapSize.Height * 32);
            var tileset = new Bitmap(tileSetFilePath);

            using (Graphics grD = Graphics.FromImage(newMap))
                for (int y = 0; y < mapSize.Height; y++)
                    for (int x = 0; x < mapSize.Width; x++)
                    {
                        var index = y * mapSize.Width + x + 1;
                        var tileId = mapInfo[index * 2];
                        
                        // Spice
                        if (mapInfo[index * 2 + 1] == 1)
                            tileId = 799;

                        // Thick spice
                        if (mapInfo[index * 2 + 1] == 2)
                            tileId = 301;
                        
                        var tileX = tileId % 20;
                        var tileY = tileId / 20;
                        grD.DrawImage(tileset, new Rectangle(x * 32, y * 32, 32, 32),
                                   new Rectangle(tileX * 32, tileY * 32, 32, 32), GraphicsUnit.Pixel);
                    }

            var mapName = Path.GetFileNameWithoutExtension(mapFilePath);
            var tileSetName = Path.GetFileNameWithoutExtension(tileSetFilePath);
            var fileName = string.Format("{0} - {1} - {2}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), mapName, tileSetName);
            var filePath = Path.Combine(Environment.CurrentDirectory, "Generated", fileName);

            newMap.Save(filePath);
        }
    }
}

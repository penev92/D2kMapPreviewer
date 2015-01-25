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
        static Dictionary<int, string> actorCodes = new Dictionary<int, string>
        {
            {20, "wormspawner"},
            {23, "mpspawn"},
            {41, "spicebloom"},
            {42, "spicebloom"},
            {43, "spicebloom"},
            {44, "spicebloom"},
            {45, "spicebloom"},
            // Atreides:
            {4, "WALLA"},
            {5, "PWRA"},
            {8, "CONYARDA"},
            {11, "BARRA"},
            {14, "REFA"},
            {17, "RADARA"},
            {63, "LIGHTA"},
            {69, "SILOA"},
            {72, "HEAVYA"},
            {75, "REPAIRA"},
            {78, "GUNTOWERA"},
            {120, "HIGHTECHA"},
            {123, "ROCKETTOWERA"},
            {126, "RESEARCHA"},
            {129, "STARPORTA"},
            {132, "PALACEA"},
            {180, "RIFLE"},
            {181, "BAZOOKA"},
            {182, "FREMEN"},
            //{183, Atreides sardaukar
            {184, "ENGINEER"},
            {185, "HARVESTER"},
            {186, "MCVA"},
            {187, "TRIKE"},
            {188, "QUAD"},
            {189, "COMBATA"},
            {190, "MISSILETANK"},
            {191, "SIEGETANK"},
            {192, "CARRYALLA"},
            {194, "SONICTANK"},
            // Harkonnen:
            {204, "WALLH"}, // Harkonnen wall
            {205, "PWRH"}, // Harkonnen wind trap
            {208, "CONYARDH"}, // Harkonnen construction yard
            {211, "BARRH"}, // Harkonnen barracks
            {214, "REFH"}, // Harkonnen refinery
            {217, "RADARH"}, // Harkonnen outpost
            {263, "LIGHTH"}, // Harkonnen light factory
            {269, "SILOH"}, // Harkonnen silo
            {272, "HEAVYH"}, // Harkonnen heavy factory
            {275, "REPAIRH"}, // Harkonnen rep. pad
            {278, "GUNTOWERH"}, // Harkonnen gun turret
            {320, "HIGHTECHH"}, // Harkonnen hi tech factory
            {323, "ROCKETTOWERH"}, // Harkonnen rocket turret
            {326, "RESEARCHH"}, // Harkonnen ix. res. centre
            {329, "STARPORTH"}, // Harkonnen starport
            {332, "PALACEH"}, // Harkonnen palace
            {360, "RIFLE"}, // Harkonnen light inf.
            {361, "BAZOOKA"}, // Harkonnen trooper
            //{362, ""}, // Harkonnen fremen
            {363, "SARDAUKAR"}, // Harkonnen sardaukar
            {364, "ENGINEER"}, // Harkonnen engineer
            {365, "HARVESTER"}, // Harkonnen harvester
            {366, "MCVH"}, // Harkonnen MCV
            {367, "TRIKE"}, // Harkonnen trooper
            {368, "QUAD"}, // Harkonnen quad
            {369, "COMBATH"}, // Harkonnen combat tank
            {370, "MISSILETANK"}, // Harkonnen mis. tank
            {371, "SIEGETANK"}, // Harkonnen siege tank
            {372, "CARRYALLH"}, // Harkonnen carryal
            {374, "DEVAST"}, // Harkonnen devastator 
        };

        static Dictionary<Point, int> resourceTiles;
        static Dictionary<Point, string> actors;

        public static void Run()
        {
            actors = new Dictionary<Point, string>();
            resourceTiles = new Dictionary<Point, int>();

            var mapFilePath = @"E:\Work\Programming\C#\Dune 2000 map reader\resources\Dune2000_Maps\Dune2000 Maps\ArakeenApproach.map";
            //var mapFilePath = @"H:\MISSIONS\A1V1.MAP";
            //var mapFilePath = @"E:\Work\Programming\C#\Dune 2000 map reader\Dune2000_Maps\Dune2000 Maps\IslandsOfHope.map";
            //var mapFilePath = @"E:\Work\Programming\C#\Dune 2000 map reader\Dune2000_Maps\Dune2000 Maps\MegaPack\o1m5.map";
            //var mapFilePath = @"F:\Games\RTS\Dune 2000\Data\maps\8PLAY1.MAP";
            //var tileSetFilePath = @"E:\Work\Programming\C#\Dune 2000 map reader\resources\tilesets\d2k_BLOXBASE.bmp";
            //var tileSetFilePath = @"E:\Work\Programming\C#\Dune 2000 map reader\resources\tilesets\d2k_BLOXBAT.bmp";
            var tileSetFilePath = @"E:\Work\Programming\C#\Dune 2000 map reader\resources\tilesets\d2k_BLOXBGBS.bmp";
            //var tileSetFilePath = @"E:\Work\Programming\C#\Dune 2000 map reader\resources\tilesets\d2k_BLOXICE.bmp";
            //var tileSetFilePath = @"E:\Work\Programming\C#\Dune 2000 map reader\resources\tilesets\d2k_BLOXWAST.bmp";
            //var tileSetFilePath = @"E:\Work\Programming\C#\Dune 2000 map reader\resources\tilesets\d2k_BLOXTREE.bmp";
            var mapBytes = File.ReadAllBytes(mapFilePath);
            var mapInfo = new int[mapBytes.Length / 2];
            for (int i = 0; i < mapBytes.Length; i += 2)
                mapInfo[i / 2] = mapBytes[i] + 256 * mapBytes[i + 1];

            var mapSize = new Size(mapInfo[0], mapInfo[1]);
            var newMap = new Bitmap(mapSize.Width * 32, mapSize.Height * 32);//, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            var tileset = new Bitmap(tileSetFilePath);

            using (Graphics grD = Graphics.FromImage(newMap))
                for (int y = 0; y < mapSize.Height; y++)
                    for (int x = 0; x < mapSize.Width; x++)
                    {
                        var index = y * mapSize.Width + x + 1;
                        var tileId = mapInfo[index * 2];
                        if (mapInfo[index * 2 + 1] == 1)
                        {
                            tileId = 799;
                            resourceTiles.Add(new Point(x, y), 1);
                        }
                        if (mapInfo[index * 2 + 1] == 2)
                        {
                            tileId = 301;
                            resourceTiles.Add(new Point(x, y), 2);
                        }
                        if (mapInfo[index * 2 + 1] > 2)
                        {
                            // TODO: Handle actors here
                            actors.Add(new Point(x, y), actorCodes[mapInfo[index * 2 + 1]]);
                        }
                        var tileX = tileId % 20;
                        var tileY = tileId / 20;
                        grD.DrawImage(tileset, new Rectangle(x * 32, y * 32, 32, 32),
                                   new Rectangle(tileX * 32, tileY * 32, 32, 32), GraphicsUnit.Pixel);
                    }

            newMap.Save(string.Format("map {0} - {1}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), Path.GetFileNameWithoutExtension(mapFilePath)));
            File.Copy(tileSetFilePath, "tileset.bmp", true);
        }
    }
}

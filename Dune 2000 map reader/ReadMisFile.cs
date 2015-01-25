using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dune_2000_map_reader
{
    static class ReadMisFile
    {
        public static void Run()
        {
            var misFilePath = @"H:\MISSIONS\_A1V1.MIS";
            var bytes = File.ReadAllBytes(misFilePath);
            var fileInfo = new MisFileDataObject();
            var currentIndexInFile = 0;

            // 1. House Tech level
            for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + 8; currentIndexInFile++)
            {
                fileInfo.HouseTechLevel[currentIndexInFile - startIndex] = bytes[currentIndexInFile];
            }

            // 2. Starting money
            for (int startIndex = currentIndexInFile, count = 0; currentIndexInFile < startIndex + 32; currentIndexInFile+=4)
            {
                fileInfo.StartingMoney[count++] = bytes[currentIndexInFile] + bytes[currentIndexInFile + 1] * 256
                    + bytes[currentIndexInFile + 2] * 256 * 256 + bytes[currentIndexInFile + 3] * 256 * 256 * 256;
            }

            // 3. Unknown region of 40 bytes
            for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + 40; currentIndexInFile++)
            {
                fileInfo.UnknownRegion1[currentIndexInFile - startIndex] = bytes[currentIndexInFile];
            }

            // 4. House index allocation
            for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + 8; currentIndexInFile++)
            {
                fileInfo.HouseIndexAllocation[currentIndexInFile - startIndex] = bytes[currentIndexInFile];
            }

            // 5. AI Section
            for (int i = 0; i < 8; i++)
            {
                var len = 7608;
                fileInfo.AISection[i].data = new byte[len];
                for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + len; currentIndexInFile++)
                    fileInfo.AISection[i].data[currentIndexInFile - startIndex] = bytes[currentIndexInFile];
            }

            // 6. Diplomacy
            for (int i = 0; i < 8; i++)
            {
                var len = 8;
                fileInfo.Diplomacy[i].data = new byte[len];
                for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + len; currentIndexInFile++)
                    fileInfo.Diplomacy[i].data[currentIndexInFile - startIndex] = bytes[currentIndexInFile];
            }

            // 7. Events
            for (int i = 0; i < 64; i++)
            {
                var len = 72;
                fileInfo.Events[i].data = new byte[len];
                for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + len; currentIndexInFile++)
                    fileInfo.Events[i].data[currentIndexInFile - startIndex] = bytes[currentIndexInFile];
            }

            // 8. Conditions
            for (int i = 0; i < 48; i++)
            {
                var len = 28;
                fileInfo.Conditions[i].data = new byte[len];
                for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + len; currentIndexInFile++)
                    fileInfo.Conditions[i].data[currentIndexInFile - startIndex] = bytes[currentIndexInFile];
            }

            // 9. Tileset image name
            //var startIndex = 66968;
            for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + 200; currentIndexInFile++)
            {
                fileInfo.TilesetImageName[currentIndexInFile - startIndex] = (char)bytes[currentIndexInFile];
            }

            // 10. Tileset data file name
            for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + 200; currentIndexInFile++)
            {
                fileInfo.TilesetDataName[currentIndexInFile - startIndex] = (char)bytes[currentIndexInFile];
            }

            // 11. Active events count
            fileInfo.EventCount = bytes[currentIndexInFile++];

            // 12. Active conditions count
            fileInfo.ConditionCount = bytes[currentIndexInFile++];

            // 13. Time limit
            // TODO: Handle the next two bytes!
            fileInfo.TimeLimit = bytes[currentIndexInFile] + bytes[currentIndexInFile + 1] * 256
                + bytes[currentIndexInFile + 2] * 256 * 256 + bytes[currentIndexInFile + 3] * 256 * 256 * 256;
            currentIndexInFile += 4;

            // 14. Unknown region of remaining bytes; take only 400 to be safe; we don't use them anyway
            for (int startIndex = currentIndexInFile; currentIndexInFile < startIndex + 400; currentIndexInFile++)
            {
                fileInfo.UnknownRegion2[currentIndexInFile - startIndex] = bytes[currentIndexInFile];
            }

            var tileset = fileInfo.Tileset;
            var tilesetData = fileInfo.TilesetData;
        }
    }
}

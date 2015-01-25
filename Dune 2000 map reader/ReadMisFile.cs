using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dune_2000_map_reader
{
    static class ReadMisFile
    {
        static StringBuilder stringBuilder;

        public static void Run(string misFilePath)
        {
            var fileInfo = ReadFile(misFilePath);

            var misName = Path.GetFileNameWithoutExtension(misFilePath);
            var fileName = string.Format("{0} - {1}.txt", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), misName);
            var filePath = Path.Combine(Environment.CurrentDirectory, "Generated", fileName);

            DumpDataToFile(fileInfo, filePath);
        }

        static MisFileDataObject ReadFile(string filePath)
        {
            var fileInfo = new MisFileDataObject();

            var fileStream = new FileStream(filePath, FileMode.Open);
            var binaryReader = new BinaryReader(fileStream);

            // 1. House Tech level
            binaryReader.Read(fileInfo.HouseTechLevel, 0, fileInfo.HouseTechLevel.Length);

            // 2. Starting money
            for (var i = 0; i < fileInfo.StartingMoney.Length; i++)
                fileInfo.StartingMoney[i] = binaryReader.ReadInt32();

            // 3. Unknown region of 40 bytes
            binaryReader.Read(fileInfo.UnknownRegion1, 0, fileInfo.UnknownRegion1.Length);

            // 4. House index allocation
            binaryReader.Read(fileInfo.HouseIndexAllocation, 0, fileInfo.HouseIndexAllocation.Length);

            // 5. AI Section
            for (var i = 0; i < fileInfo.AISection.Length; i++)
            {
                fileInfo.AISection[i] = new AISection();
                for (var j = 0; j < AISection.ByteCount; j++)
                    fileInfo.AISection[i].data[j] = binaryReader.ReadByte();
            }

            // 6. Diplomacy
            for (var i = 0; i < fileInfo.AISection.Length; i++)
            {
                fileInfo.Diplomacy[i] = new DiplomacyRow();
                for (var j = 0; j < DiplomacyRow.ByteCount; j++)
                    fileInfo.Diplomacy[i].data[j] = binaryReader.ReadByte();
            }

            // 7. Events
            for (var i = 0; i < fileInfo.Events.Length; i++)
                fileInfo.Events[i] = new Event { data = binaryReader.ReadBytes(Event.ByteCount) };

            // 8. Conditions
            for (var i = 0; i < fileInfo.Conditions.Length; i++)
                fileInfo.Conditions[i] = new Condition { data = binaryReader.ReadBytes(Condition.ByteCount) };

            // 9. Tileset image name
            binaryReader.Read(fileInfo.TilesetImageName, 0, fileInfo.TilesetImageName.Length);

            // 10. Tileset data file name
            binaryReader.Read(fileInfo.TilesetDataName, 0, fileInfo.TilesetDataName.Length);

            // 11. Active events count
            fileInfo.EventCount = binaryReader.ReadByte();

            // 12. Active conditions count
            fileInfo.ConditionCount = binaryReader.ReadByte();

            // 13. Time limit
            // TODO: Handle the next two bytes!
            fileInfo.TimeLimit = binaryReader.ReadInt32();

            // 14. Unknown region of remaining bytes; take only 400 to be safe; we don't use them anyway
            binaryReader.Read(fileInfo.UnknownRegion2, 0, fileInfo.UnknownRegion2.Length);

            binaryReader.Close();
            fileStream.Close();

            return fileInfo;
        }

        static void DumpDataToFile(MisFileDataObject fileInfo, string filePath)
        {
            stringBuilder = new StringBuilder();

            PrintPerHouse("Tech levels", fileInfo.HouseTechLevel);
            PrintPerHouse("Starting money", fileInfo.StartingMoney);
            PrintPerHouse("House allocation index", fileInfo.HouseIndexAllocation);
            PrintPerHouse("AI Section", fileInfo.AISection);
            PrintPerHouse("Diplomacy", fileInfo.Diplomacy);

            PrintAsList("Events", fileInfo.Events, fileInfo.EventCount);
            PrintAsList("Conditions", fileInfo.Conditions, fileInfo.ConditionCount);

            PrintValue("Tileset", fileInfo.Tileset);
            PrintValue("Tileset Data", fileInfo.TilesetData);
            PrintValue("Time limit", fileInfo.TimeLimit);

            var str = stringBuilder.ToString();

            File.WriteAllText(filePath, stringBuilder.ToString());
        }

        static void PrintPerHouse<T>(string sectionTitle, IList<T> values)
        {
            var index = 0;

            stringBuilder.AppendFormat("---------------------------------------------------------------------------{0}{0}", Environment.NewLine);
            stringBuilder.AppendFormat("{0}:{1}{1}", sectionTitle, Environment.NewLine);

            stringBuilder.Append("Atreides: ");
            stringBuilder.AppendLine(values[index++].ToString());

            stringBuilder.Append("Harkonnen: ");
            stringBuilder.AppendLine(values[index++].ToString());

            stringBuilder.Append("Ordos: ");
            stringBuilder.AppendLine(values[index++].ToString());

            stringBuilder.Append("Corrino: ");
            stringBuilder.AppendLine(values[index++].ToString());

            stringBuilder.Append("Fremen: ");
            stringBuilder.AppendLine(values[index++].ToString());

            stringBuilder.Append("Smugglers: ");
            stringBuilder.AppendLine(values[index++].ToString());

            stringBuilder.Append("Mercenaries: ");
            stringBuilder.AppendLine(values[index++].ToString());

            stringBuilder.Append("Creeps: ");
            stringBuilder.AppendLine(values[index].ToString());
            stringBuilder.AppendLine();
        }

        static void PrintAsList<T>(string sectionTitle, IList<T> values, int valuesCount)
        {
            stringBuilder.AppendFormat("---------------------------------------------------------------------------{0}{0}", Environment.NewLine);
            stringBuilder.AppendFormat("{0}: ({1} active){2}{2}", sectionTitle, valuesCount, Environment.NewLine);

            for (var i = 0; i < valuesCount; i++)
                stringBuilder.AppendLine(values[i].ToString());

            stringBuilder.AppendLine();
        }

        static void PrintValue<T>(string sectionTitle, T value)
        {
            stringBuilder.AppendFormat("---------------------------------------------------------------------------{0}{0}", Environment.NewLine);
            stringBuilder.AppendFormat("{0}:{2}{2}{1}{2}{2}", sectionTitle, value, Environment.NewLine);
        }
    }
}

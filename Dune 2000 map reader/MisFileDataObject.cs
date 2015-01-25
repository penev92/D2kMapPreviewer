using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dune_2000_map_reader
{
    struct AISection
    {
        public byte[] data;     // 7608 bytes
    };

    struct DiplomacyRow
    {
        public byte[] data;     // 8 bytes
    };

    struct Event
    {
        public byte[] data;     // 72 bytes
    };

    struct Condition
    {
        public byte[] data;     // 28 bytes
    };

    class MisFileDataObject
    {
        public byte[] HouseTechLevel;           // ok
        public Int32[] StartingMoney;           // ok
        public byte[] UnknownRegion1;           // wtf
        public byte[] HouseIndexAllocation;     // wtf
        public AISection[] AISection;           // wtf
        public DiplomacyRow[] Diplomacy;        // ok
        public Event[] Events;                  // wtf, needs more research
        public Condition[] Conditions;          // wtf, needs more research
        public char[] TilesetImageName;         // ok
        public char[] TilesetDataName;          // ?, needs more research
        public byte EventCount;                 // ok, useless at the moment
        public byte ConditionCount;             // ok, useless at the moment
        public Int32 TimeLimit;                 // ok
        public byte[] UnknownRegion2;           // wtf

        public string Tileset { get { return new string(TilesetImageName).Replace("\0", ""); } }
        public string TilesetData { get { return new string(TilesetDataName).Replace("\0", ""); } }

        public MisFileDataObject()
        {
            HouseTechLevel = new byte[8];
            StartingMoney = new Int32[8];
            UnknownRegion1 = new byte[40];
            HouseIndexAllocation = new byte[8];
            AISection = new AISection[8];
            Diplomacy = new DiplomacyRow[8];
            Events = new Event[64];
            Conditions = new Condition[48];
            TilesetImageName = new char[200];
            TilesetDataName = new char[200];
            UnknownRegion2 = new byte[692];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dune_2000_map_reader
{
    class AISection
    {
        public const int ByteCount = 7608;

        public byte[] data = new byte[ByteCount];

        public override string ToString()
        {
            // Current implementation until we can actually make use of this
            return string.Empty;
        }
    }

    class DiplomacyRow
    {
        public const int ByteCount = 8;

        public byte[] data = new byte[ByteCount];

        public override string ToString()
        {
            return string.Join(",", data);
        }
    }

    class Event
    {
        public const int ByteCount = 72;

        public byte[] data = new byte[ByteCount];

        public override string ToString()
        {
            // Current implementation until we get more information on this
            return "Not currently supported.";
        }
    }

    class Condition
    {
        public const int ByteCount = 28;

        public byte[] data = new byte[ByteCount];

        public override string ToString()
        {
            // Current implementation until we get more information on this
            return "Not currently supported.";
        }
    };

    class MisFileDataObject
    {
        /// <summary> Tech level per faction </summary>
        public byte[] HouseTechLevel;           // ok
        /// <summary> Starting money per faction </summary>
        public int[] StartingMoney;             // ok
        public byte[] UnknownRegion1;           // wtf
        /// <summary> ? per faction </summary>
        public byte[] HouseIndexAllocation;     // wtf
        /// <summary> AI section per faction? </summary>
        public AISection[] AISection;           // wtf
        /// <summary> DiplomacyRow per faction </summary>
        public DiplomacyRow[] Diplomacy;        // ok
        public Event[] Events;                  // wtf, needs more research
        public Condition[] Conditions;          // wtf, needs more research
        public char[] TilesetImageName;         // ok
        public char[] TilesetDataName;          // ?, needs more research
        public byte EventCount;                 // ok, useless at the moment
        public byte ConditionCount;             // ok, useless at the moment
        public int TimeLimit;                   // ok
        public byte[] UnknownRegion2;           // wtf

        public string Tileset { get { return new string(TilesetImageName).Replace("\0", ""); } }
        public string TilesetData { get { return new string(TilesetDataName).Replace("\0", ""); } }

        public MisFileDataObject()
        {
            HouseTechLevel = new byte[8];
            StartingMoney = new int[8];
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

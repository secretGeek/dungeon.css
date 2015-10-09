using Karcero.Engine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Generator.Models
{
    public class DungeonParameters
    {
        public DungeonParameters()
        {
            Init();
        }
        public void Init()
        {
            ThemeColors = new Dictionary<string, ThemeColor>();
            ThemeColors.Add("Dungeon", new ThemeColor { RockColor = "#794a1c", FloorColor = "#be9641" });
            ThemeColors.Add("Castle", new ThemeColor { RockColor = "#787b9c", FloorColor = "#c0c0c0" });
            ThemeColors.Add("SpaceBase", new ThemeColor { RockColor = "#5a53b0", FloorColor = "#9350af" });
            ThemeColors.Add("Forest", new ThemeColor { RockColor = "#006a00", FloorColor = "#914800" });
        }
        //public DungeonConfiguration config { get; set; }
        public DungeonConfiguration GetConfig()
        {
            return new DungeonConfiguration
            {
                ChanceToRemoveDeadends = ChanceToRemoveDeadends,
                Height = Height,
                MaxRoomHeight = MaxRoomHeight,
                MaxRoomWidth = MaxRoomWidth,
                MinRoomHeight = MinRoomHeight,
                MinRoomWidth = MinRoomWidth,
                Randomness = Randomness,
                RoomCount = RoomCount,
                Sparseness = Sparseness,
                Width = Width
            };
        }

        [DisplayName("Cell size (px)")]
        public int CellSize { get; set; }

        [DisplayName("Rock Color")]
        public string RockColor { get; set; }

        [DisplayName("Floor Color")]
        public string FloorColor { get; set; }
        public string Theme { get; set; }

        [DisplayName("Chance to remove dead ends")]
        public double ChanceToRemoveDeadends { get; set; }

        public int Height { get; set; }

        [DisplayName("Max Height")]
        public int MaxRoomHeight { get; set; }
        [DisplayName("Max Width")]
        public int MaxRoomWidth { get; set; }
        [DisplayName("Min Height")]
        public int MinRoomHeight { get; set; }
        [DisplayName("Min Width")]
        public int MinRoomWidth { get; set; }
        public double Randomness { get; set; }
        [DisplayName("Room Count")]
        public int RoomCount { get; set; }
        public double Sparseness { get; set; }
        public int Width { get; set; }

        public string Result { get; set; }

        [XmlIgnore]
        public string[] ThemeNames = new string[] { "Dungeon", "Castle", "SpaceBase", "Forest" };
        public Dictionary<string, ThemeColor> ThemeColors;


        [XmlIgnore]
        public IEnumerable<SelectListItem> Themes
        {
            get
            {
                return (ThemeNames).Select(x => new SelectListItem { Text = x, Value = x });
            }
        }
    }

    public class ThemeColor
    {
        public string RockColor { get; set; }
        public string FloorColor { get; set; }
    }
}
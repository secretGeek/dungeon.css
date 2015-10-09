using Generator.Models;
using Karcero.Engine;
using Karcero.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Generator.Controllers
{
    public class HomeController : Controller
    {

        Regex colorPattern = new Regex("^#[0-9A-Fa-f]{6}$");


        public ActionResult Index(string theme, string rockColor, string floorColor)
        {
            var config = new DungeonParameters { ChanceToRemoveDeadends = 0.5, Height = 33, Width = 33, MaxRoomHeight = 7, MinRoomHeight = 3, MaxRoomWidth = 7, MinRoomWidth = 3, Randomness = 0.3, RoomCount = 12, Sparseness = 0.5, RockColor = rockColor, FloorColor = floorColor, CellSize = 32, Theme = theme };

            if (string.IsNullOrWhiteSpace(theme) || !config.ThemeNames.Contains(theme))
            {
                config.Theme = "Dungeon";
            }
            rockColor = string.IsNullOrWhiteSpace(rockColor) ? "" : "#" + rockColor;
            floorColor = string.IsNullOrWhiteSpace(floorColor) ? "" : "#" + floorColor;

            if (string.IsNullOrWhiteSpace(rockColor) || !colorPattern.IsMatch(rockColor))
            {
                config.RockColor = config.ThemeColors[config.Theme].RockColor;
            }

            if (string.IsNullOrWhiteSpace(floorColor) || !colorPattern.IsMatch(floorColor))
            {
                config.FloorColor = config.ThemeColors[config.Theme].FloorColor;
            }


            return View(config);
        }

        [HttpPost]
        public ActionResult Index(DungeonParameters dungeon)
        {
            if (dungeon.Width > 200) dungeon.Width = 200;
            if (dungeon.Height > 300) dungeon.Height = 300;
            if (dungeon.RoomCount < 0) dungeon.RoomCount = 0;
            if (dungeon.RoomCount > 250) dungeon.RoomCount = 250;
            if (dungeon.Randomness > 1) dungeon.Randomness = 1;
            if (dungeon.Randomness < 0) dungeon.Randomness = 0;
            if (dungeon.Sparseness > 1) dungeon.Sparseness = 1;
            if (dungeon.Sparseness < 0) dungeon.Sparseness = 0;
            if (dungeon.ChanceToRemoveDeadends > 1) dungeon.ChanceToRemoveDeadends = 1;
            if (dungeon.ChanceToRemoveDeadends < 0) dungeon.ChanceToRemoveDeadends = 0;
            if (!dungeon.ThemeNames.Contains(dungeon.Theme))
            {
                dungeon.Theme = "Dungeon";
            }

            if (!colorPattern.IsMatch(dungeon.RockColor))
            {
                dungeon.RockColor = "#794a1c";
            }

            if (!colorPattern.IsMatch(dungeon.FloorColor))
            {
                dungeon.FloorColor = "#be9641";
            }

            //TODO: When Theme is changed -- set default colors for theme.

            var generator = new DungeonGenerator<Cell>();
            var config = dungeon.GetConfig();
            var map = generator.Generate(config);

            var result = new StringBuilder();
            result.AppendLine("<table class='map dungeon'>");

            for (int y = 0; y < map.Height; y++)
            {
                result.AppendLine("<tr>");

                for (int x = 0; x < map.Width; x++)
                {
                    var cell = map.GetCell(y, x);
                    var adj = map.GetAllAdjacentCellsByDirection(cell);

                    switch (cell.Terrain)
                    {
                        case (TerrainType.Rock):
                            result.Append("<td  class='rock'></td>");
                            break;
                        case (TerrainType.Door):
                            if (adj[Direction.East] == null || adj[Direction.East].Terrain == TerrainType.Rock)
                            {
                                result.Append("<td class='door east west'></td>");
                            }
                            else
                            {
                                result.Append("<td class='door north south'></td>");
                            }
                            break;
                        case (TerrainType.Floor):
                            var walls = new List<string>();
                            if (adj[Direction.North] == null || adj[Direction.North].Terrain == TerrainType.Rock) walls.Add("north");
                            if (adj[Direction.South] == null || adj[Direction.South].Terrain == TerrainType.Rock) walls.Add("south");
                            if (adj[Direction.East] == null || adj[Direction.East].Terrain == TerrainType.Rock) walls.Add("east");
                            if (adj[Direction.West] == null || adj[Direction.West].Terrain == TerrainType.Rock) walls.Add("west");
                            result.Append("<td class='" + string.Join(" ", walls.ToArray()) + "'></td>");
                            break;
                    }

                    result.AppendLine();
                }
                result.AppendLine("</tr>");

            }

            result.AppendLine("</table>");
            dungeon.Result = result.ToString();
            ViewBag.DungeonTable = result.ToString();
            return View(dungeon);
        }
    }
}
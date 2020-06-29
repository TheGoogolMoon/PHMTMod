using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;
using Terraria.ID;

namespace PreHMTeleportation
{
	public class PHMTWorld : ModWorld
    {
        private readonly List<(int X, int Y)> transporterCoreLocations = new List<(int X, int Y)>();
        private readonly List<(int X, int Y)> killTileLocations = new List<(int X, int Y)>();

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int Index = tasks.FindIndex(genpass => genpass.Name == "Spider Caves");
            if (Index != -1)
            {
                tasks.Insert(Index + 1, new PassLegacy("PHMT Mod Spaceship", PHMTSpaceShip));
            }

            Index = tasks.FindIndex(genpass => genpass.Name == "Larva");
            if (Index != -1)
            {
                tasks.Insert(Index + 1, new PassLegacy("PHMT Spaceship Core", PlaceTransporterCores));
            }
        }

        public void PHMTSpaceShip(GenerationProgress progress)
        {
            progress.Message = "Crashing a teleporting spaceship";
            int w = Main.maxTilesX;
            int h = Main.maxTilesY;
            int randomNumber = w / 3000;
            int centralNumber = w / 4000;

            for (int k = 0; k < randomNumber; k++)
            {
                bool added = false;
                while(!added)
                {
                    int x = WorldGen.genRand.Next(100, w - 100);
                    int y = WorldGen.genRand.Next((int)WorldGen.rockLayerHigh, h - 100);
                    added = AddSpaceshipStructure(x, y);
                }
            }
            for (int k = 0; k < centralNumber; k++)
            {
                bool added = false;
                while (!added)
                {
                    int x = WorldGen.genRand.Next(w / 3, 2 * w / 3);
                    int y = WorldGen.genRand.Next((int)WorldGen.rockLayerHigh, h - 100);
                    added = AddSpaceshipStructure(x, y);
                }
            }

            bool AddSpaceshipStructure(int xcoord, int ycoord)
            {
                (int X, int Y)[] blockCoords = new (int X, int Y)[]
                {
                    (1, 2), (1, 3), (1, 13),
                    (2, 2), (2, 6),
                    (4, 1), (4, 5), (4, 6), (4, 7), (4, 11), (4, 13), (4, 14), (4, 15), (4, 16),
                    (5, 4), (5, 12), (5, 14), (5, 16), (5, 17),
                    (6, 5), (6, 6), (6, 13), (6, 17), (6, 18),
                    (7, 1), (7, 5), (7, 6), (7, 9), (7, 12), (7, 13), (7, 16), (7, 18), (7, 19),
                    (8, 7), (8, 11), (8, 13), (8, 14), (8, 19), (8, 20),
                    (9, 6), (9, 9), (9, 12), (9, 14), (9, 15), (9, 16), (9, 17), (9, 20), (9, 21),
                    (10, 5), (10, 11), (10, 13), (10, 21), (10, 22),
                    (11, 9), (11, 10), (11, 17), (11, 19), (11, 22),
                    (12, 6), (12, 8), (12, 10), (12, 17), (12, 21), (12, 22),
                    (13, 4), (13, 8), (13, 11), (13, 17), (13, 21), (13, 22),
                    (14, 6), (14, 7), (14, 17), (14, 21), (14, 22),
                    (15, 4), (15, 5), (15, 10), (15, 13), (15, 14), (15, 15), (15, 16), (15, 17), (15, 21),
                    (16, 4), (16, 6), (16, 7), (16, 8), (16, 10), (16, 11), (16, 20), (16, 21), (16, 24),
                    (17, 7), (17, 8), (17, 18), (17, 19), (17, 20),
                    (18, 8), (18, 9), (18, 18),
                    (19, 9), (19, 10), (19, 17), (19, 18),
                    (20, 10), (20, 11), (20, 12), (20, 13), (20, 14), (20, 15), (20, 16), (20, 17),
                    (21, 11), (21, 12), (21, 13), (21, 14), (21, 15), (21, 16)
                };
                (int X, int Y)[] wallCoords = new (int X, int Y)[]
                {
                    (1, 2), (1, 3),
                    (2, 2), (2, 3), (2, 5), (2, 6),
                    (3, 5), (3, 6),
                    (4, 7), (4, 8), (4, 12), (4, 13), (4, 14), (4, 15),
                    (5, 3), (5, 4), (5, 7), (5, 8), (5, 9), (5, 12), (5, 13), (5, 14), (5, 15),
                    (6, 3), (6, 4), (6, 8), (6, 9), (6, 14), (6, 15),
                    (7, 14), (7, 15),
                    (8, 5), (8, 6), (8, 16), (8, 17),
                    (9, 5), (9, 6), (9, 11), (9, 12), (9, 13), (9, 16), (9, 17),
                    (10, 7), (10, 8), (10, 11), (10, 12), (10, 13), (10, 14),
                    (11, 7), (11, 8), (11, 11), (11, 12), (11, 13), (11, 14), (11, 15), (11, 18), (11, 19),
                    (12, 7), (12, 8), (12, 9), (12, 11), (12, 12), (12, 13), (12, 14), (12, 15), (12, 16), (12, 17), (12, 18), (12, 19),
                    (13, 8), (13, 9), (13, 12), (13, 13), (13, 14), (13, 15), (13, 16), (13, 17), (13, 18), (13, 19),
                    (14, 13), (14, 14), (14, 15), (14, 16), (14, 18), (14, 19),
                    (15, 18), (15, 19),
                    (16, 11), (16, 12), (16, 15), (16, 16), (16, 17), (16, 18),
                    (17, 11), (17, 12), (17, 13), (17, 14), (17, 15), (17, 16), (17, 17), (17, 18),
                    (18, 13), (18, 14)
                };
                /*empty for:
                 * x=11, y=12..15
                 * x=12, y=12..16
                 * x=13, y=12..16
                 * x=14, y=13.16 */
                //place core on x=12, y=16
                
                foreach ((int Xrel, int Yrel) in blockCoords)
                {
                    int X = Xrel + xcoord;
                    int Y = Yrel + ycoord;
                    if (WorldGen.InWorld(X, Y, 30))
                    {
                        Tile tile = Framing.GetTileSafely(X, Y);
                        tile.type = TileID.MartianConduitPlating;
                        tile.active(true);
                    }
                    else return false;
                }
                foreach ((int Xrel, int Yrel) in wallCoords)
                {
                    int X = Xrel + xcoord;
                    int Y = Yrel + ycoord;
                    if (WorldGen.InWorld(X, Y, 30))
                    {
                        Tile tile = Framing.GetTileSafely(X, Y);
                        tile.wall = WallID.MartianConduit;
                    }
                    else return false;
                }
                for (int x = 11; x <= 14; x++)
                {
                    for (int y = 12; y <= 16; y++)
                    {
                        if (x == 11 && y == 16) ;
                        else if (x == 14 && y == 12) ;
                        else killTileLocations.Add((x + xcoord, y + ycoord));
                    }
                }
                transporterCoreLocations.Add((12 + xcoord, 16 + ycoord));
                return true;
            }
        }

        public void PlaceTransporterCores(GenerationProgress progress)
        {
            foreach ((int X, int Y) in killTileLocations)
            {
                WorldGen.KillTile(X, Y);
            }
            foreach ((int X, int Y) in transporterCoreLocations)
            {
                WorldGen.Place2xX(X, Y, (ushort)TileType<Tiles.Core>());
            }
        }
    }
}
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System.Linq;
using System;
using Steamworks;
using Terraria.ID;

namespace PreHMTeleportation
{
	public class PHMTPlayer : ModPlayer
    {
        public readonly Dictionary<(int x, int y), string> transporterLocations = new Dictionary<(int x, int y), string>();
        //private readonly Dictionary<(int x, int y), string> transporterLocationNames = new Dictionary<(int x, int y), string>();
        public (int x, int y) chosenTransporter = (0, 0);

        public void UpdateTransporters()
        {
            transporterLocations.Clear();

            int index = 1;
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (tile != null && tile.type == TileType<Tiles.Transporter>())
                    {
                        //if (transporterLocationNames.ContainsKey((x, y))) transporterLocations.Add((x, y), transporterLocationNames[(x, y)]); //else
                        transporterLocations.Add((x, y), index++.ToString());
                    }
                }
            }
        }

        public void RenameIntegerTransporters()
        {
            int index = 1;
            var orderedTransporters = transporterLocations.OrderBy(valPair => valPair.Key.x);
            foreach (var valPair in orderedTransporters)
            {
                if (Int32.TryParse(valPair.Value, out int prevIndex))
                {
                    transporterLocations[valPair.Key] = index++.ToString();
                }
            }
        }

        public override void OnEnterWorld(Player player)
        {
            UpdateTransporters();
            base.OnEnterWorld(player);
        }
    }

	public class PreHMTeleportation : Mod
	{
    }
}
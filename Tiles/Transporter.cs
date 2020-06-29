using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System.Linq;

namespace PreHMTeleportation.Tiles
{
	public class Transporter : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.CoordinateHeights = new[] { 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Transporter");
			AddMapEntry(Color.White, name, MapName);
		}

        public override void PlaceInWorld(int i, int j, Item item)
        {
			foreach (Player player in Main.player.Where(p => p.active))
            {
				var modPlayer = player.GetModPlayer<PHMTPlayer>();
				int index = modPlayer.transporterLocations.Count + 1;
				modPlayer.transporterLocations.Add((i, j), index.ToString());
				modPlayer.RenameIntegerTransporters();
            }
            base.PlaceInWorld(i, j, item);
        }

		public string MapName(string name, int i, int j)
        {
			var modPlayer = Main.LocalPlayer.GetModPlayer<PHMTPlayer>();
			if (!modPlayer.transporterLocations.ContainsKey((i, j))) modPlayer.UpdateTransporters();
			return name + " " + modPlayer.transporterLocations[(i, j)];
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			fail = false;
			base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);

			Item.NewItem(new Vector2(i * 16, j * 16), ItemType<Items.Transporter>());

			foreach (Player player in Main.player.Where(p => p.active))
			{
				var modPlayer = player.GetModPlayer<PHMTPlayer>();
				if (modPlayer.chosenTransporter == (i, j)) modPlayer.chosenTransporter = (0, 0);
				if (modPlayer.transporterLocations.ContainsKey((i, j)))
				{
					int index = modPlayer.transporterLocations.Keys.ToList().IndexOf((i, j));
					modPlayer.transporterLocations.Remove((i, j));
					modPlayer.RenameIntegerTransporters();
				}
				else modPlayer.UpdateTransporters();
			}
        }
    }
}
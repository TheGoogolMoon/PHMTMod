using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace PreHMTeleportation.Tiles
{
	public class Core : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.addTile(Type);

			animationFrameHeight = 64;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Transportation Core");
			AddMapEntry(Color.White, name);
		}

        public override void MouseOver(int i, int j)
        {
			Player player = Main.LocalPlayer;
			player.showItemIconText = "Transportation Core";
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			r = 0.38f;
			g = 0.78f;
			b = 0.88f;
            base.ModifyLight(i, j, ref r, ref g, ref b);
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 10)
            {
				frameCounter = 0;
				if (++frame >= 4) frame %= 4;
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);
			Item.NewItem(i * 16, j * 16, 32, 64, ItemType<Items.Core>());
        }
    }
}
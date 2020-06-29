using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace PreHMTeleportation.Items
{
	public class Core : ModItem
	{
		public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("The broken core of a crashed extraterrestrial transport unit" +
				"\nThe leaking liquid still retains some power of transportation" +
				"\n'Let's hope they don't come back for this'");
        }

        public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.White;
			item.value = Item.sellPrice(silver: 50);
			item.createTile = TileType<Tiles.Core>();
		}
    }
}
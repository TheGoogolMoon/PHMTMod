using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace PreHMTeleportation.Items
{
	public class TransporterEmpty : ModItem
	{
		public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Uncharged Transporter Key");
			Tooltip.SetDefault("Try filling it with transporter fluid");
        }

        public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(silver: 5);
		}

		public override void AddRecipes()
        {
			ModRecipe rec = new ModRecipe(mod);
			rec.AddIngredient(ItemID.GoldenKey);
			rec.AddIngredient(ItemID.MartianConduitPlating, 10);
			rec.AddTile(TileType<Tiles.Core>());
			rec.SetResult(this);
			rec.AddRecipe();
        }
    }
}
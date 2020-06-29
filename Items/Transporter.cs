using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace PreHMTeleportation.Items
{
	public class Transporter : ModItem
	{
		public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Teleport to this transporter with the Transporter Key" +
				"\n'Let's hope nothing else comes through this'");
        }

        public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(silver: 50);
			item.createTile = TileType<Tiles.Transporter>();
		}

        public override void AddRecipes()
        {
			ModRecipe rec = new ModRecipe(mod);
			rec.AddIngredient(ItemID.MartianConduitPlating, 20);
			rec.AddTile(TileType<Tiles.Core>());
			rec.SetResult(this);
			rec.AddRecipe();
        }
    }
}
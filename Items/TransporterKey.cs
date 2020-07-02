using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PreHMTeleportation.Items
{
	public class TransporterKey : ModItem
	{
		public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Transporter Key");
			Tooltip.SetDefault("Teleports you to the chosen transporter" +
				"\nIf bugged, right-click in inventory to reset" +
				"\nRight-click to cycle between transporters" +
				"\nLeft-click to teleport");
        }

        public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.useTurn = true;
			item.autoReuse = false;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(gold: 1);
		}

		public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
			player.GetModPlayer<PHMTPlayer>().UpdateTransporters();
			player.PutItemInInventory(item.type);
        }

        public override bool AltFunctionUse(Player player) => true;

		public override bool UseItem(Player player)
		{
			var modPlayer = player.GetModPlayer<PHMTPlayer>();
			var locations = modPlayer.transporterLocations;
			var choice = modPlayer.chosenTransporter;
			if (locations.Count <= 0) return false;

			if (choice == (0, 0))
			{
				choice = locations.Keys.First();
				Main.LocalPlayer.GetModPlayer<PHMTPlayer>().chosenTransporter = choice;
			}

			#region Choosing Transporter
			if (player.altFunctionUse == 2)
			{
				var transXY = locations.Keys.ToList();
				if (transXY.Contains(choice))
				{
					int index = transXY.IndexOf(choice);
					choice = transXY[index + 1 == transXY.Count ? 0 : index + 1];
				}
				else choice = transXY[0];

				modPlayer.chosenTransporter = choice;
			}
			#endregion
			#region Teleport Somewhere
			else
			{
				if (locations.Count <= 0) return false;

				foreach ((int x, int y) in locations.Keys)
				{
					Tile tile = Main.tile[x, y];
					if (tile == null || tile.type != TileType<Tiles.Transporter>())
					{
						Main.NewText("Transporter removed. Update your choice.");
						return false;
					}
				}

				player.Teleport(new Vector2(Main.leftWorld + choice.x * 16, Main.topWorld + choice.y * 16 - 2 * 16));
			}
			#endregion
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<PHMTPlayer>();
			var locations = modPlayer.transporterLocations;
			var choice = modPlayer.chosenTransporter;

			if (locations.Count > 0)
			{
				if (choice == (0, 0))
                {
					modPlayer.chosenTransporter = modPlayer.transporterLocations.First().Key;
					tooltips.Add(new TooltipLine(mod, "TransporterChoice", "Current transporter: " + locations.First()));
				}
				else if (locations.ContainsKey(choice)) tooltips.Add(new TooltipLine(mod, "TransporterChoice", "Current transporter: " + locations[choice]));
				else tooltips.Add(new TooltipLine(mod, "TransporterChoice", "No transporter selected"));
			}
		}

		public override void AddRecipes()
        {
			ModRecipe rec = new ModRecipe(mod);
			rec.AddIngredient(ItemType<Items.TransporterEmpty>());
			rec.AddIngredient(ItemID.Bottle);
			rec.AddTile(TileType<Tiles.Core>());
			rec.SetResult(this);
			rec.AddRecipe();
        }
    }
}
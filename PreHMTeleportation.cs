using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria.UI;
using Microsoft.Xna.Framework;
using PreHMTeleportation.UI;
using Terraria.GameInput;
using Terraria.ModLoader.IO;

namespace PreHMTeleportation
{
	public class PHMTPlayer : ModPlayer
    {
        public readonly Dictionary<(int x, int y), string> transporterLocations = new Dictionary<(int x, int y), string>();
        public readonly Dictionary<(int x, int y), string> customTransporterNames = new Dictionary<(int x, int y), string>();
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
                        if (customTransporterNames.ContainsKey((x, y))) transporterLocations.Add((x, y), customTransporterNames[(x, y)]);
                        else transporterLocations.Add((x, y), index++.ToString());
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

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (PreHMTeleportation.TUIHotKey.JustPressed)
            {
                PreHMTeleportation.ToggleTUI();
            }
        }

        public override void OnEnterWorld(Player player)
        {
            UpdateTransporters();
            base.OnEnterWorld(player);
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("TransporterNames")) ApplyTransporterNamesFromSave(tag.GetList<string>("TransporterNames"));
        }

        private void ApplyTransporterNamesFromSave(IList<string> names)
        {
            foreach (string name in names)
            {
                var data = name.Split(' ');
                if (data.Length == 3 && int.TryParse(data[0], out int x) && int.TryParse(data[1], out int y)) customTransporterNames[(x, y)] = data[2];
            }
        }

        public override TagCompound Save()
        {
            //not the best solution, but a string of the form "x y name" works well enough here
            //another approach would be separate TagCompound entries and Zip them together
            var names = new List<string>();
            foreach (var pair in customTransporterNames)
            {
                names.Add("" + pair.Key.x + " " + pair.Key.y + " " + pair.Value);
            }

            return new TagCompound
            {
                ["TransporterNames"] = names
            };
        }
    }

	public class PreHMTeleportation : Mod
	{
        public static ModHotKey TUIHotKey;

        private static UserInterface _transporterUserInterface;
        internal static TUINameSetter TransporterNameUI;

        public override void Load()
        {
            TUIHotKey = RegisterHotKey("Transporter UI", "T");

            if (!Main.dedServ)
            {
                _transporterUserInterface = new UserInterface();
                _transporterUserInterface.SetState(null);
            }
        }

        public override void Unload()
        {
            TUIHotKey = null;
            TransporterNameUI = null;
            _transporterUserInterface = null;
        }

        public static void ToggleTUI()
        {
            if (_transporterUserInterface.CurrentState == null) OpenTNameUI();
            else CloseTNameUI();
        }

        private static void OpenTNameUI()
        {
            if (!Main.dedServ)
            {
                TransporterNameUI = new TUINameSetter();
                _transporterUserInterface.SetState(TransporterNameUI);
            }
        }

        private static void CloseTNameUI()
        {
            if (!Main.dedServ) _transporterUserInterface.SetState(null);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (_transporterUserInterface.CurrentState != null) _transporterUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer
                    (
                    "TransporterUI: Mouse Text",
                    delegate
                    {
                        _transporterUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
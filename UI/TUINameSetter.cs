using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria;

namespace PreHMTeleportation.UI
{
    internal class TUINameSetter : UIState
    {
        public TUIPanel transporterPanel;

        public override void OnInitialize()
        {
            int transporters = Main.LocalPlayer.GetModPlayer<PHMTPlayer>().transporterLocations.Count;

            transporterPanel = new TUIPanel();
            transporterPanel.SetPadding(0);

            transporterPanel.Left.Set(600, 0f);
            transporterPanel.Top.Set(60f, 0f);
            transporterPanel.Width.Set(455f, 0f);
            transporterPanel.Height.Set(15f + 45f * transporters, 0f);
            transporterPanel.BackgroundColor = new Color(73, 94, 171);

            transporterPanel.AddTransporterNameSetters();

            Append(transporterPanel);
        }
    }
}
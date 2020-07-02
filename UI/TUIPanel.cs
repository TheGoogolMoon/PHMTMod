using Terraria;
using Terraria.ID;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;

namespace PreHMTeleportation.UI
{
    internal class TUIPanel : UIPanel
    {
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ContainsPoint(Main.MouseScreen)) Main.LocalPlayer.mouseInterface = true;
        }

        public void AddTransporterNameSetters()
        {
            int iteration = 0;
            var modPlayer = Main.LocalPlayer.GetModPlayer<PHMTPlayer>();
            foreach (KeyValuePair<(int x, int y), string> pair in Main.LocalPlayer.GetModPlayer<PHMTPlayer>().transporterLocations)
            {
                TextBox box = new TextBox(pair.Value);
                box.Left.Set(30f, 0f);
                box.Top.Set(15f + 45f * iteration, 0f);
                box.Height.Set(30f, 0f);
                box.Width.Set(200f, 0f);
                Append(box);

                UIPanel saveButton = new UIPanel();
                saveButton.Left.Set(260f, 0f);
                saveButton.Top.Set(19f + 45f * iteration, 0f);
                saveButton.Height.Set(22f, 0f);
                saveButton.Width.Set(60f, 0f);
                saveButton.OnClick += (evt, element) => { ChangeTransporterEntry(pair, box); };
                UIText saveButtonText = new UIText("Save");
                saveButtonText.Left.Set(0f, 0f);
                saveButtonText.Top.Set(-10f, 0f);
                saveButton.Append(saveButtonText);
                Append(saveButton);

                UIPanel chooseButton = new UIPanel();
                chooseButton.Left.Set(335f, 0f);
                chooseButton.Top.Set(19f + 45f * iteration, 0f);
                chooseButton.Height.Set(22f, 0f);
                chooseButton.Width.Set(80, 0f);
                chooseButton.OnClick += (evt, element) => { modPlayer.chosenTransporter = pair.Key; };
                UIText chooseButtonText = new UIText("Choose");
                chooseButtonText.Left.Set(0f, 0f);
                chooseButtonText.Top.Set(-10f, 0f);
                chooseButton.Append(chooseButtonText);
                Append(chooseButton);

                iteration++;
            }
        }

        public void ChangeTransporterEntry(KeyValuePair<(int x, int y), string> pair, TextBox box)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<PHMTPlayer>();
            modPlayer.customTransporterNames[pair.Key] = box.displayName;
            modPlayer.UpdateTransporters();
            Main.PlaySound(SoundID.MenuTick);
        }
    }

    internal class TextBox : UIPanel
    {
        private readonly string oldName;
        public string displayName = "";
        private bool typing = false;

        internal TextBox(string text)
        {
            oldName = text;
            displayName = text;
            BackgroundColor = Color.White;
            BorderColor = Color.Black;
        }

        public override void Click(UIMouseEvent evt)
        {
            StartTyping();
        }

        internal void StartTyping()
        {
            if (!typing)
            {
                typing = true;
                Main.clrInput();
                Main.blockInput = true;
            }
        }

        internal void EndTyping()
        {
            if (typing)
            {
                typing = false;
                Main.blockInput = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!ContainsPoint(Main.MouseScreen) && (Main.mouseRight || Main.mouseLeft)) EndTyping();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 position = GetDimensions().Position() + new Vector2(10, 4);
            if (typing)
            {
                Terraria.GameInput.PlayerInput.WritingText = true;
                string newName = Main.GetInputText(displayName);
                displayName = newName;

                if (Main.inputText.IsKeyDown(Keys.Enter) || Main.oldInputText.IsKeyDown(Keys.Enter))
                {
                    Main.drawingPlayerChat = false;
                    EndTyping();
                }

                if (displayName == "") spriteBatch.DrawString(Main.fontMouseText, oldName, position, Color.Gray);
                else spriteBatch.DrawString(Main.fontMouseText, displayName, position, Color.Black);
            }
            else
            {
                if (displayName == "") spriteBatch.DrawString(Main.fontMouseText, oldName, position, Color.Gray);
                else spriteBatch.DrawString(Main.fontMouseText, displayName, position, Color.Black);
            }
        }
    }
}
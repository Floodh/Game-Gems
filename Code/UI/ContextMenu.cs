using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;

class ContextMenu
{
    Texture2D menuTexture;
    private static readonly Size textureSize = new(450/2, 178/2);
    private Building building;
    public static FontSystem FontSystem { get => _fontSystem; set => _fontSystem = value; }
    internal Building Building { get => building; set => building = value; }
    private static FontSystem _fontSystem;
    Vector2 menuVec;
    Rectangle menuRect;
    protected bool mousePressed = false;

    public ContextMenu()
    {
        FontSystem = new FontSystem();
        FontSystem.AddFont(System.IO.File.ReadAllBytes(@"Data/Fonts/PTC55F.ttf"));
        this.menuTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/context-menu2.png");
    }

    public void Update()
    {
        this.Building = Building.selectedBuilding;

        if(this.building != null && this.building.State == Building.EState.Selected)
        {
            Rectangle menuRect = new(this.building.DrawArea.X + 64, this.building.DrawArea.Y + 64, 128, 128);
            menuRect = Camera.ModifiedDrawArea(menuRect, Camera.zoomLevel);

            this.menuVec = new(menuRect.Left - textureSize.Width/2, menuRect.Top - textureSize.Height/2);
            this.menuRect = new(menuVec.ToPoint().X, menuVec.ToPoint().Y, textureSize.Width, textureSize.Height);
        }
    }

    // Return true if "hit" and therefore shadowing any click on objects behind
    public bool UpdateByMouse(MouseState mouseState)
    {
        if(this.building != null && this.building.State == Building.EState.Selected)
        {
            if(this.menuRect.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                if(mouseState.LeftButton == ButtonState.Pressed)
                {
                    this.mousePressed = true;
                    Console.WriteLine("Press ContextMenu");
                }
                else if(mouseState.LeftButton == ButtonState.Released && this.mousePressed == true)
                {
                    Console.WriteLine("Release ContextMenu");
                    this.mousePressed = false;
                    var cannon = this.building as Cannon;
                    bool result = cannon.TryUpgrade();
                }
                return true;
            }

        }
        return false;
    }

    public void Draw()
    {
        if(this.building != null && this.building.State == Building.EState.Selected)
        {
            var cannon = this.building as Cannon;
            if(cannon.Tier >= Cannon.MaxTier)
                return;

            Vector2 menuVec = this.menuVec;

            GameWindow.spriteBatch.Draw(
                this.menuTexture, this.menuRect, null, new Color(Color.White, 1f), 0f, 
                new Vector2(0, 0), SpriteEffects.None, 0f);

            SpriteFontBase font18 = ResourcesUi.FontSystem.GetFont(18);
            menuVec = menuVec + new Vector2(40, 10);
            GameWindow.spriteBatch.DrawString(font18, cannon.GetUpgradeCost().blue.ToString(), menuVec, Color.Black);
            menuVec = menuVec + new Vector2(108, 0);
            GameWindow.spriteBatch.DrawString(font18, cannon.GetUpgradeCost().green.ToString(), menuVec, Color.Black); 
            menuVec = menuVec + new Vector2(0, 48);
            GameWindow.spriteBatch.DrawString(font18, cannon.GetUpgradeCost().purple.ToString(), menuVec, Color.Black);
            menuVec = menuVec + new Vector2(-108, 0);
            GameWindow.spriteBatch.DrawString(font18, cannon.GetUpgradeCost().orange.ToString(), menuVec, Color.Black); 
        }
    }

   
}
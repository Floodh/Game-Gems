using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;

class ResourcesUi
{
    private class ResourceTypeUi
    {
        Point topLeftPoint;
        Texture2D gemTexture;
        Rectangle gemRect;
        Texture2D bgTexture;
        Rectangle bgRect;
        private static readonly Size textureSize = new(32, 32);
        private static readonly Size resourceSize = new(96, 32);
        public static Size ResourceSize => resourceSize;
        public delegate int Callback();
        public Callback _callback;

        public ResourceTypeUi(Texture2D texture, Callback callback, Point topLeftPoint)
        {
            this._callback = callback;


            this.topLeftPoint = topLeftPoint;
            this.gemTexture = texture;
            this.bgTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/brick3.png");

            this.bgRect = new Rectangle(
                this.topLeftPoint.X, this.topLeftPoint.Y, ResourceTypeUi.resourceSize.Width, ResourceTypeUi.resourceSize.Height);

            this.gemRect = new Rectangle(
                this.topLeftPoint.X, this.topLeftPoint.Y, ResourceTypeUi.textureSize.Width, ResourceTypeUi.textureSize.Height);            
        }

        public void Draw()
        {
            GameWindow.spriteBatch.Draw(
                this.bgTexture, this.bgRect, null, new Color(Color.White, 1f), 0f, 
                new Vector2(0, 0), SpriteEffects.None, 0f);                

            GameWindow.spriteBatch.Draw(
                this.gemTexture, this.gemRect, null, new Color(Color.White, 1f), 0f, 
                new Vector2(0, 0), SpriteEffects.None, 0f);

            Vector2 vec = this.topLeftPoint.ToVector2() + new Vector2(32, 8);
            SpriteFontBase font18 = ResourcesUi.FontSystem.GetFont(18);
            GameWindow.spriteBatch.DrawString(font18, this._callback().ToString(), vec, Color.Black);   
        }
    }

    Size displaySize;
    private static List<ResourceTypeUi> resourceList = new();
    private static readonly int padding = 2;
    private static FontSystem _fontSystem;

    public ResourcesUi(Size displaySize)
    {
        FontSystem = new FontSystem();
        FontSystem.AddFont(System.IO.File.ReadAllBytes(@"Data/Fonts/PTC55F.ttf"));

        this.displaySize = displaySize;

        System.Reflection.PropertyInfo prop = typeof(Resources).GetProperty("Blue");
        int x = this.TopLeftPoint.X;
        int y = 0;
        ResourcesUi.resourceList.Add(
            new ResourceTypeUi(Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/BlueGem.png"),
            Resources.GetBlue,
            new Point(x, y)));

        x += ResourceTypeUi.ResourceSize.Width + ResourcesUi.padding*2;
        ResourcesUi.resourceList.Add(
            new ResourceTypeUi(Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/GreenGem.png"),
            Resources.GetGreen,
            new Point(x, y)));

        x += ResourceTypeUi.ResourceSize.Width + ResourcesUi.padding*2;
        ResourcesUi.resourceList.Add(
            new ResourceTypeUi(Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/PurpleGem.png"),
            Resources.GetPurple,
            new Point(x, y)));

        x += ResourceTypeUi.ResourceSize.Width + ResourcesUi.padding*2;
        ResourcesUi.resourceList.Add(
            new ResourceTypeUi(Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/RedGem.png"),
            Resources.GetRed,
            new Point(x, y)));

    }

    public Point TopLeftPoint
    {
        get
        {   
            return new Point(this.displaySize.Width/2 - (ResourceTypeUi.ResourceSize.Width * 2 + ResourcesUi.padding * 3));
        }
    }

    public static FontSystem FontSystem { get => _fontSystem; set => _fontSystem = value; }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach(var resource in ResourcesUi.resourceList)
        {
            resource.Draw();
        }            
    }
}
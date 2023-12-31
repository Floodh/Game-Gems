using System.Globalization;
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
        private static readonly Size textureSize = new(24, 24);
        private static readonly Size resourceSize = new(96, 32);
        public static Size ResourceSize => resourceSize;
        public delegate int Callback();
        public Callback _callback;

        public ResourceTypeUi(Texture2D texture, Callback callback, Point topLeftPoint)
        {
            this._callback = callback;


            this.topLeftPoint = topLeftPoint;
            this.gemTexture = texture;
            this.bgTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/brick4.png");

            this.bgRect = new Rectangle(
                this.topLeftPoint.X, this.topLeftPoint.Y, ResourceTypeUi.resourceSize.Width, ResourceTypeUi.resourceSize.Height);

            this.gemRect = new Rectangle(
                this.topLeftPoint.X + 2, this.topLeftPoint.Y + 2, ResourceTypeUi.textureSize.Width, ResourceTypeUi.textureSize.Height);
        }

        public void ReSize(Point topLeftPoint)
        {
            this.topLeftPoint = topLeftPoint;
            this.bgRect = new Rectangle(
                this.topLeftPoint.X, this.topLeftPoint.Y, ResourceTypeUi.resourceSize.Width, ResourceTypeUi.resourceSize.Height);

            this.gemRect = new Rectangle(
                this.topLeftPoint.X + 2, this.topLeftPoint.Y + 2, ResourceTypeUi.textureSize.Width, ResourceTypeUi.textureSize.Height);
        }

        public void Draw()
        {
            GameWindow.spriteBatchUi.Draw(
                this.bgTexture, this.bgRect, null, new Color(Color.White, 1f), 0f,
                new Vector2(0, 0), SpriteEffects.None, 0f);

            GameWindow.spriteBatchUi.Draw(
                this.gemTexture, this.gemRect, null, new Color(Color.White, 1f), 0f,
                new Vector2(0, 0), SpriteEffects.None, 0f);

            Vector2 vec = this.topLeftPoint.ToVector2() + new Vector2(38, 6);
            SpriteFontBase font18 = ResourcesUi.FontSystem.GetFont(18);
            GameWindow.spriteBatchUi.DrawString(font18, this._callback().ToString(), vec, Color.Black);
        }
    }

    Size displaySize;
    private static List<ResourceTypeUi> resourceList = new();
    private static readonly int padding = 2;
    private static FontSystem _fontSystem;

    public ResourcesUi(Size displaySize)
    {
        Texture2D[] textures = TextureSource.LoadIcons();
        if (textures.Length != 4)
            throw new Exception("Incorrect number of UI textures loaded!");

        this.displaySize = displaySize;

        int x = this.TopLeftPoint.X;
        int y = 0;
        ResourcesUi.resourceList.Add(
            new ResourceTypeUi(textures[0],
            Resources.GetBlue,
            new Point(x, y)));

        x += ResourceTypeUi.ResourceSize.Width + ResourcesUi.padding * 2;
        ResourcesUi.resourceList.Add(
            new ResourceTypeUi(textures[1],
            Resources.GetGreen,
            new Point(x, y)));

        x += ResourceTypeUi.ResourceSize.Width + ResourcesUi.padding * 2;
        ResourcesUi.resourceList.Add(
            new ResourceTypeUi(textures[2],
            Resources.GetPurple,
            new Point(x, y)));

        x += ResourceTypeUi.ResourceSize.Width + ResourcesUi.padding * 2;
        ResourcesUi.resourceList.Add(
            new ResourceTypeUi(textures[3],
            Resources.GetOrange,
            new Point(x, y)));

    }

    public void ReSize(Size displaySize)
    {
        this.displaySize = displaySize;
        int x = this.TopLeftPoint.X;
        int y = 0;

        resourceList[0].ReSize(new Point(x, y));
        x += ResourceTypeUi.ResourceSize.Width + ResourcesUi.padding * 2;
        resourceList[1].ReSize(new Point(x, y));
        x += ResourceTypeUi.ResourceSize.Width + ResourcesUi.padding * 2;
        resourceList[2].ReSize(new Point(x, y));
        x += ResourceTypeUi.ResourceSize.Width + ResourcesUi.padding * 2;
        resourceList[3].ReSize(new Point(x, y));
    }

    public Point TopLeftPoint
    {
        get
        {
            return new Point(this.displaySize.Width / 2 - (ResourceTypeUi.ResourceSize.Width * 2 + ResourcesUi.padding * 3));
        }
    }

    public static FontSystem FontSystem
    {
        get
        {    //  this is a stupid idea on my part buuuuuuuut, might aswell try new things
            if (_fontSystem == null)
            {
                _fontSystem = new FontSystem();
                _fontSystem.AddFont(System.IO.File.ReadAllBytes(@"Data/Fonts/PTC55F.ttf"));
            }
            return _fontSystem;
        }
        set => _fontSystem = value;
    }

    public void Draw()
    {
        foreach (var resource in ResourcesUi.resourceList)
        {
            resource.Draw();
        }
    }
}
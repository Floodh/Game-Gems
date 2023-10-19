

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Mineral : Building
{

    public enum Type
    {
        Blue = 0,
        Green = 1,
        Purple = 2,
        Orange = 3,
    }
    public readonly Type type;
    private static Texture2D[] baseTextures;

    public int quantity = 10000;

    public Mineral(Type type)
        : base(Faction.Neutral)
    {
        this.type = type;
        if (baseTextures == null)
        {
            baseTextures = TextureSource.LoadMinerals();

            //  swap r and g values for one texture
            Texture2D redTexture = baseTextures[0];
            Color[] data = new Color[redTexture.Width * redTexture.Height];
            redTexture.GetData(data);
            for (int y = 0; y < redTexture.Height; y++)
            for (int x = 0; x < redTexture.Width; x++)
            {
                int index = y * redTexture.Width + x;
                Color color = data[index];
                color = new Color(color.G, color.R, color.B, color.A);
                data[index] = color;
            }
            redTexture.SetData(data);
        }
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTextures[((int)this.type)], Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
        }
        base.Draw();
    }

    public override Building CreateNew()
    {
        return new Mineral(this.type);
    }

    public override void Tick()
    {
        if (this.quantity == 0)
            this.Die();
    }

    public override void UpdateByMouse(Microsoft.Xna.Framework.Input.MouseState mouseState)
    {
        Console.WriteLine("Mineral Mouse");
    }
}
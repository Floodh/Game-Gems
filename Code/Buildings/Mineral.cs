

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

            //  swap green and blue texture
            Texture2D blueTexture = baseTextures[((int)Type.Green)];
            baseTextures[((int)Type.Green)] = baseTextures[((int)Type.Blue)];
            baseTextures[((int)Type.Blue)] = blueTexture;
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

    public override void PlayerInteraction()
    {
        base.PlayerInteraction();

        switch (this.type)
        {
            case Type.Blue:
                Resources.Gain(1,0,0,0);
                break;
            case Type.Green:
                Resources.Gain(0,1,0,0);
                break;
            case Type.Purple:
                Resources.Gain(0,0,1,0);
                break;
            case Type.Orange:
                Resources.Gain(0,0,0,1);
                break;                                
        }

    }
}
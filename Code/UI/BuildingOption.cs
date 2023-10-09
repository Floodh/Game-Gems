using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;

class BuildingOption
{

    private readonly Color bgColor = Color.Green;

    private Point center;
    private Vector2 rotationCenter;
    private float rotationAngle;
    private Size bgSize;
    private Size fgSize;
    Texture2D bgTexture;
    Texture2D fgTexture;

    const float floatPI = (float)Math.PI;

    public Point Center { get => center; set => center = value; }


    public BuildingOption(string foregroundTexturePath)
    {
        this.bgSize = new Size(84, 84);
        this.fgSize = new Size(64, 64);

        this.bgTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/pentagon-128.png");
        this.fgTexture = Texture2D.FromFile(GameWindow.graphicsDevice, foregroundTexturePath);
    }

    public void UpdateByMouse(MouseState mouseState)
    {
        if(this.Bounds.Contains(mouseState.X, mouseState.Y))
        {
            // Console.WriteLine($"Hover-{this.Center.ToString()}");
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine($"Hit-{this.Center.ToString()}");
            }
        }     
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var rect =  new Rectangle(this.center.X, this.center.Y, 128, 128);
        GameWindow.spriteBatch.Draw(this.bgTexture, rect, null, Color.White, this.RotationAngle, new Vector2(this.bgTexture.Width / 2, this.bgTexture.Height / 2 + 155), SpriteEffects.None, 0f);

        // float angleInRadians = MathHelper.ToRadians(angle);
        float angle = this.RotationAngle - floatPI/2;
        var vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))*155;
        var fgTopLeftPoint = new Point(this.Center.X - this.fgSize.Width/2, this.Center.Y - this.fgSize.Height/2);
        var topLeftPoint = new Point(fgTopLeftPoint.X + (int)vec.X, fgTopLeftPoint.Y + (int)vec.Y);
        var fgDrawArea = new Rectangle(topLeftPoint.X, topLeftPoint.Y, this.fgSize.Width, this.fgSize.Height);
        GameWindow.spriteBatch.Draw(this.fgTexture,fgDrawArea, Sunlight.Mask);
    }
    
    public Rectangle Bounds
    {
        get
        {
            var bgTopLeftPoint = new Point(this.Center.X - this.bgSize.Width/2, this.Center.Y - this.bgSize.Height/2);
            return new Rectangle(bgTopLeftPoint.X, bgTopLeftPoint.Y, bgSize.Width, bgSize.Height);
        }
    }

    public Vector2 RotationCenter { get => rotationCenter; set => rotationCenter = value; }
    public float RotationAngle { get => rotationAngle; set => rotationAngle = value; }
}
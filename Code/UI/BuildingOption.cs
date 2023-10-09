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
    private float bgAngle;
    private float fgAngle;
    private Size bgSize;
    private Size fgSize;
    Texture2D bgTexture;
    Texture2D fgTexture;

    public Texture2D PlacementTexture
    {
        get
        {
            return this.fgTexture;
        }
    }

    const float floatPI = (float)Math.PI;

    private readonly BuildingSelector root;

    public Point Center { get => center; set => center = value; }

    private State state = new();

    public float Angle{
        get{
            return this.bgAngle;
        }
    }

    private class State
    {
        private bool highlight = false;

        public bool Highlight { get => highlight; set => highlight = value; }

        public Color Colour{
            get{
                if(!this.highlight)
                    return new Color(Color.White, 0.7f);
                else
                    return new Color(Color.White, 1f);
            }
        }
    }


    public BuildingOption(BuildingSelector root, string foregroundTexturePath, float angle)
    {
        this.root = root;
        this.bgAngle = angle;
        this.bgSize = new Size(160, 160);
        this.fgSize = new Size(128, 128);

        // float angleInRadians = MathHelper.ToRadians(angle);
        this.fgAngle = this.bgAngle - floatPI/2;
        var vec = new Vector2((float)Math.Cos(this.fgAngle), (float)Math.Sin(this.fgAngle))*185;
        this.center = root.Center + vec.ToPoint();

        this.bgTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/pentagon3_128.png");
        this.fgTexture = Texture2D.FromFile(GameWindow.graphicsDevice, foregroundTexturePath);
    }

    public void UpdateByMouse(MouseState mouseState)
    {
        if(GetDistance(this.center.X, this.center.Y, mouseState.X, mouseState.Y) < this.Radius )
        {
            this.root.SelectedItem = this;
            state.Highlight = true;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine($"Distance:{GetDistance(this.center.X, this.center.Y, mouseState.X, mouseState.Y)}, R:{this.Radius}");
                Console.WriteLine(state.Colour.ToString()) ;
            }
        }
        else
        {     
            state.Highlight = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var bgDrawArea =  new Rectangle(this.root.Center.X, this.root.Center.Y, this.bgSize.Width, this.bgSize.Height);
        GameWindow.spriteBatch.Draw(
            this.bgTexture, bgDrawArea, null, state.Colour, this.bgAngle, 
            new Vector2(this.bgTexture.Width / 2, this.bgTexture.Height / 2 + 155), SpriteEffects.None, 0f);

        var fgDrawArea =  new Rectangle(this.Center.X, this.Center.Y, this.fgSize.Width, this.fgSize.Height);
        GameWindow.spriteBatch.Draw(
            this.fgTexture, fgDrawArea, null, state.Colour, 0f, 
            new Vector2(this.fgTexture.Width / 2, this.fgTexture.Height / 2), SpriteEffects.None, 0f);
    }
    
    private static double GetDistance(double x1, double y1, double x2, double y2)
    {
    return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
    }

    public int Radius
    {
        get
        {
            return 70;
        }
    }
}
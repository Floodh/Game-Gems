using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;

class BuildingSelector
{

    // public delegate void UserPlacementOrder(Point selectedPoint, Building.Type building);
    // public UserPlacementOrder placementOrderResponse = null;

    // public BuildingSelector(Texture2D[] textures, Size windowSize)
    // {
    //     throw new NotImplementedException();
    // }

    private readonly Color bgColor = Color.Green;
    Texture2D centerTexture;

    private Point center;
    private Size displaySize;

    private Rectangle drawArea;

    private const string Path_Option1Texture = "Data/Texture/GreenGem.png";
    public int State = 0;
    List<BuildingOptionSprite> spriteList = new List<BuildingOptionSprite>();
    const float floatPI = (float)Math.PI;

    public BuildingSelector(Size displaySize)
    {
        this.displaySize = displaySize;
        this.center = new Point(this.displaySize.Width/2, this.displaySize.Height/2);
        this.centerTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/circle-128.png");

        this.spriteList.Add(new BuildingOptionSprite("Data/Texture/GreenGem.png"));
        this.spriteList.Last().Center = this.center;
        this.spriteList.Last().RotationAngle = -1.7f;

        this.spriteList.Add(new BuildingOptionSprite("Data/Texture/BlueGem.png"));
        this.spriteList.Last().Center = this.center;
        this.spriteList.Last().RotationAngle = -0.85f;

        this.spriteList.Add(new BuildingOptionSprite("Data/Texture/PurpleGem.png"));
        this.spriteList.Last().Center = this.center;
        this.spriteList.Last().RotationAngle = 0f;

        this.spriteList.Add(new BuildingOptionSprite("Data/Texture/GreenGem.png"));
        this.spriteList.Last().Center = this.center;
        this.spriteList.Last().RotationAngle = 0.85f;

        this.spriteList.Add(new BuildingOptionSprite("Data/Texture/BlueGem.png"));
        this.spriteList.Last().Center = this.center;
        this.spriteList.Last().RotationAngle = 1.7f;


    }

    public void UpdateByKeyboard(KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
        {
             this.State = 1;
        } 
        else{
            this.State = 0;
        }
    }

    public void UpdateByMouse(MouseState mouseState)
    {
        if(this.State <= 0)
            return;
            
        foreach(var sprite in this.spriteList)
        {
            sprite.UpdateByMouse(mouseState);
        }  
    }

    public void Update()
    {

   
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if( this.State > 0)
        {
            int segments = 5;
            var center = new Vector2(this.center.X, this.center.Y);
            float radius = 200f;
            int sides = 320;
            float startingAngle = floatPI;
            float endAngle = 2*floatPI;
            float radians = floatPI / segments;
            float thickness = 3.0f;

            Color color = this.bgColor;

            for(float angle = startingAngle; angle <= endAngle; angle+=radians)
            {
                DrawArc(spriteBatch, center, radius, sides, angle, radians, color, thickness);
                DrawArc(spriteBatch, center, 110f, sides, angle, radians, color, thickness);
                // Console.WriteLine($"Angle : {angle}, Color : {color}");

                if(color == Color.Green)
                    color = Color.Red;
                else
                    color = Color.Green;
            }

            ShapeUtils.DrawCircle(spriteBatch, center, 90f, sides, Color.Blue, thickness);

            foreach (var sprite in this.spriteList)
            {
                sprite.Draw(spriteBatch);
            }

            int size = 128;
            var topLeftPoint = new Point(this.center.X - size/2, this.center.Y - size/2);
            var drawArea = new Rectangle(topLeftPoint.X, topLeftPoint.Y, size, size);
            GameWindow.spriteBatch.Draw(this.centerTexture,drawArea, Sunlight.Mask);
        }
    }

    public static void DrawArc(SpriteBatch spriteBatch, Vector2 center, float radius, int sides, float startingAngle, float radians, Color color, float thickness)
    {
        List<Vector2> arc = ShapeUtils.CreateArc(radius, sides, startingAngle, radians);
        ShapeUtils.DrawPoints(spriteBatch, center, arc, color, thickness);
    }

    public void Update()
    {

   
    }
   public void Draw(SpriteBatch spriteBatch)
    {


        if( this.State > 0)
        {
            int segments = 5;
            var center = new Vector2(this.center.X, this.center.Y);
            float radius = 200f;
            int sides = 320;
            float startingAngle = floatPI;
            float endAngle = 2*floatPI;
            float radians = floatPI / segments;
            float thickness = 3.0f;
    
            Color color = this.bgColor;


            for(float angle = startingAngle; angle <= endAngle; angle+=radians)
            {
                DrawArc(spriteBatch, center, radius, sides, angle, radians, color, thickness);
                DrawArc(spriteBatch, center, 110f, sides, angle, radians, color, thickness);
                // Console.WriteLine($"Angle : {angle}, Color : {color}");

                if(color == Color.Green)
                    color = Color.Red;
                else
                    color = Color.Green;
            }

            ShapeUtils.DrawCircle(spriteBatch, center, 90f, sides, Color.Blue, thickness);
        }
    }

     public static void DrawArc(SpriteBatch spriteBatch, Vector2 center, float radius, int sides, float startingAngle, float radians, Color color, float thickness)
    {
        List<Vector2> arc = ShapeUtils.CreateArc(radius, sides, startingAngle, radians);
        ShapeUtils.DrawPoints(spriteBatch, center, arc, color, thickness);
    }

}
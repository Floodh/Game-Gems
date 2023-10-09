using System;
using System.Collections.Generic;
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

     private Point center;
    private Size displaySize;

    private Rectangle drawArea;

    public int State = 0;

    const float floatPI = (float)Math.PI;
    public BuildingSelector(Size displaySize)
    {
        this.displaySize = displaySize;
        this.center = new Point(this.displaySize.Width/2, this.displaySize.Height/2);
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
        throw new NotImplementedException();
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
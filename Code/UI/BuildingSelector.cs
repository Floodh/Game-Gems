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
    List<BuildingOption> spriteList = new List<BuildingOption>();
    const float floatPI = (float)Math.PI;

    public BuildingSelector(Size displaySize)
    {
        this.displaySize = displaySize;
        this.center = new Point(this.displaySize.Width/2, this.displaySize.Height/2);
        this.centerTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/circle-128.png");

        this.spriteList.Add(new BuildingOption("Data/Texture/GreenGem.png"));
        this.spriteList.Last().Center = this.center;
        this.spriteList.Last().RotationAngle = -1.7f;

        this.spriteList.Add(new BuildingOption("Data/Texture/BlueGem.png"));
        this.spriteList.Last().Center = this.center;
        this.spriteList.Last().RotationAngle = -0.85f;

        this.spriteList.Add(new BuildingOption("Data/Texture/PurpleGem.png"));
        this.spriteList.Last().Center = this.center;
        this.spriteList.Last().RotationAngle = 0f;

        this.spriteList.Add(new BuildingOption("Data/Texture/GreenGem.png"));
        this.spriteList.Last().Center = this.center;
        this.spriteList.Last().RotationAngle = 0.85f;

        this.spriteList.Add(new BuildingOption("Data/Texture/BlueGem.png"));
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
}
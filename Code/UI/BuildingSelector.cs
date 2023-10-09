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


    Texture2D centerTexture;
    Texture2D centerTexturePointing;

    private Point center;

    private Rectangle BuildingToPlaceRect;
    private Texture2D BuildingToPlaceTexture;
    private Size displaySize;

    private int? selectedIndex = null;
    private BuildingOption selectedItem = null;

    private const string Path_Option1Texture = "Data/Texture/GreenGem.png";
    public int State = 0;
    List<BuildingOption> spriteList = new List<BuildingOption>();

    public Point Center
    {
        get
        {
            return this.center;
        }
    }

    public int? SelectedIndex { get => selectedIndex; set => selectedIndex = value; }
    internal BuildingOption SelectedItem { get => selectedItem; set => selectedItem = value; }

    public BuildingSelector(Size displaySize)
    {
        this.displaySize = displaySize;
        this.center = new Point(this.displaySize.Width/2, this.displaySize.Height/2);
        this.centerTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/center3-null.png");
        this.centerTexturePointing = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/center3.png");

        this.spriteList.Add(new BuildingOption(this, "Data/Texture/GreenGem.png", -1.7f));
        this.spriteList.Add(new BuildingOption(this, "Data/Texture/BlueGem.png", -0.85f));
        this.spriteList.Add(new BuildingOption(this, "Data/Texture/PurpleGem.png", 0f));
        this.spriteList.Add(new BuildingOption(this, "Data/Texture/GreenGem.png", 0.85f));
        this.spriteList.Add(new BuildingOption(this, "Data/Texture/BlueGem.png", 1.7f));
    }

    public void UpdateByKeyboard(KeyboardState keyboardState)
    {
        if(this.State < 2)
        {
            if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
            {
                this.State = 1;
            } 
            else{
                this.State = 0;
            }
        }
        else
        {
            // Add escape handling
        }
    }

    private static bool selectingOption = false;

    public void UpdateByMouse(MouseState mouseState)
    {
        if(this.State <= 0)
            return;
            
        foreach(var sprite in this.spriteList)
        {
            sprite.UpdateByMouse(mouseState);
        }

        if(this.State == 1)
        {
            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                selectingOption = true;
            }

            if(selectingOption == true && mouseState.LeftButton == ButtonState.Released)
            {
                if(this.selectedItem != null)
                {
                    this.BuildingToPlaceTexture = this.selectedItem.PlacementTexture;
                    this.State = 2;
                }
                else
                {
                    this.State = 0;
                }
                selectingOption = false;
            }
        } 
        else if(this.State == 2)
        {
            this.BuildingToPlaceRect = new Rectangle(
                        mouseState.X, mouseState.Y, this.BuildingToPlaceTexture.Width, this.BuildingToPlaceTexture.Height);

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                selectingOption = true;
            }

            if(selectingOption == true &&  mouseState.LeftButton == ButtonState.Released)
            {
                // TODO Send build data
                this.State = 0;
            }

            if(mouseState.RightButton == ButtonState.Pressed)
            {
                this.State = 0;
            }
        }
        // Console.WriteLine($"State:{this.State}, LB:{mouseState.LeftButton.ToString()}");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if(this.State <= 0)
            return;

        if( this.State == 1)
        {
            // Draw the options
            foreach (var sprite in this.spriteList)
            {
                sprite.Draw(spriteBatch);
            }

            // Draw center "knob"
            int size = 200;
            var drawArea =  new Rectangle(this.Center.X, this.Center.Y, size, size);
            if(this.selectedItem == null)         
            {
                GameWindow.spriteBatch.Draw(
                    this.centerTexture, drawArea, null, new Color(Color.White, 0.9f), 0f, 
                    new Vector2(this.centerTexture.Width / 2, this.centerTexture.Height / 2), SpriteEffects.None, 0f);
            }
            else
            {
                GameWindow.spriteBatch.Draw(
                    this.centerTexturePointing, drawArea, null, new Color(Color.White, 0.9f), this.selectedItem.Angle, 
                    new Vector2(this.centerTexturePointing.Width / 2, this.centerTexturePointing.Height / 2), SpriteEffects.None, 0f);
            }
            this.selectedItem = null; // Otherwise the last selected item will be pointed at forever
        }
        else if(this.State == 2)
        {
            // TODO adjust to map view
            GameWindow.spriteBatch.Draw(
                    this.BuildingToPlaceTexture, this.BuildingToPlaceRect, null, new Color(Color.White, 0.3f), 0f, 
                    new Vector2(this.BuildingToPlaceTexture.Width / 2, this.BuildingToPlaceTexture.Height / 2), SpriteEffects.None, 0f);
        }

        
    }

}
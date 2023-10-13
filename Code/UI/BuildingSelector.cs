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

    public enum EState 
    {
        NotVisible = 0,
        Visible = 1,
        PlacementPending = 2
    }   


    Texture2D centerTexture;
    Texture2D centerTexturePointing;

    private Point center;

    private Rectangle BuildingToPlaceRect;
    private Texture2D BuildingToPlaceTexture;

    private Building buildingToPlace;

    private Size displaySize;

    private int? selectedIndex = null;
    private BuildingOption selectedItem = null;

    private const string Path_Option1Texture = "Data/Texture/GreenGem.png";
    public EState State = EState.NotVisible;
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
        this.spriteList.Add(new BuildingOption(this, "Data/Texture/RedGem.png", 0.85f));
        this.spriteList.Add(new BuildingOption(this, "Data/Texture/GreenGem.png", 1.7f));
    }

    public void UpdateByKeyboard(KeyboardState keyboardState)
    {
        if(this.State == EState.NotVisible || this.State == EState.Visible)
        {
            if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
            {
                this.State = EState.Visible;
            } 
            else{
                this.State = EState.NotVisible;
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
        if(this.State == EState.NotVisible)
            return;
            
        foreach(var sprite in this.spriteList)
        {
            sprite.UpdateByMouse(mouseState);
        }

        if(this.State == EState.Visible)
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
                    this.buildingToPlace = new Cannon(); // TODO fix generic
                    this.State = EState.PlacementPending;
                }
                else
                {
                    this.State = EState.NotVisible;
                }
                selectingOption = false;
            }
        } 
        else if(this.State == EState.PlacementPending)
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
                selectingOption = false;
                this.State = EState.NotVisible;
            }

            if(mouseState.RightButton == ButtonState.Pressed)
            {
                selectingOption = false;
                this.State = EState.NotVisible;
            }
        }
        // Console.WriteLine($"State:{this.State}, LB:{mouseState.LeftButton.ToString()}");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if(this.State == EState.NotVisible)
            return;

        if( this.State == EState.Visible)
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
        else if(this.State == EState.PlacementPending)
        {     
            Vector2 worldCenterVec = Camera.ScreenToWorld(new Vector2(this.BuildingToPlaceRect.Center.X, this.BuildingToPlaceRect.Center.Y));
            Rectangle worldRect = new(worldCenterVec.ToPoint().X, worldCenterVec.ToPoint().Y, this.BuildingToPlaceTexture.Width, this.BuildingToPlaceTexture.Height);

            // Grid based positioning (for placement)
            Point gridCenterPoint = Grid.WorldToGrid(worldCenterVec.ToPoint());
            Rectangle gridRect = new(gridCenterPoint.X, gridCenterPoint.Y, 1, 1);
            bool canplace = Building.grid.CanPlace(gridRect);

            if(canplace) // TODO move this to mouseclick for placement
            {
                this.buildingToPlace.Place(gridCenterPoint);
                this.buildingToPlace = new Cannon();
            }

            // Draw by Camera/Screen based positioning (remove when debugging done)
            GameWindow.spriteBatch.Draw(
                    this.BuildingToPlaceTexture, this.BuildingToPlaceRect, null, new Color(Color.White, 0.3f), 0f, 
                    new Vector2(this.BuildingToPlaceTexture.Width / 2, this.BuildingToPlaceTexture.Height / 2), SpriteEffects.None, 0f);

            // Draw by World based positioning
            float intensity = canplace?1f:0.5f;
            GameWindow.spriteBatch.Draw(
                    this.BuildingToPlaceTexture, worldRect, null, new Color(Color.White, intensity), 0f, 
                    new Vector2(this.BuildingToPlaceTexture.Width / 2, this.BuildingToPlaceTexture.Height / 2), SpriteEffects.None, 0f);

            Console.WriteLine($"screen:{this.BuildingToPlaceRect.Center.ToString()}, world:{worldCenterVec.ToString()}, grid:{gridCenterPoint.ToString()}"); 
            Console.WriteLine($"place:{canplace}"); 
        }    
    }

}
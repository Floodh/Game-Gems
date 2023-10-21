using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;

class BuildingSelector
{
    public enum EState 
    {
        NotVisible = 0,
        Visible = 1,
        PlacementPending = 2
    }   

    Rectangle worldRect;
    bool canplace = false;

    Texture2D centerTexture;
    Texture2D centerTexturePointing;

    private Point center;

    private Texture2D BuildingToPlaceTexture;

    private Building buildingToPlace;

    private Size displaySize;

    private int? selectedIndex = null;
    private BuildingOption selectedItem = null;

    public EState State = EState.NotVisible;
    List<BuildingOption> spriteList = new List<BuildingOption>();

    public BuildingSelector(Size displaySize)
    {
        this.displaySize = displaySize;
        this.center = new Point(this.displaySize.Width/2, this.displaySize.Height/2);
        this.centerTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/center3-null.png");
        this.centerTexturePointing = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/center3.png");

        this.spriteList.Add(new BuildingOption(this, new Cannon(), "Data/Texture/GemStructure/Purple_0.png", -1.7f));
        this.spriteList.Add(new BuildingOption(this, new Generator(), "Data/Texture/GemStructure/Orange_0.png", -0.85f));
        this.spriteList.Add(new BuildingOption(this, new Healer(), "Data/Texture/GemStructure/Green_0.png", 0f));
        this.spriteList.Add(new BuildingOption(this, new Wall(), "Data/Texture/GemStructure/Blue_0.png", 0.85f));
        this.spriteList.Add(new BuildingOption(this, new Wall(), "Data/Texture/GemStructure/Blue_0.png", 1.7f));
    }

    public Point Center
    {
        get
        {
            return this.center;
        }
    }

    public int? SelectedIndex { get => selectedIndex; set => selectedIndex = value; }
    internal BuildingOption SelectedItem { get => selectedItem; set => selectedItem = value; }

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
        // Currently Escape quites the program
        // if(this.State == EState.PlacementPending  && keyboardState.IsKeyDown(Keys.Escape))
        // {
        //     selectingOption = false;
        //     this.State = EState.NotVisible;
        // }
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
                    this.buildingToPlace = this.selectedItem.Building;
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
            Vector2 mouseVec = new(mouseState.X, mouseState.Y);
            Vector2 worldCenterVec = Camera.ScreenToWorld(mouseVec);
            Vector2 worldTopLeftVec = worldCenterVec - new Vector2(this.BuildingToPlaceTexture.Width / 2, this.BuildingToPlaceTexture.Height / 2);
            this.worldRect = new(worldTopLeftVec.ToPoint().X, worldTopLeftVec.ToPoint().Y, this.BuildingToPlaceTexture.Width, this.BuildingToPlaceTexture.Height);
            Point gridCenterPoint = Grid.WorldToGrid(worldCenterVec.ToPoint());
            Rectangle gridRect = new(gridCenterPoint.X, gridCenterPoint.Y, 2, 2); // Asuming build size 2 atm.
            this.canplace = Building.grid.CanPlace(gridRect);

            Map.hightlightGridArea = gridRect;  //  in order to highlight valid tiles

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                selectingOption = true;
            }

            if(selectingOption == true &&  mouseState.LeftButton == ButtonState.Released)
            {
                if(this.canplace)
                {
                    this.buildingToPlace.CreateNew().Place(gridCenterPoint);
                    this.buildingToPlace = null;
                    selectingOption = false;
                    this.selectedItem = null;
                    this.State = EState.NotVisible;
                    Map.hightlightGridArea = Rectangle.Empty;   //  to stop higlighting areas
                }
            }

            if(mouseState.RightButton == ButtonState.Pressed)
            {
                selectingOption = false;
                this.State = EState.NotVisible;
            }
        }
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
            float intensity = this.canplace?1f:0.5f;
            GameWindow.spriteBatch.Draw(this.BuildingToPlaceTexture, Camera.ModifiedDrawArea(this.worldRect, Camera.zoomLevel), new Color(Color.White, intensity));
            // Console.WriteLine($"Rect:{this.worldRect}, Can place:{this.canplace}"); 
        }    
    }
}
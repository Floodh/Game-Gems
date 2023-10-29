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
    Texture2D blankTexture;
    private Point center;
    private Texture2D BuildingToPlaceTexture;
    private BuildingOption.BuildingDelegate buildingToPlace;
    private Size displaySize;

    private BuildingOption selectedItem = null;

    public EState State = EState.NotVisible;
    List<BuildingOption> spriteList = new List<BuildingOption>();

    public BuildingSelector(Size displaySize)
    {
        this.displaySize = displaySize;
        this.center = new Point(this.displaySize.Width/2, this.displaySize.Height/2);
        this.centerTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/center3-null.png");
        this.centerTexturePointing = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/center3.png");

        this.blankTexture = new Texture2D(GameWindow.graphicsDevice, 1, 1);
        this.blankTexture.SetData(new Color[] { Color.White });

        this.spriteList.Add(new BuildingOption(
            this, Healer.CreateNew, Healer.GetTextures, Healer.GetRectangle, 0f, "Data/TextureSources/healing-tower1-tier1.png"));
        this.spriteList.Add(new BuildingOption(
            this, Generator.CreateNew, Generator.GetTextures, Generator.GetRectangle, -0.85f, "Data/TextureSources/energy-tower1-tier1.png"));
        this.spriteList.Add(new BuildingOption(
            this, Booster.CreateNew, Booster.GetTextures, Booster.GetRectangle, 0.85f, "Data/TextureSources/income-tower3-tier1.png"));
        this.spriteList.Add(new BuildingOption(
            this, Cannon.CreateNew, Cannon.GetTextures, Cannon.GetRectangle, -1.7f, "Data/TextureSources/attack-tower1-tier1.png"));
        this.spriteList.Add(new BuildingOption(
            this, Wall.CreateNew, Wall.GetTextures, Wall.GetRectangle, 1.7f, "Data/TextureSources/walls2-menu2.png"));       
    }

    public Point Center
    {
        get
        {
            return this.center;
        }
    }
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
                    this.buildingToPlace = this.selectedItem._buildingCallback;
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
            this.worldRect = new(
                worldTopLeftVec.ToPoint().X + this.BuildingToPlaceTexture.Width/2, 
                worldTopLeftVec.ToPoint().Y+this.BuildingToPlaceTexture.Height/2-64+8-32, 
                this.BuildingToPlaceTexture.Width / 6, this.BuildingToPlaceTexture.Height / 6);

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
                    Building building = this.buildingToPlace();
                    building.Place(gridCenterPoint);
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
            Rectangle hightlightGridArea = Map.hightlightGridArea;
            int mapPixelToTexturePixel_Multiplier = Map.mapPixelToTexturePixel_Multiplier;

            if (hightlightGridArea != Rectangle.Empty)
            {  
                DrawGridAvailability(hightlightGridArea, mapPixelToTexturePixel_Multiplier);
                DrawBuildingRangeCoverage(hightlightGridArea, mapPixelToTexturePixel_Multiplier);
                DrawGridSnappedBuilding(hightlightGridArea, mapPixelToTexturePixel_Multiplier);   
            }
            // Mouse follow
            // GameWindow.spriteBatch.Draw(
            //     this.BuildingToPlaceTexture, Camera.ModifiedDrawArea(this.worldRect, Camera.zoomLevel), col*0.4f);
        }    
    }

    private void DrawGridAvailability(Rectangle hightlightGridArea, int mapPixelToTexturePixel_Multiplier)
    {
        for (int y = hightlightGridArea.Y; y < hightlightGridArea.Bottom; y++)
        {
            for (int x = hightlightGridArea.X; x < hightlightGridArea.Right; x++)
            {         
                Rectangle tileArea = new Rectangle(x * mapPixelToTexturePixel_Multiplier, y * mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier);
                tileArea = Camera.ModifiedDrawArea(tileArea, Camera.zoomLevel);
                if (Building.grid.IsTileTaken(x, y))
                {
                    GameWindow.spriteBatch.Draw(this.blankTexture, tileArea, Color.Red * 0.4f);
                }
                else
                {
                    GameWindow.spriteBatch.Draw(this.blankTexture, tileArea, Color.Green * 0.4f);
                }
            }
        }
    }

    private void DrawBuildingRangeCoverage(Rectangle hightlightGridArea, int mapPixelToTexturePixel_Multiplier)
    {
        int range = 8; // TODO Fetch from building
        Point point = hightlightGridArea.Location;
 
        for(int y = point.Y-range; y < point.Y+range; y+=2)
        {
            for(int x = point.X-range; x < point.X+range; x+=2)
            {       
                if(inside_circle(point, new Point(x, y), range))
                {
                    Rectangle rect = new (
                        (x) * mapPixelToTexturePixel_Multiplier, 
                        (y) * mapPixelToTexturePixel_Multiplier, 
                        mapPixelToTexturePixel_Multiplier*2, 
                        mapPixelToTexturePixel_Multiplier*2);
                    GameWindow.spriteBatch.Draw(this.blankTexture, Camera.ModifiedDrawArea(rect, Camera.zoomLevel), Color.LightBlue * 0.3f);
                }
            }
        }
    }

    private void DrawGridSnappedBuilding(Rectangle hightlightGridArea, int mapPixelToTexturePixel_Multiplier)
    {
        Color color = this.canplace?Color.Green:Color.Red;

        Point point = new (hightlightGridArea.X * mapPixelToTexturePixel_Multiplier, hightlightGridArea.Y * mapPixelToTexturePixel_Multiplier);
        Rectangle rect = this.selectedItem._rectCallback(point);

        GameWindow.spriteBatch.Draw(
            this.BuildingToPlaceTexture, Camera.ModifiedDrawArea(rect, Camera.zoomLevel), color * 0.4f);
    }

    private bool inside_circle(Point center, Point tile, int diameter)
    {
        int dx = center.X - tile.X,
            dy = center.Y - tile.Y;
        int distance_squared = dx*dx + dy*dy;
        return 4 * distance_squared <= diameter*diameter;
    }
}
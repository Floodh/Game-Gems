using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;
using FontStashSharp;
// using System.Numerics;

class BuildingSelector
{
    public enum EState
    {
        NotVisible = 0,
        Visible = 1,
        PlacementPending = 2
    }

    // Rectangle worldRect;
    bool canplace = false;
    Texture2D centerTexture;
    Texture2D centerTextureInfo;
    Texture2D centerTexturePointing;
    Texture2D gridValidTexture;
    Texture2D gridInvalidTexture;
    Texture2D blankTexture;
    private Point center;
    private Texture2D BuildingToPlaceTexture;
    private BuildingOption.BuildingDelegate buildingToPlace;
    private BuildingOption _hoverOverBuildingOption = null;
    private Size displaySize;
    private readonly int fonstSize = 18;

    private BuildingOption selectedItem = null;

    public EState State = EState.NotVisible;
    List<BuildingOption> _buildingOptions = new List<BuildingOption>();

    public BuildingSelector(Size displaySize)
    {
        this.displaySize = displaySize;
        this.center = new Point(this.displaySize.Width / 2, this.displaySize.Height / 2);
        this.centerTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/center3-null.png");
        this.centerTextureInfo = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/build-menu-info-1.png");
        this.centerTexturePointing = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/center3.png");
        this.gridValidTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/Grid_ValidTile.png");
        this.gridInvalidTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/Grid_InValidTile.png");
        this.blankTexture = new Texture2D(GameWindow.graphicsDevice, 1, 1);
        this.blankTexture.SetData(new Color[] { Color.White });

        this._buildingOptions.Add(new BuildingOption(
            this, Healer.Buy, Healer.GetTextures, Healer.GetRectangle, 0f,
            Healer.costs[0], "Healer", "Data/Texture/Buildings/healing-tower1-tier1.png"));
        this._buildingOptions.Add(new BuildingOption(
            this, Generator.Buy, Generator.GetTextures, Generator.GetRectangle, -0.85f,
            Generator.costs[0], "Generator", "Data/Texture/Buildings/energy-tower1-tier1.png"));
        this._buildingOptions.Add(new BuildingOption(
            this, Booster.Buy, Booster.GetTextures, Booster.GetRectangle, 0.85f,
            Booster.costs[0], "Upgrades", "Data/Texture/Buildings/income-tower3-tier1.png"));
        this._buildingOptions.Add(new BuildingOption(
            this, Cannon.Buy, Cannon.GetTextures, Cannon.GetRectangle, -1.7f,
            Cannon.costs[0], "Cannon", "Data/Texture/Buildings/attack-tower1-tier1.png"));
        this._buildingOptions.Add(new BuildingOption(
            this, Wall.Buy, Wall.GetTextures, Wall.GetRectangle, 1.7f,
            Wall.costs[0], "Wall", "Data/Texture/UI/walls2-menu2.png"));

        foreach (BuildingOption bo in this._buildingOptions)
            bo.OnOver += BuildingOption_OnOver;

    }

    private void BuildingOption_OnOver(BuildingOption sender)
    {
        _hoverOverBuildingOption = sender;
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
        if (this.State == EState.NotVisible || this.State == EState.Visible)
        {
            if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
            {
                this.State = EState.Visible;
            }
            else
            {
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
        if (this.State == EState.NotVisible)
            return;

        _hoverOverBuildingOption = null;

        foreach (var buildingOption in this._buildingOptions)
        {
            buildingOption.Update(mouseState);
        }

        if (this.State == EState.Visible)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                selectingOption = true;
            }

            if (selectingOption == true && mouseState.LeftButton == ButtonState.Released)
            {
                if (this.selectedItem != null)
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
        else if (this.State == EState.PlacementPending)
        {
            Vector2 worldCenterVec = InputManager.WorldMousePosition; ;
            Vector2 worldTopLeftVec = worldCenterVec - new Vector2(this.BuildingToPlaceTexture.Width / 2, this.BuildingToPlaceTexture.Height / 2);

            Point gridCenterPoint = Grid.WorldToGrid(worldCenterVec.ToPoint());
            Rectangle gridRect = new(gridCenterPoint.X, gridCenterPoint.Y, 2, 2); // Asuming build size 2 atm.
            this.canplace = Building.grid.CanPlace(gridRect);

            Map.hightlightGridArea = gridRect;  //  in order to highlight valid tiles

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                selectingOption = true;
            }

            if (selectingOption == true && mouseState.LeftButton == ButtonState.Released)
            {
                if (this.canplace)
                {
                    Building building = this.buildingToPlace();
                    if (building != null)
                        building.Place(gridCenterPoint);
                    this.buildingToPlace = null;
                    this.selectedItem = null;
                    this.State = EState.NotVisible;
                    Map.hightlightGridArea = Rectangle.Empty;   //  to stop higlighting areas
                }

                selectingOption = false;
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                selectingOption = false;
                this.State = EState.NotVisible;
            }
        }
    }

    public void Draw()
    {
        if (this.State == EState.NotVisible)
            return;

        if (this.State == EState.Visible)
        {
            DrawBuildingOptions();
            DrawCenterKnob();
            DrawCostInformation();
            this.selectedItem = null; // Otherwise the last selected item will be pointed at forever
        }
        else if (this.State == EState.PlacementPending)
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
            //     this.BuildingToPlaceTexture, this.worldRect, col*0.4f);
        }
    }

    private void DrawBuildingOptions()
    {
        foreach (var buildingOption in this._buildingOptions)
        {
            buildingOption.Draw();
        }
    }

    private void DrawCenterKnob()
    {
        int size = 200;
        var drawArea = new Rectangle(this.Center.X, this.Center.Y, size, size);
        if (this.selectedItem == null)
        {
            GameWindow.spriteBatchUi.Draw(
                this.centerTexture, drawArea, null, new Color(Color.White, 0.9f), 0f,
                new Vector2(this.centerTexture.Width / 2, this.centerTexture.Height / 2), SpriteEffects.None, 0f);
        }
        else
        {
            GameWindow.spriteBatchUi.Draw(
                this.centerTexturePointing, drawArea, null, new Color(Color.White, 0.9f), this.selectedItem.Angle,
                new Vector2(this.centerTexturePointing.Width / 2, this.centerTexturePointing.Height / 2), SpriteEffects.None, 0f);
        }
    }

    private void DrawCostInformation()
    {
        if (_hoverOverBuildingOption == null)
            return;

        Rectangle infoRect = new(this.Center.X, this.Center.Y, (int)(this.centerTextureInfo.Width * 0.4), (int)(this.centerTextureInfo.Height * 0.4));
        Vector2 infoVec = new(this.centerTextureInfo.Width / 2, this.centerTextureInfo.Height / 2 - 77);
        GameWindow.spriteBatchUi.Draw(this.centerTextureInfo, infoRect, null, Color.White, 0f, infoVec, SpriteEffects.None, 0f);

        Vector2 textVec = this.Center.ToVector2();
        SpriteFontBase font = ResourcesUi.FontSystem.GetFont(fonstSize);
        Vector2 fontVec = new(font.MeasureString(_hoverOverBuildingOption.BuildingName).Length() / 2, fonstSize / 2);
        textVec = textVec - fontVec;
        GameWindow.spriteBatchUi.DrawString(font, _hoverOverBuildingOption.BuildingName, textVec, Color.Black);


        Vector2 costVec;
        Vector2 topLeftVec = new(-30, 30);
        Vector2 bottomLeftVec = new(-30, 60);
        Vector2 topRightVec = new(30, 30);
        Vector2 bottomRightVec = new(30, 60);

        fontVec = new(font.MeasureString(_hoverOverBuildingOption.Cost.blue.ToString()).Length() / 2, fonstSize / 2);
        costVec = this.center.ToVector2() + topLeftVec - fontVec;
        GameWindow.spriteBatchUi.DrawString(font, _hoverOverBuildingOption.Cost.blue.ToString(), costVec, Color.Black);

        fontVec = new(font.MeasureString(_hoverOverBuildingOption.Cost.purple.ToString()).Length() / 2, fonstSize / 2);
        costVec = this.center.ToVector2() + bottomLeftVec - fontVec;
        GameWindow.spriteBatchUi.DrawString(font, _hoverOverBuildingOption.Cost.purple.ToString(), costVec, Color.Black);

        fontVec = new(font.MeasureString(_hoverOverBuildingOption.Cost.green.ToString()).Length() / 2, fonstSize / 2);
        costVec = this.center.ToVector2() + topRightVec - fontVec;
        GameWindow.spriteBatchUi.DrawString(font, _hoverOverBuildingOption.Cost.green.ToString(), costVec, Color.Black);

        fontVec = new(font.MeasureString(_hoverOverBuildingOption.Cost.orange.ToString()).Length() / 2, fonstSize / 2);
        costVec = this.center.ToVector2() + bottomRightVec - fontVec;
        GameWindow.spriteBatchUi.DrawString(font, _hoverOverBuildingOption.Cost.orange.ToString(), costVec, Color.Black);
    }

    private void DrawGridAvailability(Rectangle hightlightGridArea, int mapPixelToTexturePixel_Multiplier)
    {
        for (int y = hightlightGridArea.Y; y < hightlightGridArea.Bottom; y++)
        {
            for (int x = hightlightGridArea.X; x < hightlightGridArea.Right; x++)
            {
                Rectangle tileArea = new Rectangle(x * mapPixelToTexturePixel_Multiplier, y * mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier);
                if (Building.grid.IsTileTaken(x, y))
                {
                    // GameWindow.spriteBatch.Draw(this.blankTexture, tileArea, Color.Red * 0.4f);
                    GameWindow.spriteBatch.Draw(this.gridInvalidTexture, tileArea, Color.Red * 0.4f);
                }
                else
                {
                    // GameWindow.spriteBatch.Draw(this.blankTexture, tileArea, Color.Green * 0.4f);
                    GameWindow.spriteBatch.Draw(this.gridValidTexture, tileArea, Color.Green * 0.4f);
                }
            }
        }
    }

    private void DrawBuildingRangeCoverage(Rectangle hightlightGridArea, int mapPixelToTexturePixel_Multiplier)
    {
        int range = 8; // TODO Fetch from building
        Point point = hightlightGridArea.Location;

        for (int y = point.Y - range; y < point.Y + range; y += 2)
        {
            for (int x = point.X - range; x < point.X + range; x += 2)
            {
                if (inside_circle(point, new Point(x, y), range))
                {
                    Rectangle rect = new(
                        (x) * mapPixelToTexturePixel_Multiplier,
                        (y) * mapPixelToTexturePixel_Multiplier,
                        mapPixelToTexturePixel_Multiplier * 2,
                        mapPixelToTexturePixel_Multiplier * 2);
                    GameWindow.spriteBatch.Draw(this.blankTexture, rect, Color.LightBlue * 0.3f);
                }
            }
        }
    }

    private void DrawGridSnappedBuilding(Rectangle hightlightGridArea, int mapPixelToTexturePixel_Multiplier)
    {
        Color color = this.canplace ? Color.Green : Color.Red;

        Point point = new(hightlightGridArea.X * mapPixelToTexturePixel_Multiplier, hightlightGridArea.Y * mapPixelToTexturePixel_Multiplier);
        Rectangle rect = this.selectedItem._rectCallback(point);

        GameWindow.spriteBatch.Draw(
            this.BuildingToPlaceTexture, rect, color * 0.4f);
    }

    private bool inside_circle(Point center, Point tile, int diameter)
    {
        int dx = center.X - tile.X,
            dy = center.Y - tile.Y;
        int distance_squared = dx * dx + dy * dy;
        return 4 * distance_squared <= diameter * diameter;
    }
}
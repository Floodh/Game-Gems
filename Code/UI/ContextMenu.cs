using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;

class ContextMenu
{
    Texture2D menuTexture;
    Texture2D menuTextureBoost;
    private Building _building;
    private Booster _boosterBuilding;
    public static FontSystem FontSystem { get => _fontSystem; set => _fontSystem = value; }
    internal Building Building { get => _building; set => _building = value; }
    private static FontSystem _fontSystem;
    Vector2 menuVec;
    Vector2 topMiddleVec;
    Rectangle menuRect;
    protected bool mousePressed = false;
    private Resources _cost;

    private readonly List<UpgradeButton> _buttons = new();
    private readonly List<UpgradeButton> _buttons_boost = new();


    public ContextMenu()
    {
        FontSystem = new FontSystem();
        FontSystem.AddFont(System.IO.File.ReadAllBytes(@"Data/Fonts/PTC55F.ttf"));
        this.menuTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/UpgradeBuilding/upgrade-building.png");
        this.menuTextureBoost = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/UpgradeBuilding/upgrade-boost-building.png");

        UpgradeButton button;
        button = this.AddButton(new Vector2(0, 0));
        button.OnClick += UpgradeBuilding;
        button.OnOver += OverBuilding;

        button = this.AddBoostButton(new Vector2(0, 390/2));
        button.OnClick += UpgradeAllGems;
        button.OnOver += OverAllGems;

        button = this.AddBoostButton(new Vector2(-154/2, 298/2));
        button.OnClick += UpgradeBlueGem;
        button.OnOver += OverBlueGems;

        button = this.AddBoostButton(new Vector2(-51/2, 298/2));
        button.OnClick += UpgradeGreenGem;
        button.OnOver += OverGreenGems;

        button = this.AddBoostButton(new Vector2(52/2, 298/2));
        button.OnClick += UpgradePurpleGem;
        button.OnOver += OverPurpleGems;

        button = this.AddBoostButton(new Vector2(157/2, 298/2));
        button.OnClick += UpgradeOrangeGem;
        button.OnOver += OverOrangeGems;
    }

    public UpgradeButton AddButton(Vector2 vector)
    {
        UpgradeButton button = new(vector);
        _buttons.Add(button);
        return button;
    }

    public UpgradeButton AddBoostButton(Vector2 vector)
    {
        UpgradeButton button = new(vector);
        _buttons_boost.Add(button);
        return button;
    }

    public void OverBuilding(object sender, EventArgs e)
    {
        _cost = (this._building as UpgradeableBuilding).GetUpgradeCost();
    }

    public void UpgradeBuilding(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradeBuilding");
        var upgradableBuilding = this._building as UpgradeableBuilding;
        bool result = upgradableBuilding.TryUpgrade();
    }

    public void OverAllGems(object sender, EventArgs e)
    {  
        _cost = new Resources(1, 1, 1, 1);
    }

    public void UpgradeAllGems(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradeAllGems");
    }

    public void OverBlueGems(object sender, EventArgs e)
    { 
        _cost = new Resources(1, 0, 0, 0);
    }

    public void UpgradeBlueGem(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradeBlueGem");
    }

    public void OverGreenGems(object sender, EventArgs e)
    { 
        _cost = new Resources(0, 1, 0, 0);
    }

    public void UpgradeGreenGem(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradeGreenGem");
    }

    public void OverPurpleGems(object sender, EventArgs e)
    { 
        _cost = new Resources(0, 0, 1, 0);
    }

    public void UpgradePurpleGem(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradePurpleGem");
    }

    public void OverOrangeGems(object sender, EventArgs e)
    {  
        _cost = new Resources(0, 0, 0, 1);
    }

    public void UpgradeOrangeGem(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradeOrangeGem");
    }

    public bool Update(MouseState mouseState)
    {
        this.Building = Building.selectedBuilding;

        if(this._building != null && this._building.State == Building.EState.Selected)
        {
            _boosterBuilding = this._building as Booster;
            _cost = new Resources(0, 0, 0, 0);
            
            int yMenuOffset;
            int yButtonOffset;
            Size textureSize;
            if(_boosterBuilding == null)
            {
                yMenuOffset = 0;
                yButtonOffset = 48;
                textureSize = new Size(this.menuTexture.Width/2, this.menuTexture.Height/2);
            }
            else
            {
                yMenuOffset = 0;
                yButtonOffset = 16+1-3;
                textureSize = new Size(this.menuTextureBoost.Width/2, this.menuTextureBoost.Height/2);
            }

            Rectangle menuRect = new(_building.DrawArea.X +64, _building.DrawArea.Y +64 +yMenuOffset, 1, 1);
            menuRect = Camera.ModifiedDrawArea(menuRect, Camera.zoomLevel);

            this.menuVec = new(menuRect.Left - textureSize.Width/2, menuRect.Top - textureSize.Height/2);
            this.menuRect = new(menuVec.ToPoint().X, menuVec.ToPoint().Y, textureSize.Width, textureSize.Height);

            if(this.menuRect.Contains(mouseState.Position))
            {   
                Rectangle buttonOriginRect = new(_building.DrawArea.X + 64, _building.DrawArea.Y +yButtonOffset, 1, 1);
                buttonOriginRect = Camera.ModifiedDrawArea(buttonOriginRect, Camera.zoomLevel);

                foreach(UpgradeButton button in _buttons)
                {
                    button.Update(mouseState, buttonOriginRect.Location.ToVector2());               
                } 

                if(_boosterBuilding != null)
                {
                    foreach(UpgradeButton button in _buttons_boost)
                    {
                        button.Update(mouseState, buttonOriginRect.Location.ToVector2());               
                    }
                }

                return true;
            }
        }
        return false;
    }

    public void Draw()
    {
        if(_building != null && _building.State == Building.EState.Selected)
        {
            var upgradableBuilding = _building as UpgradeableBuilding;
            if(upgradableBuilding.Tier >= upgradableBuilding.MaxTier)
                return;

            Vector2 menuVec = this.menuVec;
            Texture2D texture = _boosterBuilding == null?this.menuTexture:this.menuTextureBoost;

            var blankTexture = new Texture2D(GameWindow.graphicsDevice, 1, 1);
                blankTexture.SetData(new Color[] { Color.White });
                GameWindow.spriteBatch.Draw(
                    blankTexture, this.menuRect, null, new Color(Color.White, 1f)*0.5f, 0f, 
                    new Vector2(0, 0), SpriteEffects.None, 0f);

            GameWindow.spriteBatch.Draw(
                texture, this.menuRect, null, new Color(Color.White, 1f), 0f, 
                new Vector2(0, 0), SpriteEffects.None, 0f);


            SpriteFontBase font18 = ResourcesUi.FontSystem.GetFont(18);
            menuVec = menuVec + new Vector2(40, 79);
            GameWindow.spriteBatch.DrawString(font18, _cost.blue.ToString(), menuVec, Color.Black);
            menuVec = menuVec + new Vector2(108, 0);
            GameWindow.spriteBatch.DrawString(font18, _cost.green.ToString(), menuVec, Color.Black); 

            menuVec = menuVec + new Vector2(-108, 42);
            GameWindow.spriteBatch.DrawString(font18, _cost.purple.ToString(), menuVec, Color.Black);
            menuVec = menuVec + new Vector2(108, 0);
            GameWindow.spriteBatch.DrawString(font18, _cost.orange.ToString(), menuVec, Color.Black);

            foreach(UpgradeButton button in _buttons)
            {
                button.Draw();
            }

            if(_boosterBuilding != null)
            {
                foreach(UpgradeButton button in _buttons_boost)
                {
                    button.Draw();
                }
            }
        }
    }

   
}
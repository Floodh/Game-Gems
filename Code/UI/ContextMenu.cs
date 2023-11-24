using System;
using System.Collections.Generic;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


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
        button = this.AddButton(new Vector2(0, -26));
        button.OnClick += UpgradeBuilding;
        button.OnOver += OverBuilding;

        int offset = -88;
        button = this.AddBoostButton(new Vector2(0, offset));
        button.OnClick += UpgradeBuilding;
        button.OnOver += OverBuilding;

        button = this.AddBoostButton(new Vector2(0, 390 / 2 + offset));
        button.OnClick += UpgradeAllGems;
        button.OnOver += OverAllGems;

        button = this.AddBoostButton(new Vector2(-154 / 2, 298 / 2 + offset));
        button.OnClick += UpgradeBlueGem;
        button.OnOver += OverBlueGems;

        button = this.AddBoostButton(new Vector2(-51 / 2, 298 / 2 + offset));
        button.OnClick += UpgradeGreenGem;
        button.OnOver += OverGreenGems;

        button = this.AddBoostButton(new Vector2(52 / 2, 298 / 2 + offset));
        button.OnClick += UpgradePurpleGem;
        button.OnOver += OverPurpleGems;

        button = this.AddBoostButton(new Vector2(157 / 2, 298 / 2 + offset));
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

    public bool Update()
    {
        this.Building = Building.selectedBuilding;

        if (this._building != null && this._building.State == Building.EState.Selected)
        {
            _boosterBuilding = this._building as Booster;
            _cost = new Resources(0, 0, 0, 0);

            // Position of menu image
            Point textureSize = _boosterBuilding == null ? new Point(this.menuTexture.Width / 2, this.menuTexture.Height / 2) : new Point(this.menuTextureBoost.Width / 2, this.menuTextureBoost.Height / 2);
            Vector2 vec = _building.TargetPosition.ToVector2() + new Vector2(0, 0);
            this.menuVec = vec - new Vector2(textureSize.X / 2, textureSize.Y / 2);
            this.menuRect = new Rectangle(this.menuVec.ToPoint(), textureSize);

            // Position of buttons
            if (this.menuRect.Contains(InputManager.WorldMousePosition))
            {
                Vector2 buttonVec = _building.TargetPosition.ToVector2();

                if (_boosterBuilding == null)
                {
                    foreach (UpgradeButton button in _buttons)
                        button.Update(buttonVec);
                }
                else
                {
                    foreach (UpgradeButton button in _buttons_boost)
                        button.Update(buttonVec);
                }

                return true;
            }
        }
        return false;
    }

    public void Draw()
    {
        if (_building != null && _building.State == Building.EState.Selected)
        {
            var upgradableBuilding = _building as UpgradeableBuilding;
            if (upgradableBuilding.Tier >= upgradableBuilding.MaxTier)
                return;

            // Draw marking of grid
            var blankTexture = new Texture2D(GameWindow.graphicsDevice, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });
            GameWindow.spriteBatch.Draw(
                blankTexture, this.menuRect, null, new Color(Color.White, 1f) * 0.5f, 0f,
                new Vector2(0, 0), SpriteEffects.None, 0f);

            // Draw menu image
            Texture2D texture = _boosterBuilding == null ? this.menuTexture : this.menuTextureBoost;
            GameWindow.spriteBatch.Draw(
                texture, this.menuRect, null, new Color(Color.White, 1f), 0f,
                new Vector2(0, 0), SpriteEffects.None, 0f);


            // Draw cost text
            SpriteFontBase font18 = ResourcesUi.FontSystem.GetFont(18);
            Vector2 menuVec = this.menuVec;
            menuVec += new Vector2(40, 79);
            GameWindow.spriteBatch.DrawString(font18, _cost.blue.ToString(), menuVec, Color.Black);
            menuVec += new Vector2(108, 0);
            GameWindow.spriteBatch.DrawString(font18, _cost.green.ToString(), menuVec, Color.Black);
            menuVec += new Vector2(-108, 42);
            GameWindow.spriteBatch.DrawString(font18, _cost.purple.ToString(), menuVec, Color.Black);
            menuVec += new Vector2(108, 0);
            GameWindow.spriteBatch.DrawString(font18, _cost.orange.ToString(), menuVec, Color.Black);

            // Draw buttons
            if (_boosterBuilding == null)
            {
                foreach (UpgradeButton button in _buttons)
                    button.Draw();
            }
            else
            {
                foreach (UpgradeButton button in _buttons_boost)
                    button.Draw();
            }
        }
    }


}
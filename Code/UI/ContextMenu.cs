using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
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
    private Resources? _cost;

    private readonly List<UpgradeButton> _buttons = new();
    private readonly List<UpgradeButton> _buttons_boost = new();

    private UpgradeButton btnUpgradeBuilding;
    private UpgradeButton btnUpgradeBoostBuilding;
    private UpgradeButton btnUpgradeAllGems;
    private UpgradeButton btnUpgradeBlueGem;
    private UpgradeButton btnUpgradeGreenGem;
    private UpgradeButton btnUpgradePurpleGem;
    private UpgradeButton btnUpgradeOrangeGem;


    public ContextMenu()
    {
        FontSystem = new FontSystem();
        FontSystem.AddFont(System.IO.File.ReadAllBytes(@"Data/Fonts/PTC55F.ttf"));
        this.menuTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/UpgradeBuilding/upgrade-building.png");
        this.menuTextureBoost = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/UpgradeBuilding/upgrade-boost-building.png");

        // Generic Building
        btnUpgradeBuilding = this.AddButton(new Vector2(0, -26));
        btnUpgradeBuilding.OnClick += UpgradeBuilding;
        btnUpgradeBuilding.OnOver += OverBuilding;

        // Boost Building
        int offset = -88;
        btnUpgradeBoostBuilding = this.AddBoostButton(new Vector2(0, offset));
        btnUpgradeBoostBuilding.OnClick += UpgradeBuilding;
        btnUpgradeBoostBuilding.OnOver += OverBuilding;

        btnUpgradeAllGems = this.AddBoostButton(new Vector2(0, 390 / 2 + offset));
        btnUpgradeAllGems.OnClick += UpgradeAllGems;
        btnUpgradeAllGems.OnOver += OverAllGems;

        btnUpgradeBlueGem = this.AddBoostButton(new Vector2(-154 / 2, 298 / 2 + offset));
        btnUpgradeBlueGem.OnClick += UpgradeBlueGem;
        btnUpgradeBlueGem.OnOver += OverBlueGems;

        btnUpgradeGreenGem = this.AddBoostButton(new Vector2(-51 / 2, 298 / 2 + offset));
        btnUpgradeGreenGem.OnClick += UpgradeGreenGem;
        btnUpgradeGreenGem.OnOver += OverGreenGems;

        btnUpgradePurpleGem = this.AddBoostButton(new Vector2(52 / 2, 298 / 2 + offset));
        btnUpgradePurpleGem.OnClick += UpgradePurpleGem;
        btnUpgradePurpleGem.OnOver += OverPurpleGems;

        btnUpgradeOrangeGem = this.AddBoostButton(new Vector2(157 / 2, 298 / 2 + offset));
        btnUpgradeOrangeGem.OnClick += UpgradeOrangeGem;
        btnUpgradeOrangeGem.OnOver += OverOrangeGems;
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
        //Console.WriteLine("UpgradeBuilding");
        var upgradableBuilding = this._building as UpgradeableBuilding;
        bool result = upgradableBuilding.TryUpgrade();
    }

    public void OverAllGems(object sender, EventArgs e)
    {
        _cost = Booster.GetGemUpgradeCost(Mineral.Type.All, _boosterBuilding);
    }

    public void UpgradeAllGems(object sender, EventArgs e)
    {
        //Console.WriteLine("UpgradeAllGems");
        Booster.TryUpgrade(Mineral.Type.All, _boosterBuilding);
    }

    public void OverBlueGems(object sender, EventArgs e)
    {
        _cost = Booster.GetGemUpgradeCost(Mineral.Type.Blue, _boosterBuilding);
    }

    public void UpgradeBlueGem(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradeBlueGem");
        Booster.TryUpgrade(Mineral.Type.Blue, _boosterBuilding);
    }

    public void OverGreenGems(object sender, EventArgs e)
    {
        _cost = Booster.GetGemUpgradeCost(Mineral.Type.Green, _boosterBuilding);
    }

    public void UpgradeGreenGem(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradeGreenGem");
        Booster.TryUpgrade(Mineral.Type.Green, _boosterBuilding);
    }

    public void OverPurpleGems(object sender, EventArgs e)
    {
        _cost = Booster.GetGemUpgradeCost(Mineral.Type.Purple, _boosterBuilding);
    }

    public void UpgradePurpleGem(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradePurpleGem");
        Booster.TryUpgrade(Mineral.Type.Purple, _boosterBuilding);
    }

    public void OverOrangeGems(object sender, EventArgs e)
    {
        _cost = Booster.GetGemUpgradeCost(Mineral.Type.Orange, _boosterBuilding);
    }

    public void UpgradeOrangeGem(object sender, EventArgs e)
    {
        Console.WriteLine("UpgradeOrangeGem");
        Booster.TryUpgrade(Mineral.Type.Orange, _boosterBuilding);
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
            vec = Camera.WorldToScreen(vec, GameWindow.WorldTranslation);

            this.menuVec = vec - new Vector2(textureSize.X / 2, textureSize.Y / 2);
            this.menuRect = new Rectangle(this.menuVec.ToPoint(), textureSize);

            // Position of buttons
            if (this.menuRect.Contains(InputManager.MousePosition))
            {
                if (_boosterBuilding == null)
                {
                    btnUpgradeBuilding.Update(vec);
                }
                else
                {
                    foreach (UpgradeButton button in _buttons_boost)
                        button.Update(vec);
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

            // Draw marking of grid
            var blankTexture = new Texture2D(GameWindow.graphicsDevice, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });
            GameWindow.spriteBatchUi.Draw(
                blankTexture, this.menuRect, null, new Color(Color.White, 1f) * 0.5f, 0f,
                new Vector2(0, 0), SpriteEffects.None, 0f);

            // Draw menu image
            Texture2D texture = _boosterBuilding == null ? this.menuTexture : this.menuTextureBoost;
            GameWindow.spriteBatchUi.Draw(
                texture, this.menuRect, null, new Color(Color.White, 1f), 0f,
                new Vector2(0, 0), SpriteEffects.None, 0f);


            // Draw cost text
            SpriteFontBase font18 = ResourcesUi.FontSystem.GetFont(18);
            Vector2 menuVec = this.menuVec;
            menuVec += new Vector2(40, 79);
            GameWindow.spriteBatchUi.DrawString(font18, _cost == null ? "-" : _cost.Value.blue.ToString(), menuVec, Color.Black);
            menuVec += new Vector2(108, 0);
            GameWindow.spriteBatchUi.DrawString(font18, _cost == null ? "-" : _cost.Value.green.ToString(), menuVec, Color.Black);
            menuVec += new Vector2(-108, 42);
            GameWindow.spriteBatchUi.DrawString(font18, _cost == null ? "-" : _cost.Value.purple.ToString(), menuVec, Color.Black);
            menuVec += new Vector2(108, 0);
            GameWindow.spriteBatchUi.DrawString(font18, _cost == null ? "-" : _cost.Value.orange.ToString(), menuVec, Color.Black);

            // Draw buttons
            if (_boosterBuilding == null)
            {
                if (upgradableBuilding.Tier < upgradableBuilding.MaxTierLevel - 1)
                    btnUpgradeBuilding.Draw();
            }
            else
            {
                if (upgradableBuilding.Tier < upgradableBuilding.MaxTierLevel - 1)
                    btnUpgradeBoostBuilding.Draw();

                if (Booster.GetGemUppgrade(Mineral.Type.Blue) < Booster.GetGemMaxTier(_boosterBuilding) - 1)
                    btnUpgradeBlueGem.Draw();
                if (Booster.GetGemUppgrade(Mineral.Type.Green) < Booster.GetGemMaxTier(_boosterBuilding) - 1)
                    btnUpgradeGreenGem.Draw();
                if (Booster.GetGemUppgrade(Mineral.Type.Purple) < Booster.GetGemMaxTier(_boosterBuilding) - 1)
                    btnUpgradePurpleGem.Draw();
                if (Booster.GetGemUppgrade(Mineral.Type.Orange) < Booster.GetGemMaxTier(_boosterBuilding) - 1)
                    btnUpgradeOrangeGem.Draw();
                if (Booster.GetGemUppgrade(Mineral.Type.All) < Booster.GetGemMaxTier(_boosterBuilding) - 1)
                    btnUpgradeAllGems.Draw();
            }
        }
    }
}
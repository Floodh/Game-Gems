using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public interface IUpgradableBuilding
{
    int GetBlueUpgradeCost();
    int GetPurpleUpgradeCost();
    int GetRedUpgradeCost();
    int GetGreenUpgradeCost();
    int Tier { get; }
    bool TryUpgrade();
}

class Cannon : Building, IUpgradableBuilding
{
    private static List<Texture2D> textureList = new List<Texture2D>();
    private HealthBar hpBar;
    private int tier = 0;
    public static readonly int MaxTier = 3;

    public Cannon()
        : base(Faction.Player)
    {
        hpBar = new HealthBar(this);
    }

    private static List<Texture2D> TextureList
    {
        get
        {
            if(textureList.Count == 0)
            {
                for(int index = 0; index < 4; index++)
                    textureList.Add(Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/Texture/GemStructure/Purple_{index}.png"));
            }  
            return textureList;
        }
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Texture2D texture = Cannon.TextureList[this.Tier];
            GameWindow.spriteBatch.Draw(texture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
            hpBar.Update();
            hpBar.Draw();
        }
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override Building CreateNew()
    {
        return new Cannon();
    }
    
    public override string ToString()
    {
        return $"Cannon : {this.Hp} / {this.MaxHp} / tier:{this.tier}";
    }

    public int GetBlueUpgradeCost()
    {
        return 10;
    }

    public int GetPurpleUpgradeCost()
    {
        return 0;
    }

    public int GetRedUpgradeCost()
    {
        return 0;
    }

    public int GetGreenUpgradeCost()
    {
        return 0;
    }

    public int Tier => this.tier;

    public bool TryUpgrade()
    {
        bool result = false;

        if(this.tier < Cannon.MaxTier)
        {
            result = Resources.BuyFor(
                this.GetBlueUpgradeCost(),
                this.GetPurpleUpgradeCost(),
                this.GetRedUpgradeCost(),
                this.GetGreenUpgradeCost());

            if(result)
            {
                this.tier++;
            }
        }
        return result;
    }
}
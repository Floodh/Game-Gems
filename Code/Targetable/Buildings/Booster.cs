using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Booster : UpgradeableBuilding
{


    private const int _maxGemTier = 5;
    private static int[] _currentGemTier = { 0, 0, 0, 0, 0 };
    public static int GetGemMaxTier(Booster boostBuilding)
    {
        return Math.Min(_maxGemTier, boostBuilding.CurrentTier + 2);
    }

    public static int GetGemBonus(Mineral.Type gemType)
    {
        if (gemType == Mineral.Type.All)
            throw new NotSupportedException("You cant mina ALL gems!");
        else
        {
            int seletedGemTier = _currentGemTier[(int)gemType];
            int allGemTier = _currentGemTier[4];
            return 1 + seletedGemTier + allGemTier;
        }
    }
    public static int GetGemUppgrade(Mineral.Type gemType)
    {
        return _currentGemTier[(int)gemType];
    }

    public static Resources? GetGemUpgradeCost(Mineral.Type gemType, Booster boostBuilding)
    {
        int tier = _currentGemTier[(int)gemType];
        if (tier >= GetGemMaxTier(boostBuilding) - 1)
            return null;
        else
        {
            int value = Convert.ToInt32(Math.Pow(2, tier + 2));
            value *= 8;
            if (gemType == Mineral.Type.Blue)
                return new Resources(value, 0, 0, 0);
            if (gemType == Mineral.Type.Green)
                return new Resources(0, value, 0, 0);
            if (gemType == Mineral.Type.Purple)
                return new Resources(0, 0, value, 0);
            if (gemType == Mineral.Type.Orange)
                return new Resources(0, 0, 0, value);
            if (gemType == Mineral.Type.All)
                return new Resources(value, value, value, value);
        }
        // }
        throw new NotSupportedException();
    }

    public static bool TryUpgrade(Mineral.Type gemType, Booster boostBuilding)
    {
        bool result = false;

        int gemTier = _currentGemTier[(int)gemType];

        if (gemTier < GetGemMaxTier(boostBuilding) - 1)
        {
            result = Resources.BuyFor(GetGemUpgradeCost(gemType, boostBuilding).Value);

            if (result)
            {
                _currentGemTier[(int)gemType]++;
            }
        }
        return result;
    }

    public static readonly Resources[] costs = new Resources[]
    {
        new Resources(64,32,32,32),
        new Resources(128,64,64,64),
        new Resources(256,128,128,128),
        new Resources(512,256,256,256),
        // new Resources(512,512,512,512),
        // new Resources(1024,1024,1024,1024),
    };
    private static readonly int[] maxHealth = new int[]
    {
        100,
        200,
        400,
        800,
        // 1600,
    };

    private const int textureSet = 4;


    public Booster()
        : base("income-tower3", textureSet)
    { }

    public override void Tick()
    {
        base.Tick();
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Rectangle rect = new(DrawArea.X + 32, DrawArea.Y - 8 - 64, DrawArea.Width / 2, DrawArea.Height / 2 * 3);
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][CurrentTier], rect, Sunlight.Mask);
            hpBar.Draw();
        }
    }
    protected override void UpdateStats()
    {
        this.MaxHp = maxHealth[CurrentTier];
        this.Hp = this.MaxHp;
    }
    public override Resources? GetUpgradeCost()
    {
        if (CurrentTier >= this.MaxTierLevel - 1)
            return null;
        else
            return costs[CurrentTier];
    }
    public static new Building CreateNew()
    {
        return new Booster();
    }

    public static new Building Buy()
    {
        if (Resources.BuyFor(costs[0]))
            return CreateNew();
        else
            return null;
    }

    public static new Texture2D[] GetTextures()
    {
        return baseTextures[textureSet];
    }

    public static new Rectangle GetRectangle(Point point)
    {
        return new Rectangle(point.X + 32, point.Y - 8 - 64, Map.mapPixelToTexturePixel_Multiplier, Map.mapPixelToTexturePixel_Multiplier * 3);
    }

    public override string ToString()
    {
        return $"Booster : {this.Hp} / {this.MaxHp} / tier:{this.Tier}";
    }

}
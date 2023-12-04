using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Booster : UpgradeableBuilding
{

    public enum EGemType
    {
        blue, green, purple, orange, all
    }
    private const int _maxGemTier = 5;
    private static int[] _currentGemTier = { 0, 0, 0, 0 };
    public static int[] GemTier { get { return _currentGemTier; } }
    public static int GetGemMaxTier(Booster boostBuilding)
    {
        return Math.Min(_maxGemTier, boostBuilding.CurrentTier + 2);
    }

    public static Resources? GetGemUpgradeCost(EGemType gemType, Booster boostBuilding)
    {

        if (gemType == EGemType.all)
        {
            if (GemTier.Min() >= GetGemMaxTier(boostBuilding) - 1)
                return null;
            else
            {
                int highestTier = GemTier.Max();
                int value = Convert.ToInt32(Math.Pow(2, highestTier + 2));
                value *= 4;
                return new Resources(value, value, value, value);
            }
        }
        else
        {
            int tier = GemTier[(int)gemType];
            if (tier >= GetGemMaxTier(boostBuilding) - 1)
                return null;
            else
            {
                int value = Convert.ToInt32(Math.Pow(2, tier + 2));
                value *= 8;
                if (gemType == EGemType.blue)
                    return new Resources(value, 0, 0, 0);
                if (gemType == EGemType.green)
                    return new Resources(0, value, 0, 0);
                if (gemType == EGemType.purple)
                    return new Resources(0, 0, value, 0);
                if (gemType == EGemType.orange)
                    return new Resources(0, 0, 0, value);
            }
        }
        throw new NotSupportedException();
    }

    public static bool TryUpgrade(EGemType gemType, Booster boostBuilding)
    {
        string txt = "";
        for (int i = 0; i < _currentGemTier.Length; i++)
            txt += _currentGemTier[i].ToString();
        Console.Write("A:" + txt);
        bool result = false;

        // As long as any gem can be upgraded, upgrade all can be used
        int gemTier;
        if (gemType == EGemType.all)
            gemTier = GemTier.Min();
        else
            gemTier = GemTier[(int)gemType];

        int currentAllowedMaxTier = Math.Min(_maxGemTier, boostBuilding.CurrentTier + 2);

        if (gemTier < GetGemMaxTier(boostBuilding) - 1)
        {
            result = Resources.BuyFor(GetGemUpgradeCost(gemType, boostBuilding).Value);

            if (result)
            {

                if (gemType == EGemType.all)
                {
                    for (int i = 0; i < _currentGemTier.Length; i++)
                        if (_currentGemTier[i] < currentAllowedMaxTier - 1)
                            _currentGemTier[i]++;
                }
                else
                {
                    _currentGemTier[(int)gemType]++;
                }
            }
        }
        txt = "";
        for (int i = 0; i < _currentGemTier.Length; i++)
            txt += _currentGemTier[i].ToString();
        Console.WriteLine(" B:" + txt);
        return result;
    }

    public static readonly Resources[] costs = new Resources[]
    {
        new Resources(64,64,64,64),
        new Resources(128,128,128,128),
        new Resources(256,256,256,256),
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
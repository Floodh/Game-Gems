using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Wall : UpgradeableBuilding
{

    private static readonly Resources[] costs = new Resources[]
    {
        new Resources(64,0,0,0),
        new Resources(128,0,0,0),
        new Resources(256,0,0,0),
        new Resources(512,0,0,0),
        new Resources(1024,0,0,0),
    };
    private static readonly int[] maxHealth = new int[]
    {
        400,
        800,
        1600,
        3200,
        6400,
    };

    private const int textureSet = 0;


    public Wall()
        : base("walls2", textureSet)
    {}

    public override void Tick()
    {
        base.Tick();
    }
    protected override void UppdateStats()
    {
        this.MaxHp = maxHealth[currentTierIndex];
        this.Hp = this.MaxHp;
    }
    public override Resources GetUpgradeCost()
    {
        return costs[currentTierIndex];
    }    
    public static new Building CreateNew()
    {
        return new Wall();
    }
    public override string ToString()
    {
        return $"Wall : {this.Hp} / {this.MaxHp}";
    }
}
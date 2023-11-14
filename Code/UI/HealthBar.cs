using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class HealthBar : Bar
{
    private static readonly Color enemyFillColor = Color.OrangeRed;
    private static readonly Color freindlyFillColor = Color.Green;
    private static readonly Color enemyEmptyColor = Color.LightGray;
    private static readonly Color freindlyEmptyColor = Color.DarkRed;

    public HealthBar(Targetable building)
        : base(building, building.faction == Faction.Enemy ?  enemyFillColor : freindlyFillColor, building.faction == Faction.Enemy ?  enemyEmptyColor : freindlyEmptyColor, Color.Black, Point.Zero)
    {}


    protected override double Percentace()
    {
        return (double)entity.Hp / entity.MaxHp;
    }
}
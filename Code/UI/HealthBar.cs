using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class HealthBar : Bar
{

    public HealthBar(Building building)
        : base(building, Color.Green, Point.Zero)
    {}


    protected override double Percentace()
    {
        return (double)building.Hp / building.MaxHp;
    }
}
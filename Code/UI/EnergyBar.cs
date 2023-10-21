using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class EnergyBar : Bar
{

    public EnergyBar(Building building)
        : base(building, Color.Purple, Color.Bisque, new Point(0, 16))
    {}

    protected override double Percentace()
    {
        return (double)building.Energy / building.MaxEnergy;
    }

}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class EnergyBar : Bar
{

    public EnergyBar(Building building)
        : base(building, Color.Purple, new Point(2, 6))
    {}


    protected override double Percentace()
    {
        return (double)building.Energy / building.MaxEnergy;
    }

}
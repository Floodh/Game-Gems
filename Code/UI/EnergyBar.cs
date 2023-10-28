using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class EnergyBar : Bar
{

    public EnergyBar(Building building)
        : base(building, Color.Purple, Color.Gray, Color.Black, new Point(0, 16))
    {}

    protected override double Percentace()
    {
        return (double)entity.Energy / entity.MaxEnergy;
    }

}
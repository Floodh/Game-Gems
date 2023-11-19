using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class EnergyBar : Bar
{

    public EnergyBar(Building building)
        : base(building, Color.Purple, Color.Gray, Color.Black, new Vector2(0, EnergyBar._barHeight + EnergyBar._barBorderSize))
    {}

    protected override double Percentace()
    {
        return (double)_entity.Energy / _entity.MaxEnergy;
    }

    protected override int MaxUnit()
    {
        return _entity.MaxEnergy;
    }

}
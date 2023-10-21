using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Cannon : UpgradeableBuilding
{
    private const int textureSet = 3;

    private EnergyBar energyBar;

    public Cannon()
        : base("Purple", textureSet)
    {
        this.energyBar = new EnergyBar(this);
        this.MaxEnergy = 100;
        this.Energy = 100;
        this.Regen_Energy = 0;
    }

    public override void Tick()
    {
        base.Tick();
        this.energyBar.Update();
    }

    public override void Draw()
    {
        base.Draw();
        this.energyBar.Draw();
    }

    public override Building CreateNew()
    {
        return new Cannon();
    }
    
    public override string ToString()
    {
        return $"Cannon : {this.Hp} / {this.MaxHp} / tier:{this.Tier}";
    }

}
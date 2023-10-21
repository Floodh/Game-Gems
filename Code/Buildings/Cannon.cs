using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Cannon : UpgradeableBuilding
{
    private const int textureSet = 3;

    public Cannon()
        : base("Purple", textureSet)
    {}

    public override void Tick()
    {
        base.Tick();
    }

    public override Building CreateNew()
    {
        return new Cannon();
    }
    
    public override string ToString()
    {
        return $"Cannon : {this.Hp} / {this.MaxHp} / tier:{this.tier}";
    }

}
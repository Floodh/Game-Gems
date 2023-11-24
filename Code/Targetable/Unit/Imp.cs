using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Imp : Enemy
{   
    private const string Path_BaseTexture = "Data/Texture/Units/demon2.png";

    public Imp(Point spawnGridPosition) 
        : base(spawnGridPosition, Path_BaseTexture)
    {
        this.MaxHp = 50;
        this.Hp = this.MaxHp;
        this.Regen_Health = 1;
        _weapon = new Weapon(this, 5);
        _weapon.Scale = 0.09f;
    }

    public override void Draw()
    {
        Rectangle enemyRect = new(DrawArea.X, DrawArea.Y-6, base.baseTexture.Width, base.baseTexture.Height);
        enemyRect = GetRectHeightScaledTo(enemyRect, 40);
        base.Draw(enemyRect);
    }
}
using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Demon : Enemy
{   
    private const string Path_BaseTexture = "Data/Texture/Units/demon1.png";

    public Demon(Point spawnGridPosition) 
        : base(spawnGridPosition, Path_BaseTexture)
    {
        this.MaxHp = 150;
        this.Hp = this.MaxHp;
        this.Regen_Health = 1;
        _weapon = new Weapon(this, 5);
        _weapon.Scale = 0.15f;
    }

    public override void Draw()
    {
        Rectangle enemyRect = new(DrawArea.X-0, DrawArea.Y-10, base.baseTexture.Width, base.baseTexture.Height);
        enemyRect = GetRectHeightScaledTo(enemyRect, DrawArea.Width);
        base.Draw(enemyRect);
    }
}
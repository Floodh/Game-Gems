using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Fighter : Enemy
{
    private const string Path_BaseTexture = "Data/Texture/Units/fighter1.png";

    public Fighter(Point spawnGridPosition) 
        : base(spawnGridPosition, Path_BaseTexture)
    {
        this.MaxHp = 100;
        this.Hp = this.MaxHp;
        this.Regen_Health = 1;        
        _weapon = new Weapon(this, 6);
        _weapon.Scale = 0.4f;
    }  

    public override void Draw()
    {
        Rectangle enemyRect = new(DrawArea.X, DrawArea.Y-6, base.baseTexture.Width, base.baseTexture.Height);
        enemyRect = GetRectHeightScaledTo(enemyRect, DrawArea.Width);
        base.Draw(enemyRect);
    }
}
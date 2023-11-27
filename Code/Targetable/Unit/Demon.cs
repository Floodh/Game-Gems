using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Demon : Enemy
{   
    private const string Path_BaseTexture = "Data/Texture/Units/demon1.png";

    public Demon(Point spawnGridPosition, NightDifficulty.DiffucultyModifier diffucultyModifier) 
        : base(spawnGridPosition, Path_BaseTexture)
    {
        this.MaxHp = (int)(85 * diffucultyModifier.healthModifier);
        this.Hp = this.MaxHp;
        this.Regen_Health = 1;    
        this.AttackDmg = (int)(12.5 * diffucultyModifier.damageModifier);      
        _weapon = new Weapon(this, 5, AttackDmg);
        _weapon.Scale = 0.15f;
    }

    public override void Draw()
    {
        Rectangle enemyRect = new(DrawArea.X-0, DrawArea.Y-10, base.baseTexture.Width, base.baseTexture.Height);
        enemyRect = GetRectHeightScaledTo(enemyRect, DrawArea.Width);
        base.Draw(enemyRect);
    }
}
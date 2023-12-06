using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Fighter : Enemy
{
    private const string Path_BaseTexture = "Data/Texture/Units/fighter1.png";

    public Fighter(Point spawnGridPosition, NightDifficulty.DiffucultyModifier diffucultyModifier) 
        : base(spawnGridPosition, Path_BaseTexture)
    {
        this.MaxHp = (int)(100 * diffucultyModifier.healthModifier);
        this.Hp = this.MaxHp;
        this.Regen_Health = 0;    
        this.AttackDmg = (int)(10 * diffucultyModifier.damageModifier);    
        _weapon = new Weapon(this, 6, AttackDmg);
        _weapon.Scale = 0.4f;
    }  

    public override void Draw()
    {
        Rectangle enemyRect = new(DrawArea.X, DrawArea.Y-6, base.baseTexture.Width, base.baseTexture.Height);
        enemyRect = GetRectHeightScaledTo(enemyRect, DrawArea.Width);
        base.Draw(enemyRect);
    }
}
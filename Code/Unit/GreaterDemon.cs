using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class GreaterDemon : Enemy
{   
    private const string Path_BaseTexture = "Data/Texture/Units/great-demon1.png";

    public GreaterDemon(Point spawnGridPosition) 
        : base(spawnGridPosition, Path_BaseTexture)
    {
        this.MaxHp = 300;
        this.Hp = this.MaxHp;
        this.Regen_Health = 1;
        this.projectileTextureId = 5;
    }

    public override void Draw()
    {
        Rectangle enemyRect = new(DrawArea.X-0, DrawArea.Y, base.baseTexture.Width, base.baseTexture.Height);
        enemyRect = GetRectHeightScaledTo(enemyRect, 100);
        base.Draw(enemyRect);
    }
}
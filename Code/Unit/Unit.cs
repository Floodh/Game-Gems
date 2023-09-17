using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

abstract class Unit : Targetable
{

    public static List<Unit> allUnits = new List<Unit>();

    protected int hp;
    protected int attackDmg;

    public void Draw()
    {}

    public abstract void Update();

    protected override void Die()
    {
    }

}

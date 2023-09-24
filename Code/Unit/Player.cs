using System;
using System.Numerics;
using Microsoft.Xna.Framework.Input;

class Player : Unit
{
    public Player(Vector2 spawnPosition)
    {
        this.exactPosition = spawnPosition;
    }

    public override void Draw()
    {
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
        //  depending on the last mouse click mapped to the world, move towards that
    }

}
using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Enemy : Unit
{
    private const string Path_BaseTexture = "Data/Texture/Enemy.png";
    Texture2D baseTexture;

    Targetable target = null;

    public Enemy(Vector2 spawnPosition) 
        : base(Faction.Enemy)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        this.exactPosition = spawnPosition;
    }

    public override void Draw()
    {
        GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Color.White);
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
        if (this.target == null)
        {

        }

    }

    private Targetable FindTarget()
    {
        double distanceSquared = double.MaxValue;
        Targetable target = null;

        foreach (Building building in Building.allBuildings)
        {

            Point diffP = base.TargetPosition - building.TargetPosition;
            double newDistance = diffP.X * diffP.X + diffP.Y + diffP.Y;

            if (newDistance < distanceSquared)
            {
                distanceSquared = newDistance;
                target = building;
            }

        }

        foreach (Unit unit in Unit.allUnits)
        {

            Point diffP = base.TargetPosition - unit.TargetPosition;
            double newDistance = diffP.X * diffP.X + diffP.Y + diffP.Y;

            if (newDistance < distanceSquared)
            {
                distanceSquared = newDistance;
                target = unit;
            }

        }


        return target;

    }

}
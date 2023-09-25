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
            Console.WriteLine("Trying to find target");
            this.target = this.FindTarget();
            Console.WriteLine($"Found target: {target}");
        }
        else
        {

            // Calculate the direction vector
            Vector2 destination = new Vector2(target.TargetPosition.X, target.TargetPosition.Y);
            Vector2 direction = destination - (this.exactPosition + new Vector2(DrawArea.Width / 2, DrawArea.Height / 2));
            direction.Normalize();
            float speed = 5.0f;
            float distanceToCenter = Vector2.Distance(exactPosition + new Vector2(DrawArea.Width / 2, DrawArea.Height / 2), destination);
            if (distanceToCenter > speed)
            {
                // Move towards the destination
                Vector2 movement = direction * speed;
                exactPosition += movement;
            }
        }

    }            

        
    private Targetable FindTarget()
    {
        double distanceSquared = double.MaxValue;
        Targetable target = null;

        foreach (Building building in Building.allBuildings)
            if (building.faction == Faction.Player)
        {

            Point diffP = base.TargetPosition - building.TargetPosition;
            double newDistance = (diffP.X * diffP.X) + (diffP.Y * diffP.Y);

            if (newDistance < distanceSquared)
            {
                distanceSquared = newDistance;
                target = building;
            }

        }

        foreach (Unit unit in Unit.allUnits)
            if (unit.faction == Faction.Player)
        {

            Point diffP = base.TargetPosition - unit.TargetPosition;
            double newDistance = diffP.X * diffP.X + diffP.Y * diffP.Y;

            if (newDistance < distanceSquared)
            {
                distanceSquared = newDistance;
                target = unit;
            }

        }


        return target;

    }

}
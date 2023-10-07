using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Enemy : Unit
{
    private const string Path_BaseTexture = "Data/Texture/Enemy.png";

    public static int NumberOfEnemies {get; protected set;}


    Texture2D baseTexture;
    Targetable target = null;

    public Enemy(Vector2 spawnPosition) 
        : base(Faction.Enemy)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        this.exactPosition = spawnPosition;
        NumberOfEnemies++;
    }

    public override void Draw()
    {
        GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
        base.Draw();
    }


    int attackCounter = 0;

    public override void Tick()
    {
        base.Tick();
        if (this.target == null || this.target.IsDead)
        {
            //Console.WriteLine("Trying to find target");
            this.target = this.FindTarget(this, Faction.Player, false, false);
            //Console.WriteLine($"Found target: {target}");
        }
        else
        {

            // Calculate the direction vector
            Vector2 destination = new Vector2(target.TargetPosition.X, target.TargetPosition.Y);
            Vector2 direction = destination - (this.exactPosition + new Vector2(DrawArea.Width / 2, DrawArea.Height / 2));
            direction.Normalize();
            float speed = 2.0f;
            float distanceToCenter = Vector2.Distance(exactPosition + new Vector2(DrawArea.Width / 2, DrawArea.Height / 2), destination);
            if (distanceToCenter > speed * 25)
            {
                // Move towards the destination
                Vector2 movement = direction * speed;
                exactPosition += movement;
                attackCounter = 0;

            }
            else
            {
                attackCounter++;
                if (attackCounter >= AttackRate)
                {
                    Projectile projectile = new Projectile(10, 0, target, this);
                    attackCounter = 0;
                }
            }
        }

    }     

    protected override void Die()
    {
        base.Die();
        NumberOfEnemies--;
    }     

        

}
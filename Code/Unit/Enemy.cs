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

    public Enemy(Point spawnGridPosition) 
        : base(Faction.Enemy, spawnGridPosition)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
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

            // Calculate the movement

            if (false)
            {
                // Move towards the destination


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
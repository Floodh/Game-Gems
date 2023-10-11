using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Enemy : Unit
{
    private const string Path_BaseTexture = "Data/Texture/Enemy.png";
    private const int movementRate = 200;

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


    int opertunityCounter = 0;

    public override void Tick()
    {
        base.Tick();
        // Calculate the movement

        // Figure out an available tile
        
        
        int currentValue = Building.grid.GetEnemyValue(GridArea.X, GridArea.Y);
        if (currentValue == int.MaxValue)
        {
            this.target ??= this.FindTarget(this, Faction.Player, false, false);

            //  perform attack
            opertunityCounter++;
            if (opertunityCounter >= AttackRate)
            {
                Projectile projectile = new Projectile(10, 0, target, this);
                opertunityCounter = 0;
            }
                        
        }
        else
        {

            if (opertunityCounter++ > movementRate)
            {
                opertunityCounter = 0;
                this.target = null;

                int nextValue = currentValue;
                Point nextPos = this.GridArea.Location;
                for (int i = 0; i < Grid.offsets.Length / 2; i++)
                {
                    int newX = GridArea.X + Grid.offsets[i * 2];
                    int newY = GridArea.Y + Grid.offsets[i * 2 + 1];
                    int newValue = Building.grid.GetEnemyValue(newX, newY);
                    if (newValue >= nextValue)
                    {
                        if (Building.grid.IsTileTaken(newX, newY) == false)
                        {
                            nextValue = newValue;
                            nextPos = new Point(newX, newY);
                        }
                    }
                }
                
                //  verification of the new position has already been done
                this.GridArea = new Rectangle(nextPos, new Point(1,1));
            }

        }

    }     

    protected override void Die()
    {
        base.Die();
        NumberOfEnemies--;
    }     

        

}
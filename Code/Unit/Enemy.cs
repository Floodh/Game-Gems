using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Enemy : Unit
{
    private const string Path_BaseTexture = "Data/Texture/fighter1.png";

    public static int NumberOfEnemies {get; protected set;}


    Texture2D baseTexture;
    Targetable target = null;

    public Enemy(Point spawnGridPosition) 
        : base(Faction.Enemy, spawnGridPosition)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        NumberOfEnemies++;
        this.MoveTo(spawnGridPosition);
    }

    public override void Draw()
    {
        Rectangle enemyRect = new(DrawArea.X, DrawArea.Y-8, DrawArea.Width, DrawArea.Height);
        GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(enemyRect, Camera.zoomLevel), Sunlight.Mask);
        base.Draw();
    }


    int opertunityCounter = 0;
    Point previusGridPoint = Point.Zero;

    public override void Tick()
    {
        base.Tick();
        // Calculate the movement

        // Figure out an available tile
        
        int currentValue = Building.grid.GetEnemyValue(GridArea.X, GridArea.Y);
        if (currentValue == int.MaxValue)
        {
            this.target ??= this.FindTarget(this, Faction.Player, false, false);
            if (this.target.IsDead)
            {
                Console.WriteLine("Warning! : Attacking dead target");
                this.target = null;
                return;
            }
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

                int nextValue = currentValue - 2;
                Point nextPos = this.GridArea.Location;
                for (int i = 0; i < Grid.offsets.Length / 2; i++)
                {
                    int newX = GridArea.X + Grid.offsets[i * 2];
                    int newY = GridArea.Y + Grid.offsets[i * 2 + 1];
                    int newValue = Building.grid.GetEnemyValue(newX, newY);
                    if (new Point(newX, newY) == previusGridPoint)
                        newValue--; //  this is safe since the condition can't be true if newValue = int.MinValue    
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
                if (nextPos != this.GridArea.Location)
                {
                    this.MoveToFrom(nextPos, this.GridArea.Location);
                    this.previusGridPoint = this.GridArea.Location;
                    this.GridArea = new Rectangle(nextPos, new Point(1,1));
                    
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
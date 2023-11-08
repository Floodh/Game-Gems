using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Enemy : Unit
{
    protected const int sunDmg = 1000;
    public static int NumberOfEnemies {get; protected set;}
    protected Texture2D baseTexture;
    protected int projectileTextureId;
    protected Targetable target = null;

    protected Enemy(Point spawnGridPosition, string Path_BaseTexture) 
        : base(Faction.Enemy, spawnGridPosition)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        NumberOfEnemies++;
        this.MoveTo(spawnGridPosition);
    }

    public static Enemy CreateNewEnemy(Point spawnGridPosition)
    {
        if(NumberOfEnemies % 4 == 0)
            return new Fighter(spawnGridPosition);
        else if((NumberOfEnemies+1) % 4 == 0)
            return new Imp(spawnGridPosition);
        else if((NumberOfEnemies+2) % 4 == 0)
            return new Demon(spawnGridPosition);
        else
            return new GreaterDemon(spawnGridPosition);
    }

    public virtual void Draw(Rectangle enemyRect)
    {
        // Rectangle enemyRect = new(DrawArea.X, DrawArea.Y-8, DrawArea.Width, DrawArea.Height);
        GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(enemyRect, Camera.zoomLevel), Sunlight.Mask);
        base.Draw();
    }


    int opertunityCounter = 0;
    Point previusGridPoint = Point.Zero;

    public override void Tick()
    {
        if (Sunlight.dayNightCycle.IsDay)
            this.Hp -= sunDmg;
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
                _ = new Projectile(10, 0, 1f, target, this.TargetPosition.ToVector2(), this.projectileTextureId, 30);
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

    protected Rectangle GetRectHeightScaledTo(Rectangle rect, int targetSize)
    {
        float ratio = (float)targetSize /  rect.Height;

        int width = (int)(rect.Width * ratio);
        int height = targetSize;
        rect = new Rectangle(rect.X, rect.Y, width, height);

        Console.WriteLine("H|" + rect);

        int x = rect.X + DrawArea.Width/2 - rect.Width/2; // Center with DrawArea
        int y = rect.Y +  DrawArea.Height  - targetSize; // Align with bottom DrawArea

        return new Rectangle(x, y, width, height);
    }

    // Todo fix incorrect alignments
    // protected Rectangle GetRectWidthScaledTo(Rectangle rect, int targetSize)
    // {
    //     float ratio = (float)targetSize /  rect.Width;

    //     int height = (int)(rect.Height * ratio);
    //     int width = targetSize;
    //     rect = new Rectangle(rect.X, rect.Y, width, height);

    //     Console.WriteLine("W|" + rect);

    //     int x = rect.X + DrawArea.Width/2 - rect.Width/2; // Center with DrawArea
    //     int y = rect.Y +  DrawArea.Height  - targetSize; // Align with bottom DrawArea

    //     return new Rectangle(x, y, width, height);
    // }

        

}
using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

abstract class Enemy : Unit
{
    public enum Type
    {
        Fighter,
        Imp,
        Demon,
        GreaterDemon
    }

    protected const int sunDmg = 100;
    public static int NumberOfEnemies { get; set; }
    protected Texture2D baseTexture;
    protected int projectileTextureId;
    protected Targetable target = null;
    protected Weapon _weapon;

    protected Enemy(Point spawnGridPosition, string Path_BaseTexture)
        : base(Faction.Enemy, spawnGridPosition)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        NumberOfEnemies++;
        this.MoveTo(spawnGridPosition);
    }

    public static Enemy CreateNewEnemy(Point spawnGridPosition, NightDifficulty.DiffucultyModifier diffucultyModifier, Type type)
    {
        if (type == Type.Fighter)
            return new Fighter(spawnGridPosition, diffucultyModifier);
        else if (type == Type.Imp)
            return new Imp(spawnGridPosition, diffucultyModifier);
        else if (type == Type.Demon)
            return new Demon(spawnGridPosition, diffucultyModifier);
        else if (type == Type.GreaterDemon)
            return new GreaterDemon(spawnGridPosition, diffucultyModifier);
        else
            throw new ArgumentException($"Invalid enemy type : {type}");
    }

    public virtual void Draw(Rectangle enemyRect)
    {
        GameWindow.spriteBatch.Draw(baseTexture, enemyRect, Sunlight.Mask);
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

        if (!this.IsDead)
        {

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
                _weapon?.Tick(target, ref opertunityCounter);
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
                        this.GridArea = new Rectangle(nextPos, new Point(1, 1));

                    }
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
        float ratio = (float)targetSize / rect.Height;

        int width = (int)(rect.Width * ratio);
        int height = targetSize;
        rect = new Rectangle(rect.X, rect.Y, width, height);

        int x = rect.X + DrawArea.Width / 2 - rect.Width / 2; // Center with DrawArea
        int y = rect.Y + DrawArea.Height - targetSize; // Align with bottom DrawArea

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
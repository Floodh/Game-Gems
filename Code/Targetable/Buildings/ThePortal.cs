using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class ThePortal : Building
{

    private const string Path_BaseTexture = "Data/Texture/portal2.png";
    private const int MaxSpawnedUnits = 24;

    private readonly Random random = new Random();

    Texture2D baseTexture;

    private readonly DayNightCycle dayNightCycle;

    public ThePortal(DayNightCycle dayNightCycle)
        : base(Faction.Enemy)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        this.dayNightCycle = dayNightCycle;
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, DrawArea, Sunlight.Mask);
        }
        base.Draw();
    }


    private int spawnCounter = 0;
    private const int threshHold = 75;

    public override void Tick()
    {
        base.Tick();
        if (spawnCounter++ > threshHold)
            if (Enemy.NumberOfEnemies < MaxSpawnedUnits)
                if (this.dayNightCycle.IsNight)
                {
                    Point[] eligibleSpawns = this.EligibleSpawns();
                    foreach (int i in Enumerable.Range(0, eligibleSpawns.Length).OrderBy(x => random.Next()))
                        if (!grid.IsTileTaken(eligibleSpawns[i]))
                    {
                        spawnCounter = 0;

                        int number = random.Next(6) + Sunlight.dayNightCycle.nightNumber;
                        Enemy.Type enemyType;
                        if (number < 3)
                            enemyType = Enemy.Type.Fighter;
                        else if (number < 7)
                            enemyType = Enemy.Type.Imp;
                        else if (number < 12)
                            enemyType = Enemy.Type.Demon;
                        else
                            enemyType = Enemy.Type.GreaterDemon;

                        Enemy.CreateNewEnemy(eligibleSpawns[i], NightDifficulty.GetModifier(this.dayNightCycle.nightNumber), enemyType);
                        break;
                    }

                }
    }

    public static new Building CreateNew()
    {
        throw new NotImplementedException();
    }
    public override string ToString()
    {
        return $"ThePortal : {this.Hp} / {this.MaxHp}";
    }


    private Point[] EligibleSpawns()
    {
        Rectangle outerShell = new Rectangle(this.GridArea.X - 1, this.GridArea.Y - 1, this.GridArea.Width + 2, this.GridArea.Height + 2);
        List<Point> result = new List<Point>();
        int index = 0;

        for (int x = outerShell.Left; x < outerShell.Right; x++)
        {
            result.Add( new Point(x, outerShell.Top) );
            result.Add( new Point(x, outerShell.Bottom) );
        }
        for (int y = this.GridArea.Top; y < this.GridArea.Bottom; y++)
        {
            result.Add(new Point(outerShell.Left, y));
            result.Add(new Point(outerShell.Right, y));
        }


        return result.ToArray();


    }
}
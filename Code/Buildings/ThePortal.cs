using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class ThePortal : Building
{

    private const string Path_BaseTexture = "Data/Texture/portal2.png";
    private const int MaxSpawnedUnits = 12;

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
            GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
        }
        base.Draw();
    }


    private int spawnCounter = 0;
    private const int threshHold = 200;

    public override void Tick()
    {
        base.Tick();
        if (spawnCounter++ > threshHold)
        if (Enemy.NumberOfEnemies < MaxSpawnedUnits)
        if (this.dayNightCycle.IsNight)
        {
            foreach (Point spawnLocation in this.EligibleSpawns()) 
                if (!grid.IsTileTaken(spawnLocation))
            {
                spawnCounter = 0;
                _ = new Enemy(spawnLocation);
                break;
            }          
                       
        }
    }

    public override Building CreateNew()
    {
        return new ThePortal(this.dayNightCycle);
    }
    public override string ToString()
    {
        return $"ThePortal : {this.Hp} / {this.MaxHp}";
    }


    private Point[] EligibleSpawns()
    {
        Rectangle outerShell = new Rectangle(this.GridArea.X - 1, this.GridArea.Y - 1, this.GridArea.Width + 2, this.GridArea.Height + 2);
        Point[] results = new Point[outerShell.Width * outerShell.Height - this.GridArea.Width * this.GridArea.Height];
        int index = 0;

        // Console.WriteLine(results.Length);
        // Console.WriteLine(outerShell);
        
        for (int x = outerShell.Left; x < outerShell.Right; x++)
        {
            //Console.WriteLine($"x {x}");
            results[index++] = new Point(x, outerShell.Top);
            results[index++] = new Point(x, outerShell.Bottom);
        }
        for (int y = this.GridArea.Top; y < this.GridArea.Bottom; y++)
        {
            //Console.WriteLine($"y {y}");
            results[index++] = new Point(outerShell.Left, y);
            results[index++] = new Point(outerShell.Right, y);
        }

        if (results.Length != index)
            throw new Exception("Eligiable spawn location results array was not filled");

        return results;


    }
}
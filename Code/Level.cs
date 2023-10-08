using System;
using Microsoft.Xna.Framework;

using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;
using System.Data;
using System.Diagnostics.CodeAnalysis;

class Level
{
    public const int tickPerSec = 30;
    public const int slowDownThreashold = 10;

    public int CurrentTick {get; private set;} = 0;
    public TimeSpan ElapsedTime { get {return DateTime.Now - startTime;} }
    public TimeSpan ElapsedGameTime { get {return TimeSpan.FromSeconds(CurrentTick) / tickPerSec;}}



    private int skippedTicks = 0;
    private readonly Random r = new Random();
    private DateTime startTime;

    public Level(Bitmap bitmap)
    {

        this.r = new Random();

        //  test
        Wall wall = new Wall();
        wall.Place(new Point(47,45));
        Cannon cannon = new Cannon();
        cannon.Place(new Point(43,43));
        Healer healer = new Healer();
        healer.Place(85,41);
        Generator generator = new Generator();
        generator.Place(41,45);
        Size size = bitmap.Size * Map.mapPixelToGridTile_Multiplier;

        ThePortal thePortal = new ThePortal();
        thePortal.Place(size.Width / 2 - 1, size.Height / 2 - 1);

        Player player = new Player(new Point(size.Width / 2 - 4, size.Height / 2 - 3));

        int numberOfRocks = 10000;

        for (int i = 0; i < numberOfRocks; i++)
        {
            int x = r.Next() % size.Width;
            int y = r.Next() % size.Height;
            int dx = thePortal.GridArea.X - x,
                dy = thePortal.GridArea.Y - y;
            int distanceSquared = dx * dx + dy * dy;
            if (distanceSquared >  5 * 5)
            {
                Boulder rock = new Boulder();
                rock.Place(x, y);
            }
        }

        this.startTime = DateTime.Now;

        Camera.zoomLevel = 5.0f;
        Building.grid.CalculateEnemyValue();

    }

    public void Tick()
    {
        //  First make things take damage
        //  then do tick, the tick will check if the unit died
        Projectile.TickAll();
        Building.TickAll();
        Unit.TickAll();
        Animation.TickAll();

    }

    //  
    public bool MayTick()
    {
        int ticks = OverdueTicks();
        if (ticks > slowDownThreashold)
            skippedTicks += ticks - slowDownThreashold;
        if (ticks > 0)
        {
            this.Tick();
            this.CurrentTick++;
            return true;
        }
        return false;
    }

    private int OverdueTicks()
    {
        return (ElapsedTime.Seconds * tickPerSec) - skippedTicks;
    }
}
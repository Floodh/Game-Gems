using System;
using Microsoft.Xna.Framework;

using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;

class Level
{
    public const long tickPerSec = 30;
    public const long slowDownThreashold = 10;

    public long CurrentTick {get; private set;} = 0;

    private long startTime_s;
    private long currentTime_s;
    private long skippedTicks = 0;

    private readonly Random r = new Random();

    public DayNightCycle dayNightCycle;

    public Level(GameArguments gameArguments)
    {

        Bitmap bitmap = new Bitmap(gameArguments.mapPath);

        this.r = new Random();

        this.startTime_s = DateTimeOffset.Now.ToUnixTimeSeconds();
        this.currentTime_s = startTime_s;
        this.dayNightCycle = new(GameWindow.windowSize);

        Size size = bitmap.Size * Map.mapPixelToGridTile_Multiplier;


        ThePortal thePortal = new ThePortal(dayNightCycle);
        thePortal.Place(size.Width / 2 - 1, size.Height / 2 - 1);
        _ = new Player(new Point(thePortal.GridArea.X - 1, thePortal.GridArea.Y));
        

        int numberOfMinerals = 4 * 3; 
        int numberOfRocks = 1000;

        for (int i = 0; i < numberOfMinerals; i++)
        {
            Mineral.Type type = (Mineral.Type)(i % 4);
            Mineral mineral = new Mineral(type);
            while (true)
            {
                int x = r.Next() % size.Width;
                int y = r.Next() % size.Height;
                int dx = thePortal.GridArea.X - x,
                    dy = thePortal.GridArea.Y - y;
                int distanceSquared = dx * dx + dy * dy;
                if (distanceSquared >  5 * 5)
                    if (mineral.Place(x, y))
                        break;
            }

        }

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

        this.dayNightCycle.Tick((int)this.CurrentTick);

    }

    //  
    public bool MayTick()
    {
        this.currentTime_s = DateTimeOffset.Now.ToUnixTimeSeconds();

        long overdueTicks = OverdueTicks();
        if (overdueTicks > slowDownThreashold)
        {
            skippedTicks += overdueTicks - slowDownThreashold;
        }
        if (overdueTicks > 0)
        {
            this.Tick();
            this.CurrentTick++;
            return true;
        }
        return false;
    }

    private long OverdueTicks()
    {
        return (this.currentTime_s * tickPerSec) - skippedTicks;
    }
}
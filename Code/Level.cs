using System;
using Microsoft.Xna.Framework;

using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;
using System.Data;
using System.Diagnostics.CodeAnalysis;

class Level
{
    public const long tickPerSec = 30;
    public const long slowDownThreashold = 10;

    public long CurrentTick {get; private set;} = 0;

    private long startTime_s;
    private long currentTime_s;
    private long skippedTicks = 0;

    private readonly Random r = new Random();

    public Level(Bitmap bitmap)
    {

        this.r = new Random();

        //  test
        Wall wall = new Wall();
        wall.Place(new Point(52,50));
        Cannon cannon = new Cannon();
        cannon.Place(new Point(47,48));
        Healer healer = new Healer();
        healer.Place(90,46);
        Generator generator = new Generator();
        generator.Place(46,50);
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

        this.startTime_s = DateTimeOffset.Now.ToUnixTimeSeconds();
        this.currentTime_s = startTime_s;

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
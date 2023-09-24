using System;
using Microsoft.Xna.Framework;

using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;
using System.Data;

class Level
{
    public const int tickPerSec = 30;
    public const int slowDownThreashold = 10;

    public int Tick {get; private set;} = 0;
    public TimeSpan ElapsedTime { get {return DateTime.Now - startTime;} }
    public TimeSpan ElapsedGameTime { get {return TimeSpan.FromSeconds(Tick) / tickPerSec;}}



    private int skipedTicks = 0;
    private readonly Random r = new Random();
    private DateTime startTime;

    public Level(Bitmap bitmap)
    {

        this.r = new Random();

        //  test
        Boulder boulder = new Boulder();
        boulder.Place(new Point(1, 0));
        Wall wall = new Wall();
        wall.Place(new Point(15,13));
        Cannon cannon = new Cannon();
        cannon.Place(new Point(13,13));
        Healer healer = new Healer();
        healer.Place(13,11);
        Generator generator = new();
        generator.Place(11,15);
        Size size = bitmap.Size * Map.mapPixelToGridTile_Multiplier;


        int numberOfRocks = 10000;

        for (int i = 0; i < numberOfRocks; i++)
        {
            int x = r.Next() % size.Width;
            int y = r.Next() % size.Height;
            Boulder rock = new Boulder();
            rock.Place(x, y);
        }

        this.startTime = DateTime.Now;

    }

    //  
    public bool DoTick()
    {
        int ticks = OverdueTicks();
        this.skipedTicks += ticks / slowDownThreashold;
        ticks %= slowDownThreashold;
        if (ticks > 0)
            this.Tick++;

        return ticks > 0;
    }

    private int OverdueTicks()
    {
        return (ElapsedTime.Seconds * tickPerSec) - skipedTicks;
    }
}
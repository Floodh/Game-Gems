using Microsoft.VisualBasic.CompilerServices;
using System.Reflection.Metadata.Ecma335;

readonly struct Resources
{

    //  static sections
    public static int Blue      {get => Current.blue;}
    public static int Green     {get => Current.green;}
    public static int Purple    {get => Current.purple;}
    public static int Orange    {get => Current.orange;}
    public static Resources Current {get; private set;} = new Resources(800,800,800,800);

    public static bool CanBuy(Resources resources)
    {
        return CanBuy(resources.blue, resources.green, resources.purple, resources.orange);
    }
    public static bool CanBuy(int blue, int green, int purple, int orange)
    {
        return blue <= Blue && green <= Green && purple <= Purple && orange <= Orange;
    }
    public static bool BuyFor(Resources resources)
    {
        if (CanBuy(resources))
        {       
            Current -= resources;
            return true;
        }
        else
        {
            return false;
        }        
    }
    public static bool BuyFor(int blue, int green, int purple, int orange)
    {
        return BuyFor(new Resources(blue, green, purple, orange));
    }
    public static void Gain(Resources resources)
    {
        Current += resources;
    }
    public static void Gain(int blue, int green, int purple, int orange)
    {
        Gain(new Resources(blue, green, purple, orange));
    }

    public static int GetBlue()
    {
        return Blue;
    }
    public static int GetGreen()
    {
        return Green;
    }
    public static int GetPurple()
    {
        return Purple;
    }
    public static int GetOrange()
    {
        return Orange;
    }

    //  non static section

    public readonly int blue, green, purple, orange;
    public Resources(int blue, int green, int purple, int orange)
    {
        this.blue = blue;
        this.green = green;
        this.purple = purple;
        this.orange = orange;
    }

    public static Resources operator +(Resources a, Resources b)
    {
        return new Resources(a.blue + b.blue, a.green + b.green, a.purple + b.purple, a.orange + b.orange);

    }
    public static Resources operator -(Resources a, Resources b)
    {
        return new Resources(a.blue - b.blue, a.green - b.green, a.purple - b.purple, a.orange - b.orange);
    }


}
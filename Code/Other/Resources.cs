static class Resources
{

    public static int Blue      {get; private set;} = 64;
    public static int Green     {get; private set;} = 0;
    public static int Purple    {get; private set;} = 0;
    public static int Orange       {get; private set;} = 0;

    public static void Init()
    {
        Blue = 64;
        Green = 0;
        Purple = 0;
        Orange = 0;        
    }


    public static bool CanBuy(int blue, int green, int purple, int orange)
    {
        return blue <= Blue && green <= Green && purple <= Purple && orange <= Orange;
    }

    public static bool BuyFor(int blue, int green, int purple, int orange)
    {
        if (CanBuy(blue, green, purple, orange))
        {       
            Blue -= blue;
            Green -= green;
            Purple -= purple;
            Orange -= orange;
            return true;
        }
        else
        {
            return false;
        }

    }

    public static void Gain(int blue, int green, int purple, int orange)
    {
        Blue += blue;
        Green += green;
        Purple += purple;
        Orange += orange;
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
}
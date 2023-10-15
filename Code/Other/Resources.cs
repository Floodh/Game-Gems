static class Resources
{

    public static int Blue      {get; private set;} = 64;
    public static int Green     {get; private set;} = 0;
    public static int Purple    {get; private set;} = 0;
    public static int Red       {get; private set;} = 0;

    public static void Init()
    {
        Blue = 64;
        Green = 0;
        Purple = 0;
        Red = 0;        
    }


    public static bool CanBuy(int blue, int green, int purple, int red)
    {
        return blue <= Blue && green <= Green && purple <= Purple && red <= Red;
    }

    public static bool BuyFor(int blue, int green, int purple, int red)
    {
        if (CanBuy(blue, green, purple, red))
        {       
            Blue -= blue;
            Green -= green;
            Purple -= purple;
            Red -= red;
            return true;
        }
        else
        {
            return false;
        }

    }

    public static void Gain(int blue, int green, int purple, int red)
    {
        Blue += blue;
        Green += green;
        Purple += purple;
        Red += red;
    }


}
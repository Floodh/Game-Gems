using Microsoft.Xna.Framework;

static class Sunlight
{
    public static Color Mask = Color.LightGray;

    public static DayNightCycle dayNightCycle;

    public static void AlterSunlight(float r, float g, float b)
    {
        Mask = new Color(
            r * 0.5f + NormalizePixelColor(Mask.R) * 0.5f,
            g * 0.5f + NormalizePixelColor(Mask.G) * 0.5f,
            b * 0.5f + NormalizePixelColor(Mask.B) * 0.5f
        );
    }

    private static float NormalizePixelColor(byte pixelValue)
    {
        return (float)pixelValue / 255;
    }
}
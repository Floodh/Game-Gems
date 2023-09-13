using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;

static class Camera
{

    private static Point offset = Point.Zero;
    private static int zoomDivider = 2;
    private static Size windowSize;


    public static void Init(int windowWidth, int windowHeight)
    {
        windowSize = new Size(windowWidth, windowHeight);
    }

    //  return a modified  Point
    public static Point Offset(Point position)
    {
        return Point.Zero;
    }

    public static Size Offset(Size size)
    {
        return new Size(size.Width / zoomDivider, size.Height / zoomDivider);
    }

    //  return a rectangle modified 
    public static Rectangle Offset(Rectangle rectangle)
    {
        return Rectangle.Empty;
    }

    //
    public static bool IsVisable(Rectangle area)
    {
        return true;
    }

    public static void UpdateByMouse(MouseState mouseState)
    {
    }

    public static void UpdateByKeyboard(KeyboardState keyboardState)
    {
    }
    
}
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

static class Camera
{
    private static Point offset = Point.Zero;
    private static int zoomDivider = 2;
    private static Size cameraWindowSize;



    public static void Init(Size drawTextureSize)
    {  
        cameraWindowSize = new Size(drawTextureSize.Width*5, drawTextureSize.Height*5);
    }

    //  return a modified  Point
    public static Point PointOffset(Point position)
    {
        position = offset;
        return offset;
    }

    public static Size SizeOffset(Size size)
    { 
        return  new Size(size.Width / zoomDivider, size.Height / zoomDivider);    
    }

    //  return a rectangle modified 
    public static Rectangle rectOffset(Rectangle rectangle)
    {
         rectangle = new Rectangle(offset.X,offset.Y,
         cameraWindowSize.Width, cameraWindowSize.Height);
         return rectangle;
    }   

    //
    public static bool IsVisible(Rectangle area)
    {
        return true;
    }

    public static void UpdateByMouse(MouseState mouseState, GraphicsDeviceManager graphics)
    {
      
        if (mouseState.Position.X >= graphics.PreferredBackBufferWidth)
        {
            offset.X -= 10;
        }
        if (mouseState.Position.X <= graphics.PreferredBackBufferWidth)
        {
            offset.X += 10;
        }
        if (mouseState.Position.Y >= )
        {
            offset.Y += 10;
        }
        if (mouseState.Position.Y = )
        {
            offset.Y -= 10;
        }
    }

    public static void UpdateByKeyboard(KeyboardState keyboardState)
    {
        if(keyboardState.IsKeyDown(Keys.Right))
        {
            offset.X -= 10;
        }
        if(keyboardState.IsKeyDown(Keys.Left))
        {
            offset.X += 10;
        }
        if(keyboardState.IsKeyDown(Keys.Down))
        {
            offset.Y -= 10;
        }
        if(keyboardState.IsKeyDown(Keys.Up))
        {
            offset.Y += 10;
        }
    }
    
}
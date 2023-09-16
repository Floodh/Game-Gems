using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

static class Camera
{
    private static Point offset = Point.Zero;
    private static Size cameraWindowSize;
    private static Size drawTextureSize;

    private static int zoomDivider = 2;
    private static float zoomLevel = 0.5f; //default zoom level


    private static float minZoom = 0.3f;
    private static float maxZoom = 2.0f;
    private static float zoomSpeed = 0.0003f;
    private static int previousScrollValue = 0;




    public static void Init(Size drawTextureSize)
    {  
        Camera.drawTextureSize = drawTextureSize;
        cameraWindowSize = drawTextureSize;
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
        int margin = 20; //define the margin in pixels from the edge of the screen
        int leftMargin = margin;
        int rightMargin = graphics.PreferredBackBufferWidth - margin;
        int topMargin = margin;
        int bottomMargin = graphics.PreferredBackBufferHeight - margin;

        if (mouseState.Position.X < leftMargin)
        {
            offset.X += 10;
        }
        if (mouseState.Position.X > rightMargin)
        {
            offset.X -= 10;
        }
        if (mouseState.Position.Y < topMargin)
        {
            offset.Y += 10;
        }
        if (mouseState.Position.Y > bottomMargin)
        {
            offset.Y -= 10;
        }
        // Calculate the scroll delta based on the change in scroll wheel value
        int scrollDelta = mouseState.ScrollWheelValue - previousScrollValue;
        previousScrollValue = mouseState.ScrollWheelValue;

        // Calculate the zoom factor based on the scroll delta and zoom speed
        float zoomFactor = 1.0f + scrollDelta * zoomSpeed;

        // Apply the zoom factor to the zoom level
        zoomLevel *= zoomFactor;

        // Limit the zoom level within specified bounds
        zoomLevel = MathHelper.Clamp(zoomLevel, minZoom, maxZoom);

        // Calculate the new offset to keep the center of the screen fixed
        Point centerScreen = new Point(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
        int xOffset = (int)((centerScreen.X - offset.X) * zoomFactor);
        int yOffset = (int)((centerScreen.Y - offset.Y) * zoomFactor);
        Point newOffset = new Point(centerScreen.X - xOffset, centerScreen.Y - yOffset);

        // Update the offset
        offset = newOffset;

        // Adjust camera window size based on the zoom level
        cameraWindowSize = new Size((int)(drawTextureSize.Width * zoomLevel), (int)(drawTextureSize.Height * zoomLevel));


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
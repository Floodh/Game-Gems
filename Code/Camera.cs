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
    private static float zoomLevel = 2.0f; //default zoom level
    private static float minZoom = 1.8f;
    private static float maxZoom = 3.0f;
    private static float zoomSpeed = 0.0003f;
    private static int previousScrollValue = 0;

    public static void Init(Size mapsize)
    {  
        drawTextureSize = mapsize;
        cameraWindowSize = drawTextureSize;
        offset.X = (drawTextureSize.Width - cameraWindowSize.Width) / 2;
        offset.Y = (drawTextureSize.Height - cameraWindowSize.Height) / 2;

    }
    //  return a modified  Point
    // public static Point PointOffset(Point position)
    // {
    //     position = offset;
    //     return offset;
    // }

    // public static Size SizeOffset(Size size)
    // { 
    //     return  new Size(size.Width / zoomDivider, size.Height / zoomDivider);    
    // }
    public static Rectangle GetManipulatedViewArea(Rectangle rectangle)
    {
         rectangle = new Rectangle(offset.X,offset.Y,
         cameraWindowSize.Width, cameraWindowSize.Height);
         return rectangle;
    }   

    //
    public static bool IsVisible(Rectangle area)
    {

        Rectangle checkRec = new Rectangle(offset.X, offset.Y, cameraWindowSize.Width, cameraWindowSize.Height);
        // Check if every corner of 'area' is inside the camera view
        bool topLeftInside = checkRec.Contains(area.Left, area.Top);
        bool topRightInside = checkRec.Contains(area.Right, area.Top);
        bool bottomLeftInside = checkRec.Contains(area.Left, area.Bottom);
        bool bottomRightInside = checkRec.Contains(area.Right, area.Bottom);

        return topLeftInside && topRightInside && bottomLeftInside && bottomRightInside;
    }

    public static void UpdateByMouse(MouseState mouseState, GraphicsDeviceManager graphics)
    {
        int margin = 20; // define the margin in pixels from the edge of the screen
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

        // Calculate the new zoom level and limit it within specified bounds
        float newZoomLevel = zoomLevel + scrollDelta * zoomSpeed;
        newZoomLevel = MathHelper.Clamp(newZoomLevel, minZoom, maxZoom);

        // Calculate the change in zoom level
        float zoomFactor = newZoomLevel / zoomLevel;

        // Calculate the center of the screen in world coordinates
        Point centerScreen = new Point(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

        // Calculate the new offset to keep the center of the screen fixed while zooming
        offset.X = centerScreen.X - (int)((centerScreen.X - offset.X) * zoomFactor);
        offset.Y = centerScreen.Y - (int)((centerScreen.Y - offset.Y) * zoomFactor);

        // Update the zoom level
        zoomLevel = newZoomLevel;

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
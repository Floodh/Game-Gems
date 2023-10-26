using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

static class Camera
{
    public static Point offset = Point.Zero;
    // public static Point Center { 
    //     get {return new Point((int)((offset.X + cameraWindowSize.Width) / zoomLevel), (int)((offset.Y + cameraWindowSize.Height) / zoomLevel));}   
    //     set {offset = new Point((int)((value.X - cameraWindowSize.Width) * zoomLevel), (int)((value.Y - cameraWindowSize.Height) * zoomLevel));}
    // }

    private static Size cameraWindowSize;
    private static Size drawTextureSize;

    public static float zoomLevel = 1.75f; //default zoom level
    private static float minZoom = 1.0f;
    private static float maxZoom = 1.75f;
    private static float zoomSpeed = 0.0009f;
    private static int previousScrollValue = 0;

    public static void Init(Size mapsize)
    {
        drawTextureSize = mapsize;
        cameraWindowSize = drawTextureSize;

        offset = new Point(-mapsize.Width/2, -mapsize.Height/2);
    }
    public static Vector2 ModifyPoint(Vector2 point)
    {
        float x = ((point.X + offset.X) * zoomLevel);
        float y = ((point.Y + offset.Y) * zoomLevel);  
        return new Vector2(x, y);      
    }
    //  untested
    public static Point ModifyPoint(Point point)
    {
        int x = (int)((point.X + offset.X) * zoomLevel);
        int y = (int)((point.Y + offset.Y) * zoomLevel);  
        return new Point(x, y);      
    }

    public static Rectangle ModifiedDrawArea(Rectangle area, float zoomLevel)
    {
        int xOffset = (int)((area.X + offset.X) * zoomLevel);
        int yOffset = (int)((area.Y + offset.Y) * zoomLevel);
        int width = (int)(area.Width * zoomLevel);
        int height = (int)(area.Height * zoomLevel);

        return new Rectangle(xOffset, yOffset, width, height);
    }

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

        // Calculate the scroll delta based on the change in scroll wheel value
        int scrollDelta = mouseState.ScrollWheelValue - previousScrollValue;
        previousScrollValue = mouseState.ScrollWheelValue;

        // Calculate the new zoom level and limit it within specified bounds
        float newZoomLevel = zoomLevel + scrollDelta * zoomSpeed;
        newZoomLevel = MathHelper.Clamp(newZoomLevel, minZoom, maxZoom);

        // Calculate the change in zoom level
        float zoomFactor = newZoomLevel / zoomLevel;

        // Calculate the mouse position in world coordinates
        Point mouseWorldPosition = new Point(
            (int)((mouseState.Position.X - offset.X) / zoomLevel),
            (int)((mouseState.Position.Y - offset.Y) / zoomLevel)
        );

        // Calculate the new camera offset to keep the mouse position fixed while zooming
        offset.X -= (int)((mouseWorldPosition.X * zoomFactor) - mouseWorldPosition.X);
        offset.Y -= (int)((mouseWorldPosition.Y * zoomFactor) - mouseWorldPosition.Y);

        // Update the zoom level
        zoomLevel = newZoomLevel;

        // Adjust camera window size based on the zoom level
        cameraWindowSize = new Size((int)(drawTextureSize.Width * zoomLevel), (int)(drawTextureSize.Height * zoomLevel));
    }

    public static void UpdateByKeyboard(KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.Right))
        {
            offset.X -= 10;
        }
        if (keyboardState.IsKeyDown(Keys.Left))
        {
            offset.X += 10;
        }
        if (keyboardState.IsKeyDown(Keys.Down))
        {
            offset.Y -= 10;
        }
        if (keyboardState.IsKeyDown(Keys.Up))
        {
            offset.Y += 10;
        }
    }

    //  untested
    public static Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        // Inverse of the ModifiedDrawArea logic
        float worldX = (screenPosition.X / zoomLevel) - offset.X;
        float worldY = (screenPosition.Y / zoomLevel) - offset.Y;
        return new Vector2(worldX, worldY);
    }

}
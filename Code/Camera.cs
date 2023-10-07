using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

static class Camera
{
    public static Point offset = Point.Zero;
    private static Size cameraWindowSize;
    private static Size drawTextureSize;
    private static int zoomDivider = 2;
    public static float zoomLevel = 2.0f; //default zoom level
    private static float minZoom = 1.8f;
    private static float maxZoom = 3.0f;
    private static float zoomSpeed = 0.0003f;
    private static int previousScrollValue = 0;

    public static void Init(Size mapsize)
    {
        drawTextureSize = mapsize;
        cameraWindowSize = drawTextureSize;
        //offset.X = (drawTextureSize.Width - cameraWindowSize.Width) / 2;
        // offset.Y = (drawTextureSize.Height - cameraWindowSize.Height) / 2;

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
       int margin = 20; // define the margin in pixels from the edge of the screen
    int leftMargin = margin;
    int rightMargin = graphics.PreferredBackBufferWidth - margin;
    int topMargin = margin;
    int bottomMargin = graphics.PreferredBackBufferHeight - margin;

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

    // // Check if the mouse is near the screen edges for panning
    // if (mouseState.Position.X < leftMargin)
    // {
    //     offset.X += 10;
    // }
    // if (mouseState.Position.X > rightMargin)
    // {
    //     offset.X -= 10;
    // }
    // if (mouseState.Position.Y < topMargin)
    // {
    //     offset.Y += 10;
    // }
    // if (mouseState.Position.Y > bottomMargin)
    // {
    //     offset.Y -= 10;
    // }

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
    public static Vector2 ScreenToWorld(Vector2 screenPosition)
{
    // Inverse of the ModifiedDrawArea logic
    float worldX = (screenPosition.X / zoomLevel) - offset.X;
    float worldY = (screenPosition.Y / zoomLevel) - offset.Y;
    return new Vector2(worldX, worldY);
}

}
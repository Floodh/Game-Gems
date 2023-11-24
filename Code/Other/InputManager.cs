
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public static class InputManager
{
    private static MouseState _lastMouseState;
    private static Vector2 _direction;
    private static Vector2 _directionArrows;
    public static Vector2 DirectionWASD => _direction;
    public static Vector2 DirectionArrows => _directionArrows;
    public static Vector2 MousePosition => Mouse.GetState().Position.ToVector2();
    public static Vector2 WorldMousePosition => Vector2.Transform(MousePosition, Matrix.Invert(GameWindow.WorldTranslation));

    public static bool MouseLeftClicked { get; private set; }
    public static bool MouseRightClicked { get; private set; }
    public static bool MouseMiddleClicked { get; private set; }

    public static bool MovingWASD => _direction != Vector2.Zero;
    public static bool MovingArrow => _directionArrows != Vector2.Zero;

    public enum EMoveType
    {
        Keyboard,
        MouseFollow,
        MouseClick,
        NodeMove
    }

    public static EMoveType MoveType = EMoveType.Keyboard;

    public static void Update()
    {
        var keyboardState = Keyboard.GetState();
        _direction = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.W)) _direction.Y--;
        if (keyboardState.IsKeyDown(Keys.S)) _direction.Y++;
        if (keyboardState.IsKeyDown(Keys.A)) _direction.X--;
        if (keyboardState.IsKeyDown(Keys.D)) _direction.X++;

        _directionArrows = Vector2.Zero;
        if (keyboardState.IsKeyDown(Keys.Up)) _directionArrows.Y--;
        if (keyboardState.IsKeyDown(Keys.Down)) _directionArrows.Y++;
        if (keyboardState.IsKeyDown(Keys.Left)) _directionArrows.X--;
        if (keyboardState.IsKeyDown(Keys.Right)) _directionArrows.X++;

        MouseLeftClicked = (Mouse.GetState().LeftButton == ButtonState.Pressed) && (_lastMouseState.LeftButton == ButtonState.Released);
        MouseRightClicked = (Mouse.GetState().RightButton == ButtonState.Pressed) && (_lastMouseState.RightButton == ButtonState.Released);
        MouseMiddleClicked = (Mouse.GetState().MiddleButton == ButtonState.Pressed) && (_lastMouseState.MiddleButton == ButtonState.Released);
        _lastMouseState = Mouse.GetState();
    }
}

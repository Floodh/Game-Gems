using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

public class Camera
{
    private const float SPEED = 400;
    private const float _minZoom = 0.25f, _maxZoom = 2.5f;
    private int previousScrollValue = 0;
    public Camera()
    {
        Zoom = 1.0f;
    }
    // Centered Position of the Camera in pixels.
    public Vector2 Center { get => _center; set => _center = value; }
    private Vector2 _center;

    public Point MapSize { get => _mapSize; set => _mapSize = value; }
    private Point _mapSize;

    public float Zoom { get; private set; }
    public float Rotation { get; private set; }
    // Height and width of the viewport window which should be adjusted when the player resizes the game window.
    public int ViewportWidth { get; set; }
    public int ViewportHeight { get; set; }
    // Center of the Viewport does not account for scale
    public Vector2 ViewportCenter
    {
        get
        {
            return new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
        }
    }
    // create a matrix for the camera to offset everything we draw, the map and our objects. since the
    // camera coordinates are where the camera is, we offset everything by the negative of that to simulate
    // a camera moving. we also cast to integers to avoid filtering artifacts
    public Matrix TranslationMatrix
    {
        get
        {
            return Matrix.CreateTranslation(-(int)Center.X, -(int)Center.Y, 0) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
        }
    }

    public void AdjustZoom(float amount)
    {
        Zoom += amount;
        if (Zoom < _minZoom)
            Zoom = _minZoom;
        else if (Zoom > _maxZoom)
            Zoom = _maxZoom;
    }

    public Rectangle ViewportWorldBoundry()
    {
        Vector2 viewPortCorner = ScreenToWorld(new Vector2(0, 0));
        Vector2 viewPortBottomCorner = ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));
        return new Rectangle((int)viewPortCorner.X, (int)viewPortCorner.Y, (int)(viewPortBottomCorner.X - viewPortCorner.X),
                                            (int)(viewPortBottomCorner.Y - viewPortCorner.Y));
    }

    // Clamp the camera so it never leaves the visible area of the map.
    private Vector2 MapClampedPosition(Vector2 position)
    {
        var cameraMax = new Vector2(_mapSize.X - (ViewportWidth / Zoom / 2),
        _mapSize.Y - (ViewportHeight / Zoom / 2));
        return Vector2.Clamp(position, new Vector2(ViewportWidth / Zoom / 2, ViewportHeight / Zoom / 2), cameraMax);
    }
    public Vector2 WorldToScreen(Vector2 worldPosition)
    {
        return Vector2.Transform(worldPosition, TranslationMatrix);
    }
    public Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
    }

    public static Vector2 WorldToScreen(Vector2 worldPosition, Matrix translationMatrix)
    {
        return Vector2.Transform(worldPosition, translationMatrix);
    }

    public static Vector2 ScreenToWorld(Vector2 screenPosition, Matrix translationMatrix)
    {
        return Vector2.Transform(screenPosition, Matrix.Invert(translationMatrix));
    }


    public void UpdateCenter(Vector2 center, bool clampToMap = false)
    {
        _center = center;
        if (clampToMap)
            _center = MapClampedPosition(_center);
    }

    public void UpdateCenterByInput(bool clampToMap = false)
    {
        if (InputManager.MovingWASD)
        {
            Vector2 vec = Vector2.Normalize(InputManager.DirectionWASD);
            _center += vec * Camera.SPEED * GameWindow.Time;

            if (clampToMap)
                _center = MapClampedPosition(_center);
        }
        else if (InputManager.MovingArrow) // Secondary key input
        {
            Vector2 vec = Vector2.Normalize(InputManager.DirectionArrows);
            _center += vec * Camera.SPEED * GameWindow.Time;

            if (clampToMap)
                _center = MapClampedPosition(_center);
        }
    }

    public void UpdateZoom(bool clampToMap = false)
    {
        var currentMouseState = Mouse.GetState();

        if (currentMouseState.ScrollWheelValue < previousScrollValue)
        {
            AdjustZoom(-0.25f);
        }
        else if (currentMouseState.ScrollWheelValue > previousScrollValue)
        {
            AdjustZoom(+0.25f);
        }
        previousScrollValue = currentMouseState.ScrollWheelValue;

        if (clampToMap)
            _center = MapClampedPosition(_center);
    }
}
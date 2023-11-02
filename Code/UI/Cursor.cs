using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Cursor
{
    private Rectangle clickRect;
    private GameWindow gameWin;
    private ClickConfirm animation = null;

    public Cursor(GameWindow gameWin)
    {
        Texture2D cursorTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/TextureSources/cursor2.png");
        Mouse.SetCursor(MouseCursor.FromTexture2D(cursorTexture, 0, 0));
        this.gameWin = gameWin;
        this.gameWin.IsMouseVisible = true;
    }

    public void Update(MouseState mouseState, BuildingSelector.EState buildingState)
    {
        gameWin.IsMouseVisible = buildingState != BuildingSelector.EState.PlacementPending;
        Vector2 mouseVec = mouseState.Position.ToVector2() + new Vector2(-16,-16);
        this.clickRect = new(mouseVec.ToPoint(), new Point(32, 32));
    }

    public void Play()
    {
        // Animation.Remove(this.animation);
        Rectangle rect = this.clickRect;
        this.animation = new(rect);
        this.animation.Play();
    }
}
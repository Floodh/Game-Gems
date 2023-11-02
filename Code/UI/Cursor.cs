using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Cursor
{
    private Texture2D clickTexture; // TODO remove
    private Rectangle clickRect;
    private GameWindow gameWin;

    public Cursor(GameWindow gameWin)
    {
        Texture2D cursorTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/TextureSources/cursor2.png");
        Mouse.SetCursor(MouseCursor.FromTexture2D(cursorTexture, 0, 0));
        //this.clickTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/TextureSources/click-confirmation1.png"); // TODO remove

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
        Rectangle rect = this.clickRect;
        ClickConfirm animation = new(rect);
        animation.Play();
    }

}
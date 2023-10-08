using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player : Unit
{
    private const string Path_BaseTexture = "Data/Texture/Player.png";
    Texture2D baseTexture;
    private Vector2 lastMouseClickPosition;
    private bool isMoving;



    public Player(Point spawnGridPosition)
        : base(Faction.Player, spawnGridPosition)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        this.lastMouseClickPosition = Vector2.Zero; // Initialize it to some default value
        this.isMoving = false;
    }

    public override void Draw()
    {
        GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
        base.Draw();
    }

   public override void Tick()
{
    base.Tick();

    // Check if right mouse button is clicked
    if (Mouse.GetState().RightButton == ButtonState.Pressed)
    {
        // Update the last clicked position
        lastMouseClickPosition = Camera.ScreenToWorld(Mouse.GetState().Position.ToVector2());
        isMoving = true;
    }

    if (isMoving)
    {
        // Calculate the direction vector

        // Check if the player has reached the center of the target
        if (false)
        {
            // Move towards the destination

        }
        else
        {
            // Stop moving once the center is reached
            isMoving = false;
        }
    }
}
    public void HandleMouseClick(Vector2 clickPosition)
    {
        lastMouseClickPosition = Camera.ScreenToWorld(clickPosition);
    }

}
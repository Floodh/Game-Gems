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



    public Player(Vector2 spawnPosition)
        : base(Faction.Player)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        this.exactPosition = spawnPosition;
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
        Vector2 direction = lastMouseClickPosition - (exactPosition + new Vector2(DrawArea.Width / 2, DrawArea.Height / 2));
        direction.Normalize();

        // Adjust the movement speed as needed
        float speed = 5.0f;

        // Calculate the distance to the destination's center
        float distanceToCenter = Vector2.Distance(exactPosition + new Vector2(DrawArea.Width / 2, DrawArea.Height / 2), lastMouseClickPosition);

        // Check if the player has reached the center of the target
        if (distanceToCenter > speed)
        {
            // Move towards the destination
            exactPosition += direction * speed;
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
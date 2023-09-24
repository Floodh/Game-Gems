using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player : Unit
{
    private const string Path_BaseTexture = "Data/Texture/Player.png";
    Texture2D baseTexture;

    public Player(Vector2 spawnPosition) 
        : base(Faction.Player)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        this.exactPosition = spawnPosition;
    }

    public override void Draw()
    {
        GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Color.White);
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
        //  depending on the last mouse click mapped to the world, move towards that
    }

}
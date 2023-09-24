using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Cannon : Building
{

    private const string Path_BaseTexture = "Data/Texture/Cannon.png";

    Texture2D baseTexture;

    public Cannon()
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Color.White);
        }
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
    }

}
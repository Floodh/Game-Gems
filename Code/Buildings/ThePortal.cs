using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class ThePortal : Building
{

    private const string Path_BaseTexture = "Data/Texture/ThePortal.png";

    Texture2D baseTexture;

    public ThePortal()
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, DrawArea, Color.White);
        }
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
    }

}
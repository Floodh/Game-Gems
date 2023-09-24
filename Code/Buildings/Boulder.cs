
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Boulder : Building
{

    private const string Path_BaseTexture = "Data/Texture/Boulder.png";

    Texture2D baseTexture;

    public Boulder()
        : base(Faction.Neutral)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
    }

   public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Rectangle drawArea = Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel);
            GameWindow.spriteBatch.Draw(baseTexture, drawArea, Color.White);
        }
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
    }

}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Healer : Building
{

    private const string Path_BaseTexture = "Data/Texture/Healer.png";

    Texture2D baseTexture;
    private HealthBar hpBar;

    public Healer()
         : base(Faction.Player)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        hpBar = new HealthBar(this);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Color.White);
            hpBar.update();
            hpBar.Draw();
        }
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
    }
    public override string ToString()
    {
        return $"Healer : {this.Hp} / {this.MaxHp}";
    }
}
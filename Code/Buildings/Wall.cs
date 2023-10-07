using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Wall : Building
{

    private const string Path_BaseTexture = "Data/Texture/Wall.png";

    Texture2D baseTexture;
    private HealthBar hpBar;
    public Wall()
        : base(Faction.Player)
    {
        this.MaxHp = 500;
        this.Hp = this.MaxHp;

        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        hpBar = new HealthBar(this);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
            hpBar.Update();
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
        return $"Wall : {this.Hp} / {this.MaxHp}";
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Boulder : Building
{

    private const string Path_BaseTexture = "Data/Texture/Boulder.png";
    private HealthBar hpBar;
    Texture2D baseTexture;

    public Boulder()
        : base(Faction.Neutral)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        hpBar = new HealthBar(this);
    }

   public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Rectangle drawArea = Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel);
            GameWindow.spriteBatch.Draw(baseTexture, drawArea, Color.White);
            hpBar.update();
            if(this.MaxHp != Hp)
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
        return $"Boulder : {this.Hp} / {this.MaxHp}";
    }

}
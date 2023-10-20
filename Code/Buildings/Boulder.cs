
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Boulder : Building
{

    private const string Path_BaseTexture = "Data/Texture/Boulder.png";
    private HealthBar hpBar;
    private static Texture2D baseTexture;

    public Boulder()
        : base(Faction.Neutral)
    {
        baseTexture ??= Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        hpBar = new HealthBar(this);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Rectangle drawArea = Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel);
            GameWindow.spriteBatch.Draw(baseTexture, drawArea, Sunlight.Mask);
            hpBar.Update();
            if(this.MaxHp != Hp)
                hpBar.Draw();
        }
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override Building CreateNew()
    {
        return new Boulder();
    }

    public override string ToString()
    {
        return $"Boulder : {this.Hp} / {this.MaxHp}";
    }

}
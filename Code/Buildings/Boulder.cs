
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Boulder : Building
{

    private const string Path_BaseTexture = "Data/Texture/Boulder.png";
    private static Texture2D baseTexture;

    public Boulder()
        : base(Faction.Neutral)
    {
        this.MaxHp = 2;
        this.Hp = 2;
        this.Regen_Health = 0;

        baseTexture ??= Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Rectangle drawArea = Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel);
            GameWindow.spriteBatch.Draw(baseTexture, drawArea, Sunlight.Mask);
            if(this.MaxHp != Hp)
                hpBar.Draw();
        }
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
        this.hpBar.Update();
    }

    public override void PlayerInteraction()
    {
        base.PlayerInteraction();
        this.Hp--;
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
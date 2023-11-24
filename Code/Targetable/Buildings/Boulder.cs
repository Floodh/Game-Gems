
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Boulder : Building
{

    private const string Path_BaseTexture = "Data/Texture/Boulder.png";
    private static Texture2D baseTexture;

    public Boulder()
        : base(Faction.Neutral)
    {
        this.MaxHp = 200;
        this.Hp = this.MaxHp;
        this.Regen_Health = 0;

        baseTexture ??= Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, DrawArea, Sunlight.Mask);
            if (this.MaxHp != Hp)
                hpBar.Draw();
        }
        base.Draw();
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override void PlayerInteraction()
    {
        base.PlayerInteraction();
        this.Hp -= 120;
    }

    public static new Building CreateNew()
    {
        return new Boulder();
    }

    public override string ToString()
    {
        return $"Boulder : {this.Hp} / {this.MaxHp}";
    }

}
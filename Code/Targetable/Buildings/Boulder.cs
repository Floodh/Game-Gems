
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Boulder : Building
{

    private const string Path_BaseTexture = "Data/Texture/Boulder.png";
    private static Texture2D baseTexture;

    private Random random = new();

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

    protected override void Die()
    {
        Resources resources;
        NumberAnimation numberAnimation;

        int num = random.Next() % 4;
        Rectangle area = this.DrawArea;
        area.Inflate(-area.Width / 4, -area.Width / 4);
        if (num == 0)
        {
            resources = new(16,0,0,0);
            numberAnimation = new NumberAnimation(area, "+16", Color.Blue); 
        }
        else if (num == 1)
        {
            resources = new(0,16,0,0);
            numberAnimation = new NumberAnimation(area, "+16", Color.Green); 
        }
        else if (num == 2)
        {
            resources = new(0,0,16,0);
            numberAnimation = new NumberAnimation(area, "+16", Color.Purple); 
        }
        else
        {
            resources = new(0,0,0,16);
            numberAnimation = new NumberAnimation(area, "+16", Color.Orange); 
        }

        numberAnimation.Play();
        Resources.Gain(resources);
        base.Die();

    }

}
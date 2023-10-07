using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class ThePortal : Building
{

    private const string Path_BaseTexture = "Data/Texture/ThePortal.png";
    private const int MaxSpawnedUnits = 4;

    Texture2D baseTexture;

    public ThePortal()
        : base(Faction.Enemy)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
        }
        base.Draw();
    }


    private int spawnCounter = 0;
    private int threshHold = 50;

    public override void Tick()
    {
        base.Tick();
        if (spawnCounter++ > threshHold)
        if (Enemy.NumberOfEnemies < MaxSpawnedUnits)
        {
            spawnCounter = 0;
            Enemy spawn = new Enemy(new Vector2(this.TargetPosition.X, this.TargetPosition.Y));
        }
    }
    public override string ToString()
    {
        return $"ThePortal : {this.Hp} / {this.MaxHp}";
    }
}
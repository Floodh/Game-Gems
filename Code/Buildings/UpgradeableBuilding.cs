using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

interface IUpgradeableBuilding
{
    Resources GetUpgradeCost();
    int Tier {get;}
    void Upgrade();
}

abstract class UpgradeableBuilding : Building, IUpgradeableBuilding
{

    private const int textureSets = 4;
    private const int maxTier = 4;
    protected static Texture2D[][] baseTextures;

    public int Tier{get{return this.currentTier;}}
    protected int currentTier = 1;
    private HealthBar hpBar;
    private readonly int textureSet;

    protected UpgradeableBuilding(string colorName, int textureSet)
        : base(Faction.Player)
    {
        this.textureSet = textureSet;
        baseTextures ??= new Texture2D[textureSets][];

        if (baseTextures[textureSet] == null)
        {
            baseTextures[textureSet] = new Texture2D[4];
            for (int i = 0; i < maxTier; i++)
                baseTextures[textureSet][i] = Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/Texture/GemStructure/{colorName}_{i}.png");
        }

        hpBar = new HealthBar(this);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][currentTier-1], Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
            hpBar.Update();
            hpBar.Draw();
        }
        base.Draw();
    }

    public Resources GetUpgradeCost()
    {
        return new Resources(64, 64, 64, 64);
    }

    public int GetTier()
    {
        throw new NotImplementedException();
    }

    public void Upgrade()
    {
        throw new NotImplementedException();
    } 

}
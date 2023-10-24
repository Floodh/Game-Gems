using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

interface IUpgradeableBuilding
{
    Resources GetUpgradeCost();
    int Tier {get;}
    bool TryUpgrade();
}

abstract class UpgradeableBuilding : Building, IUpgradeableBuilding
{

    private const int textureSets = 4;
    private const int maxTier = 4;
    protected static Texture2D[][] baseTextures;

    public int Tier{get{return this.currentTier;}}
    public int MaxTier{get{ return UpgradeableBuilding.maxTier; }}
    protected int currentTier = 1;
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
        return new Resources(16, 0, 0, 0);
    }

    public bool TryUpgrade()
    {
        bool result = false;

        if(currentTier < maxTier)
        {
            result = Resources.BuyFor(GetUpgradeCost());

            if(result)
            {
                currentTier++;
            }
        }
        return result;
    }

}
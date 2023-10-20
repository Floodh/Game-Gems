using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public interface IUpgradableBuilding
{
    int GetBlueUpgradeCost();
    int GetTier();
    void Upgrade();
}

class Cannon : Building, IUpgradableBuilding
{

    private const string Path_BaseTexture = "Data/Texture/Cannon.png";
    private HealthBar hpBar;
    Texture2D baseTexture;

    public Cannon()
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

    public override Building CreateNew()
    {
        return new Cannon();
    }

    public override void UpdateByMouse(MouseState mouseState)
    {
        Console.WriteLine("Cannon Mouse");
    }
    
    public override string ToString()
    {
        return $"Cannon : {this.Hp} / {this.MaxHp}";
    }

    public int GetBlueUpgradeCost()
    {
        return 2345;
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
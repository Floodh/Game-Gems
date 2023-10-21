using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Wall : UpgradeableBuilding
{

    private const int textureSet = 0;


    public Wall()
        : base("Blue", textureSet)
    {
        this.MaxHp = 500;
        this.Hp = this.MaxHp;
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override Building CreateNew()
    {
        return new Wall();
    }
    public override string ToString()
    {
        return $"Wall : {this.Hp} / {this.MaxHp}";
    }
}
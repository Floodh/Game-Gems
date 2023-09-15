using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

class Wall : Building
{

    public Wall(Point position)
    {
        this.position = position;
        Building.allBuildings.Add(this);
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public void Draw()
    {
        // if (Camera.IsVisable(this.drawArea))
        //     //  draw me
        //Rectangle drawArea = new Rectangle(this.position.X - camera.position.X, this.position.Y - camera.position.Y, 20, 20);

    }

}
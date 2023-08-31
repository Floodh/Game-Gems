using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

class Wall : Building
{

    public Wall(Point position)
    {
        this.position = position;
        this.gridSize = new Size(2, 2);

        Building.allBuildings.Add(this);
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

}
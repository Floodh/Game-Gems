using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player : Unit
{
    private const string Path_BaseTexture = "Data/Texture/wizard1.png";


    public Point GridLocation {get{return this.GridArea.Location;}}
    public bool IsMoving {get{return this.GridLocation != this.gridDestination;}}

    private Point gridDestination;
    private Texture2D baseTexture;
    




    public Player(Point spawnGridPosition)
        : base(Faction.Player, spawnGridPosition)
    {
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        this.gridDestination = GridLocation;
    }

    public override void Draw()
    {
        GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
        base.Draw();
    }

    int opertunityCounter = 0;
    public override void Tick()
    {
        base.Tick();

        this.HandleMouse(GameWindow.contextMouseState, GameWindow.interactingWithUI);
        if (this.IsMoving)
        {
            int currentValue = Building.grid.GetPlayerValue(GridArea.X, GridArea.Y);
            if (opertunityCounter++ > movementRate)
            {
                opertunityCounter = 0;
                int nextValue = currentValue;
                Point nextPos = this.GridArea.Location;
                for (int i = 0; i < Grid.offsets.Length / 2; i++)
                {
                    int newX = GridArea.X + Grid.offsets[i * 2];
                    int newY = GridArea.Y + Grid.offsets[i * 2 + 1];
                    int newValue = Building.grid.GetPlayerValue(newX, newY);
                    if (newValue > nextValue)
                    {
                        if (Building.grid.IsTileTaken(newX, newY) == false)
                        {
                            nextValue = newValue;
                            nextPos = new Point(newX, newY);
                        }
                    }
                }
                
                //  verification of the new position has already been done
                this.GridArea = new Rectangle(nextPos, new Point(1,1));
            }            
        }
    }
    private void HandleMouse(MouseState mouseState, bool interactingWithUI)
    {

        if (!interactingWithUI)
        {
            Point mousePosition = mouseState.Position;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 worldPoint = Camera.ScreenToWorld(new Vector2(mousePosition.X, mousePosition.Y));
                Point gridPoint = Grid.WorldToGrid(new Point((int)worldPoint.X, (int)worldPoint.Y));
                if ((this.gridDestination != gridPoint) && Building.grid.InsideBounds(gridPoint))
                {
                    Console.WriteLine("     Recalculateing player destination...");
                    this.gridDestination = gridPoint;
                    Building.grid.CalculatePlayerValue(this.gridDestination);
                    Console.WriteLine("     Done!");
                }
            }
        }

    }

}
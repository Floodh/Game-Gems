using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player : Unit
{
    public const string Path_BaseTexture_0 = "Data/Texture/Units/wizard1.png";
    public const string Path_BaseTexture_1 = "Data/Texture/Units/smileyOrb.png";

    //Data\Texture\Units\smileyOrb.png


    public Point GridLocation { get { return this.GridArea.Location; } }
    public bool IsMoving { get { return this.GridLocation != this.gridDestination || currentTarget != null; } }
    private Targetable currentTarget = null;

    private Point gridDestination;
    private Texture2D baseTexture;


    private Animation numberAnimation;
    private Mineral.Type collectionBonus;





    public Player(Point spawnGridPosition, Mineral.Type collectionBonus, GameArguments.Avatar avatar)
        : base(Faction.Player, spawnGridPosition)
    {
        if (avatar == GameArguments.Avatar.Wizard)
            this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture_0);
        else if (avatar == GameArguments.Avatar.Orb)
            this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture_1);
        else
            throw new ArgumentException($"Invalid avatar : {avatar}");
        this.gridDestination = GridLocation;
        this.MoveTo(gridDestination);
        this.collectionBonus = collectionBonus;
    }

    public override void Draw()
    {
        Rectangle playerRect = new(DrawArea.X, DrawArea.Y - 8, DrawArea.Width, DrawArea.Height);
        GameWindow.spriteBatch.Draw(baseTexture, playerRect, Sunlight.Mask);
        base.Draw();
    }

    int opertunityCounter = 0;
    public override void Tick()
    {
        base.Tick();

        this.HandleMouse(
            GameWindow.contextMouseState,
            GameWindow.interactingWithUI,
            GameWindow.interactingWithContextMenu,
             GameWindow.interactingWithSelectableBuilding);
        if (this.IsMoving)
        {
            int currentValue = Building.grid.GetPlayerValue(GridArea.X, GridArea.Y);
            if (opertunityCounter++ > movementRate)
            {
                opertunityCounter = 0;
                if (this.currentTarget != null && currentValue == int.MaxValue)
                {
                    this.currentTarget.PlayerInteraction();
                    if (this.currentTarget.mineralType != null)
                    {
                        int number = Booster.GetGemBonus((Mineral.Type)this.currentTarget.mineralType);
                        this.numberAnimation = new NumberAnimation(Grid.ToDrawArea(this.GridArea), $"+{number}", Mineral.ToMineralColor((Mineral.Type)this.currentTarget.mineralType));
                        this.numberAnimation.drawArea = this.DrawArea;
                        this.numberAnimation.drawArea.Offset(0, -this.DrawArea.Height * 0.85f);
                        this.numberAnimation.Play();

                        //  collection bonus
                        if (this.currentTarget.mineralType == this.collectionBonus)
                        {
                            this.opertunityCounter = movementRate / 3;
                        }
                    }

                }
                else
                {
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
                    if (nextPos != this.GridArea.Location)
                    {
                        this.MoveToFrom(nextPos, this.GridArea.Location);
                        this.GridArea = new Rectangle(nextPos, new Point(1, 1));
                        Building.grid.CalculateEnemyValue();
                    }
                }
            }
        }
    }
    private void HandleMouse(MouseState mouseState, bool interactingWithUI, bool interactingWithContextMenu, bool interactingWithSelectableBuilding)
    {

        if (!interactingWithUI && !interactingWithContextMenu && !interactingWithSelectableBuilding)
        {
            Point mousePosition = InputManager.WorldMousePosition.ToPoint();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 worldPoint = new Vector2(mousePosition.X, mousePosition.Y);
                Point gridPoint = Grid.WorldToGrid(new Point((int)worldPoint.X, (int)worldPoint.Y));

                if ((this.gridDestination != gridPoint) && Building.grid.InsideBounds(gridPoint))
                    if (!Building.grid.IsTileTaken(gridPoint))
                    {
                        this.currentTarget = null;
                        this.gridDestination = gridPoint;
                        Building.grid.CalculatePlayerValue(this.gridDestination, this.GridLocation);
                    }
                    else
                    {
                        //  in this senario the player might try to move to attack a boulder or mine a resource
                        Targetable ocupant = Building.grid.FindOcupant(gridPoint);
                        if (ocupant != null)
                        {
                            if (currentTarget != ocupant)
                                Building.grid.CalculatePlayerValue(ocupant, this.GridLocation);
                            this.currentTarget = ocupant;
                        }

                    }

            }
        }

    }
    protected override void Die()
    {
        base.Die();
        Building.grid.CalculateEnemyValue();
    }
}
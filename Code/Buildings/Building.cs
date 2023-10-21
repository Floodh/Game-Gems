using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;


abstract class Building : Targetable
{

    public static List<Building> allBuildings = new List<Building>();
    public static Grid grid;
    public EState State = EState.Normal;
    

    public static void SetGrid(Bitmap sourceImage)
    {
        grid = new Grid(sourceImage);
    }

    public static void DrawAll()
    {
        foreach (Building building in allBuildings)
        {
            building.Draw();
        }
    }

    public static void TickAll()
    {
        for (int i = 0; i < allBuildings.Count; i++)
        {
            Building building = allBuildings[i];
            building.Tick();
            if (building.IsDead)
                i--;
        }
    }

    public override Rectangle GridArea {get; protected set;} = Rectangle.Empty;

    public override Point TargetPosition {
        get 
        {
            return new Point(
                DrawArea.X + DrawArea.Width / 2,
                DrawArea.Y + DrawArea.Height / 2               
            );
        }
    }

    public Building(Faction faction)
        : base(faction)
    {
        
    }
    
    //  can overide this
    public static Size GridSize{
        get {return new Size(2,2);}
    }
    public Rectangle DrawArea
    {   get
        {   return
            Grid.ToDrawArea(GridArea);
        }      
    }

    public bool isSelected = false;

    public virtual void Draw()
    {
        if(this.State == EState.Selected)
        {
            Texture2D _texture;
            _texture = new Texture2D(GameWindow.graphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.White });

            GameWindow.spriteBatch.Draw(_texture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Color.White * 0.5f);
        }
    }

    // public virtual void Action()
    // {}

    public override void Tick()
    {
        base.Tick();
    }

    public bool Place(int x, int y)
    {
        return Place(new Point(x, y));
    }

    public bool Place(Point position)
    {

        Rectangle area = new Rectangle(position.X, position.Y, GridSize.Width, GridSize.Height);
        return Place(area);

    }

    private bool Place(Rectangle area)
    {
        //Console.WriteLine(area);

        if (grid.PlaceIfPossible(this, area))
        {
            allBuildings.Add(this);
            this.GridArea = area;
            if (this.faction != Faction.Neutral)
                grid.CalculateEnemyValue();
            return true;
        }
        return false;

    }

    //  returns true if health is negative
    public override bool Hit(Projectile projectile)
    {
        this.TakeDmg(projectile); 
        return this.Hp <= 0;
    }

    public abstract Building CreateNew();

    protected override void Die()
    {
        this.IsDead = true;
        //Console.WriteLine($"died {}");
        allBuildings.Remove(this);  //  consider delaying the removal of this object from the list for potential death animation
        grid.RemoveBuilding(this);
    }

    public static void UpdateAllByMouse(MouseState mouseState)
    {
        if(mouseState.LeftButton == ButtonState.Pressed)
        {
            if(Building.selectedBuilding != null)
            {
                Building.selectedBuilding.State = EState.Normal;
                Building.selectedBuilding = null;
            }
            
            foreach(Building building in Building.GetClickableBuildings())
            {
                Point gridPoint = Building.GetMouseGridPoint(mouseState);
                if(building.GridArea.Contains(gridPoint))
                {
                    Building.selectedBuilding = building;
                    Building.selectedBuilding.State = EState.Normal;
                    Console.WriteLine("Press on: " + building.ToString());
                }
            }
        }  
        else if(mouseState.LeftButton == ButtonState.Released)
        {    
            if(Building.selectedBuilding != null && Building.selectedBuilding.State != EState.Selected)
            {
                Point gridPoint = Building.GetMouseGridPoint(mouseState);
                // Ensure that press and release is on the same building
                if(Building.selectedBuilding.GridArea.Contains(gridPoint)) 
                {
                    Building.selectedBuilding.State = EState.Selected;

                    Console.WriteLine("Selected: " + Building.selectedBuilding.ToString());
                }  
            }  
        }

        if(mouseState.RightButton == ButtonState.Pressed)  
        {
            if(Building.selectedBuilding != null)
            {
                Building.selectedBuilding.State = EState.Normal;
                Building.selectedBuilding = null;
            }
        }
    }

    public enum EState 
    {
        Normal = 0,
        Selected = 1,
        BuildingTransition = 2,
        DestroyTransition = 3
    }

    public static Building selectedBuilding = null;

    public virtual void UpdateByMouse(MouseState mouseState)
    {
        Console.WriteLine("UpdateByMouse| " + this.ToString());
    }

    public static List<Building> GetClickableBuildings()
    {
        List<Building>list = new();
        foreach (Building building in Building.allBuildings)
        {
            // TODO Implement IClickable or something
            if(building.faction == Faction.Player && building.GetType() == typeof(Cannon))
            {
                list.Add(building);
            }
        }
        return list;
    }

    public static Point GetMouseGridPoint(MouseState mouseState)
    {
        Vector2 worldVec = Camera.ScreenToWorld(new Vector2(mouseState.X, mouseState.Y));
        Point gridPoint = Grid.WorldToGrid(worldVec.ToPoint());
        return gridPoint;
    }

    // If child doesnt have a ToString()
    public override string ToString()
    {
        Type type = this.GetType();
        return $"Building| Pos:{this.GridArea.ToString()}, Type:{type.ToString()}, Faction:{this.faction.ToString()}";
    }  
}


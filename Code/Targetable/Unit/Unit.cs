using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

abstract class Unit : Targetable
{
    private const string Path_DeathSoundEffect = "Data/Audio/SoundEffects/EnemyDeath.wav";

    protected const int movementRate = 26;

    public static List<Unit> allUnits = new List<Unit>();

    private static SoundEffect deathSoundSeffect;
    private static SoundEffectInstance deathSoundSeffect_instance;
    
    

    public static void DrawAll()
    {
        foreach (Unit unit in allUnits)
        {
            unit.Draw();
        }
    }

    public static void TickAll()
    {
        for (int i = 0; i < allUnits.Count; i++)
        {
            Unit unit = allUnits[i];
            if (!unit.IsDead)
                unit.Tick();
            else
                i--;
        }
    }

    public override Point TargetPosition {
        get 
        {
            return new Point(
                DrawArea.X + DrawArea.Width / 2,
                DrawArea.Y + DrawArea.Height / 2               
            );
        }
    }

    protected HealthBar hpBar;

    public Unit(Faction faction, Point gridPosition)
        : base(faction)
    {
        allUnits.Add(this);
        this.GridArea = new Rectangle(gridPosition, new Point(1,1));
        hpBar = new HealthBar(this);
    }

    protected Rectangle DrawArea 
    {
        get 
        {
            return Grid.ToDrawArea(GridArea);
        }
    }
    
    public virtual void Draw()
    {
        this.hpBar.Draw();
    }

    public override void Tick()
    {
        base.Tick();
        this.hpBar.Update();
    }

    protected override void Die()
    {
        base.Die();
        this.IsDead = true;
        allUnits.Remove(this);
        this.MoveFrom(this.GridArea.Location);
        if (deathSoundSeffect == null)
        {
            deathSoundSeffect = SoundEffect.FromFile(Path_DeathSoundEffect);
            deathSoundSeffect_instance = deathSoundSeffect.CreateInstance();
            deathSoundSeffect_instance.Volume = 0.2f;
        }
        deathSoundSeffect_instance.Play();        
    }

    //  returns true if health is negative
    public override bool Hit(Projectile projectile)
    {
        this.TakeDmg(projectile); 
        return this.Hp <= 0;
    }

    protected bool CanMoveTo(Point gridPosition)
    {
        return !Building.grid.IsTileTaken(gridPosition.X, gridPosition.Y);
    }
    protected void MoveTo(Point gridDestination)
    {
        if (Building.grid.IsTileTaken(gridDestination))
            throw new ArgumentException($"Tried to move to a taken tile! {gridDestination}");

        Building.grid.Mark(gridDestination, true);
    }
    protected void MoveFrom(Point gridOrigin)
    {
        if (!Building.grid.IsTileTaken(gridOrigin))
            throw new ArgumentException($"Tried to move from a position that isn't taken by anything! {gridOrigin}");
        Building.grid.Mark(gridOrigin, false);
    }
    protected void MoveToFrom(Point gridDestination, Point gridOrigin)
    {
        if (gridDestination == gridOrigin)
            throw new ArgumentException("Tried to move to the same position!");
        MoveFrom(gridOrigin);
        MoveTo(gridDestination);
    }

}

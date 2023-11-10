using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using MonoGame.Extended.Particles.Modifiers.Interpolators;

class Weapon
{
    protected Unit _parent = null;
    private int _projectileTextureId;
    private float _scale = 0.15f;
    private int _attackRate = 50;
    private int _damage = 10;
    private float _projectileSpeed = 1f;
    private int _projectileSpawnOffset = 30;

    public Weapon(Unit parent, int projectileTextureId)
    {
        _parent = parent;
        _projectileTextureId = projectileTextureId;
    }

    public int ProjectileTextureId { get => _projectileTextureId; set => _projectileTextureId = value; }
    public float Scale { get => _scale; set => _scale = value; }
    public int AttackRate { get => _attackRate; set => _attackRate = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public float ProjectileSpeed { get => _projectileSpeed; set => _projectileSpeed = value; }
    public int ProjectileSpawnOffset { get => _projectileSpawnOffset; set => _projectileSpawnOffset = value; }

    public void Tick(Targetable target, ref int opertunityCounter)
    {   
        opertunityCounter++;
        if (opertunityCounter >= _attackRate)
        {
            _ = new Projectile(
                _damage, 
                0, // energy transfer
                _projectileSpeed,
                target,
                _parent.TargetPosition.ToVector2(),
                _projectileTextureId,
                _projectileSpawnOffset,
                null, 
                _scale);

            opertunityCounter = 0;
        }                     
    }     
}
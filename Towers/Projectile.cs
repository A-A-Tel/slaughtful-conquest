using System;
using Godot;
using SlaughtfulConquest.Enemies;

namespace SlaughtfulConquest.Towers;

public abstract partial class Projectile : Area2D
{
    public Vector2? Angle { get; set; }
    protected abstract float Speed { get; }
    public int Damage { get; set; }

    public override void _Ready()
    {
        AreaEntered += OnAreaEnter;
    }

    public override void _Process(double delta)
    {
        if (Angle == null) return;
        
        Position += Angle.Value * Speed * (float)delta;
    }

    private void OnAreaEnter(Area2D area)
    {
        Enemy enemy = area.GetParentOrNull<Enemy>();

        if (enemy == null) return;
        
        enemy.TakeDamage(Damage);
        Console.WriteLine(Damage);
        QueueFree();
    }
}
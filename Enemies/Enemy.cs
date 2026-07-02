using System;
using Godot;

namespace SlaughtfulConquest.Enemies;

public abstract partial class Enemy : PathFollow2D
{
    protected abstract float Speed { get; }
    protected abstract int MaxHealth { get; }
    protected abstract int Health { get; set; }
    protected abstract int Reward { get; }

    private Sprite2D _texture;
    private Vector2 _previousProgress;

    public override void _Ready()
    {
        _texture = GetNode<Sprite2D>("Texture");
        Loop = false;
        Rotates = false;
        Health = MaxHealth;
        Rotation = 0;

        _previousProgress = Position;
    }
    
    public override void _Process(double delta)
    {
        ProgressRatio += Speed * (float)delta;

        

        _texture.FlipH = Position <= _previousProgress;
    
        _previousProgress = Position;
        if (Progress >= 1880) Kamikaze();
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health > 0) return;
        
        Globals.CurrentMap.Bones += Reward;
        QueueFree();
    }

    private void Kamikaze()
    {
        Globals.CurrentMap.Health -= Health;
        QueueFree();
    }
}
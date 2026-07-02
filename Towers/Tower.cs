using Godot;
using SlaughtfulConquest.Enemies;

namespace SlaughtfulConquest.Towers;

public abstract partial class Tower : Node2D
{
    protected abstract float Radius { get; set; }
    protected abstract float FireRate { get; set;  }
    protected abstract int Damage { get; set; }

    private Area2D _detection;
    private CollisionShape2D _shape;
    private Sprite2D _texture;
    private Projectile _projectile;
    private Timer _cooldown;

    public override void _Ready()
    {
        _detection = GetNode<Area2D>("DetectionArea");
        _shape = GetNode<CollisionShape2D>("DetectionArea/CollisionShape2D");
        _texture = GetNode<Sprite2D>("Texture");
        _projectile = GetNode<Projectile>("Projectile");
        _cooldown = GetNode<Timer>("Cooldown");

        CircleShape2D shape = new();
        shape.Radius = Radius;
        _shape.Shape = shape;

        _detection.AreaEntered += OnAreaEnter;
        
        _projectile.Damage = Damage;
        
        _cooldown.WaitTime = FireRate;
        _cooldown.Timeout += OnCooldownEnd;
    }

    private void OnAreaEnter(Area2D area)
    {
        if (!_cooldown.IsStopped()) return;
        Enemy enemy = area.GetParentOrNull<Enemy>();

        if (enemy == null) return;
        Fire(enemy);
    }

    private void OnCooldownEnd()
    {
        Vector2 position = new (0,0);
        _texture.SetRegionRect(new Rect2(position, _texture.RegionRect.Size));

        foreach (Area2D area in _detection.GetOverlappingAreas())
        {
            Enemy enemy = area.GetParentOrNull<Enemy>();

            if (enemy == null) continue;
            Fire(enemy);
            break;
        }
    }

    private void Fire(Enemy enemy)
    {
        if (_projectile.Duplicate() is not Projectile newProjectile) return;

        AddChild(newProjectile);
        newProjectile.Show();
        newProjectile.Damage = Damage;

        Vector2 direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
        newProjectile.Angle = direction;

        Vector2 position = new(32, 0);
        _texture.SetRegionRect(new Rect2(position, _texture.RegionRect.Size));
        _cooldown.Start();
    }
}
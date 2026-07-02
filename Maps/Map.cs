using System.IO;
using Godot;
namespace SlaughtfulConquest.Maps;
public abstract partial class Map : Node2D
{
    protected Path2D EnemyPath { get; set; }

    public PackedScene SlingshotTowerScene { get; set; }
    public PackedScene GorillaTowerScene { get; set; }

    private const int SlingshotCost = 20;
    private const int GorillaCost = 50;

    private int _bones;
    public int Bones
    {
        get => _bones;
        set
        {
            _boneLabel.Text = value.ToString();
            _bones = value;
        }
    }
    private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _healthLabel.Text = value.ToString();
            _health = value;
        }
    }
    private RichTextLabel _healthLabel;
    private RichTextLabel _boneLabel;
    
    public override void _Ready()
    {
        Globals.CurrentMap = this;
        
        _healthLabel = GetNode<RichTextLabel>("HealthLabel");
        _boneLabel = GetNode<RichTextLabel>("BoneLabel");
        EnemyPath = GetNode<Path2D>("EnemyPath");

        SlingshotTowerScene = ResourceLoader.Load<PackedScene>("uid://dh1diaackduc2");
        GorillaTowerScene = ResourceLoader.Load<PackedScene>("uid://07svc47rx7tc");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey { Pressed: true, Echo: false } keyEvent) return;

        switch (keyEvent.Keycode)
        {
            case Key.S:
                TryPlaceTower(SlingshotTowerScene, SlingshotCost);
                break;
            case Key.G:
                TryPlaceTower(GorillaTowerScene, GorillaCost);
                break;
        }
    }

    private void TryPlaceTower(PackedScene towerScene, int cost)
    {
        if (towerScene == null) return;
        if (Bones < cost) return;

        if (towerScene.Instantiate() is not Towers.Tower tower) return;

        tower.GlobalPosition = GetGlobalMousePosition();
        AddChild(tower);
        Bones -= cost;
    }
}
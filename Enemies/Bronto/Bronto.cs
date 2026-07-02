using Godot;

namespace SlaughtfulConquest.Enemies.Bronto;

public partial class Bronto : Enemy
{
	protected override float Speed => 0.0375f;
	protected override int MaxHealth => 500;
	protected override int Health { get; set; }
	protected override int Reward => 20;
}

using System;

namespace SlaughtfulConquest.Enemies.Trike;

public partial class Trike : Enemy
{
	protected override float Speed => Health < MaxHealth - Health ? 0.08f : 0.05f;
	protected override int MaxHealth => 100;
	protected override int Health { get; set; }
	protected override int Reward => 10;
}

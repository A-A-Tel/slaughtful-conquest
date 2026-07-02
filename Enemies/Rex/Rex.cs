namespace SlaughtfulConquest.Enemies.Rex;

public partial class Rex : Enemy
{
	protected override float Speed => 0.075f;
	protected override int MaxHealth => 350;
	protected override int Health { get; set; }
	protected override int Reward => 50;
}

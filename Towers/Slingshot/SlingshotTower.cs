namespace SlaughtfulConquest.Towers.Slingshot;

public partial class SlingshotTower : Tower
{
	protected override float Radius { get; set; } = 75f;
	protected override float FireRate { get; set; } = 0.5f;
	protected override int Damage { get; set; } = 80;
}

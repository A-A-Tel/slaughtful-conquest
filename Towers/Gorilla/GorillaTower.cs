namespace SlaughtfulConquest.Towers.Gorilla;

public partial class GorillaTower : Tower
{
    protected override float Radius { get; set; } = 125f;
    protected override float FireRate { get; set; } = 0.6f;
    protected override int Damage { get; set; } = 180;
}
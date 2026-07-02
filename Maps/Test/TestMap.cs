using Godot;

namespace SlaughtfulConquest.Maps.Test;

public partial class TestMap : Map
{
	public override void _Ready()
	{
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			Globals.Ui.Open("uid://dy1l22h3u34at");
		}
	}
}

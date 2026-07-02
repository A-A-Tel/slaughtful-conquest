using System.Linq;
using Godot;

namespace SlaughtfulConquest.Main;

public partial class Game : Node2D
{
	public override void _Ready()
	{
		Globals.Game = this;
	}
	
	public void Add(string path)
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>(path);
		AddChild(scene.Instantiate());
	}
	
	public void Remove<TMenu>()
		where TMenu : Node2D
	{
		TMenu menu = GetChildren()
			.OfType<TMenu>()
			.FirstOrDefault();

		if (menu != null)
			TerminateChild(menu);
	}

	private void TerminateChild(Node node)
	{
		RemoveChild(node);
		node.QueueFree();
	}
}

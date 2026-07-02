using System.Linq;
using Godot;

namespace SlaughtfulConquest.Main;

public partial class Ui : CanvasLayer
{
	public override void _Ready()
	{
		Globals.Ui = this;
		CallDeferred(nameof(Open), "uid://bghlty6bw37pv");
	}

	public void Open(string path)
	{
		PackedScene scene = GD.Load<PackedScene>(path);
		AddChild(scene.Instantiate());
	}

	
	public void Close<TMenu>()
		where TMenu : Control
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

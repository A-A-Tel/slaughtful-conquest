using Godot;
using SlaughtfulConquest.Data;

namespace SlaughtfulConquest.Bootstrap;

public partial class Entry : Node
{
	public override void _Ready()
	{
		Globals.Settings = new SettingsManager();
		Globals.Settings.LoadSettings();
		GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, "uid://dk20qy0xfglyb");
	}
}

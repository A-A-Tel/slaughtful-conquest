using Godot;
using SlaughtfulConquest.Main;

namespace SlaughtfulConquest.Menus.Main;

public partial class MainMenu : Control
{
	private Button _playButton;
	private Button _settingsButton;
	private Button _quitButton;
	
	public override void _Ready()
	{
		_playButton = GetNode<Button>("Play");
		_settingsButton = GetNode<Button>("Settings");
		_quitButton = GetNode<Button>("Quit");
		
		_playButton.Pressed += OnPlayButtonPressed;
		_settingsButton.Pressed += OnSettingsButtonPressed;
		_quitButton.Pressed += OnQuitButtonPressed;
	}

	private static void OnPlayButtonPressed()
	{
		Globals.Game.Add("uid://h5cyxtybvs5e");
		Globals.Ui.Close<MainMenu>();
	}

	private static void OnSettingsButtonPressed()
	{
		Globals.Ui.Open("uid://djksu3dv4b3n4");
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}

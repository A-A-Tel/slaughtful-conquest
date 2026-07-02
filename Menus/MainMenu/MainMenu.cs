using System;
using Godot;
	
namespace SlaughtfulConquest.Menus.MainMenu;

public partial class MainMenu : Control
{
	private Button _playButton;
	private Button _settingsButton;
	
	public override void _Ready()
	{
		_playButton = GetNode<Button>("Play");
		_settingsButton = GetNode<Button>("Settings");
		
		_playButton.Pressed += OnPlayButtonPressed;
		_settingsButton.Pressed += OnSettingsButtonPressed;
	}

	private void OnPlayButtonPressed()
	{
		Console.WriteLine("Pressed");
	}

	private void OnSettingsButtonPressed()
	{
		Globals.Ui.Open("uid://djksu3dv4b3n4");
	}
}

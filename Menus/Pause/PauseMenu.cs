using Godot;
using SlaughtfulConquest.Menus.Settings;

namespace SlaughtfulConquest.Menus.Pause;

public partial class PauseMenu : Control
{
	private Button _resumeButton;
	private Button _settingsButton;
	private Button _quitButton;
	
	public override void _Ready()
	{
		GetTree().Paused = true;
		
		_resumeButton = GetNode<Button>("Resume");
		_settingsButton = GetNode<Button>("Settings");
		_quitButton = GetNode<Button>("Quit");
		
		_resumeButton.Pressed += OnResumePressed;
		_settingsButton.Pressed += OnSettingsPressed;
		_quitButton.Pressed += OnQuitPressed;
		
		
	}
	
	private void OnResumePressed()
	{
		GetTree().Paused = false;
		Globals.Ui.Close<PauseMenu>();
	}

	private static void OnSettingsPressed()
	{
		Globals.Ui.Open("uid://djksu3dv4b3n4");
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!@event.IsActionPressed("pause")) return;
		
		GetViewport().SetInputAsHandled();
		Globals.Ui.Close<SettingsMenu>();
		OnResumePressed();
	}
}

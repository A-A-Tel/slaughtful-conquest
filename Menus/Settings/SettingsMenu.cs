using System.Collections.Generic;
using Godot;
using SlaughtfulConquest.Data;

namespace SlaughtfulConquest.Menus.Settings;

public partial class SettingsMenu : Control
{
	private List<Vector2I> _resolutions = [];
	
	private Slider _masterSlider;
	private Slider _musicSlider;
	private Slider _effectsSlider;

	private OptionButton _resolutionSelect;
	private OptionButton _windowModeSelect;

	private Slider _fpsSlider;
	private CheckBox _vsyncCheckBox;

	private Button _saveButton;

	public override void _Ready()
	{
		_masterSlider = GetNode<Slider>("Master");
		_musicSlider = GetNode<Slider>("Music");
		_effectsSlider = GetNode<Slider>("Effects");
		
		_resolutionSelect = GetNode<OptionButton>("Resolution");
		_windowModeSelect = GetNode<OptionButton>("WindowMode");
		
		_fpsSlider = GetNode<Slider>("Fps");
		_vsyncCheckBox = GetNode<CheckBox>("Vsync");
		
		_saveButton = GetNode<Button>("Save");


		_masterSlider.ValueChanged += OnMasterChange;
		_musicSlider.ValueChanged += OnMusicChange;
		_effectsSlider.ValueChanged += OnEffectsChange;

		_resolutionSelect.ItemSelected += OnResolutionChange;
		_windowModeSelect.ItemSelected += OnWindowModeChange;

		_fpsSlider.ValueChanged += OnFpsChange;
		_vsyncCheckBox.Toggled += OnVsyncChange;

		_saveButton.Pressed += OnSavePressed;
		
		SetFields();
	}

	private void SetFields()
	{
		_masterSlider.Value = Globals.Settings.MasterVolume;
		_musicSlider.Value = Globals.Settings.MusicVolume;
		_effectsSlider.Value = Globals.Settings.EffectsVolume;
		
		SetResolutionFields();
		_windowModeSelect.Selected = (int)Globals.Settings.WindowModeSetting;

		_fpsSlider.Value = Globals.Settings.FpsLock;
		_vsyncCheckBox.ButtonPressed = Globals.Settings.Vsync;
	}

	private void SetResolutionFields()
	{
		Vector2I screen = DisplayServer.ScreenGetSize();
		bool isGoldenRatio = screen.X * 10 == screen.Y * 16;
		
		_resolutions.Clear();
		_resolutionSelect.Clear();
		
		_resolutions.AddRange([
			new Vector2I(1280, 720),
			new Vector2I(1920, 1080),
			new Vector2I(2560, 1440),
			new Vector2I(3840, 2160),
			new Vector2I(7680, 4320)
		]);

		if (isGoldenRatio)
		{
			_resolutions.AddRange([
				new Vector2I(1280, 800),
				new Vector2I(1440, 900),
				new Vector2I(1680, 1050),
				new Vector2I(1920, 1200),
				new Vector2I(2560, 1600),
				new Vector2I(3840, 2400)
			]);
		}
		
		_resolutions.Sort((first, second) => first.Y - second.Y);
		for (int i = 0; i < _resolutions.Count; i++)
		{
			var resolution = _resolutions[i];

			if (resolution.X > screen.X || resolution.Y > screen.Y) continue;
			
			_resolutionSelect.AddItem($"{resolution.X}x{resolution.Y}", i);

			if (resolution == Globals.Settings.Resolution)
				_resolutionSelect.Selected = _resolutionSelect.ItemCount - 1;
		}
	}
	

	private static void OnMasterChange(double value)
	{
		Globals.Settings.MasterVolume = (int)value;
	}

	private static void OnMusicChange(double value)
	{
		Globals.Settings.MusicVolume = (int)value;
	}

	private static void OnEffectsChange(double value)
	{
		Globals.Settings.EffectsVolume = (int)value;
	}


	private void OnResolutionChange(long index)
	{
		Globals.Settings.Resolution = _resolutions[_resolutionSelect.Selected];
	}

	private void OnWindowModeChange(long index)
	{
		Globals.Settings.WindowModeSetting = (WindowMode)_windowModeSelect.Selected;
	}


	private static void OnFpsChange(double value)
	{
		Globals.Settings.FpsLock = (int)value;
	}

	private static void OnVsyncChange(bool state)
	{
		Globals.Settings.Vsync = state;
	}


	private void OnSavePressed()
	{
		Globals.Settings.SaveSettings();
		Globals.Ui.Close<SettingsMenu>();
		QueueFree();
	}
}

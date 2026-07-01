using System;
using Godot;

namespace SlaughtfulConquest.Data;

public enum WindowMode
{
    Borderless,
    Fullscreen,
    Windowed
}

public partial class SettingsManager : Node
{
    private const string ConfigPath = "user://config/settings.ini"; 
    
    public int MasterVolume { get; set; } = 80;
    public int MusicVolume { get; set; } = 80;
    public int EffectsVolume { get; set; } = 80;
    
    public Vector2I Resolution { get; set; } = new(1280, 720);
    public WindowMode WindowModeSetting { get; set; } = WindowMode.Borderless;
    
    public int FpsLock { get; set; } = 120;
    public bool Vsync { get; set; } = true;

    public void LoadSettings()
    {
        if (FileAccess.FileExists(ConfigPath))
        {
            ConfigFile config = new();
            config.Load(ConfigPath);
            
            MasterVolume = config.GetValue("audio", nameof(MasterVolume), MasterVolume).As<int>();
            MusicVolume = config.GetValue("audio", nameof(MusicVolume), MusicVolume).As<int>();
            EffectsVolume = config.GetValue("audio", nameof(EffectsVolume), EffectsVolume).As<int>();
            
            Resolution = config.GetValue("video", nameof(Resolution), Resolution).As<Vector2I>();
            WindowModeSetting = config.GetValue("video", nameof(WindowModeSetting)).As<WindowMode>();
            
            FpsLock = config.GetValue("frames", nameof(FpsLock), FpsLock).As<int>();
            Vsync = config.GetValue("frames", nameof(Vsync), Vsync).As<bool>();

            ApplyAllSettings();
        }
        else
        {
            SaveSettings();
        }
    }

    public void SaveSettings()
    {
        ConfigFile config = new();
        
        config.SetValue("audio", nameof(MasterVolume), MasterVolume);
        config.SetValue("audio", nameof(MusicVolume), MusicVolume);
        config.SetValue("audio", nameof(EffectsVolume), EffectsVolume);
        
        config.SetValue("video", nameof(Resolution), Resolution);
        config.SetValue("video", nameof(WindowModeSetting), (int)WindowModeSetting);
        
        config.SetValue("frames", nameof(FpsLock), FpsLock);
        config.SetValue("frames", nameof(Vsync), Vsync);
        
        config.Save(ConfigPath);
        ApplyAllSettings();
    }

    private void ApplyAllSettings()
    {
        ChangeWindowMode();
        ApplyResolution();
        ApplyVsync();
        ApplyFpsLock();

        SetBusVolume("Master", MasterVolume);
        SetBusVolume("Music", MusicVolume);
        SetBusVolume("SFX", EffectsVolume);
    }

    private void ChangeWindowMode()
    {
        switch (WindowModeSetting)
        {
            case WindowMode.Borderless:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
                break;

            case WindowMode.Fullscreen:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                break;

            case WindowMode.Windowed:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ApplyResolution()
    {
        if (WindowModeSetting != WindowMode.Windowed) return;
        
        DisplayServer.WindowSetSize(Resolution);
        CenterWindow();
    }

    private void ApplyVsync()
    {
        DisplayServer.VSyncMode vsyncMode = Vsync 
            ? DisplayServer.VSyncMode.Enabled 
            : DisplayServer.VSyncMode.Disabled;
            
        DisplayServer.WindowSetVsyncMode(vsyncMode);
    }

    private void ApplyFpsLock()
    {
        Engine.MaxFps = FpsLock > 240 ? 0 : FpsLock;
    }

    private static void CenterWindow()
    {
        Vector2I screenSize = DisplayServer.ScreenGetSize();
        Vector2I windowSize = DisplayServer.WindowGetSize();
        Vector2I centerPos = screenSize / 2 - windowSize / 2;
        DisplayServer.WindowSetPosition(centerPos);
    }

    private static void SetBusVolume(string busName, int volumePercentage)
    {
        int busIndex = AudioServer.GetBusIndex(busName);
        if (busIndex == -1)
        {
            GD.PrintErr($"Audio bus '{busName}' not found! Check your Godot Audio mixer panel.");
            return;
        }

        float alpha = Math.Clamp(volumePercentage / 100f, 0f, 1f);

        float db = Mathf.LinearToDb(alpha);
        
        AudioServer.SetBusVolumeDb(busIndex, db);

        AudioServer.SetBusMute(busIndex, volumePercentage <= 0);
    }
}
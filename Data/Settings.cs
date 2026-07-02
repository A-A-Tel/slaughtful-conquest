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
    private const string ConfigPath = "user://settings.ini"; 
    
    public int MasterVolume { get; set; }
    public int MusicVolume { get; set; }
    public int EffectsVolume { get; set; }
    
    public Vector2I Resolution { get; set; }
    public WindowMode WindowModeSetting { get; set; }
    
    public int FpsLock { get; set; }
    public bool Vsync { get; set; }

    public void LoadSettings()
    {
        ConfigFile config = new();
        config.Load(ConfigPath);
        
        MasterVolume = config.GetValue("audio", nameof(MasterVolume), 80).As<int>();
        MusicVolume = config.GetValue("audio", nameof(MusicVolume), 80).As<int>();
        EffectsVolume = config.GetValue("audio", nameof(EffectsVolume), 80).As<int>();
        
        Resolution = config.GetValue("video", nameof(Resolution), new Vector2I(1280,720)).As<Vector2I>();
        WindowModeSetting = (WindowMode)config.GetValue("video", nameof(WindowModeSetting), 0).As<int>();
        
        FpsLock = config.GetValue("frames", nameof(FpsLock), 120).As<int>();
        Vsync = config.GetValue("frames", nameof(Vsync), false).As<bool>();

        if (FileAccess.FileExists(ConfigPath))
        {
            SaveSettings();
        }
        else
        {
            ApplyAllSettings();
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
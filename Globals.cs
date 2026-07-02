using SlaughtfulConquest.Data;
using SlaughtfulConquest.Main;
using SlaughtfulConquest.Maps;

namespace SlaughtfulConquest;

public static class Globals
{
    public static SettingsManager Settings { get; set; }
    public static Ui Ui { get; set; }
    public static Game Game { get; set; }
    public static Map CurrentMap { get; set; }
}
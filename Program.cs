using FallingSandSimulator;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

NativeWindowSettings nativeWindowSettings = new()
{
    Title = "Falling Sand Simulator",
    Flags = ContextFlags.ForwardCompatible,
    WindowState = WindowState.Fullscreen,
};

Vector2i worldSize = new(640, 480);

using (Game window = new(GameWindowSettings.Default, nativeWindowSettings, worldSize))
{
    window.UpdateFrequency = 60;
    window.Run();
}

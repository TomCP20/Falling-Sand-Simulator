using FallingSandSimulator;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
{
    Title = "Falling Sand Simulator",
    Flags = ContextFlags.ForwardCompatible,
    WindowState = WindowState.Fullscreen,
};

using (Game window = new(GameWindowSettings.Default, nativeWindowSettings))
{
    window.UpdateFrequency = 60;
    window.Run();
}

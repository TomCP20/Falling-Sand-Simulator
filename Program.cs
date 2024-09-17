using FallingSandSimulator;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

NativeWindowSettings nativeWindowSettings = new()
{
    Title = "Falling Sand Simulator",
    Flags = ContextFlags.ForwardCompatible,
};

using (Game window = new(GameWindowSettings.Default, nativeWindowSettings, 640, 480, 10))
{
    window.UpdateFrequency = 60;
    window.Run();
}

using FallingSandSimulator;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

const int screenWidth = 640;
const int screenHeight = 480;

NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
{
    ClientSize = new Vector2i(screenWidth, screenHeight),

    Title = "Falling Sand Simulator",
    // This is needed to run on macos
    Flags = ContextFlags.ForwardCompatible,
    WindowState = WindowState.Fullscreen,
};

using (Game window = new(GameWindowSettings.Default, nativeWindowSettings))
{
    window.UpdateFrequency = 60;
    window.Run();
}

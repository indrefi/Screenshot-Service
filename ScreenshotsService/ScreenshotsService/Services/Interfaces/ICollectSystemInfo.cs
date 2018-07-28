namespace ScreenshotsService.Services.Interfaces
{
    public interface ICollectSystemInfo
    {
        (int, int) GetDefaultDisplayWidthAndHeight();
    }
}

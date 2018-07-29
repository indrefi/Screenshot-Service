using ScreenshotsService.Models;
using System.Collections.Generic;

namespace ScreenshotsService.Services.Interfaces
{
    public interface IExecuteTask
    {
        List<ScreenshotResponseModel> Execute(List<string> urlList);
    }
}

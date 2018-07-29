using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.Extensions;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScreenshotsService.Services
{
    public class ProcessImagePhantomJS : IProcessImage
    {
        private readonly ILogger _Logger;
        private readonly IOptions<ImageConfigModel> _ImageOptions;

        public ProcessImagePhantomJS(ILogger<ProcessImagePhantomJS> logger, IOptions<ImageConfigModel> imageOptions)
        {
            _Logger = logger;
            _ImageOptions = imageOptions;
        }

        public async Task<MemoryStream> MakeScreenshot(string url, string hashValue)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var driver = new PhantomJSDriver(_ImageOptions.Value.PhantomJSDriverPath);

                    driver.Manage().Window.Maximize();
                    driver.Navigate().GoToUrl(url);

                    MemoryStream resultStream = new MemoryStream(driver.TakeScreenshot().AsByteArray);

                    driver.Quit();

                    return resultStream;
                }
                catch (Exception ex)
                {
                    _Logger.LogError($"Error occured: ", ex);

                    return null;
                }
            });
        }

        public Task<MemoryStream> MakeScreenshot(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}

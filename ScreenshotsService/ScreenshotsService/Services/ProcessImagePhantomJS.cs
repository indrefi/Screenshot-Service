using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.Extensions;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;
using System;
using System.IO;

namespace ScreenshotsService.Services
{
    public class ProcessImagePhantomJS : IProcessImage
    {
        private readonly ILogger _Logger;
        private readonly IOptions<ImageConfigModel> _ImageOptions;
        private readonly IOptions<PhantomJsSettingsModel> _PhantomJsOptinons;

        public ProcessImagePhantomJS(ILogger<ProcessImageScreenshot> logger, IOptions<ImageConfigModel>  imageOptions, IOptions<PhantomJsSettingsModel> phantomOptions )
        {
            _Logger = logger;
            _ImageOptions = imageOptions;
            _PhantomJsOptinons = phantomOptions;
        }

        public MemoryStream MakeScreenshot(string url, string hashValue, int width, int height)
        {
            try
            {
                var driver = new PhantomJSDriver(_PhantomJsOptinons.Value.PhantomJsDriverPath);

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
        }
    }
}

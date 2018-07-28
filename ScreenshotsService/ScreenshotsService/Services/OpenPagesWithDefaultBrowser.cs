using Microsoft.Extensions.Logging;
using ScreenshotsService.Services.Interfaces;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ScreenshotsService.Services
{
    public class OpenPagesWithDefaultBrowser : IOpenPages
    {
        private readonly ILogger _Logger;

        public OpenPagesWithDefaultBrowser(ILogger<PersistToLocalDisk> logger )
        {
            _Logger = logger;
        }

        public void OpenUrl(string url)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = false });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
            }
            catch(Exception ex)
            {
                _Logger.LogError("Error occured: ", ex);
            }
        }
    }
}

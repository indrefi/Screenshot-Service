using Microsoft.Extensions.Logging;
using ScreenshotsService.UtilServices.Interfaces;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

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
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                    Thread.Sleep(1000);

                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                    Thread.Sleep(1000);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                    Thread.Sleep(1000);
                }
            }
            catch(Exception ex)
            {
                _Logger.LogError("Error occured: ", ex);
            }
        }
    }
}

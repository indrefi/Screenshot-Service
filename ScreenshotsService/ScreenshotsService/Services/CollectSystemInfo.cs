using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ScreenshotsService.Services
{
    public class CollectSystemInfo : ICollectSystemInfo
    {
        private readonly ILogger _Logger;
        private readonly IOptions<ImageConfigModel> _ImageOptions;
        private readonly IOptions<DisplaySizeSettingsModel> _DisplaySizeSettings;


        public CollectSystemInfo(ILogger<PersistToLocalDisk> logger, IOptions<ImageConfigModel> imageOptions, IOptions<DisplaySizeSettingsModel> displaySizeSettings)
        {
            _Logger = logger;
            _ImageOptions = imageOptions;
            _DisplaySizeSettings = displaySizeSettings;
        }

        public (int, int) GetDefaultDisplayWidthAndHeight()
        {
            int width = _ImageOptions.Value.DefaultWidth;
            int height = _ImageOptions.Value.DefaultHeight;

            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var size = GetDefaultDisplayWidthAndHeightForWindows();
                    width = size.Item1;
                    height = size.Item2;

                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    //Process.Start("xdg-open", url);
                    //Xaxis=$(xrandr --current | grep '*' | uniq | awk '{print $1}' | cut -d 'x' -f1)
                    //Yaxis =$(xrandr--current | grep '*' | uniq | awk '{print $1}' | cut - d 'x' - f2)                    
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    //Process.Start("system_profiler SPDisplaysDataType | grep Resolution");
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError("Error occured: ", ex);
            }

            return (width, height);
        }

        private (int, int) GetDefaultDisplayWidthAndHeightForWindows()
        {
            int width = _ImageOptions.Value.DefaultWidth;
            int height = _ImageOptions.Value.DefaultHeight;

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    Arguments = _DisplaySizeSettings.Value.WindowsCommandsPath
                };

                using (Process p = Process.Start(psi))
                {
                    var strOutput = p.StandardOutput.ReadToEnd().Split(' ');

                    int.TryParse(strOutput[0], out width);
                    int.TryParse(strOutput[1], out height);

                    p.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError("Error occured: ", ex);
            }

            return (width, height);
        }
    }
}
using Microsoft.Extensions.Logging;
using ScreenshotsService.Services.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ScreenshotsService.Services
{
    public class ProcessImageScreenshot : IProcessImage
    {
        private readonly ILogger _logger;

        public ProcessImageScreenshot(ILogger<ProcessImageScreenshot> logger)
        {
            _logger = logger;
        }

        public MemoryStream MakeScreenshot(int width, int height)
        {
            try
            {
                using (Bitmap bmpScreenCapture = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                    {
                        Rectangle captureRectangle = new Rectangle(0, 0, width, height);
                        g.CopyFromScreen(0, 0, 0, 0, captureRectangle.Size);
                    }

                    MemoryStream resultStream = new MemoryStream();
                    bmpScreenCapture.Save(resultStream, ImageFormat.Jpeg);

                    return resultStream;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error occured: ", ex);

                return new MemoryStream();
            }
        }
    }
}
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScreenshotsService.Models;
using ScreenshotsService.UtilServices.Interfaces;
using System;

namespace ScreenshotsService.Services
{
    public class DisplaySize: IDisplaySize
    {
        private int? _Width { get; set; }
        private int? _Height { get; set; }

        private readonly object _Lock = new object();

        private readonly ILogger _Logger;
        private readonly IOptions<ImageConfigModel> _ImageOptions;
        private readonly ICollectSystemInfo _CollectSystemInfo;
        
        public DisplaySize(ILogger<DisplaySize> logger, ICollectSystemInfo collectSystemInfo, IOptions<ImageConfigModel> imageOptions)
        {
            _Logger = logger;
            _ImageOptions = imageOptions;
            _CollectSystemInfo = collectSystemInfo;
        }

        public (int,int) GetSize()
        {
            try
            {
                if (!_Width.HasValue || !_Height.HasValue)
                {
                    lock (_Lock)
                    {
                        var size = _CollectSystemInfo.GetDefaultDisplayWidthAndHeight();
                        _Width = size.Item1;
                        _Height = size.Item2;
                    }

                    return (_Width.Value, _Height.Value);
                }
            }
            catch(Exception ex)
            {
                _Width = _ImageOptions.Value.DefaultWidth;
                _Height = _ImageOptions.Value.DefaultHeight;

                _Logger.LogError("Exception occured: ", ex);
            }

            return (_Width.Value, _Height.Value);
        }
    }
}

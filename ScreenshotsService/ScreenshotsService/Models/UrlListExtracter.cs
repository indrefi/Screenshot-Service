using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ScreenshotsService.Models
{
    public class UrlListExtracter : StrategyUrlExtracter
    {
        private readonly ILogger _logger;

        public UrlListExtracter()
        {
           // _logger =  CreateLogger(GetType().Name);
        }

        public override List<string> ExtractUrl(UrlModel urlModel)
        {
            List<string> result = new List<string>();
            try
            {
                result = new List<string>(urlModel.Urls.Split(';'));
            }
            catch (Exception ex)
            {
              // _logger.LogError($"Error occurent in { GetType().Name }", ex);
            }

            return result;
        }
    }
}

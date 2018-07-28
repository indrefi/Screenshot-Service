using System.Collections.Generic;

namespace ScreenshotsService.Models
{
    public abstract class StrategyUrlExtracter
    {
        public abstract List<string> ExtractUrl(UrlModel urlModel);
    }
}

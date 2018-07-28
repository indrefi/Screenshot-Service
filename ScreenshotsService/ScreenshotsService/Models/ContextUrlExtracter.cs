using System.Collections.Generic;

namespace ScreenshotsService.Models
{
    public class ContextUrlExtracter
    {
        private StrategyUrlExtracter _strategyUrlParser;

        public ContextUrlExtracter(StrategyUrlExtracter strategyUrlParser)
        {
            _strategyUrlParser = strategyUrlParser;
        }

        public List<string> ParseContext(UrlModel urlModel )
        {
            return _strategyUrlParser.ExtractUrl(urlModel);
        }
    }
}

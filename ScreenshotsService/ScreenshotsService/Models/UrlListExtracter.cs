using System.Collections.Generic;
using System.Linq;

namespace ScreenshotsService.Models
{
    public class UrlListExtracter : StrategyUrlExtracter
    {

        public override List<string> ExtractUrl(UrlModel urlModel)
        {
            List<string> result = new List<string>();
            try
            {
                var initialList = new List<string>(urlModel.Urls.Split(';'));
                result = initialList.Where(o => o.Length > 0).ToList();
            }
            catch
            {
                return null;
            }

            return result;
        }
    }
}

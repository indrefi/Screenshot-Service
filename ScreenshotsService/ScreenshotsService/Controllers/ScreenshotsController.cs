using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;

namespace ScreenshotsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenshotsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProcessImage _processImage;
        private readonly IPersistData _persistData;
        private readonly IHashService _hashService;

        public ScreenshotsController(ILogger<ScreenshotsController> logger, IProcessImage processImage, IPersistData persistData, IHashService hashService)
        {
            _logger = logger;
            _processImage = processImage;
            _persistData = persistData;
            _hashService = hashService;
        }

        // GET: api/Screenshots/191347bfe55d0ca9a574db77bc8648275ce258461450e793528e0cc6d2dcf8f5
        [HttpGet("{path}", Name = "Get")]
        public string Get(string path)
        {
            return "screenshotId";
        }

        // POST: api/screenshots
        [HttpPost]
        public ActionResult<string> Post(UrlModel urlModel)
        {
            List<ScreenshotResponseModel> result = new List<ScreenshotResponseModel>();

            List<string> urlList = new ContextUrlExtracter(new UrlListExtracter()).ParseContext(urlModel);
            foreach(var url in urlList)
            {
                var hashValue = _hashService.GetHash(url);
                using (MemoryStream memoryStream = _processImage.MakeScreenshot(3200, 1800))
                {
                    var s3ResultPath = _persistData.PersistImage(memoryStream, hashValue);
                    result.Add(new ScreenshotResponseModel { SourceUrl = url, HashName = hashValue, Path = s3ResultPath });
                }             
            }

            return JsonConvert.SerializeObject(result);
        }
    }
}

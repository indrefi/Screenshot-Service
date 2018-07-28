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
        private readonly ILogger _Logger;
        private readonly IProcessImage _ProcessImage;
        private readonly IPersistData _PersistData;
        private readonly IHashService _HashService;
        private readonly IOpenPages _OpenPages;
        private readonly IDisplaySize _DisplaySize;

        public ScreenshotsController(ILogger<ScreenshotsController> logger, IProcessImage processImage, IPersistData persistData,
            IHashService hashService, IDisplaySize displaySize, IOpenPages openPages)
        {
            _Logger = logger;
            _ProcessImage = processImage;
            _PersistData = persistData;
            _HashService = hashService;
            _OpenPages = openPages;
            _DisplaySize = displaySize;
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

            (int, int) size = _DisplaySize.GetSize();

            List<string> urlList = new ContextUrlExtracter(new UrlListExtracter()).ParseContext(urlModel);
            foreach(var url in urlList)
            {
                var hashValue = _HashService.GetHash(url);
                _OpenPages.OpenUrl(url);
                using (MemoryStream memoryStream = _ProcessImage.MakeScreenshot(size.Item1, size.Item2))
                {
                    var s3ResultPath = _PersistData.PersistImage(memoryStream, hashValue);
                    result.Add(new ScreenshotResponseModel { SourceUrl = url, HashName = hashValue, Path = s3ResultPath });
                }             
            }

            return JsonConvert.SerializeObject(result);
        }
    }
}

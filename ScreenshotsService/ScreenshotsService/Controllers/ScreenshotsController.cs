using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ScreenshotsService.Helpers;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;
using ScreenshotsService.UtilServices.Interfaces;

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
        private readonly ILoadData _LoadData;
        private readonly IOptions<ImageConfigModel> _ImageConfig;

        public ScreenshotsController(ILogger<ScreenshotsController> logger, IProcessImage processImage, IPersistData persistData,
            IHashService hashService, IDisplaySize displaySize, IOpenPages openPages, ILoadData loadData, IOptions<ImageConfigModel> imageConfig)
        {
            _Logger = logger;
            _ProcessImage = processImage;
            _PersistData = persistData;
            _HashService = hashService;
            _OpenPages = openPages;
            _DisplaySize = displaySize;
            _LoadData = loadData;
            _ImageConfig = imageConfig;
        }

        // GET: api/Screenshots/191347bfe55d0ca9a574db77bc8648275ce258461450e793528e0cc6d2dcf8f5
        [HttpGet("{path}")]
        public async Task<IActionResult> Get(string path)
        {
            var dataStream =  await _LoadData.LoadImageAsync(path);
            if (dataStream is null) return StatusCode(500);

            dataStream.Position = 0;

            return new FileStreamResult(dataStream, $"image/{_ImageConfig.Value.ImageFormat}");
        }

        // POST: api/screenshots
        // JSON body that maps UrlModel
        [HttpPost]
        public async Task<ActionResult<string>> Post(UrlModel urlModel)
        {
            var result = new List<ScreenshotResponseModel>();
           
            List<string> urlList = urlModel.Urls.ExtractUrl();
            if (urlList is null) return StatusCode(500);

            foreach(var currentUrl in urlList)
            {
                var composedName = $"{currentUrl}-{Guid.NewGuid()}";
                var hashValue = _HashService.GetHash(composedName);
                using (MemoryStream memoryStream = await _ProcessImage.MakeScreenshot(currentUrl, hashValue))
                {
                    await _PersistData.PersistImageAsync(memoryStream, hashValue);
                    result.Add(new ScreenshotResponseModel { SourceUrl = currentUrl, RemoteFileKey = hashValue });
                }
            }

            return Ok( new { result = JsonConvert.SerializeObject(result) });
        }

        // POST: api/screenshots/upload
        // Form-Data with file attached
        [HttpPost("Upload")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var result = new List<ScreenshotResponseModel>();
            var fileContent = file.ReadAsStringAsync().Result.Trim();

            (int, int) size = _DisplaySize.GetSize();

            List<string> urlList = fileContent.ExtractUrl();
            if (urlList is null) return StatusCode(500);

            //Parallel.ForEach(urlList, (currentUrl) => {
            //    var composedName = $"{currentUrl}-{Guid.NewGuid()}";
            //    var hashValue = _HashService.GetHash(composedName);
            //    _BrowserService.MakeScreenshot(currentUrl, hashValue);
            //    result.Add(new ScreenshotResponseModel { SourceUrl = currentUrl, RemoteFileKey = hashValue });
            //});
            foreach (var currentUrl in urlList)
            {
                var composedName = $"{currentUrl}-{Guid.NewGuid()}";
                var hashValue = _HashService.GetHash(composedName);
                using (MemoryStream memoryStream = await _ProcessImage.MakeScreenshot(currentUrl, hashValue))
                {
                    await _PersistData.PersistImageAsync(memoryStream, hashValue);
                    result.Add(new ScreenshotResponseModel { SourceUrl = currentUrl, RemoteFileKey = hashValue });
                }
            }

            return Ok(new { result = JsonConvert.SerializeObject(result) });
        }
    }
}

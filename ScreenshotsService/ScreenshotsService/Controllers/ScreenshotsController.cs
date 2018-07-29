using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ScreenshotsService.Helpers;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;

namespace ScreenshotsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenshotsController : ControllerBase
    {
        private readonly ILogger _Logger;
        private readonly ILoadData _LoadData;
        private readonly IExecuteTask _ExecuteTask;
        private readonly IOptions<ImageConfigModel> _ImageConfig;

        public ScreenshotsController(ILogger<ScreenshotsController> logger,  ILoadData loadData, IOptions<ImageConfigModel> imageConfig, IExecuteTask executeTask)
        {
            _Logger = logger;
            _LoadData = loadData;
            _ImageConfig = imageConfig;
            _ExecuteTask = executeTask;
        }

        // GET: api/Screenshots/191347bfe55d0ca9a574db77bc8648275ce258461450e793528e0cc6d2dcf8f5
        [HttpGet("{path}")]
        public IActionResult Get(string path)
        {
            var dataStream = _LoadData.LoadImage(path);
            if (dataStream is null) return StatusCode(500);

            dataStream.Position = 0;

            return new FileStreamResult(dataStream, $"image/{_ImageConfig.Value.ImageFormat}");
        }

        // POST: api/screenshots
        // JSON body that maps UrlModel
        [HttpPost]
        public ActionResult<string> Post(UrlModel urlModel)
        {
            var result = new List<ScreenshotResponseModel>();

            List<string> urlList = urlModel.Urls.ExtractUrl();
            if (urlList is null) return StatusCode(500);

            _ExecuteTask.Execute(urlList);

            return Ok( new { result = JsonConvert.SerializeObject(result) });
        }

        // POST: api/screenshots/upload
        // Form-Data with file attached
        [HttpPost("Upload")]
        public IActionResult Post(IFormFile file)
        {
            var result = new List<ScreenshotResponseModel>();
            var fileContent = file.ReadAsStringAsync().Result.Trim();

            List<string> urlList = fileContent.ExtractUrl();
            if (urlList is null) return StatusCode(500);

            _ExecuteTask.Execute(urlList);

            return Ok(new { result = JsonConvert.SerializeObject(result) });
        }
    }
}

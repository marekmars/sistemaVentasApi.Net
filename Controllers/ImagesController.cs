using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Services;

namespace Web_Service_.Net_Core.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }
        [HttpPost]

        public async Task<ActionResult<ApiResponse<QuickType.ImgResponse>>> AddImage([FromBody] ImageUploadModel imageUploadModel)
        {
            var result = await _imageService.AddImage(imageUploadModel);
            return result;
        }
        [HttpDelete("{deleteHash}")]
        public async Task<ActionResult<ApiResponse<Object>>> DeleteImage(string deleteHash)
        {
            var result = await _imageService.DeleteImage(deleteHash);
            return result;
        }
        [HttpDelete("deleteByUrl")]
        public async Task<ActionResult<ApiResponse<Object>>> DeleteImageByUrl([FromQuery]string url)
        {
            var result = await _imageService.DeleteImageByUrl(url);
            return result;
        }
    }
}
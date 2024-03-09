using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;

namespace Web_Service_.Net_Core.Services
{
  public interface IImageService
  {
    public Task<ApiResponse<QuickType.ImgResponse>> AddImage(ImageUploadModel imageUploadModel);
    public Task<ApiResponse<Object>> DeleteImage(string deleteHash);
    public Task<ApiResponse<Object>> DeleteImageByUrl(string url);
  }
}
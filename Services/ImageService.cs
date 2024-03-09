using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QuickType;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Services;

public class ImageService : IImageService
{
    private readonly IConfiguration _config;
    private readonly DataContext _context;
    public ImageService(IConfiguration config, DataContext context)
    {
        _config = config;
        _context = context;
    }
    public async Task<ApiResponse<ImgResponse>> AddImage(ImageUploadModel imageUploadModel)
    {
        try
        {
            if (imageUploadModel.Image == null) throw new Exception("There was an error processing the image. Please try again later.");

            byte[] imageBytes = Convert.FromBase64String(imageUploadModel.Image);

            string? uploadUrl = _config["ImgurServerUrl"];
            // string? token = _config["ClientId"];
            string? token = _config["ImgurToken"];
            Console.WriteLine("Token: " + token);
            Console.WriteLine("Url: " + uploadUrl);
            if (uploadUrl == null || token == null) throw new Exception("There was an error processing the image. Please try again later.");

            using (var client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var content = new ByteArrayContent(imageBytes))
                {
                    var response = await client.PostAsync(uploadUrl, content);
                    Console.WriteLine("Response: " + response);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonSerializer.Deserialize<ImgResponse>(jsonResponse);
                        if (responseObject == null) throw new Exception("There was an error processing the image. Please try again later.");
                        return new ApiResponse<ImgResponse>
                        {
                            Success = 1,
                            Message = "Imagen subida exitosamente al servidor remoto.",
                            Data = [responseObject]
                        };
                    }
                    else
                    {
                        throw new Exception("There was an error processing the image. Please try again later");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("There was an error processing the image. Please try again later. Error message: " + ex.Message);
        }
    }

    public async Task<ApiResponse<Object>> DeleteImage(string deleteHash)
    {
        try
        {
            if (deleteHash == null) throw new Exception("There was an error deleting the image. Please try again later.");



            string? uploadUrl = $"{_config["ImgurServerUrl"]}/{deleteHash}";
            string? token = _config["imgurToken"];


            if (uploadUrl == null || token == null) throw new Exception("There was an error deleting the image. Please try again later.");

            using (var client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync(uploadUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<Object>(jsonResponse);
                    if (responseObject == null) throw new Exception("There was an error deleting the image. Please try again later.");
                    return new ApiResponse<Object>
                    {
                        Success = 1,
                        Message = "Image deleted successfully",
                        Data = [responseObject]
                    };
                }
                else
                {
                    throw new Exception("There was an error deleting the image. Please try again later");
                }

            }
        }
        catch (Exception ex)
        {
            throw new Exception("There was an error dweleting the image. Please try again later. Error message: " + ex.Message);
        }
    }
    public async Task<ApiResponse<Object>> DeleteImageByUrl(string url)
    {
        try
        {
            Console.WriteLine("Url: " + url);
            if (url == null) throw new Exception("There was an error deleting the image. Please try again later.");
            var img = _context.Images.SingleOrDefault(x => x.Url == url);

            if (img == null) throw new Exception("There was an error deleting the image. Please try again later.");
            string? uploadUrl = $"{_config["ImgurServerUrl"]}/{img.DeleteHash}";
            string? token = _config["imgurToken"];

            if (uploadUrl == null || token == null) throw new Exception("There was an error deleting the image. Please try again later.");

            using (var client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync(uploadUrl);
                if (response.IsSuccessStatusCode)
                {
                    _context.Remove(img);
                    _context.SaveChanges();
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<Object>(jsonResponse);
                    if (responseObject == null) throw new Exception("There was an error deleting the image. Please try again later.");
                    return new ApiResponse<Object>
                    {
                        Success = 1,
                        Message = "Image deleted successfully",
                        Data = [responseObject]
                    };
                }
                else
                {
                    throw new Exception("There was an error deleting the image. Please try again later");
                }

            }


        }
        catch (Exception ex)
        {
            throw new Exception("There was an error dweleting the image. Please try again later. Error message: " + ex.Message);
        }
    }


}
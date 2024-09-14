using CodeMegaGithubAPI.Models;
using Newtonsoft.Json;
using RestSharp;

namespace CodeMegaGithubAPI.GithubAPIService;

public class GithubAPIService : IGithubAPIService
{
    private readonly IConfiguration _configuration;

    public GithubAPIService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<GitResponseModel> UploadImageAsync(IFormFile file, string folderSource)
    {
        var fileName = ToFileName(file);
        var commit = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt").Replace("/", "_");
        
        await using MemoryStream ms = new();
        await file.CopyToAsync(ms);

        var fileBytes = ms.ToArray();
        
        RestRequest request = new() { Method = Method.Put };

        request.AddHeader("Authorization", "Token " + _configuration["GithubAPI:AccessToken"]);
        request.AddHeader("accept", "application/vnd.github.v3+json");
        request.AddJsonBody(
            new
            {
                message = commit,
                content = Convert.ToBase64String(fileBytes),
                branch = _configuration["GithubAPI:Branch"]
            });

        var client = new RestClient($"{_configuration["GithubAPI:BaseUrl"]}/{folderSource}/{fileName}");

        var response = await client.ExecuteAsync(request);
        var jsonGit = response.Content?.Replace("download_url", "downloadUrl");

        if (jsonGit == null)
        {
            return new GitResponseModel { Success = false };
        }

        var resGit = JsonConvert.DeserializeObject<GitResponseModel>(jsonGit);
        
        if (resGit == null)
        {
            return new GitResponseModel { Success = false };
        }

        resGit.Success = true;

        return resGit;
    }

    private string ToFileName(IFormFile file)
    {
        var stringName = "";
        var ticks = DateTime.UtcNow.Ticks;
        var fileName = file.FileName.Split('.').First();
        
        if (file.FileName.EndsWith(".png") || file.FileName.EndsWith(".PNG"))
        {
            stringName = fileName + "_" + ticks + ".png";
        }
        else if (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".JPG"))
        {
            stringName = fileName + "_" + ticks + ".jpg";
        }
        else if (file.FileName.EndsWith(".jpeg") || file.FileName.EndsWith(".JPEG"))
        {
            stringName = fileName + "_" + ticks + ".jpg";
        }
        else if (file.FileName.EndsWith(".gif") || file.FileName.EndsWith(".GIF"))
        {
            stringName = fileName + "_" + ticks + ".gif";
        }
        else if (file.FileName.EndsWith(".webp") || file.FileName.EndsWith(".WEBP"))
        {
            stringName = fileName + "_" + ticks + ".webp";
        }
        else
        {
            stringName = fileName + "_" + ticks + ".jpg";
        }

        return stringName;
    }
}
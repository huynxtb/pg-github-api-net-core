using CodeMegaGithubAPI.GithubAPIService;
using Microsoft.AspNetCore.Mvc;

namespace CodeMegaGithubAPI.Controllers;

public class FileController : Controller
{
    private readonly IGithubAPIService _githubApiService;

    public FileController(IGithubAPIService githubApiService)
    {
        _githubApiService = githubApiService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile imageFile)
    {
        var folderName = "User";

        var res = await _githubApiService.UploadImageAsync(imageFile, folderName);
        
        return RedirectToAction("ResponseData", new { fileURL = res.Content?.DownloadUrl });
    }
    
    [HttpGet]
    public IActionResult ResponseData(string fileURL)
    {
        return Json(new
        {
            FileURL = fileURL
        });
    }
}
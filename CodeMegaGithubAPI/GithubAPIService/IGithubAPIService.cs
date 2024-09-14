using CodeMegaGithubAPI.Models;

namespace CodeMegaGithubAPI.GithubAPIService;

public interface IGithubAPIService
{
    Task<GitResponseModel> UploadImageAsync(IFormFile file, string folderSource);
}
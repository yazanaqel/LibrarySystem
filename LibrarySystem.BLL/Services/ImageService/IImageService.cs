using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


namespace LibrarySystem.BLL.Services.ImageService;
public interface IImageService
{
	string UpLoadFile(IFormFile file, IWebHostEnvironment webHostEnvironment);
	void Delete(string imageUrl, IWebHostEnvironment webHostEnvironment);
}

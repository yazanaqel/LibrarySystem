using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.ImageService;
public class ImageService: IImageService
{
	public string UpLoadFile(IFormFile file, IWebHostEnvironment webHostEnvironment)
	{
		if (file != null)
		{
			string images = Path.Combine(webHostEnvironment.WebRootPath, "covers");
			string unique = Guid.NewGuid().ToString() + "_" + file.FileName;
			string fullPath = Path.Combine(images, unique);
			using (var fileStream = new FileStream(fullPath, FileMode.Create))
			{
				file.CopyTo(fileStream);
			}


			return unique;
		}
		else
		{
			return string.Empty;
		}
	}
	public void Delete(string imageUrl, IWebHostEnvironment webHostEnvironment)
	{
		string images = Path.Combine(webHostEnvironment.WebRootPath, "covers");

		//Delete Old File:
		string oldPath = Path.Combine(images, imageUrl);
		System.IO.File.Delete(oldPath);
	}
}

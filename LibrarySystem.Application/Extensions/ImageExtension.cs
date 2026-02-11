using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Application.Extensions;

internal static class ImageExtension
{
    public static string Upload(this IFormFile file)
    {
        if(file != null)
        {
            var root = Directory.GetCurrentDirectory();

            string path = Path.Combine(root,"wwwroot","covers");

            string unique = Guid.NewGuid().ToString() + "_" + file.FileName;

            string fullPath = Path.Combine(path,unique);

            using(var fileStream = new FileStream(fullPath,FileMode.Create))
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

    public static void Delete(this string imageUrl)
    {
        var root = Directory.GetCurrentDirectory();

        string path = Path.Combine(root,"wwwroot","covers");

        string oldPath = Path.Combine(path,imageUrl);

        File.Delete(oldPath);
    }
}

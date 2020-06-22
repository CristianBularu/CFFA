using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CFFA_API.Controllers.Helpers
{

    public interface IPhotoManager
    {
        public Task<string> SaveProfilePhoto(string userId, IFormFile file);
        public Task<string> SavePostPhoto(string postId, IFormFile file);
        public Task<string> SaveSketchPhoto(string sketchId, IFormFile file);
    }

    public class PhotoManager: IPhotoManager
    {
        
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ILogger logger;

        public PhotoManager(IWebHostEnvironment hostEnvironment, ILogger<PhotoManager> logger)
        {
            this.hostEnvironment = hostEnvironment;
            this.logger = logger;
        }

        public async Task<string> SaveProfilePhoto(string userId, IFormFile file)
        {
            return await savePhoto(userId, "User", file);
        }

        public async Task<string> SavePostPhoto(string postId, IFormFile file)
        {
            return await savePhoto(postId, "Post", file);
        }

        public async Task<string> SaveSketchPhoto(string sketchId, IFormFile file)
        {
            return await savePhoto(sketchId, "Sketch", file);
        }

        private async Task<string> savePhoto(string Id, string folderName, IFormFile file)
        {
            try
            {
                string imageName;
                string filePath;
                string directoryPath = $"{folderName}\\{Id}";
                string realDirectoryPath = $"{hostEnvironment.WebRootPath}\\{directoryPath}";
                if (!Directory.Exists(realDirectoryPath))
                    Directory.CreateDirectory(realDirectoryPath);
                imageName = string.Concat("o", Path.GetExtension(file.FileName));
                filePath = Path.Combine(realDirectoryPath, imageName);
                FileStream stream = File.Create(filePath);
                
                await file.CopyToAsync(stream);
                    
                new Thread(() =>
                {
                    try
                    {

                        var original = new Bitmap(stream);
                        //stream.Flush();
                        var height = original.Height;
                        var width = original.Width;

                        int smallSize = 100;
                        int mediumSize = 500;
                        //make sure the image is bigger than the resizes, else change resize scale
                        if (smallSize > height || smallSize > width)
                        {
                            smallSize = Math.Min(height, width);
                        }
                        if (mediumSize > height || mediumSize > width)
                        {
                            mediumSize = Math.Min(height, width);
                        }
                        Bitmap finalSmall;
                        if (height < width)
                            finalSmall = new Bitmap(original, smallSize * width / height, smallSize);
                        else
                            finalSmall = new Bitmap(original, smallSize, smallSize * height / width);
                        finalSmall.Save(Path.Combine(realDirectoryPath, string.Concat("s", Path.GetExtension(file.FileName))));
                        
                        Bitmap finalMedium;
                        if (height < width)
                            finalMedium = new Bitmap(original, mediumSize * width / height, mediumSize);
                        else
                            finalMedium = new Bitmap(original, mediumSize, mediumSize * height / width);
                        finalMedium.Save(Path.Combine(realDirectoryPath, string.Concat("m", Path.GetExtension(file.FileName))));
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, ex.Message);
                    }
                    finally
                    {
                        stream.Dispose();
                    }
                })
                {
                    //Priority = ThreadPriority.BelowNormal,
                    IsBackground = true
                }.Start();

                return Path.GetExtension(file.FileName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}

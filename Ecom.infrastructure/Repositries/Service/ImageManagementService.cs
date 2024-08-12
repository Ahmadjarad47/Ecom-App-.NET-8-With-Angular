using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries.Service
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider fileProvider;
        public ImageManagementService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            List<string> SaveImageSrc = new List<string>();

            var ImageDirctory = Path.Combine("wwwroot", "Images", src);

            if (Directory.Exists(ImageDirctory) is not true)
            {
                Directory.CreateDirectory(ImageDirctory);
            }

            foreach (var item in files)
            {
                if (item.Length > 0)
                {
                    // get Image Name
                    var ImageName = item.FileName;

                    var ImageSrc = $"/Images/{src}/{ImageName}";

                    var root = Path.Combine(ImageDirctory, ImageName);

                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(ImageSrc);
                }
            }
            return SaveImageSrc;

        }



        public void DeleteImageAsync(string src)
        {
            var info = fileProvider.GetFileInfo(src);

            var root = info.PhysicalPath;
            File.Delete(root);
        }
    }
}

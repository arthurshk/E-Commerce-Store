using E_Commerce_Store.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Store.Services
{
    public class ImageStorage
    {
        private readonly IWebHostEnvironment _environment;
        public readonly SiteContext _siteContext;
        public string _uploadFolder = "uploads";
        public ImageStorage(IWebHostEnvironment environment, SiteContext context)
        {
            _siteContext = context;
            _environment = environment;
        }   
        public string UploadFolder
        {
            get { return _uploadFolder; }
            set
            {
                var path = Path.Combine(_environment.WebRootPath, value);
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                _uploadFolder = value;
            }
        }
        public void Remove(Image model)
        {
            System.IO.File.Delete(Path.Combine(_environment.WebRootPath, UploadFolder, model.Filename));
            _siteContext.Images.Remove(model);
        }
        public async Task<Image> UploadAsync(IFormFile file)
        { 

                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                using (var writer = new FileStream(Path.Combine(_environment.WebRootPath, UploadFolder , filename), FileMode.Create))
                {
                 await   file.CopyToAsync(writer);
                }
            var image = new Image()
            {
                Filename = filename
            };
           await _siteContext.Images.AddAsync(image);
            return image;
            }
        }
    }
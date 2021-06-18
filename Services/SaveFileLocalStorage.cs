using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cinema.Services
{
    public class SaveFileLocalStorage : ISaveFile
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SaveFileLocalStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string url, string container)
        {
            if (url != null)
            {
                string fileName = Path.GetFileName(url);
                string fileFolder = Path.Combine(env.WebRootPath, container, fileName);

                if (File.Exists(fileFolder))
                {
                    File.Delete(fileFolder);
                }
            }

            return Task.FromResult(0);

        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string url, string contentType)
        {
            await DeleteFile(url, container);
            return await SaveFile(content, extension, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            string fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string url = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(url, content);

            string actualUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            string dataBaseUrl = Path.Combine(actualUrl, container, fileName).Replace("\\", "/");
            return dataBaseUrl;
        }
    }
}

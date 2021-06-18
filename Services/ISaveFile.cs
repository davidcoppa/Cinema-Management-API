using System.Threading.Tasks;

namespace Cinema.Services
{
    public interface ISaveFile
    {
        Task<string> EditFile(byte[] content, string extension, string container, string url, string contentType);
        Task DeleteFile(string url, string container);
        Task<string> SaveFile(byte[] content, string extension, string container, string contentType);
    }
}

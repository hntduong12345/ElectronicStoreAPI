using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interfaces
{
    public interface IUploadImageService
    {
        Task<string?> UploadImage(Stream imageStream, string filename, string contentType, CancellationToken cancellationToken = default);
        Task<Stream?> DownloadImage(string imageUrl, CancellationToken cancellationToken = default);
        Task<bool> DeleteImage(string imageUrl, CancellationToken cancellationToken = default);
        Task<string> GetImageAbsoluteUrl(string imageUrl,CancellationToken cancellationToken = default);

    }
}

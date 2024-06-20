using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interfaces
{
    public interface IImageModificationService
    {
        Task<Stream> ResizeImage(Stream imageStream, string fileName, int width, int height, ResizeMode mode = ResizeMode.Max); 
    }
}

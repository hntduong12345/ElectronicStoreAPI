using API.Service.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Services
{
    public class ImageModificationService : IImageModificationService
    {
        public ImageModificationService()
        {
        }
        public async Task<Stream> ResizeImage(Stream imageStream, string fileName, int width, int height, ResizeMode mode = ResizeMode.Max)
        {
            var memoryStream = new MemoryStream();
            using (var image = Image.Load(imageStream))
            {
                image.Mutate(img =>
                {
                    img.Resize(new ResizeOptions()
                    {
                        Mode = mode,
                        Size = new Size(width, height)
                    });

                    image.Save(memoryStream, image.DetectEncoder(fileName)); ;
                });
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}

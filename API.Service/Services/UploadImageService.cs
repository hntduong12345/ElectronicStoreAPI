using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Google.Apis.Upload;
using API.BO.Constants;
using API.Service.Interfaces;

namespace API.Service.Services
{
    public class UploadImageService : IUploadImageService
    {
        private readonly string BUCKET_NAME = GoogleBucketConstant.GOOGLE_BUCKET; //"prm392-ea952.appspot.com";
        private readonly string IMAGE_GALERY = GoogleBucketConstant.GOOGLE_BUCKET_IMAGE;//"image_galery";
        private readonly string PUBLIC_URL_BASE = GoogleBucketConstant.GOOGLE_BUCKET_BASE_URL;//"https://storage.googleapis.com";
        private readonly StorageClient _googleStorage;
        public UploadImageService(StorageClient storageClient)
        {
            _googleStorage = storageClient;
        }        
        public async Task<string?> UploadImage(Stream imageStream , string saveFileName,string contentType, CancellationToken cancellationToken = default)
        {
            try
            {
                var correctPath = $"{IMAGE_GALERY}/{saveFileName}";
                var uploadResult = await _googleStorage.UploadObjectAsync(BUCKET_NAME, correctPath, contentType,imageStream,null,cancellationToken,null);
                return uploadResult.Name;
            }catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Stream?> DownloadImage(string fileUrl, CancellationToken cancellationToken)
        {
            try
            {
                var memoryStream = new MemoryStream();
                var tryGetObject = await _googleStorage.GetObjectAsync(BUCKET_NAME, fileUrl, null, cancellationToken);
                if (tryGetObject is null)
                    throw new Exception("no object found");
                await _googleStorage.DownloadObjectAsync(tryGetObject, memoryStream, null, cancellationToken);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }catch (Exception ex) 
            {
                return null;
            }
        }
        public async Task<bool> DeleteImage(string fileUrl, CancellationToken cancellationToken)
        {
            try
            {
                var tryGetBucket = await _googleStorage.GetObjectAsync(BUCKET_NAME, fileUrl,null,cancellationToken);
                if (tryGetBucket == null)
                    throw new NullReferenceException("no object found");
                await _googleStorage.DeleteObjectAsync(tryGetBucket, null,cancellationToken);
                return true;    
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Task<string> GetImageAbsoluteUrl(string imageUrl, CancellationToken cancellationToken = default)
        {
            return Task.FromResult($"{PUBLIC_URL_BASE}/{BUCKET_NAME}/{imageUrl}");
        }
    }
}

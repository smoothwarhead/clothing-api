using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using KerryCoAdmin.Api.Entities.Dtos.Responses;
using KerryCoAdmin.Api.Entities.Models;
using KerryCoAdmin.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using KerryCoAdmin.Configurations;

namespace KerryCoAdmin.Api.Modules
{
    public class PhotoService : ControllerBase
    {

        /*
        private Cloudinary _cloudinary;
        private readonly ISecretService _secretService;
        private readonly List<VaultSecret> _secrets;
        private readonly string _cloudName;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        */

        public PhotoService()
        {
            /*
            _secretService = secretService;
            _secrets = _secretService.GetSecrets();
            _cloudName = GetInfo.GetASecret("Cloudinary-cloudname", _secrets);
            _apiKey = GetInfo.GetASecret("Cloudinary-apiKey", _secrets);
            _apiSecret = GetInfo.GetASecret("Cloudinary-apiSecret", _secrets);
            _cloudinary = cloudinary;

            */

            /*
            
            _cloudinarySettings = _configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            Account account = new Account(
                _cloudinarySettings.CloudName,
                _cloudinarySettings.ApiKey,
                _cloudinarySettings.ApiSecret
                );

            */

        }


        public PhotoResponseDto AddPhoto(IFormFile file, CloudinarySettings cloudinarySettings)
        {


            Account account = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);


            Cloudinary _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;


            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        UploadPreset = "kerryCo"
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            var response = new PhotoResponseDto()
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url.ToString(),
                SecureUrl = uploadResult.SecureUrl.ToString()

            };

            return response;

        }



        public static bool RemovePhoto(string id, CloudinarySettings cloudinarySettings)
        {


            Account account = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);


            Cloudinary _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;

            var deletionParams = new DeletionParams(id)
            {
                PublicId = id
            };

            var delResponse = _cloudinary.Destroy(deletionParams);
            Console.WriteLine(delResponse);

            if (delResponse.Result == "ok")
                return true;
            return false;


        }
    }
}

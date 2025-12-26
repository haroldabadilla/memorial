using System;
using System.Collections.Generic;
using System.Linq;
using MemorialBackend.Models; // Assumes you kept the Models from previous step
using MemorialBackend.Interfaces;

namespace MemorialBackend.Services
{
    public class MemorialService
    {
        private readonly IPhotoScanner _scanner;
        private readonly string _cloudName;
        private readonly string _cloudVersion;

        // Dependency Injection: We inject the scanner here.
        public MemorialService(IPhotoScanner scanner, string cloudName, string cloudVersion)
        {
            _scanner = scanner;
            _cloudName = cloudName;
            _cloudVersion = cloudVersion;
        }

        public MemorialData BuildData(string localPath)
        {
            var data = new MemorialData
            {
                HeaderMessage = "With the heaviest hearts, we announce that our beloved mother, Redempta R. Abadilla, joined our Creator on the 31st of October, 2025. She was the heart of our family, our greatest blessing. Her life was a beautiful testament to love, kindness, and unwavering grace. She filled our home with warmth, and her loving spirit touched everyone who knew her. While our world is forever changed by her loss, we are filled with immense gratitude for every moment we had with her. The love she gave us is a gift that will remain in our hearts forever. We find comfort in knowing she is now resting in eternal peace. We ask our dearest relatives, friends, and all those whose lives she touched to join us in honoring her memory and celebrating the beautiful life she lived.",
                FooterMessage = "Thank you for joining us in remembering our beloved.",
                Videos = GetStaticVideos(),
                Photos = new List<PhotoContent>()
            };

            // 1. Use the injected scanner to get files
            var fileNames = _scanner.GetPhotoFiles(localPath);

            // 2. Process the files
            foreach (var fileName in fileNames)
            {
                // Logic: Extract number from "photo_12.png"
                // Split by underscore, then split by dot.
                // "photo_12.png" -> "12.png" -> "12"
                var numberPart = fileName.Split('_').Last().Split('.').First();

                if (int.TryParse(numberPart, out int id))
                {
                    data.Photos.Add(new PhotoContent
                    {
                        Id = id,
                        Title = $"Memory {id}",
                        Url = GenerateCloudinaryUrl(fileName)
                    });
                }
            }

            // 3. Sort by ID so they appear in order
            data.Photos = data.Photos.OrderBy(p => p.Id).ToList();

            return data;
        }

        private string GenerateCloudinaryUrl(string fileName)
        {
            // Returns: https://res.cloudinary.com/dm3brobpn/image/upload/v1766733368/photo_1.jpg
            return $"https://res.cloudinary.com/{_cloudName}/image/upload/{_cloudVersion}/{fileName}";
        }

        private List<VideoContent> GetStaticVideos()
        {
            return new List<VideoContent>
            {
                new VideoContent { Id = 1, Title = "Funeral Mass", YoutubeId = "JxwmbZkT2o8" },
                new VideoContent { Id = 2, Title = "Eulogy", YoutubeId = "rpDpFeFhCgw" },
                new VideoContent { Id = 3, Title = "Life Tribute", YoutubeId = "pZcmET4r7wE" }
            };
        }
    }
}
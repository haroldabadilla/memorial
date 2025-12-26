using System.IO;
using System.Collections.Generic;
using System.Linq;
using MemorialBackend.Interfaces;

namespace MemorialBackend.Services
{
    public class LocalPhotoScanner : IPhotoScanner
    {
        public IEnumerable<string> GetPhotoFiles(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"The folder '{directoryPath}' does not exist.");
            }

            // Get all files starting with "photo_" regardless of extension
            return Directory.GetFiles(directoryPath, "photo_*.*")
                            .Select(Path.GetFileName); // Only return "photo_1.jpg", not full path
        }
    }
}
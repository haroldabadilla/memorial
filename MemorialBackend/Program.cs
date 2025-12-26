using System;
using System.IO;
using System.Text.Json;
using MemorialBackend.Services;

namespace MemorialBackend
{
    class Program
    {
        // --- CONFIGURATION ---
        const string CLOUD_NAME = "dm3brobpn";
        const string CLOUD_VERSION = "v1766735607";

        // This looks for a folder named "photos_to_upload" inside your project folder
        static readonly string LocalPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "photos_to_upload");

        static void Main(string[] args)
        {
            Console.WriteLine("--- Starting Memorial Data Generator ---");

            try
            {
                // 1. Composition Root (Wiring up the Dependencies)
                // We create the specific scanner we want to use
                var scanner = new LocalPhotoScanner();

                // We inject that scanner into the service
                var service = new MemorialService(scanner, CLOUD_NAME, CLOUD_VERSION);

                // 2. Execution
                Console.WriteLine($"Scanning folder: {LocalPhotoPath}");
                var data = service.BuildData(LocalPhotoPath);

                // 3. Output
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string json = JsonSerializer.Serialize(data, options);
                File.WriteAllText("memorial_data.json", json);

                Console.WriteLine($"[SUCCESS] Generated {data.Photos.Count} photos.");
                Console.WriteLine($"File saved to: {Path.GetFullPath("memorial_data.json")}");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"[ERROR] Could not find folder: {LocalPhotoPath}");
                Console.WriteLine("Please create a folder named 'photos_to_upload' inside this project and put your images there.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Unexpected error: {ex.Message}");
            }
        }
    }
}
using System.Collections.Generic;

namespace MemorialBackend.Interfaces
{
    // The contract: "I don't care how you find photos, just give me a list of filenames."
    public interface IPhotoScanner
    {
        IEnumerable<string> GetPhotoFiles(string directoryPath);
    }
}
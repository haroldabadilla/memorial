using System.Collections.Generic;

namespace MemorialBackend.Models
{
    /// <summary>
    /// Base class for any content item (video, photo, etc).
    /// Practices: Abstraction & Inheritance.
    /// </summary>
    public abstract class ContentItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a YouTube video tribute.
    /// </summary>
    public class VideoContent : ContentItem
    {
        public string YoutubeId { get; set; } = string.Empty;
        // e.g., "Mass", "Eulogy"
        public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a photo stored in Cloudinary.
    /// </summary>
    public class PhotoContent : ContentItem
    {
        public string Url { get; set; } = string.Empty;
    }

    /// <summary>
    /// The root object that contains all website data.
    /// </summary>
    public class MemorialData
    {
        public string HeaderMessage { get; set; } = string.Empty;
        public string FooterMessage { get; set; } = string.Empty;
        public List<VideoContent> Videos { get; set; } = new();
        public List<PhotoContent> Photos { get; set; } = new();
    }
}
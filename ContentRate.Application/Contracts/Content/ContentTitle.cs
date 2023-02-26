using ContentRate.Domain.Rooms;

namespace ContentRate.Application.Contracts.Content
{
    /// <summary>
    /// Контент, который мы получаем из внешних источников
    /// </summary>
    public class ContentImport
    {
        public string Path { get; set; } = default!;
        public string Name { get; set; } = default!;
        public ContentType ContentType { get; set; }
    }
}

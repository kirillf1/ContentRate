using Microsoft.AspNetCore.Components;
using ContentRate.Domain.Rooms;

namespace ContentRate.BlazorComponents.Content
{
    public partial class ContentViewer
    {
        [Parameter]
        public ContentType ContentType { get; set; }
        [Parameter]
        public string Path { get; set; } = "";
    }
}
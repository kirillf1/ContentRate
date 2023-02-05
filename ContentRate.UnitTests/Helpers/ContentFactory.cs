namespace ContentRate.UnitTests.Helpers
{
    public static class ContentFactory
    {
        public static Content CreateContent(Guid id)
        {
            return new Content(id, id.ToString(), ContentType.Video, $"C:\\{id}", Enumerable.Empty<Rating>());
        }
    }
}

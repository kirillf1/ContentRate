using ContentRate.UnitTests.Helpers;

namespace ContentRate.UnitTests.DomainTests
{
    public class ContentTests
    {
        [Fact]
        public void AddRating_NewRating_AvarageRatingChanged()
        {
            var content = ContentFactory.CreateContent(Guid.NewGuid());

            content.AddRating(new Rating(Guid.NewGuid(), 2));
            var averageFirst = content.GetAverageRating();
            content.AddRating(new Rating(Guid.NewGuid(), 4));
            var averageSecond = content.GetAverageRating();

            Assert.NotEqual(averageFirst, averageSecond);
            Assert.True(content.Ratings.Count == 2);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(11)]
        [InlineData(55)]
        [InlineData(10.1)]
        [InlineData(-0.1)]
        public void AddRating_RatingWithIncorrectValue_ThrowArgumentEx(double incorrectValue)
        {
            var content = ContentFactory.CreateContent(Guid.NewGuid());

            Action incorrectAdd = () => content.AddRating(new Rating(Guid.NewGuid(), incorrectValue));

            Assert.Throws<ArgumentException>(incorrectAdd);
        }
    }
}

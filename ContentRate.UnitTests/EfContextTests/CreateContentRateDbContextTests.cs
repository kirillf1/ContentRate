namespace ContentRate.UnitTests.EfContextTests
{
    public class CreateContentRateDbContextTests
    {
        [Fact]
        public void CreateDatabase_NewConnection_CreateTables_ThanDeleteDb()
        {
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());
            
            var isEmptyTable = !context.Rooms.Any();

            Assert.True(isEmptyTable);
        }
    }
}

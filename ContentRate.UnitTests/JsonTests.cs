using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContentRate.UnitTests
{
    public class JsonTests
    {
        [Fact]
        public void SerilizeTets()
        {
            //var obj = new RoomUpdate()
            //{
            //    Name = "",
            //    Password = "",
            //    Content = new List<Application.Contracts.Content.ContentDetails>
            //    {
            //        new Application.Contracts.Content.ContentDetails { Name = "", Path = "",
            //            Ratings = new List<Application.Contracts.Content.ContentRating>
            //            {
            //                new Application.Contracts.Content.ContentRating()
            //            }
            //        }
            //    }, Assessors = new List<Application.Contracts.Users.Assessor>
            //{
            //    new Application.Contracts.Users.Assessor{ Name = ""}
            //}
            //};
            var obj = new RoomEnter { AssessorId = Guid.NewGuid(), RoomId = Guid.NewGuid() };
            var str = JsonSerializer.Serialize(obj);
            var ob = JsonSerializer.Deserialize<RoomEnter>(str);

            Assert.True(str.Length > 0);
        }
    }
}

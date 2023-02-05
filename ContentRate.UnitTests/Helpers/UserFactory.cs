using ContentRate.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentRate.UnitTests.Helpers
{
    public static class UserFactory
    {
        public static User CreateUser(Guid? id = null)
        {
            id ??= Guid.NewGuid();
            return new User(id.Value, id.Value.ToString(), id.Value.ToString());
        }
        public static UserModel CreateUserModel(Guid? id = null)
        {
            id ??= Guid.NewGuid();
            return new UserModel
            {
                Id = id.Value,
                Name = id.Value.ToString(),
                Password = id.Value.ToString(),
            };
        }
        public static async Task<IEnumerable<UserModel>> AddUserModelsToContext(ContentRateDbContextTest context, int count,bool isMockUser = false)
        {
            var users = new List<UserModel>();
            for (int i = 0; i < count; i++)
            {
                var user = CreateUserModel();
                users.Add(user);
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();
            return users;
        }
    }
}

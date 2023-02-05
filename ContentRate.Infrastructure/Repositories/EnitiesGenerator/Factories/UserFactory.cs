using ContentRate.Domain.Users;
using ContentRate.Infrastructure.Models;

namespace ContentRate.Infrastructure.Repositories.EnitiesGenerator.Factories
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

    }
}

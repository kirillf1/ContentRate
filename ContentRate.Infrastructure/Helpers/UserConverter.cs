using ContentRate.Domain.Users;
using ContentRate.Infrastructure.Models;

namespace ContentRate.Infrastructure.Helpers
{
    internal static class UserConverter
    {
        public static User ConvertModelToUser(UserModel userModel)
        {
            return new User(userModel.Id, userModel.Name, userModel.Password)
            {
                IsMockUser = userModel.IsMockUser
            };
        }
        public static UserModel ConvertUserToModel(User user)
        {
            return new UserModel
            {
                Id = user.Id,
                IsMockUser = user.IsMockUser,
                Name = user.Name,
                Password = user.Password
            };
        }
    }
}

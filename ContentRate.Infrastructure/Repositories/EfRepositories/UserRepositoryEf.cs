using ContentRate.Domain.Users;
using ContentRate.Infrastructure.Contexts;
using ContentRate.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ContentRate.Infrastructure.Repositories.EfRepositories
{
    public class UserRepositoryEf : IUserRepository
    {
        private readonly ContentRateDbContext context;

        public UserRepositoryEf(ContentRateDbContext context)
        {
            this.context = context;
        }
        public async Task AddUser(User user)
        {
            var userModel = UserConverter.ConvertUserToModel(user);
            await context.Users.AddAsync(userModel);
        }

        public async Task DeleteUser(Guid id)
        {
            var userForDelete = await context.Users.SingleAsync(c => c.Id == id);
            context.Users.Remove(userForDelete);
        }

        public async Task<User> GetUserById(Guid id)
        {
            var userModel = await context.Users.SingleAsync(c => c.Id == id);
            return UserConverter.ConvertModelToUser(userModel);
        }

        public async Task<IEnumerable<User>> GetUsers(UserSearchCreteria creteria)
        {
            var query = context.Users.AsQueryable();
            var includeMockUsers = creteria.IncludeMockUsers == true;
            if (!includeMockUsers)
                query = query.Where(c => !c.IsMockUser);
            if (creteria.SkipCount.HasValue)
                query = query.Skip(creteria.SkipCount.Value);
            if (creteria.TakeCount.HasValue)
                query.Take(creteria.TakeCount.Value);
            return await query.Select(c => UserConverter.ConvertModelToUser(c)).ToListAsync();


        }

        public async Task<bool> HasUser(string name)
        {
           return await context.Users.AnyAsync(c => c.Name == name);
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> TryGetUser(string name, string password)
        {
            return await context.Users.Where(u => u.Name == name && u.Password == password).
                Select(u=>UserConverter.ConvertModelToUser(u))
                .SingleOrDefaultAsync();
        }

        public async Task UpdateUser(User user)
        {
            var userModel = await context.Users.SingleAsync(c => c.Id == user.Id);
            userModel.IsMockUser = user.IsMockUser;
            userModel.Name = user.Name;
            userModel.Password = user.Password;
            context.Users.Add(userModel);
        }
    }
}

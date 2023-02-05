namespace ContentRate.Domain.Users
{
    public record UserSearchCreteria(int? SkipCount= null, int? TakeCount = null, bool? IncludeMockUsers = null);
    public interface IUserRepository
    {
        public Task<User> GetUserById(Guid id);
        public Task<User?> TryGetUser(string name,string password);
        public Task<IEnumerable<User>> GetUsers(UserSearchCreteria creteria);
        public Task<bool> HasUser(string name);
        public Task UpdateUser(User user);
        public Task DeleteUser(Guid id);
        public Task AddUser(User user);
        public Task SaveChanges(CancellationToken cancellationToken = default);
    }
}

namespace ContentRate.Domain.Users
{
    public class User
    {
        public User(Guid id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = "";
            ChangePassword(password);
            IsMockUser = false;
        }

        public Guid Id { get; }
        public string Name { get; set; }
        public string Password { get; private set; }
        public bool IsMockUser { get; set; }
        public void ChangePassword(string password)
        {
            if (!IsValidPassword(password))
                throw new ArgumentException("Password length mast be more than 5 chars");
            Password = password;
        }
       
        private static bool IsValidPassword(string password)
        {
            return password.Length > 5;
        }
    }
}

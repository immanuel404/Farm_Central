namespace Farm_Central.Models
{
    public class UserRepository : IUserService
    {
        private readonly DBContext _dbContext;
        public UserRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // READ USERS
        public IEnumerable<User> GetAllUsers()
        {
            return _dbContext.Users;
        }

        // CREATE USER
        public void CreateUsers(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
    }

    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        void CreateUsers(User user);
    }
}

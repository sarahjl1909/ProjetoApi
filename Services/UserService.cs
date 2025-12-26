using ProjetoApi.Models;
using ProjetoApi.Dtos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.Connections;

namespace ProjetoApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Role> _rolesCollection;

        public UserService(IOptions<MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(settings.Value.UsersCollectionName);
            _rolesCollection = mongoDatabase.GetCollection<Role>(settings.Value.RolesCollectionName);
            
        }

        public async Task<Role> CreateRoleAsync(string roleName)
        {
            var existingRole = await _rolesCollection.Find(r => r.Name == roleName).FirstOrDefaultAsync();
            if (existingRole != null) return existingRole;

            var newRole = new Role { Name = roleName};
            await _rolesCollection.InsertOneAsync(newRole);
            return newRole;
        }

        public async Task<List<Role>> GetAllRolesAsync() =>
            await _rolesCollection.Find(_ => true).ToListAsync();

        public async Task<User?> CreateUserAsync(RegisterUserDto registerDto)
        {
            var existingUser = await _usersCollection.Find(u => u.UserName == registerDto.UserName).FirstOrDefaultAsync();
            if (existingUser != null) throw new System.Exception("Username já está em uso.");

            var filter = Builders<Role>.Filter.In(r => r.Name, registerDto.Roles);
            var validRoles = await _rolesCollection.Find(filter).ToListAsync();
            var roleIds = validRoles.Select(r => r.Id!).ToList();

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new User
            {
                UserName = registerDto.UserName,
                PasswordHash = passwordHash,
                RoleIds = roleIds
            };

            await _usersCollection.InsertOneAsync(newUser);
            return newUser;
        }

        public async Task<User?> ValidateUserCredentialsAsync(string userName, string password)
        {
            var user = await _usersCollection.Find(u => u.UserName == userName).FirstOrDefaultAsync();
            if (user == null) return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isPasswordValid ? user: null;
        }

        public async Task<List<User>> GetAllUsersAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserByIdAsync(string id) =>
            await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
    }
}
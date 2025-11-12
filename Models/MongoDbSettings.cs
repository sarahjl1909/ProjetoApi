namespace ProjetoApi.Models
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;

        public string UsersCollectionName { get; set; } = null!;
        public string RolesCollectionName { get; set; } = null!;
    }
}
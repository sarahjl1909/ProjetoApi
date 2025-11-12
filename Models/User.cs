using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ProjetoApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public List<string> RoleIds { get; set; } = new List<string>();
    }
}
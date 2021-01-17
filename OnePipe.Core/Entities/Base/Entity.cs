using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnePipe.Core.Entities.Base
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity<string>
    {
        public Entity() : base() { }

        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities;

public class ProductBrand: BaseEntity
{
    [BsonElement(elementName:"Name")]
    public string Name { get; set; }
}
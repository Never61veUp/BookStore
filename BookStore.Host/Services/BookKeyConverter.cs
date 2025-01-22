using System.Text.Json;
using System.Text.Json.Serialization;
using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.ValueObjects;

namespace BookStore.Host.Services;

public class BookKeyConverter : JsonConverter<Dictionary<Book, int>>
{
    public override Dictionary<Book, int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Deserialization not supported.");
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<Book, int> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key.Id.ToString()); // Идентификатор как ключ
            writer.WriteStartObject(); // Начинаем вложенный объект для книги
            writer.WriteString("Title", kvp.Key.Title);
            writer.WriteString("AuthorFullName", kvp.Key.AuthorFullName.ToString());
            writer.WriteNumber("Count", kvp.Value);
            writer.WriteEndObject();
            // writer.WritePropertyName(kvp.Key.Id.ToString());
            // writer.WritePropertyName(kvp.Key.Title);
            // writer.WritePropertyName(kvp.Key.AuthorFullName.ToString());
            // JsonSerializer.Serialize(writer, kvp.Value, options);
        }
        writer.WriteEndObject();
    }
    
}
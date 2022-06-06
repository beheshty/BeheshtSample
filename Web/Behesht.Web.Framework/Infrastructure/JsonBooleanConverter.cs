using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Behesht.Web.Framework.Infrastructure
{
    public class JsonBooleanConverter : System.Text.Json.Serialization.JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (value == "true" || value == "True")
                    return true;
                else if(value == "false" || value == "False")
                    return false;
                throw new System.Text.Json.JsonException();
            }
            else if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
            {
                return reader.GetBoolean();
            }
            throw new System.Text.Json.JsonException();
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            if (value)
            {
                writer.WriteStringValue("true");
            }
            else
            {
                writer.WriteStringValue("false");
            }
        }

    }
}

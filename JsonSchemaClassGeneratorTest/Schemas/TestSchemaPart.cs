using Newtonsoft.Json.Linq;

namespace JsonSchemaClassGeneratorTest.TestSchema
{
    public partial class ComplexObject1
    {
        public static implicit operator ComplexObject1(JObject json)
        {
            var type = json.Property("type")?.Value.ToString();
            return type == "1" ? FromJson(json.ToString()) : null;
        }
    }

    public partial class ComplexObject2
    {
        public static implicit operator ComplexObject2(JObject json)
        {
            var type = json.Property("type")?.Value.ToString();
            return type == "2" ? FromJson(json.ToString()) : null;
        }
    }
}
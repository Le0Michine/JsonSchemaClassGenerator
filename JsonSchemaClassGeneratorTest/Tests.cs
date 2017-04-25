using System.IO;
using JsonSchemaClassGenerator;
using JsonSchemaClassGeneratorTest.TestSchema;
using Microsoft.Build.Framework;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonSchemaClassGeneratorTest
{
    [TestFixture]
    public class Tests
    {
        private string _solutionRelativePath = "../../../";

        private GenerationUtility InitGenerationAction()
        {
            var generationUtility = new GenerationUtility();
            Mock<IBuildEngine> buildEngine = new Mock<IBuildEngine>();
            generationUtility.BuildEngine = buildEngine.Object;
            return generationUtility;
        }

        private string ReadJsonConfig(string jsonFile)
        {
            return File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestJson", jsonFile));
        }

        [Test]
        [TestCase(arguments: "JsonSchemaClassGeneratorTest/Schemas/TestSchema.json")]
        public void GenerateSimpleSchema(string schema)
        {
            var generationUtility = InitGenerationAction();
            var schemaPath = Path.Combine(TestContext.CurrentContext.TestDirectory, _solutionRelativePath, schema);
            var outputPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "output");
            Assert.DoesNotThrow(() => generationUtility.Generate(schemaPath, outputPath));
        }

        [Test]
        [TestCase("test-schema.json")]
        public void DeserialiseSimpleTestJson(string jsonFile)
        {
            var config = SimpleTestSchema.FromJson(ReadJsonConfig(jsonFile));
            foreach (var o in config.MyArray)
            {
                if (o is string)
                {
                    Assert.AreEqual("stringItem1", o as string);
                }
                else if (o is JObject)
                {
                    ComplexObject1 c1 = (o as JObject);
                    ComplexObject2 c2 = (o as JObject);

                    string type = (o as JObject).Property("type")?.Value.ToString();

                    switch (type)
                    {
                        case "1":
                            Assert.NotNull(c1);
                            Assert.Null(c2);
                            Assert.AreEqual(ComplexObjectType._1, c1.Type);
                            Assert.AreEqual("complexObj1", c1.Name);
                            break;
                        case "2":
                            Assert.Null(c1);
                            Assert.NotNull(c2);
                            Assert.AreEqual(ComplexObjectType._2, c2.Type);
                            Assert.AreEqual("complexObj2", c2.Name);
                            break;
                        case null:
                            Assert.Null(c1);
                            Assert.Null(c2);
                            break;
                    }
                }
            }
        }
    }
}
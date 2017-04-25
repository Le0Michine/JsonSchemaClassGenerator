using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace JsonSchemaClassGenerator
{
    public class GenerationUtility : Task
    {
        #region public properties

        /// <summary>
        /// Namespace name for generated classes.
        /// </summary>
        [Required]
        public string Namespace { get; set; }

        /// <summary>
        /// It's list of input .json files
        /// that has an GenerateJsonClasses bild action.
        /// </summary>
        [Required]
        public ITaskItem[] InputFiles { get; set; }

        /// <summary>
        /// Path to output .cs files.
        /// </summary>
        [Required]
        public string OutputFiles { get; set; }

        #endregion

        #region override methods

        public override bool Execute()
        {
            var items = InputFiles.Select(item => item.ItemSpec).ToList();
            if (!Directory.Exists(OutputFiles.Trim()))
            {
                Directory.CreateDirectory(OutputFiles.Trim());
            }
            Log.LogMessage($"Namespace: {Namespace}");
            Log.LogMessage($"Input files: {string.Join(", ", items)}");
            Log.LogMessage($"Output path: {OutputFiles}");
            items.ForEach(x => Generate(x, OutputFiles));
            return true;
        }

        public void Generate(string jsonSchemaFilePath, string outputPath = null)
        {
            if (!File.Exists(jsonSchemaFilePath))
            {
                Log.LogError($"unable to find file {jsonSchemaFilePath}");
            }

            var settings = new CSharpGeneratorSettings
            {
                Namespace = (Namespace ?? "JsonSchemaClassGenerator") + "." + Path.GetFileNameWithoutExtension(jsonSchemaFilePath),
                HandleReferences = false,
            };
            var jsonSchema4 = JsonSchema4.FromFileAsync(jsonSchemaFilePath).Result;
            var generator = new CSharpGenerator(jsonSchema4, settings, new CustomCSharpTypeResolver(settings, jsonSchema4), null);
            var file = generator.GenerateFile();

            var changeExtension = Path.ChangeExtension(jsonSchemaFilePath ?? "", "cs");
            outputPath = outputPath == null ? changeExtension : Path.Combine(outputPath, Path.GetFileName(changeExtension));

            if (!Directory.Exists(Path.GetDirectoryName(outputPath)) && !string.IsNullOrEmpty(Path.GetDirectoryName(outputPath)))
            {
                Directory.CreateDirectory(outputPath);
            }

            Log.LogMessage($"write file {Path.GetFileName(jsonSchemaFilePath)}");
            File.WriteAllText(outputPath, file);
        }

        #endregion
    }
}
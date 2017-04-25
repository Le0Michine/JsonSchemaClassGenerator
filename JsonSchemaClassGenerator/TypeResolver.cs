// Decompiled with JetBrains decompiler
// Type: NJsonSchema.CodeGeneration.CSharp.CSharpTypeResolver
// Assembly: NJsonSchema.CodeGeneration.CSharp, Version=8.32.6319.16954, Culture=neutral, PublicKeyToken=c2f9c3bdfae56102
// MVID: 80A06273-32DB-48F1-B1E2-B571757D0EBD
// Assembly location: /Users/leo/div/DNNZipBundler/packages/NJsonSchema.CodeGeneration.CSharp.8.32.6319.16954/lib/portable45-net45+win8+wp8+wpa81/NJsonSchema.CodeGeneration.CSharp.dll

using System.Linq;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace JsonSchemaClassGenerator
{
    /// <summary>Manages the generated types and converts JSON types to CSharp types. </summary>
    public class CustomCSharpTypeResolver : CSharpTypeResolver
    {
        /// <summary>Initializes a new instance of the <see cref="CSharpTypeResolver"/> class.</summary>
        /// <param name="settings">The generator settings.</param>
        /// <param name="rootObject">The root object to search for JSON Schemas.</param>
        public CustomCSharpTypeResolver(CSharpGeneratorSettings settings, object rootObject)
            : base(settings, rootObject)
        {
        }

        /// <summary>Resolves and possibly generates the specified schema.</summary>
        /// <param name="schema">The schema.</param>
        /// <param name="isNullable">Specifies whether the given type usage is nullable.</param>
        /// <param name="typeNameHint">The type name hint to use when generating the type and the type name is missing.</param>
        /// <returns>The type name.</returns>
        public override string Resolve(JsonSchema4 schema, bool isNullable, string typeNameHint)
        {
            if (schema.ActualSchema.AnyOf.Count > 0)
            {
//                var res = schema.AnyOf.Select(x => Resolve(x, false, null)).ToList();
                return "object";
            }

            return base.Resolve(schema, isNullable, typeNameHint);
        }
    }
}

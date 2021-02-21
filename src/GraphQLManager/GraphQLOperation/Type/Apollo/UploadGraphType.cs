using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLManager.GraphQLOperation.Type.Apollo
{
    public class UploadGraphType : ObjectGraphType<UploadItem>
    {
        public UploadGraphType()
        {
            Field(t => t.Id).Name("id").Description("Id for member");
            Field(t => t.Path).Name("Path").Description("Path for the file");
            Field(t => t.Filename).Name("filename").Description("Name of the file");
            Field(t => t.Mimetype).Name("mimetype").Description("Mimetype of the file");
            Field(t => t.Encoding).Name("encoding").Description("File content");

        }
    }

    public class UploadItem
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string Filename { get; set; }
        public string Mimetype { get; set; }
        public string Encoding { get; set; }
    }
}

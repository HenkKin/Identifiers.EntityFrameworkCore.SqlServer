using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    public class IdentifierIdentityAnnotationProvider : MigrationsAnnotationProvider
    {
        public IdentifierIdentityAnnotationProvider(MigrationsAnnotationProviderDependencies dependencies)
            : base(dependencies)
        {
        }

        public override IEnumerable<IAnnotation> For(IProperty property)
        {
            var baseAnnotations = base.For(property);

            var annotation = property.FindAnnotation("Identifier");
            return annotation == null
                ? baseAnnotations
                : baseAnnotations.Concat(new[] { new Annotation("SqlServer:ValueGenerationStrategy", annotation.Value)  });
        }
    }
}

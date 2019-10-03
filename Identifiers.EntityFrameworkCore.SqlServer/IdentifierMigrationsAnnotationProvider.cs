using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    internal class IdentifierMigrationsAnnotationProvider : SqlServerMigrationsAnnotationProvider
    {
        public IdentifierMigrationsAnnotationProvider(MigrationsAnnotationProviderDependencies dependencies)
            : base(dependencies)
        {
        }

        public override IEnumerable<IAnnotation> For(IProperty property)
        {
            var baseAnnotations = base.For(property);

            var annotation = property.FindAnnotation("Identifier");
            return annotation == null
                ? baseAnnotations
                : baseAnnotations.Concat(new[] { new Annotation("SqlServer:ValueGenerationStrategy", annotation.Value) });
        }
    }
}   
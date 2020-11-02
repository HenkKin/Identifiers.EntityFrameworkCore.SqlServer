using Identifiers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataAccessClientExample.DataLayer
{
    public interface IEntity
    {
        public Identifier Id { get; set; }
    }
    public abstract class BaseEntity : IEntity
    {
        public Identifier Id { get; set; }
    }
    public class ExampleEntity : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ExampleEntityTranslation> Translations { get; set; } = new Collection<ExampleEntityTranslation>();

    }

    public class ExampleEntityTranslation : IEntity
    {
        public Identifier Id { get; set; }
        public string Description { get; set; }
        public string LocaleId { get; set; }
        public ExampleEntity TranslatedEntity { get; set; }
        public int TranslatedEntityId { get; set; }
    }
}

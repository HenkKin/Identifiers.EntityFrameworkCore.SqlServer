using Identifiers;
using System;
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
    public class ExampleIdentifierEntity : BaseEntity
    {
        public string Name { get; set; }
        public Identifier? Test { get; set; }
        public ICollection<ExampleIdentifierEntityTranslation> Translations { get; set; } = new Collection<ExampleIdentifierEntityTranslation>();
    }

    public class ExampleIdentifierEntityTranslation : IEntity
    {
        public Identifier Id { get; set; }
        public string Description { get; set; }
        public string LocaleId { get; set; }
        public ExampleIdentifierEntity ExampleIdentifierEntity { get; set; }
        public Identifier ExampleIdentifierEntityId { get; set; }
    }

    public class ExampleGuidEntity
    {
        public Guid Id { get; set; }
        public Guid? Test { get; set; }
        public ICollection<ExampleGuidEntityTranslation> Translations { get; set; } = new Collection<ExampleGuidEntityTranslation>();

    }

    public class ExampleGuidEntityTranslation
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string LocaleId { get; set; }
        public ExampleGuidEntity ExampleGuidEntity { get; set; }
        public Guid ExampleGuidEntityId { get; set; }
    }

    public class ExampleIntEntity
    {
        public int Id { get; set; }
        public int? Test { get; set; }
        public ICollection<ExampleIntEntityTranslation> Translations { get; set; } = new Collection<ExampleIntEntityTranslation>();

    }

    public class ExampleIntEntityTranslation
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string LocaleId { get; set; }
        public ExampleIntEntity ExampleIntEntity { get; set; }
        public int ExampleIntEntityId { get; set; }
    }

    public class ExampleLongEntity
    {
        public long Id { get; set; }
        public long? Test { get; set; }
        public ICollection<ExampleLongEntityTranslation> Translations { get; set; } = new Collection<ExampleLongEntityTranslation>();

    }

    public class ExampleLongEntityTranslation
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string LocaleId { get; set; }
        public ExampleLongEntity ExampleLongEntity { get; set; }
        public long ExampleLongEntityId { get; set; }
    }

}

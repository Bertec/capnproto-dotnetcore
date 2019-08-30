using System.Diagnostics;

namespace CapnpC.Model
{
    class Annotation : IDefinition
    {
        public ulong Id { get; }
        public bool IsGenerated { get; }
        public TypeTag Tag => TypeTag.Annotation;
        public IHasNestedDefinitions DeclaringElement { get; }

        public Type Type { get; set; }

        public Annotation(ulong id, IHasNestedDefinitions parent)
        {
            Trace.Assert(parent != null);
            Id = id;
            IsGenerated = ((IDefinition) parent).IsGenerated;
            DeclaringElement = parent;
            parent.NestedDefinitions.Add(this);
        }
    }
}

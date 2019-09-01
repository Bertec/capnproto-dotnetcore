using System.Collections.Generic;

namespace CapnpC.Model
{
    class GenFile: IDefinition, IHasNestedDefinitions
    {
        public ulong Id { get;  }
        public bool IsGenerated { get;  }
        public TypeTag Tag => TypeTag.File;
        public IHasNestedDefinitions DeclaringElement => null;

        public string Name { get; set; }
        public string[] Namespace { get; set; }

        public IEnumerable<TypeDefinition> NestedTypes => this.GetNestedTypes();
        public ICollection<IDefinition> NestedDefinitions { get; } = new List<IDefinition>();
        public ICollection<Constant> Constants { get; } = new List<Constant>();

        public GenFile(ulong id, bool isGenerated)
        {
            Id = id;
            IsGenerated = isGenerated;
        }
    }
}

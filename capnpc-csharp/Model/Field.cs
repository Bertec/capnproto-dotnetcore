namespace CapnpC.Model
{
    class Field
    {
        public string Name { get; }
        public TypeDefinition DeclaringType { get; }
        public Field Parent { get; set; }
        public Type Type { get; set; }
        public Value DefaultValue { get; set; }
        public bool DefaultValueIsExplicit { get; set; }
        public ushort? DiscValue { get; set; }
        public uint Offset { get; set; }
        public int CodeOrder { get; set; }

        public ulong? BitOffset => (ulong)Offset * Type?.FixedBitWidth;

        public Field(string name, TypeDefinition declaringType)
        {
            Name = name;
            DeclaringType = declaringType;
        }

        public Field Clone()
        {
            var field = new Field(Name, DeclaringType)
            {
                Parent = Parent,
                Type = Type,
                DefaultValue = DefaultValue,
                DefaultValueIsExplicit = DefaultValueIsExplicit,
                DiscValue = DiscValue,
                Offset = Offset,
                CodeOrder = CodeOrder,
            };
            field.InheritFreeGenericParameters();
            return field;
        }

        public void InheritFreeGenericParameters()
        {
            Type.InheritFreeParameters(DeclaringType);
        }

        public override bool Equals(object obj)
        {
            return obj is Field other &&
                DeclaringType == other.DeclaringType &&
                Name == other.Name;
        }

        public override int GetHashCode()
        {
            return (DeclaringType, Name).GetHashCode();
        }
    }
}

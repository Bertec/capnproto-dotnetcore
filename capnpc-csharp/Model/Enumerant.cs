﻿namespace CapnpC.Model
{
    class Enumerant
    {
        string _literal;
        public TypeDefinition TypeDefinition { get; set; }
        public string Literal {
            get => _literal;
            set => _literal = IdentifierRenamer.ToNonKeyword(value);
        }
        public ushort? Ordinal { get; set; }
        public int CodeOrder { get; set; }
    }
}

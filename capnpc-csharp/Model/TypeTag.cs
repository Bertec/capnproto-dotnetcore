﻿namespace CapnpC.Model
{
    enum TypeTag
    {
        Unknown,
        Void,
        Bool,
        S8,
        U8,
        S16,
        U16,
        S32,
        U32,
        S64,
        U64,
        F32,
        F64,
        List,
        Data,
        Text,
        AnyPointer,
        StructPointer,
        ListPointer,
        CapabilityPointer,
        ParameterPointer, // TODO: unused
        ImplicitMethodParameterPointer, // TODO: unused
        Struct,
        Group,
        Interface,
        Enum,
        AnyEnum,
        Const,
        Annotation,
        File
    }
}

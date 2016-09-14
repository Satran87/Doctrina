using System;

namespace Doctrina.Enums
{
    [Flags]
    public enum WorkLikeEnum
    {
        OnlyGenerator=1,
        GeneratorAndConst=2,
        GeneratorAndLST=3
    };
}

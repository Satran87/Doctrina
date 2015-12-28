using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

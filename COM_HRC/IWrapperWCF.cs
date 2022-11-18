using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace COM_HRC
{
    [Guid("F04099E7-FF3C-48DA-8FF1-7BAA23F7EFFF")]
    interface IWrapperWCF
    {
        void Init();
        string CalcDeterminant(int[,] _value);
        string FilterAndOrderValues(int[,] _value);
        string CalcDeterminant(string[] _value);
        string FilterAndOrderValues(string[] _value);

    }
}

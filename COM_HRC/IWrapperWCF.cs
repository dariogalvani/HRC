using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM_HRC
{
    interface IWrapperWCF
    {
        void Init();
        string CalcDeterminant(int[,] _value);
        string FilterAndOrderValues(int[,] _value);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace COM_HRC
{
    public class COMWrapperWCF:IWrapperWCF
    {
        public void Init()
        {

        }
        public  string CalcDeterminant(int[,] _value)
        {
            WCFReference.WCFMatrixClient client = new WCFReference.WCFMatrixClient();
            List<int[]> curMatrixData = HRC_Service.MatrixHRC.FromMatrixtoList(_value);
            int result = client.CalcDeterminant(curMatrixData.ToArray());
            return result.ToString();
        }

        public  string FilterAndOrderValues(int[,] _value)
        {
            WCFReference.WCFMatrixClient client = new WCFReference.WCFMatrixClient();
            List<int[]> curMatrixData = HRC_Service.MatrixHRC.FromMatrixtoList(_value);
            string result= client.FilterAndOrderValues(curMatrixData.ToArray());
            return result;

        }

    }
}

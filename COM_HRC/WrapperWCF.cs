using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace COM_HRC
{
    [Guid("C1D53234-C73D-434E-9797-8283B092AF60")]
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

        public string CalcDeterminant(string[] _value)
        {
            WCFReference.WCFMatrixClient client = new WCFReference.WCFMatrixClient();
            int result = HRC_Service.MatrixHRC.CalcDeterminant(_value);
            return result.ToString();
        }
    
        public string FilterAndOrderValues(string[] _value)
        {
            WCFReference.WCFMatrixClient client = new WCFReference.WCFMatrixClient();
            string result = HRC_Service.MatrixHRC.FilterAndOrderValues(_value);
            return result;
        }
    }
}

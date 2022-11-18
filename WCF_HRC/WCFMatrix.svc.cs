using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Windows.Media;

namespace WCF_HRC
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "Service1" nel codice, nel file svc e nel file di configurazione contemporaneamente.
    // NOTA: per avviare il client di prova WCF per testare il servizio, selezionare Service1.svc o Service1.svc.cs in Esplora soluzioni e avviare il debug.
    public class WCFMAtrix : IWCFMatrix
    {
        public int CalcDeterminant(List<int[]> refMatrix)
        {

            var matrixValues = HRC_Service.MatrixHRC.CalcDeterminant(refMatrix);
            return matrixValues;

        }

        public string FilterAndOrderValues(List<int[]> refMatrix)
        {
            string retValues = HRC_Service.MatrixHRC.FilterAndOrderValues(refMatrix); ;
            return retValues;
        }
    }
}

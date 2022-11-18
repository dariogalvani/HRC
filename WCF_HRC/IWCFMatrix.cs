using System.Collections.Generic;
using System.ServiceModel;

namespace WCF_HRC
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di interfaccia "IService1" nel codice e nel file di configurazione contemporaneamente.
    [ServiceContract]
    public interface IWCFMatrix
    {

        [OperationContract]
        int CalcDeterminant(List<int[]> refMatrix);

        [OperationContract]
        string FilterAndOrderValues(List<int[]> refMatrix);

        // TODO: aggiungere qui le operazioni del servizio
    }
}

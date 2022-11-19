using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HRC_Service
{
    public class MatrixHRC
    {
        /// <summary>
        /// challenge request -> set grid initial values to random values
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static DataView GetMatrix(int n)
        {
            if (n < 0)
                throw new ArgumentException("n must be a positive integer.", "n");
            DataTable myDT = new DataTable();
            for (int _colcounter = 0; _colcounter < n; _colcounter++)
            {
                myDT.Columns.Add("col " + (_colcounter + 1), typeof(Int32));
            }
            DataSet myDS = new DataSet();
            for (int _rowcounter = 0; _rowcounter < n; _rowcounter++)
            {
                // build 1 row with all columns
                DataRow newRow = myDT.NewRow();
                for (int _counter = 0; _counter < n; _counter++)
                {
                    Thread.Sleep(10);
                    Random myRnd = new Random();
                    newRow[_counter] = (myRnd.Next(1, 9));
                }
                myDT.Rows.Add(newRow);
            }
            myDS.Tables.Add(myDT);
            return myDS.Tables[0].DefaultView;
        }

        /// <summary>
        /// transform list of int into matrix
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static int[,] FromListtoMatrix(List<int[]> myData)
        {
            int _size = myData.Count;
            var myMAtrix = new int[_size, _size];

            for (int x = 0; x < _size; x++)
            {
                int[] rowMAtrix = myData[x];
                for (int k = 0; k < _size; k++)
                {
                    myMAtrix[x, k] = rowMAtrix[k];  
                }

            }

            return myMAtrix;
        }


        /// <summary>
        /// Trasfrom array of array of int into Matrix
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static int[,] FromArraytoMatrix(int[][] myData)
        {
            int _size = myData.GetLength(0);
            var myMAtrix = new int[_size, _size];

            for (int x = 0; x < _size; x++)
            {
                for (int k = 0; k < _size; k++)
                {
                    myMAtrix[x, k] = 1;  //(int)myData[x]. ItemArray[k];
                }

            }

            return myMAtrix;
        }

        /// <summary>
        /// transform list of csv string into a matrix
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static int[,] FromListStringtoMatrix(string[] myData)
        {
            int _size = myData.Length;
            var myMAtrix = new int[_size, _size];
            string[] separatorVals = { "," };
            for (int x = 0; x < _size; x++)
            {
                string[] vals = myData[x].Split(separatorVals, StringSplitOptions.RemoveEmptyEntries);
                for (int k = 0; k < _size; k++)
                {
                    myMAtrix[x, k] = int.Parse(vals[k]);  //(int)myData[x]. ItemArray[k];
                }

            }

            return myMAtrix;
        }

        /// <summary>
        /// challenge request -> get value of matrix filtered and ordered
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static string FilterAndOrderValues(string[] myData)
        {
            List<int> retList = new List<int>();
            int[,] curMAtrix = FromListStringtoMatrix(myData);
            int _size = curMAtrix.GetLength(0);

            for (int x = 0; x < _size; x++)
            {
                for (int k = 0; k < _size; k++)
                {
                    if (curMAtrix[x, k] % 2 == 0) // even number avoid odd numbers
                    {
                        if (retList.Contains(curMAtrix[x, k]))
                        {
                            retList.Remove(curMAtrix[x, k]); //if number exists remove from the list
                        }
                        else
                        {
                            retList.Add(curMAtrix[x, k]); //if number doesn't exist add to the list
                        }
                    }

                }
            }

            string retString = "";
            retList.Sort();  // sort the list 
            foreach (int xItem in retList)
                retString += xItem + " "; //string format result
            return retString;
        }

        //from a list o array int I
        public static string FilterAndOrderValues(List<int[]> myData)
        {
            List<int> retList = new List<int>();

            int[,] curMAtrix = FromListtoMatrix(myData);

            int _size = curMAtrix.GetLength(0);

            for (int x = 0; x < _size; x++)
            {
                for (int k = 0; k < _size; k++)
                {
                    if (curMAtrix[x, k] % 2 == 0) // even number
                    {
                        if (retList.Contains(curMAtrix[x, k]))
                        {
                            retList.Remove(curMAtrix[x, k]);
                        }
                        else
                        {
                            retList.Add(curMAtrix[x, k]);
                        }
                    }
                    
                }
            }

            string retString = "";
            retList.Sort();
            foreach (int xItem in retList)
                retString += xItem + " ";
            return retString;
        }

        /// <summary>
        /// entry point for determinant calculation
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static int CalcDeterminant(string[] myData)
        {
            int[,] refMatrix = FromListStringtoMatrix(myData);
            int retVal = Determinant(refMatrix);
            return retVal;
        }

        /// <summary>
        /// entry point for determinant calculation
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static int CalcDeterminant(List<int[]> myData)
        {
            int[,] refMatrix = FromListtoMatrix(myData);
            int retVal = Determinant(refMatrix);
            return retVal;
        }

        /// <summary>
        /// challenge erquest paste data into all selected cells
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static void PasteDataIntoGrid(DataGrid objDataGrid )
        {
            try
            {
                if (!(Clipboard.ContainsData(DataFormats.Text)))
                        return;
                string data = Clipboard.GetData(DataFormats.Text).ToString();
                string[] separatorRow = { "\r\n" };
                string[] separatorCol = { "\t" };
                string[] rowsIntoClipboard = data.Split(separatorRow, StringSplitOptions.RemoveEmptyEntries);
                int maxRowClipboard = rowsIntoClipboard.Length;
                int maxColClipboard = rowsIntoClipboard[0].Split(separatorCol, StringSplitOptions.RemoveEmptyEntries).Length;

                /// BUILD LIST OF ARRAY[]
                List<int[]> valuesToPaste = fromStringToArray(data, maxColClipboard);

                IList<DataGridCellInfo> cellAvailable = objDataGrid.SelectedCells;
                var itemsSource = objDataGrid.ItemsSource as IEnumerable;
                int rowAvailable = objDataGrid.CurrentCell.Column.DisplayIndex;
                int displayOld = -1;
                int currListRowIndex = 0;
                int currIntValues = 0;
                foreach (DataGridCellInfo curCellInfo in cellAvailable)
                {
                    //cells are managed row by row scrolling columns
                    if (curCellInfo.Column.GetCellContent(curCellInfo.Item).GetType() == typeof(TextBlock))
                    {
                        TextBlock cellToUpdate = (TextBlock)curCellInfo.Column.GetCellContent(curCellInfo.Item);
                        //if column index is smaller we are into a new row
                        if (curCellInfo.Column.DisplayIndex < displayOld)
                        {
                            currListRowIndex++;
                            if (currListRowIndex >= maxRowClipboard) //to manage when row selected cells are more than copied ones
                                currListRowIndex = 0;
                            currIntValues = 0;
                            displayOld = curCellInfo.Column.DisplayIndex;
                            int[] colValues = valuesToPaste[currListRowIndex];
                            cellToUpdate.Text = colValues[currIntValues].ToString();
                            currIntValues++;
                        }
                        else
                        {
                            displayOld = curCellInfo.Column.DisplayIndex;
                            int[] colValues = valuesToPaste[currListRowIndex];
                            if (currIntValues >= colValues.Length) //to manage when columns selected cells are more than copied ones
                                currIntValues = 0;
                            cellToUpdate.Text = colValues[currIntValues].ToString();
                            currIntValues++;
                        }
                    }
                }
            }
            catch
            {
              
            }
        }
        /// <summary>
        /// transform list of string into a list of int
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        private static List<int[]> fromStringToArray(string data,int numCols)
        {
            List<int[]> retArray = new List<int[]>();
            string[] separatorRow = { "\r\n" };
            string[] separatorCol = { "\t" };
            string[] rowsIntoClipboard = data.Split(separatorRow, StringSplitOptions.RemoveEmptyEntries);

            foreach (string curRow in rowsIntoClipboard)
            {
                int[] rowData = new int[numCols];

                string[] colValues = curRow.Split(separatorCol, StringSplitOptions.RemoveEmptyEntries);
                int counterCols = 0;
                foreach (string curCol in colValues)
                {
                    rowData[counterCols] = int.Parse(curCol);
                    counterCols++;
                }

                retArray.Add(rowData);
            }

            return retArray;
        }

        private static int _row = 0;
           /// <summary>
        /// transform datatable into a matrix
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static int[,] FromDStoMatrix(DataTable myDT)
        {
            int _size = myDT.Rows.Count;
            var myMAtrix = new int[_size, _size];

            for (int x = 0; x < _size;x++)
            {
               for(int k=0;k<_size;k++)
                {
                    myMAtrix[x, k] = (int)myDT.Rows[x].ItemArray[k];
                }

            }

            _row = _size;
            return myMAtrix;

        }

        /// <summary>
        /// transform datatable into a list of int
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static List<int[]> FromDStoList(DataTable myDT)
        {
            List<int[]> retList = new List<int[]>();
            int _size = myDT.Rows.Count;

            for (int x = 0; x < _size; x++)
            {
                int[] myMatrixRow = new int[_size];
                for (int k = 0; k < _size; k++)
                {
                    myMatrixRow[k] = (int)myDT.Rows[x].ItemArray[k];
                }
                retList.Add(myMatrixRow);
            }
            return retList;
        }
        /// <summary>
        /// transform matrix into a list of int
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static List<int[]> FromMatrixtoList(int[,] myDT)
        {
            List<int[]> retList = new List<int[]>();
            int _size = myDT.GetLength(0);

            for (int x = 0; x < _size; x++)
            {
                int[] myMatrixRow = new int[_size];
                for (int k = 0; k < _size; k++)
                {
                    myMatrixRow[k] = (int)myDT[x,k];
                }
                retList.Add(myMatrixRow);
            }
            return retList;
        }

        public static int Determinant(int[,] inputMatrix)
        {
            MatrixEntity transfMatrix = new MatrixEntity(inputMatrix);
            int retVal = MatrixEntity.Determinant(transfMatrix);
            return retVal;
        }
    }
}

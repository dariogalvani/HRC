﻿using System;
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
        public static string FilterAndOrderValues(string[] myData)
        {
            List<int> retList = new List<int>();
            int[,] curMAtrix = FromListStringtoMatrix(myData);
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
            foreach (int xItem in retList)
                retString += xItem + " ";
            return retString;
        }
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
            foreach (int xItem in retList)
                retString += xItem + " ";
            return retString;
        }

        public static int CalcDeterminant(string[] myData)
        {
            int[,] refMatrix = FromListStringtoMatrix(myData);
            int retVal = Determinant(refMatrix);
            return retVal;
        }

        public static int CalcDeterminant(List<int[]> myData)
        {
            int[,] refMatrix = FromListtoMatrix(myData);
            int retVal = Determinant(refMatrix);
            return retVal;
        }

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
                    if (curCellInfo.Column.GetCellContent(curCellInfo.Item).GetType() == typeof(TextBlock))
                    {
                        TextBlock cellToUpdate = (TextBlock)curCellInfo.Column.GetCellContent(curCellInfo.Item);
                        if (curCellInfo.Column.DisplayIndex < displayOld)
                        {
                            currListRowIndex++;
                            if (currListRowIndex >= maxRowClipboard)
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
                            if (currIntValues >= colValues.Length)
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

        public static void ReplaceValIntoSelectedCells(DataGrid grdData)
        {
            //throw new NotImplementedException();
        }

        private static int _row = 0;
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

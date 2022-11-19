using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRC
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// local variable to store current value of the cell
        /// </summary>
        int oldValue = -1;

        public MainWindow()
        {
            InitializeComponent();
            initEnv();
        }

        /// <summary>
        /// Set the default behaviour of the app
        /// </summary>
        private void initEnv()
        {
            //hide error message
            lblErr.Visibility = Visibility.Hidden;

            //combobox settings
            cmbGridsize.Items.Clear();
            cmbGridsize.Items.Add("4x4");
            cmbGridsize.Items.Add("5x5");
            cmbGridsize.Items.Add("36x36");

            //handler to manage SelectionChanged on the combobox
            cmbGridsize.SelectionChanged += CmbGridsize_SelectionChanged;
            cmbGridsize.SelectedIndex = 0;
            grdData.SelectionMode = DataGridSelectionMode.Extended;
            grdData.SelectionUnit = DataGridSelectionUnit.Cell;

            //grid settings
            grdData.ClipboardCopyMode = DataGridClipboardCopyMode.ExcludeHeader;
            grdData.KeyDown += GrdData_KeyDown;
            grdData.IsReadOnly = false;
            grdData.CanUserAddRows = false;
            grdData.CanUserDeleteRows = false;
            grdData.CanUserResizeColumns = false;
            grdData.CellEditEnding += GrdData_CellEditEnding;
            grdData.PreparingCellForEdit += GrdData_PreparingCellForEdit;

        }

        /// <summary>
        /// Fires before show prompt in the cell
        /// It's important to store original value of the cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdData_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            //store cell value before editing
            var cellInfo = grdData.SelectedCells[grdData.SelectedCells.Count - 1];
            DataRowView drView = (DataRowView) e.Row.Item;
            DataRow drData = drView.Row;
            object[] myValues = drData.ItemArray;
            oldValue = (int) myValues[e.Column.DisplayIndex];

        }

        /// <summary>
        /// after the user press enter button to confirm data into the cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //validate cell value 
            lblErr.Visibility = Visibility.Hidden;
            var cellInfo = grdData.SelectedCells[grdData.SelectedCells.Count - 1];
           try
            {  
                TextBox valNew = (TextBox)cellInfo.Column.GetCellContent(cellInfo.Item);
                //validation of entered data
                if (int.Parse(valNew.Text) < 1 || int.Parse(valNew.Text) > 9)
                {
                    lblErr.Visibility = Visibility.Visible;
                    lblErr.Content = "Available numbers: 1 - 9";
                    e.Cancel = true;
                    return;
                }
                /// challenge request -> copy entered data into all selected cells
                IList<DataGridCellInfo> dataCellRange = grdData.SelectedCells;
                foreach (DataGridCellInfo curCellInfo in dataCellRange)
                {
                    if (curCellInfo.Column.GetCellContent(curCellInfo.Item).GetType() == typeof(TextBlock))
                    {
                        TextBlock cellToUpdate = (TextBlock)curCellInfo.Column.GetCellContent(curCellInfo.Item);
                        cellToUpdate.Text = valNew.Text;
                    }
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// challenge request -> manage copy/paste data into selected cell 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                Clipboard.SetData(DataFormats.CommaSeparatedValue, grdData.SelectedCells);

            }

            if (e.Key == Key.V && ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                HRC_Service.MatrixHRC.PasteDataIntoGrid(grdData);

            }
        }

        /// <summary>
        /// challenge request -> at runtime set grid size by combobox selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbGridsize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            switch (cmbGridsize.SelectedIndex)
            {
                case 0:
                    grdData.ItemsSource= HRC_Service.MatrixHRC.GetMatrix(4);
                    break;
                
                case 1:
                    grdData.ItemsSource = HRC_Service.MatrixHRC.GetMatrix(5);
                    break;
                
                case 2:
                    grdData.ItemsSource = HRC_Service.MatrixHRC.GetMatrix(36);
                    break;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// handler button determinant calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcDeterminant_Click(object sender, RoutedEventArgs e)
        {
            WCFServiceReference.WCFMatrixClient client = new WCFServiceReference.WCFMatrixClient();
            DataView curView = (DataView ) grdData.ItemsSource;
            List<int[]> curMatrixData = HRC_Service.MatrixHRC.FromDStoList(curView.ToTable());
            lblRes1.Content = client.CalcDeterminant(curMatrixData.ToArray());
        }

        /// <summary>
        /// handler button filter and order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            WCFServiceReference.WCFMatrixClient client = new WCFServiceReference.WCFMatrixClient();
            DataView curView = (DataView)grdData.ItemsSource;
            List<int[]> curMatrixData = HRC_Service.MatrixHRC.FromDStoList(curView.ToTable());
            lblRes2.Content = client.FilterAndOrderValues(curMatrixData.ToArray());
        }
    }
}

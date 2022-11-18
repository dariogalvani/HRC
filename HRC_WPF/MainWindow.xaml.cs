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

namespace HRC_WPF
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            initEnv();
        }

        private void initEnv()
        {
            cmbGridsize.Items.Clear();
            cmbGridsize.Items.Add("4x4");
            cmbGridsize.Items.Add("5x5");
            cmbGridsize.Items.Add("36x36");
            cmbGridsize.SelectionChanged += CmbGridsize_SelectionChanged;
            cmbGridsize.SelectedIndex = 0;
            grdData.IsReadOnly = false;
            grdData.CanUserAddRows = false;
            grdData.CanUserDeleteRows = false;
            grdData.CanUserResizeColumns = false;

        }

        private void CmbGridsize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbGridsize.SelectedIndex)
            {
                case 0:
                    grdData.ItemsSource = HRC_Service.MatrixHRC.GetMatrix(4);
                    break;

                case 1:
                    grdData.ItemsSource = HRC_Service.MatrixHRC.GetMatrix(5);
                    break;

                case 2:
                    grdData.ItemsSource = HRC_Service.MatrixHRC.GetMatrix(36);
                    break;
            }
        }
        private void btnCalcDeterminant_Click(object sender, RoutedEventArgs e)
        {
            DataView curView = (DataView)grdData.ItemsSource;
            COM_HRC.COMWrapperWCF wrapper = new COM_HRC.COMWrapperWCF();
            string retVal = wrapper.CalcDeterminant(HRC_Service.MatrixHRC.FromDStoMatrix(curView.ToTable()));
            lblRes1.Content = retVal;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            DataView curView = (DataView)grdData.ItemsSource;
                COM_HRC.COMWrapperWCF wrapper = new COM_HRC.COMWrapperWCF();
                string retVal = wrapper.FilterAndOrderValues(HRC_Service.MatrixHRC.FromDStoMatrix(curView.ToTable()));
            lblRes2.Content = retVal;

        }
    }
}

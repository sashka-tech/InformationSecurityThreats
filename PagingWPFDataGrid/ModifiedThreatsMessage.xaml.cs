using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Lab2
{
    
    public partial class ModifiedThreatsMessage : Window
    {
        ObservableCollection<ModifiedThreat> changesThreats = ((MainWindow)Application.Current.MainWindow).changesThreats;
        public ModifiedThreatsMessage()
        {
            InitializeComponent();
            textBox.Text = "Number of changed threats: " + Convert.ToString(changesThreats.Count);
            dataGrid.ItemsSource = changesThreats;
        }
    }
}

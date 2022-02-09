using System;
using System.Collections.Generic;
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

namespace Trove_JSON_Convert__WPF_
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string return_account { get; set; }  
        public Window1()
        {
          
            InitializeComponent();
            account_textBox.Focus();    
        }

        private void Done_with(object sender, RoutedEventArgs e)
        {
            if (account_textBox.Text == ""){
                return_account = "error";
            }
            else {
                return_account = account_textBox.Text;
            }
            
            Close();
        }
    }
}

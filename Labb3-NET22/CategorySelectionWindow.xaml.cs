using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Labb3_NET22
{
    /// <summary>
    /// Interaction logic for CategorySelectionWindow.xaml
    /// </summary>
    public partial class CategorySelectionWindow : Window
    {   public CategorySelectionViewModel ViewModel { get; set; }
        public List<string> SelectedCategories { get; set; } = new List<string>();
        public CategorySelectionWindow(IEnumerable<string>allCategories, IEnumerable<string>preSelected=null)
        {
            InitializeComponent();
            ViewModel=new CategorySelectionViewModel(allCategories,preSelected);
            DataContext = ViewModel;
        }
        public List<string> GetSelectedCategories()
        {
            return ((CategorySelectionViewModel)DataContext).SelectedCategories;
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = true;
            this.Close();

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

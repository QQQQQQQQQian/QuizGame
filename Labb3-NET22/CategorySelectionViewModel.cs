using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_NET22
{
    public class CategorySelectionViewModel: INotifyPropertyChanged
    {
        
        public ObservableCollection<CategoryItem> Categories { get; set; }
        public List<string> SelectedCategories
        {
            get
            {
                return Categories.Where(c => c.IsSelected).Select(c => c.Name).ToList();
            }
        }
        public class CategoryItem
        {
            public string Name { get; set; }
            public bool IsSelected { get; set; }
            public CategoryItem(string name, bool isSelected = false)
            {
                Name = name;
                IsSelected = isSelected;
            }
        }
        public CategorySelectionViewModel(IEnumerable<string> allCategories, IEnumerable<string> preSelected = null)
        {
            Categories = new ObservableCollection<CategoryItem>();
            foreach (var category in allCategories)
            {
                bool isSelected = preSelected != null && preSelected.Contains(category);
                Categories.Add(new CategoryItem(category, isSelected));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        
       
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }

    
}

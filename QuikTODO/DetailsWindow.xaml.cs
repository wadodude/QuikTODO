using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuikTODO
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        private DetailsViewModel vm = null;

        public DetailsWindow(Task task)
        {
            InitializeComponent();
            this.DataContext = new DetailsViewModel(task);
            vm = (DetailsViewModel)this.DataContext;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            vm.Task.TaskName = vm.OldTaskName;
            Close();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

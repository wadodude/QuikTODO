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
    /// Interaction logic for CalendarWindow.xaml
    /// </summary>
    public partial class CalendarWindow : Window
    {
        private CalendarViewModel vm;
        public CalendarWindow(Task task)
        {
            InitializeComponent();
            this.DataContext = new CalendarViewModel(task);
            vm = (CalendarViewModel)this.DataContext;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CalendarControlPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }
    }

    public class CalendarViewModel
    {
        public CalendarViewModel(Task task) { Task = task; }
        public Task Task { get; set; }
    }
}

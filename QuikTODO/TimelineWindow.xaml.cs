using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TimelineWindow.xaml
    /// </summary>
    public partial class TimelineWindow : Window
    {
        TimelineViewModel vm = null;
        public TimelineWindow(ObservableCollection<Task> tasks)
        {
            InitializeComponent();
            //this.DataContext = new TimelineViewModel(new ObservableCollection<Task>(tasks.Where(i => i.ShowThisTask).ToList()));
            this.DataContext = new TimelineViewModel(tasks);
            vm = (TimelineViewModel)this.DataContext;
        }

        private void Slider_MouseMove_1(object sender, MouseEventArgs e)
        {
            var width = MySlider.ActualWidth;
            System.Windows.Controls.Slider slider = sender as System.Windows.Controls.Slider;
            if (slider != null)
            {
                //var point = slider.TranslatePoint(Mouse.GetPosition(Application.Current.MainWindow), slider);
                //var prompt = new PromptWindow("actual width", point.ToString(), PromptType.Ok);
                //prompt.ShowDialog();
                //prompt = null;
                vm.SliderValue = (int)slider.Value;
            }
        }
    }
}

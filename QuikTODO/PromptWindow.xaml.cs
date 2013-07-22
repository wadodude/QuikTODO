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
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        public PromptViewModel Prompt = null;
        public PromptResponse Response = PromptResponse.NotSpecified;
        public PromptWindow(string title, string message, PromptType promptType)
        {
            InitializeComponent();
            this.DataContext = new PromptViewModel(title, message, promptType);
            Prompt = (PromptViewModel)this.DataContext;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Response = PromptResponse.Ok;
            Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Response = PromptResponse.Cancel;
            Close();
        }

        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            Response = PromptResponse.No;
            Close();
        }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            Response = PromptResponse.Yes;
            Close();
        }

        private void SnoozeButtonClick(object sender, RoutedEventArgs e)
        {
            Response = PromptResponse.Snooze;
            Close();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

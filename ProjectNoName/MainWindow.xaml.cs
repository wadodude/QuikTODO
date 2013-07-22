using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace QuikTODO
{
    public partial class MainWindow : Window
    {
        private TimelineWindow timeline = null;
        private DetailsWindow details = null;
        private TaskViewModel vm = null;
        private NotifyIcon ni = null;
        private CalendarWindow calendar = null;

        public MainWindow()
        {
            InitializeComponent();
            //this.Loaded += MainWindow_Loaded;
            this.DataContext = new TaskViewModel();
            vm = (TaskViewModel)this.DataContext;
            ni = new System.Windows.Forms.NotifyIcon();
            Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/QuikTODO;component/Icons/clipboard.ico")).Stream;
            ni.Icon = new System.Drawing.Icon(iconStream);
            ni.Visible = true;
            ni.DoubleClick += ni_DoubleClick;
            ni.MouseDown += ni_MouseDown;
        }

        //void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Left = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - Width;
        //    Top = 0;
        //}

        #region Focus on Mouse events

        private void TaskTextBoxPreviewMouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var textbox = sender as System.Windows.Controls.TextBox;
            if (textbox != null)
            {
                textbox.Focus();
            }
        }

        #endregion

        void ni_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState != WindowState.Normal)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        void ni_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                System.Windows.Controls.ContextMenu context = (System.Windows.Controls.ContextMenu)this.FindResource("NotifierContextMenu");
                context.IsOpen = true;
                System.Timers.Timer timer = new System.Timers.Timer(3000);
                timer.Elapsed += timer_Elapsed;
                timer.AutoReset = false;
                timer.Enabled = true;
                timer.Start();
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                new Action(delegate()
            {
                System.Windows.Controls.ContextMenu menu = (System.Windows.Controls.ContextMenu)this.FindResource("NotifierContextMenu");
                menu.IsOpen = false;
            }));
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                this.Hide();
            }
        }

        #region Keyboard events

        private void OnKeyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            //{
            //    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            //    new Action(delegate()
            //    {
            //        System.Windows.Controls.ContextMenu menu = (System.Windows.Controls.ContextMenu)this.FindResource("FileContextMenu");
            //        menu.IsOpen = !menu.IsOpen;
            //    }));
            //}
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                vm.AddExecute();
            }
            else if (Keyboard.IsKeyDown(Key.Escape))
            {
                vm.TaskToAdd = null;
            }
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.S))
            {
                DefaultSaveMethod();
                e.Handled = true;
            }
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.O))
            {
                OpenFileMethod();
                e.Handled = true;

            }
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.P))
            {
                PrintMethod();
                e.Handled = true;
            }
        }

        private void OnDateKeyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var datePicker = e.Source as DatePicker;
            if (datePicker != null)
            {
                string text = datePicker.Text;
                DateTime date;
                if (!DateTime.TryParse(text, out date))
                {
                    e.Handled = true;
                    return;
                }
                if (e.Key == Key.A || e.Key == Key.B || e.Key == Key.C || e.Key == Key.D ||
                    e.Key == Key.E || e.Key == Key.F || e.Key == Key.G || e.Key == Key.H ||
                    e.Key == Key.I || e.Key == Key.J || e.Key == Key.K || e.Key == Key.L || e.Key == Key.M ||
                    e.Key == Key.N || e.Key == Key.O || e.Key == Key.P || e.Key == Key.Q || e.Key == Key.R ||
                    e.Key == Key.S || e.Key == Key.T || e.Key == Key.U || e.Key == Key.V || e.Key == Key.W ||
                    e.Key == Key.X || e.Key == Key.Y || e.Key == Key.Z)
                {
                    e.Handled = true;
                }
                else if (Keyboard.IsKeyDown(Key.Enter))
                {
                    vm.AddExecute();
                }
                else if (Keyboard.IsKeyDown(Key.Escape))
                {
                    vm.TaskToAdd = null;
                }
                else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.S))
                {
                    DefaultSaveMethod();
                }
                else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.O))
                {
                    OpenFileMethod();
                }
                else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.P))
                {
                    PrintMethod();
                }
                else if (e.Key == Key.D1 || e.Key == Key.NumPad1 || e.Key == Key.D2 || e.Key == Key.NumPad2 ||
                        e.Key == Key.D3 || e.Key == Key.NumPad3 || e.Key == Key.D4 || e.Key == Key.NumPad4 ||
                        e.Key == Key.D5 || e.Key == Key.NumPad5 || e.Key == Key.D6 || e.Key == Key.NumPad6 ||
                        e.Key == Key.D7 || e.Key == Key.NumPad7 || e.Key == Key.D8 || e.Key == Key.NumPad8 ||
                        e.Key == Key.D9 || e.Key == Key.NumPad9 || e.Key == Key.D0 || e.Key == Key.NumPad0)
                {
                    string i = string.Empty;
                    if (e.Key == Key.D0 || e.Key == Key.NumPad0)
                        i = "0";
                    if (e.Key == Key.D1 || e.Key == Key.NumPad1)
                        i = "1";
                    if (e.Key == Key.D2 || e.Key == Key.NumPad2)
                        i = "2";
                    if (e.Key == Key.D3 || e.Key == Key.NumPad3)
                        i = "3";
                    if (e.Key == Key.D4 || e.Key == Key.NumPad4)
                        i = "4";
                    if (e.Key == Key.D5 || e.Key == Key.NumPad5)
                        i = "5";
                    if (e.Key == Key.D6 || e.Key == Key.NumPad6)
                        i = "6";
                    if (e.Key == Key.D7 || e.Key == Key.NumPad7)
                        i = "7";
                    if (e.Key == Key.D8 || e.Key == Key.NumPad8)
                        i = "8";
                    if (e.Key == Key.D9 || e.Key == Key.NumPad9)
                        i = "9";
                }
            }
        }

        private void EditTaskKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter && vm.SelectedTask != null)
            {
                vm.SelectedTask.IsEditable = false;
                vm.RefreshTasks();
            }
            else if (e.Key == Key.Escape)
            {
                MakeNotEditable();
            }
        }

        private void EditDateTaskKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.B || e.Key == Key.C || e.Key == Key.D ||
                e.Key == Key.E || e.Key == Key.F || e.Key == Key.G || e.Key == Key.H ||
                e.Key == Key.I || e.Key == Key.J || e.Key == Key.K || e.Key == Key.L || e.Key == Key.M ||
                e.Key == Key.N || e.Key == Key.O || e.Key == Key.P || e.Key == Key.Q || e.Key == Key.R ||
                e.Key == Key.S || e.Key == Key.T || e.Key == Key.U || e.Key == Key.V || e.Key == Key.W ||
                e.Key == Key.X || e.Key == Key.Y || e.Key == Key.Z)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Enter && vm.SelectedTask != null)
            {
                vm.SelectedTask.IsEditable = false;
                vm.RefreshTasks();
            }
            else if (e.Key == Key.Escape)
            {
                MakeNotEditable();
            }
        }

        private void EditTaskPreviewLostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            MakeNotEditable();
        }

        private void OnListBoxItemGotFocus(object sender, RoutedEventArgs e)
        {
            DependencyObject o = sender as DependencyObject;
            while (o != null)
            {
                var item = o as ListBoxItem;
                if (item == null)
                {
                    o = VisualTreeHelper.GetParent(o);
                }
                else
                {
                    item.IsSelected = true;
                    break;
                }
            }
        }

        private void HourTextKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                vm.SelectedTask.ShowReminder = false;
                return;
            }
            var textBox = e.Source as System.Windows.Controls.TextBox;
            if (textBox != null)
            {
                if (e.Key == Key.P)
                {
                    textBox.Text = textBox.Text.Substring(0, 5) + "PM";
                    textBox.Select(5, 2);
                    e.Handled = true;
                }
                else if (e.Key == Key.A)
                {
                    textBox.Text = textBox.Text.Substring(0, 5) + "AM";
                    textBox.Select(5, 2);
                    e.Handled = true;
                }
                else if (e.Key == Key.D1 || e.Key == Key.NumPad1 || e.Key == Key.D2 || e.Key == Key.NumPad2 ||
                    e.Key == Key.D3 || e.Key == Key.NumPad3 || e.Key == Key.D4 || e.Key == Key.NumPad4 ||
                    e.Key == Key.D5 || e.Key == Key.NumPad5 || e.Key == Key.D6 || e.Key == Key.NumPad6 ||
                    e.Key == Key.D7 || e.Key == Key.NumPad7 || e.Key == Key.D8 || e.Key == Key.NumPad8 ||
                    e.Key == Key.D9 || e.Key == Key.NumPad9 || e.Key == Key.D0 || e.Key == Key.NumPad0)
                {
                    string i = string.Empty;
                    if (e.Key == Key.D0 || e.Key == Key.NumPad0)
                        i = "0";
                    if (e.Key == Key.D1 || e.Key == Key.NumPad1)
                        i = "1";
                    if (e.Key == Key.D2 || e.Key == Key.NumPad2)
                        i = "2";
                    if (e.Key == Key.D3 || e.Key == Key.NumPad3)
                        i = "3";
                    if (e.Key == Key.D4 || e.Key == Key.NumPad4)
                        i = "4";
                    if (e.Key == Key.D5 || e.Key == Key.NumPad5)
                        i = "5";
                    if (e.Key == Key.D6 || e.Key == Key.NumPad6)
                        i = "6";
                    if (e.Key == Key.D7 || e.Key == Key.NumPad7)
                        i = "7";
                    if (e.Key == Key.D8 || e.Key == Key.NumPad8)
                        i = "8";
                    if (e.Key == Key.D9 || e.Key == Key.NumPad9)
                        i = "9";

                    if (textBox.CaretIndex == 0)
                    {
                        if (e.Key == Key.D0 || e.Key == Key.NumPad0)
                        {
                            textBox.Text = "0" + textBox.Text.Substring(1, 4) + textBox.Text.Substring(5, 2);
                            textBox.CaretIndex = 1;
                            e.Handled = true;
                        }
                        else if (e.Key == Key.D1 || e.Key == Key.NumPad1)
                        {
                            textBox.Text = "1" + textBox.Text.Substring(1, 4) + textBox.Text.Substring(5, 2);
                            textBox.CaretIndex = 1;
                            e.Handled = true;
                        }
                        else if (textBox.SelectionLength >= 5)
                        {
                            textBox.Text = "0" + i + textBox.Text.Substring(2, 3) + textBox.Text.Substring(5, 2);
                            textBox.CaretIndex = 3;
                            e.Handled = true;
                        }
                        else { e.Handled = true; }
                    }
                    else if (textBox.CaretIndex == 1)
                    {
                        if (textBox.Text.Substring(0, 1) == "0")
                        {
                            if (i != "0")
                            {
                                textBox.Text = textBox.Text.Substring(0, 1) + i + textBox.Text.Substring(2, 3) + textBox.Text.Substring(5, 2);
                                textBox.CaretIndex = 3;
                            }
                            e.Handled = true;
                        }
                        else if (textBox.Text.Substring(0, 1) == "1")
                        {
                            if (e.Key == Key.D0 || e.Key == Key.NumPad0 ||
                                e.Key == Key.D1 || e.Key == Key.NumPad1 ||
                                e.Key == Key.D2 || e.Key == Key.NumPad2)
                            {
                                textBox.Text = textBox.Text.Substring(0, 1) + i + textBox.Text.Substring(2, 3) + textBox.Text.Substring(5, 2);
                                textBox.CaretIndex = 3;
                                e.Handled = true;
                            }
                            else { e.Handled = true; }
                        }
                    }
                    else if (textBox.CaretIndex == 3)
                    {
                        if (e.Key == Key.D1 || e.Key == Key.NumPad1 || e.Key == Key.D2 || e.Key == Key.NumPad2 ||
                            e.Key == Key.D3 || e.Key == Key.NumPad3 || e.Key == Key.D4 || e.Key == Key.NumPad4 ||
                            e.Key == Key.D5 || e.Key == Key.NumPad5 || e.Key == Key.D0 || e.Key == Key.NumPad0)
                        {
                            textBox.Text = textBox.Text.Substring(0, 3) + i + textBox.Text.Substring(4, 1) + textBox.Text.Substring(5, 2);
                            textBox.CaretIndex = 4;
                            e.Handled = true;
                        }
                        else { e.Handled = true; }
                    }
                    else if (textBox.CaretIndex == 4)
                    {
                        textBox.Text = textBox.Text.Substring(0, 4) + i + textBox.Text.Substring(5, 2);
                        textBox.CaretIndex = 5;
                    }
                }
                else if (e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Tab) { }
                else if (e.Key == Key.Back)
                {
                    if (textBox.CaretIndex == 5)
                    {
                        textBox.Text = textBox.Text.Substring(0, 4) + "0" + textBox.Text.Substring(5, 2);
                        textBox.CaretIndex = 4;
                        e.Handled = true;
                    }
                    else if (textBox.CaretIndex == 4)
                    {
                        textBox.Text = textBox.Text.Substring(0, 3) + "0" + textBox.Text.Substring(4, 1) + textBox.Text.Substring(5, 2);
                        textBox.CaretIndex = 3;
                        e.Handled = true;
                    }
                    else if (textBox.CaretIndex == 3)
                    {
                        textBox.CaretIndex = 2;
                        e.Handled = true;
                    }
                    else if (textBox.CaretIndex == 2)
                    {
                        textBox.Text = textBox.Text.Substring(0, 1) + "0" + textBox.Text.Substring(2, 3) + textBox.Text.Substring(5, 2);
                        textBox.CaretIndex = 1;
                        e.Handled = true;
                    }
                    else if (textBox.CaretIndex == 1)
                    {
                        textBox.Text = "0" + textBox.Text.Substring(1, 4) + textBox.Text.Substring(5, 2);
                        textBox.CaretIndex = 0;
                        e.Handled = true;
                    }
                    else if (textBox.SelectionLength == textBox.Text.Length)
                    {
                        textBox.Text = "12:00AM";
                        textBox.CaretIndex = 0;
                        e.Handled = true;
                    }
                    else { e.Handled = true; }
                }
                else if (e.Key == Key.Delete)
                {
                    if (textBox.CaretIndex == 4)
                    {
                        textBox.Text = textBox.Text.Substring(0, 4) + "0" + textBox.Text.Substring(5, 2);
                        textBox.CaretIndex = 5;
                        e.Handled = true;
                    }
                    else if (textBox.CaretIndex == 3)
                    {
                        textBox.Text = textBox.Text.Substring(0, 3) + "0" + textBox.Text.Substring(4, 1) + textBox.Text.Substring(5, 2);
                        textBox.CaretIndex = 4;
                        e.Handled = true;
                    }
                    else if (textBox.CaretIndex == 2)
                    {
                        textBox.CaretIndex = 3;
                        e.Handled = true;
                    }
                    else if (textBox.CaretIndex == 1)
                    {
                        textBox.Text = textBox.Text.Substring(0, 1) + "0" + textBox.Text.Substring(2, 3) + textBox.Text.Substring(5, 2);
                        textBox.CaretIndex = 2;
                        e.Handled = true;
                    }
                    else if (textBox.CaretIndex == 0)
                    {
                        textBox.Text = "0" + textBox.Text.Substring(1, 4) + textBox.Text.Substring(5, 2);
                        textBox.CaretIndex = 1;
                        e.Handled = true;
                    }
                    else { e.Handled = true; }
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void ReminderKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            vm.SelectedTask.ShowReminder = false;
            vm.RefreshTasks();
        }

        #endregion

        #region Functional Methods

        private void SaveToPath(string savePath)
        {
            List<string> output = new List<string>();
            string settings = "**s" + (vm.IsSortByHighestChecked ? "1" : "0") + "h" + (vm.IsHideCompletedChecked ? "1" : "0") + "d" + (vm.IsEnableDrag ? "1" : "0");
            if (vm.IsAutoCompletedOff) { settings += "c0"; }
            else if (vm.IsCriticalCompleted) { settings += "c1"; }
            else if (vm.IsHighCompleted) { settings += "c2"; }
            else if (vm.IsMediumCompleted) { settings += "c3"; }
            else if (vm.IsLowCompleted) { settings += "c4"; }
            settings += "**";
            output.Add(settings);

            if (vm.AllTasks.Any(i => i.IsDone))
            {
                output.Add("\r\n*Completed:\r\n");
                foreach (var s in vm.AllTasks.Where(i => i.IsDone))
                {
                    string time = "       ";
                    if (s.HasReminderTime)
                    {
                        time = s.ReminderTime;
                    }
                    output.Add(time + " " + s.TaskDate.ToString("MM/dd/yyyy") + "    " + (int)s.Priority + "    " + s.TaskName.Replace(Environment.NewLine, @" \n "));
                }
            }
            if (vm.AllTasks.Any(i => !i.IsDone))
            {
                output.Add("\r\n**Incomplete:\r\n");
                foreach (var s in vm.AllTasks.Where(i => !i.IsDone))
                {
                    string time = "       ";
                    if (s.HasReminderTime)
                    {
                        time = s.ReminderTime;
                    }
                    output.Add(time + " " + s.TaskDate.ToString("MM/dd/yyyy") + "    " + (int)s.Priority + "    " + s.TaskName.Replace(Environment.NewLine, @" \n "));
                }
            }
            File.WriteAllLines(savePath, output.ToArray());
            vm.NeedsToSave = false;
        }

        private bool DefaultSaveMethod()
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(vm.FilePath))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                //dialog.FileName = Environment.CurrentDirectory;
                dialog.DefaultExt = ".txt";
                dialog.Filter = "Text Documents (.txt)|*.txt";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    vm.FilePath = dialog.FileName;
                    vm.MainDisplayName = System.IO.Path.GetFileName(vm.FilePath);
                    result = true;
                    SaveToPath(vm.FilePath);
                }
            }
            else
            {
                result = true;
                SaveToPath(vm.FilePath);
            }
            return result;
        }

        private void PrintMethod()
        {
            if (string.IsNullOrWhiteSpace(vm.FilePath))
            {
                if (vm.PromptWindow == null)
                {
                    vm.PromptWindow = new PromptWindow("Save before print", "You must save before printing.", PromptType.Ok);
                    vm.PromptWindow.ShowDialog();
                    vm.PromptWindow = null;
                }
                //System.Windows.MessageBox.Show("You must save before printing.", "Save before print",
                //    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            var result = DefaultSaveMethod();
            if (result)
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.Verb = "print";
                info.FileName = result ? vm.FilePath : vm.DefaultFilePath;
                info.WindowStyle = ProcessWindowStyle.Hidden;

                Process proc = new Process();
                proc.StartInfo = info;
                proc.Start();
            }
        }

        private void OpenFileMethod()
        {
            if (UnsavedChangesPrompt())
            {
                OpenFileDialog dialog = new OpenFileDialog();
                //dialog.FileName = Environment.CurrentDirectory;
                dialog.DefaultExt = ".txt";
                dialog.Filter = "Text Documents (.txt)|*.txt";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    vm.FilePath = dialog.FileName;
                    vm.FileName = dialog.SafeFileNames.First();
                    if (File.Exists(vm.FilePath))
                    {
                        var fileContents = File.ReadAllLines(vm.FilePath);
                        bool isCompleted = true;
                        bool isValidFile = true;
                        List<Task> taskCollection = new List<Task>();
                        foreach (var s in fileContents)
                        {
                            if (!string.IsNullOrWhiteSpace(s))
                            {
                                if (s.StartsWith("**") && s.EndsWith("**") && s.Length == 12)
                                {
                                    vm.IsSortByHighestChecked = s[3] == '1' ? true : false;
                                    vm.IsHideCompletedChecked = s[5] == '1' ? true : false;
                                    vm.IsEnableDrag = s[7] == '1' ? true : false;
                                    switch (s[9])
                                    {
                                        case '0':
                                            vm.IsAutoCompletedOff = true;
                                            break;
                                        case '1':
                                            vm.IsCriticalCompleted = true;
                                            break;
                                        case '2':
                                            vm.IsHighCompleted = true;
                                            break;
                                        case '3':
                                            vm.IsMediumCompleted = true;
                                            break;
                                        case '4':
                                            vm.IsLowCompleted = true;
                                            break;
                                    }
                                    continue;
                                }

                                if (s.Contains("**Incomplete"))
                                {
                                    isCompleted = false;
                                    continue;
                                }
                                else if (s.Contains("*Completed"))
                                {
                                    isCompleted = true;
                                    continue;
                                }

                                string dateStr = null;
                                if (s.Length > 28)
                                {
                                    dateStr = s.Substring(8, 10);
                                }
                                else
                                {
                                    continue;
                                }
                                DateTime d = new DateTime(1900, 9, 9);
                                if (!DateTime.TryParse(dateStr, out d))
                                {
                                    isValidFile = false;
                                    break;
                                }
                                int priority = -1;

                                if (!int.TryParse(s.Substring(22, 1), out priority))
                                {
                                    isValidFile = false;
                                    break;
                                }
                                var name = s.Substring(27, s.Length - 27).Replace(@" \n ", Environment.NewLine);
                                Task t = new Task(vm);

                                if (s.Substring(0, 7).Contains("AM") || s.Substring(0, 7).Contains("PM"))
                                {
                                    t.HasReminderTime = true;
                                    t.ReminderTime = s.Substring(0, 7);
                                }

                                if (priority == (int)PriorityType.Critical)
                                    t.PriorityColor = System.Windows.Media.Brushes.Red;
                                else if (priority == (int)PriorityType.High)
                                    t.PriorityColor = System.Windows.Media.Brushes.Orange;
                                else if (priority == (int)PriorityType.Medium)
                                    t.PriorityColor = System.Windows.Media.Brushes.Yellow;
                                else if (priority == (int)PriorityType.Low)
                                    t.PriorityColor = System.Windows.Media.Brushes.LimeGreen;
                                t.TaskName = name;
                                t.TaskDate = d;
                                t.IsDone = isCompleted;
                                taskCollection.Add(t);
                            }
                        }

                        string resultMsg = "File '" + vm.FileName + "' opened successfully.";
                        if (isValidFile)
                        {
                            if (taskCollection.Count == 0)
                                resultMsg = "No tasks were found.";
                            else
                            {
                                vm.TaskCollection.Clear();
                                vm.AllTasks.Clear();
                                vm.AllTasks = taskCollection;
                                taskCollection.ForEach(i => vm.TaskCollection.Add(i));
                            }
                            vm.RefreshTasks();
                            vm.RefreshRibbon();
                            vm.MainDisplayName = vm.FileName;
                            vm.NeedsToSave = false;
                        }
                        else
                        {
                            resultMsg = "Invalid File Format.";
                        }
                        if (vm.PromptWindow == null)
                        {
                            vm.PromptWindow = new PromptWindow("Open File", resultMsg, PromptType.Ok);
                            vm.PromptWindow.ShowDialog();
                            vm.PromptWindow = null;
                        }
                        //System.Windows.MessageBox.Show(resultMsg, "Open File", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private bool UnsavedChangesPrompt()
        {
            bool continueWithOpen = true;
            if (vm.NeedsToSave)
            {
                if (vm.PromptWindow == null)
                {
                    vm.PromptWindow = new PromptWindow("Unsaved Changes", "You will lose unsaved changes. Save now?", PromptType.YesNoCancel);
                    if (vm.PromptWindow.ShowDialog().HasValue)
                    {
                        if (vm.PromptWindow.Response == PromptResponse.Yes)
                        {
                            continueWithOpen = DefaultSaveMethod();
                        }
                        else if (vm.PromptWindow.Response == PromptResponse.No)
                        {
                            vm.AllTasks.Clear();
                            vm.TaskCollection.Clear();
                        }
                        else
                        {
                            continueWithOpen = false;
                        }
                    }
                    vm.PromptWindow = null;
                }
                //var result = System.Windows.MessageBox.Show("You will lose unsaved changes. Save now?", "Unsaved Changes",
                //    MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation,
                //    MessageBoxResult.Yes, System.Windows.MessageBoxOptions.DefaultDesktopOnly);

                //if (result == MessageBoxResult.Yes)
                //    continueWithOpen = DefaultSaveMethod();
                //else if (result == MessageBoxResult.No)
                //    vm.TaskCollection.Clear();
                //else if (result == MessageBoxResult.Cancel)
                //    continueWithOpen = false;
            }

            return continueWithOpen;
        }

        private void MakeNotEditable()
        {
            foreach (var t in vm.TaskCollection)
            {
                t.IsEditable = false;
                t.ShowReminder = false;
            }
            vm.RefreshTasks();
        }

        private void CloseApp()
        {
            ni.Visible = false;
            Close();
        }

        #endregion

        #region Click events

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void UnselectSelectedTask(object sender, MouseButtonEventArgs e)
        {
            vm.SelectedTask = null;
        }

        private void TaskMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (vm.SelectedTask != null)
            {
                vm.SelectedTask.IsEditable = true;
                vm.RefreshTasks();
                vm.NeedsToSave = true;
            }
        }

        private void ListBoxLabelMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (vm.SelectedTask != null)
            {
                vm.SelectedTask.ChangePriority();
                vm.RefreshTasks();
                vm.NeedsToSave = true;
            }
        }

        private void PriorityLabelMouseClick(object sender, MouseButtonEventArgs e)
        {
            vm.ChangePriorityExecute();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (vm.NeedsToSave)
            {
                if (vm.PromptWindow == null)
                {
                    vm.PromptWindow = new PromptWindow("Exiting...", "Do you want to save your changes now?", PromptType.YesNoCancel);
                    if (vm.PromptWindow.ShowDialog().HasValue)
                    {
                        if (vm.PromptWindow.Response == PromptResponse.Yes)
                        {
                            DefaultSaveMethod();
                            CloseApp();
                        }
                        else if (vm.PromptWindow.Response == PromptResponse.No)
                        {
                            CloseApp();
                        }
                    }

                    vm.PromptWindow = null;

                    //var result = System.Windows.MessageBox.Show("Do you want to save your changes now?", "Exiting...", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation,
                    //    MessageBoxResult.Yes, System.Windows.MessageBoxOptions.ServiceNotification);
                    //if (result == MessageBoxResult.Yes)
                    //{
                    //    DefaultSaveMethod();
                    //    CloseApp();
                    //}
                    //else if (result == MessageBoxResult.No)
                    //{
                    //    CloseApp();
                    //}
                }
            }
            else { CloseApp(); }
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void RemoveSelectedTaskClick(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedTask != null)
            {
                vm.AllTasks.Remove(vm.SelectedTask);
                vm.TaskCollection.Remove(vm.SelectedTask);
                vm.NeedsToSave = true;
            }
        }

        private void ClearAllClick(object sender, RoutedEventArgs e)
        {
            if (vm.TaskCollection.Count > 0)
            {
                if (vm.PromptWindow == null)
                {
                    vm.PromptWindow = new PromptWindow("Clearing All Tasks...", "Are you sure you want to clear all tasks?", PromptType.YesNo);
                    if (vm.PromptWindow.ShowDialog().HasValue)
                    {
                        if (vm.PromptWindow.Response == PromptResponse.Yes)
                        {
                            //TODO: clear all tasks not shown?
                            vm.TaskCollection.Clear();
                            if (!string.IsNullOrWhiteSpace(vm.FilePath))
                                vm.NeedsToSave = true;
                            else
                                vm.NeedsToSave = false;
                        }
                        vm.PromptWindow = null;
                    }
                }
                //var result = System.Windows.MessageBox.Show("Are you sure you want to clear all tasks?", "Clearing All Tasks...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation,
                //MessageBoxResult.Yes, System.Windows.MessageBoxOptions.DefaultDesktopOnly);
                //if (result == MessageBoxResult.Yes)
                //{
                //    vm.TaskCollection.Clear();
                //    if (!string.IsNullOrWhiteSpace(vm.FilePath))
                //        vm.NeedsToSave = true;
                //    else
                //        vm.NeedsToSave = false;
                //}
            }
            else
            {
                //TODO: clear all tasks not shown?
                vm.TaskCollection.Clear();
            }
        }

        private void PrintClick(object sender, RoutedEventArgs e)
        {
            PrintMethod();
        }

        private void HelpClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.jerrymathew.com/contact-me.html");
        }

        private void ShowClick(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.WindowState = System.Windows.WindowState.Normal;
        }

        #region Deprecated code

        private void FileClick(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(delegate()
            {
                System.Windows.Controls.ContextMenu menu = (System.Windows.Controls.ContextMenu)this.FindResource("FileContextMenu");
                menu.IsOpen = true;
            }));
        }

        private void EditClick(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(delegate()
            {
                System.Windows.Controls.ContextMenu menu = (System.Windows.Controls.ContextMenu)this.FindResource("EditContextMenu");
                menu.IsOpen = true;
            }));
        }

        private void SortByHighestClick(object sender, RoutedEventArgs e)
        {
            vm.IsSortByHighestChecked = !vm.IsSortByHighestChecked;
            vm.RefreshTasks();
        }

        #endregion

        private void NewClick(object sender, RoutedEventArgs e)
        {
            StartNewDataContext();
        }

        private void StartNewDataContext()
        {
            if (UnsavedChangesPrompt())
            {
                vm = new TaskViewModel();
                DataContext = vm;
                TaskNameTextBox.Focus();
            }
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileMethod();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            DefaultSaveMethod();
        }

        private void SaveAsClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text Documents (.txt)|*.txt";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vm.FilePath = dialog.FileName;
                vm.MainDisplayName = System.IO.Path.GetFileName(vm.FilePath);
                SaveToPath(vm.FilePath);
            }
        }

        private void HideCompletedClick(object sender, RoutedEventArgs e)
        {
            vm.IsHideCompletedChecked = !vm.IsHideCompletedChecked;
            vm.RefreshTasks();
        }

        private void ViewDetailsClick(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedTask != null)
            {
                if (details == null)
                {
                    details = new DetailsWindow(vm.SelectedTask);
                    if (details.ShowDialog().HasValue)
                    {
                        details = null;
                        vm.RefreshTasks();
                    }
                }
                //System.Windows.MessageBox.Show("Priority: " + vm.SelectedTask.Priority.ToString().ToUpper() + "\r\n" +
                //    "Due Date: " + vm.SelectedTask.TaskDate.ToShortDateString() + "\r\n" +
                //    "Status: " + (vm.SelectedTask.IsDone ? "Completed" : "Incomplete") + "\r\n" +
                //    (vm.SelectedTask.HasReminderTime ? "Reminder Time: " + vm.SelectedTask.ReminderTime + "\r\n" : string.Empty) +
                //    string.Empty.PadRight(85, '*') + "\nTODO:\n" +
                //    vm.SelectedTask.TaskName, "TODO: " + vm.SelectedTask.TaskName, MessageBoxButton.OK);
            }
        }

        private void AddReminderClick(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedTask != null)
            {
                vm.SelectedTask.ShowReminder = !vm.SelectedTask.ShowReminder;
                vm.RefreshTasks();
            }
        }

        private void SaveReminderClick(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedTask != null)
            {
                string time = vm.SelectedTask.ReminderTime;
                if (!string.IsNullOrWhiteSpace(time))
                {
                    int hourFormat = -1;
                    if (time.Length >= 2 && int.TryParse(time.Substring(0, 2), out hourFormat))
                    {
                        if (hourFormat > 12)
                            vm.SelectedTask.ReminderTime = (hourFormat - 12).ToString() + time.Substring(2, 3) + "PM";
                        else if (hourFormat == 0)
                            vm.SelectedTask.ReminderTime = "12" + time.Substring(2, 3) + "AM";
                    }
                }
                vm.SelectedTask.ShowReminder = false;
                vm.SelectedTask.HasReminderTime = true;
                vm.RefreshTasks();
            }
            e.Handled = true;
        }

        private void CancelReminderClick(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedTask != null)
            {
                vm.SelectedTask.ShowReminder = false;
                vm.SelectedTask.HasReminderTime = false;
                vm.SelectedTask.ReminderTime = null;
                vm.RefreshTasks();
            }
        }

        private void HourDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var textBox = e.Source as System.Windows.Controls.TextBox;
            if (textBox != null)
            {
                if (textBox.CaretIndex > 4)
                    textBox.Select(5, 2);
                else
                    textBox.Select(0, 5);

                e.Handled = true;
            }
        }

        #endregion

        private void TaskNameTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            TaskNameTextBox.Focus();
        }

        private void StartTimelineClick(object sender, MouseButtonEventArgs e)
        {
            timeline = new TimelineWindow(vm.TaskCollection);
            timeline.ShowDialog();
            e.Handled = true;
        }

        private void DockRight(object sender, RoutedEventArgs e)
        {
            AppBarFunctions.SetAppBar(this, ABEdge.Right);
        }

        private void DockLeft(object sender, RoutedEventArgs e)
        {
            AppBarFunctions.SetAppBar(this, ABEdge.Left);
        }

        private void DockTop(object sender, RoutedEventArgs e)
        {
            AppBarFunctions.SetAppBar(this, ABEdge.Top);
        }

        private void DockBottom(object sender, RoutedEventArgs e)
        {
            AppBarFunctions.SetAppBar(this, ABEdge.Bottom);
        }

        private void EditTaskDateClick(object sender, RoutedEventArgs e)
        {
            calendar = new CalendarWindow(vm.SelectedTask);
            calendar.ShowDialog();
            calendar = null;
            vm.RefreshTasks();
        }

        private void HourTextBox_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            if (textBox != null)
            {
                if (!textBox.IsFocused)
                {
                    textBox.Focus();
                }
            }
        }
    }
}

#region Comments
//private void SubscribeToItemsSourceChanged()
//{
//    if (ListBox != null)
//    {
//        DependencyPropertyDescriptor dependencyPropertyDescriptor =
//                DependencyPropertyDescriptor.FromProperty(System.Windows.Controls.ListBox.ItemsSourceProperty, typeof(System.Windows.Controls.ListBox));

//        if (dependencyPropertyDescriptor != null)
//        {
//            dependencyPropertyDescriptor.AddValueChanged(ListBox, ItemsSourceChanged);
//        }
//    }
//}

//private void ItemsSourceChanged(object sender, EventArgs e)
//{
//    SubscribeToCollectionChanged();
//}

//public INotifyCollectionChanged _collectionChanged { get; set; }

//private void SubscribeToCollectionChanged()
//{
//    //Unsubscribe from the original collection
//    if (_collectionChanged != null)
//    {
//        _collectionChanged.CollectionChanged -= CollectionChangedHandler;
//    }

//    _collectionChanged = ListBox.ItemsSource as INotifyCollectionChanged;
//    if (_collectionChanged != null)
//    {
//        _collectionChanged.CollectionChanged +=
//            new NotifyCollectionChangedEventHandler(CollectionChangedHandler);
//    }
//}

//private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
//{
//    if (e.Action == NotifyCollectionChangedAction.Add)
//    {
//        ListBox.ScrollIntoView(e.NewItems[0]);
//        ListBox.SelectedItem = e.NewItems[0];
//        if (ListBox.HasItems && ListBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
//        {
//            var item = (ListBoxItem)ListBox.ItemContainerGenerator.ContainerFromItem(ListBox.SelectedItem);
//            item.Loaded += new RoutedEventHandler(ItemLoadedHandler);
//        }
//        else
//        {
//            SubscribeToContainerStatusChanged();
//        }
//    }
//    else if (e.Action == NotifyCollectionChangedAction.Remove)
//    {
//        if (ListBox.HasItems)
//        {
//            var item = (ListBoxItem)ListBox.ItemContainerGenerator.ContainerFromItem(ListBox.Items[ListBox.Items.Count - 1]);
//            ListBox.ScrollIntoView(item);
//            ListBox.SelectedItem = item;
//            SetFocusToElement(item, "LbTextBox");
//        }
//    }
//}

//private void SubscribeToContainerStatusChanged()
//{
//    ListBox.ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);
//}

//private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
//{
//    if (ListBox.HasItems && ListBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
//    {
//        ListBox.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
//        var item = (ListBoxItem)ListBox.ItemContainerGenerator.ContainerFromItem(ListBox.SelectedItem);
//        if (item != null)
//        {
//            item.Loaded += new RoutedEventHandler(ItemLoadedHandler);
//        }
//    }
//}

//private void ItemLoadedHandler(object sender, RoutedEventArgs e)
//{
//    var listBoxItem = sender as ListBoxItem;
//    if (listBoxItem != null)
//    {
//        SetFocusToElement(listBoxItem, "LbTextBox");
//        listBoxItem.Loaded -= ItemLoadedHandler;
//    }
//}

//private bool SetFocusToElement(ListBoxItem listBoxItem, string elementName)
//{
//    bool found = false;
//    var childElement = FindFirstChildByName(listBoxItem, elementName) as FrameworkElement;
//    if (childElement != null)
//    {
//        //This works
//        childElement.Focus();
//        found = true;
//    }
//    return found;
//}

//public DependencyObject FindFirstChildByName(DependencyObject parentReference, string childName)
//{
//    DependencyObject foundChild = null;
//    if (parentReference != null)
//    {
//        int childrenCount = VisualTreeHelper.GetChildrenCount(parentReference);
//        for (int i = 0; i < childrenCount; i++)
//        {
//            var child = VisualTreeHelper.GetChild(parentReference, i);
//            //If this is the matching child, stop
//            var frameworkElement = child as FrameworkElement;
//            if (frameworkElement != null && frameworkElement.Name == childName)
//            {
//                foundChild = child;
//                break;
//            }
//            //If it is not the matching child, iterate through the children's elements
//            else
//            {
//                foundChild = FindFirstChildByName(child, childName);

//                var secondaryChild = foundChild as FrameworkElement;
//                if (secondaryChild != null && secondaryChild.Name == childName)
//                {
//                    break;
//                }
//            }
//        }
//    }

//    return foundChild;
//}

#endregion
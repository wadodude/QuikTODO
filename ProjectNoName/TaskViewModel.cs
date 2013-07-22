using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GongSolutions.Wpf.DragDrop;

namespace QuikTODO
{
    public class TaskViewModel : ViewModelBase, IDropTarget
    {
        #region Properties

        public PromptWindow PromptWindow = null;

        public bool _isWindowActive;
        public bool IsWindowActive
        {
            get { return _isWindowActive; }
            set
            {
                _isWindowActive = value;
                this.RaisePropertyChanged("IsWindowActive");
            }
        }
        DispatcherTimer reminderTimer = new DispatcherTimer();
        private string _appName = "QuikTODO";

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                this.RaisePropertyChanged("FileName");
            }
        }

        public string CurrentTime
        {
            get { return DateTime.Now.ToString("t"); }
        }

        public bool NeedsToSave { get; set; }

        private string _mainDisplayName = "QuikTODO";
        public string MainDisplayName
        {
            get { return _mainDisplayName; }
            set
            {
                _mainDisplayName = value;
                _mainDisplayName = string.IsNullOrWhiteSpace(_mainDisplayName) ?
                    _appName : _appName + " - " + _mainDisplayName;
                this.RaisePropertyChanged("MainDisplayName");
            }
        }

        public string DefaultFilePath = Environment.CurrentDirectory + "\\Tasks TODO.txt";
        public string FilePath;

        private string _taskToAdd;
        public string TaskToAdd
        {
            get { return _taskToAdd; }
            set
            {
                _taskToAdd = value;
                this.RaisePropertyChanged("TaskToAdd");
            }
        }

        private DateTime? _taskDate;
        public DateTime? TaskDate
        {
            get { return _taskDate; }
            set
            {
                _taskDate = value;
                this.RaisePropertyChanged("TaskDate");
            }
        }

        private SolidColorBrush _priorityColor;
        public SolidColorBrush PriorityColor
        {
            get { return _priorityColor; }
            set
            {
                _priorityColor = value;
                this.RaisePropertyChanged("PriorityTooltip");
                this.RaisePropertyChanged("PriorityColor");
            }
        }

        public string PriorityTooltip
        {
            get
            {
                if (PriorityColor == System.Windows.Media.Brushes.Yellow)
                {
                    return "Medium Priority";
                }
                else if (PriorityColor == System.Windows.Media.Brushes.Orange)
                {
                    return "High Priority";
                }
                else if (PriorityColor == System.Windows.Media.Brushes.Red)
                {
                    return "Critical Priority";
                }
                else { return "Low Priority"; }
            }
        }

        public string TimespanDescription
        {
            get
            {
                var obj = SelectedTimespan.GetType().GetField(SelectedTimespan.ToString()).GetCustomAttributes(false);
                DescriptionAttribute attrib = obj[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }

        private ObservableCollection<Task> _taskCollection;
        public ObservableCollection<Task> TaskCollection
        {
            get
            {
                if (_taskCollection == null)
                    _taskCollection = new ObservableCollection<Task>();
                return _taskCollection;
            }
        }

        public List<Task> AllTasks { get; set; }

        public bool _isSortByHighestChecked;
        public bool IsSortByHighestChecked
        {
            get { return _isSortByHighestChecked; }
            set
            {
                _isSortByHighestChecked = value;
                RefreshTasks();
                this.RaisePropertyChanged("IsSortByHighestChecked");
            }
        }

        private bool _isHideCompletedChecked;
        public bool IsHideCompletedChecked
        {
            get { return _isHideCompletedChecked; }
            set
            {
                _isHideCompletedChecked = value;
                RefreshTasks();
                this.RaisePropertyChanged("IsHideCompletedChecked");
            }
        }

        private Task _selectedTask;
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                if (_selectedTask != value && _selectedTask != null && _selectedTask.IsEditable) { _selectedTask.IsEditable = false; }
                //if (_selectedTask != value && _selectedTask.IsEditable)
                //{
                //    foreach (var t in TaskCollection)
                //    {
                //        //if (t != value)
                //        //{
                //        //    t.IsEditable = false;
                //        //    t.ShowReminder = false;
                //        //}
                //    }
                //}
                _selectedTask = value;
                this.RaisePropertyChanged("TaskCollection");
                this.RaisePropertyChanged("SelectedTask");
            }
        }

        private bool _isEnableDrag = true;
        public bool IsEnableDrag
        {
            get { return _isEnableDrag; }
            set
            {
                _isEnableDrag = value;
                this.RaisePropertyChanged("IsEnableDrag");
            }
        }

        public bool IsAutoCompletedOff
        {
            get { return SelectedPriorityCompleted == PriorityType.None; }
            set
            {
                SelectedPriorityCompleted = PriorityType.None;
                this.RaisePropertyChanged("IsAutoCompletedOff");
                this.RaisePropertyChanged("IsLowCompleted");
                this.RaisePropertyChanged("IsMediumCompleted");
                this.RaisePropertyChanged("IsHighCompleted");
                this.RaisePropertyChanged("IsCriticalCompleted");
            }
        }


        public bool IsLowCompleted
        {
            get { return SelectedPriorityCompleted == PriorityType.Low; }
            set
            {
                SelectedPriorityCompleted = PriorityType.Low;
                this.RaisePropertyChanged("IsAutoCompletedOff");
                this.RaisePropertyChanged("IsLowCompleted");
                this.RaisePropertyChanged("IsMediumCompleted");
                this.RaisePropertyChanged("IsHighCompleted");
                this.RaisePropertyChanged("IsCriticalCompleted");
            }
        }


        public bool IsMediumCompleted
        {
            get { return SelectedPriorityCompleted == PriorityType.Medium; }
            set
            {
                SelectedPriorityCompleted = PriorityType.Medium;
                this.RaisePropertyChanged("IsAutoCompletedOff");
                this.RaisePropertyChanged("IsLowCompleted");
                this.RaisePropertyChanged("IsMediumCompleted");
                this.RaisePropertyChanged("IsHighCompleted");
                this.RaisePropertyChanged("IsCriticalCompleted");
            }
        }


        public bool IsHighCompleted
        {
            get { return SelectedPriorityCompleted == PriorityType.High; }
            set
            {
                SelectedPriorityCompleted = PriorityType.High;
                this.RaisePropertyChanged("IsAutoCompletedOff");
                this.RaisePropertyChanged("IsLowCompleted");
                this.RaisePropertyChanged("IsMediumCompleted");
                this.RaisePropertyChanged("IsHighCompleted");
                this.RaisePropertyChanged("IsCriticalCompleted");
            }
        }

        public bool IsCriticalCompleted
        {
            get { return SelectedPriorityCompleted == PriorityType.Critical; }
            set
            {
                SelectedPriorityCompleted = PriorityType.Critical;
                this.RaisePropertyChanged("IsAutoCompletedOff");
                this.RaisePropertyChanged("IsLowCompleted");
                this.RaisePropertyChanged("IsMediumCompleted");
                this.RaisePropertyChanged("IsHighCompleted");
                this.RaisePropertyChanged("IsCriticalCompleted");
            }
        }

        public PriorityType SelectedPriorityCompleted { get; set; }

        public TimeSpanType SelectedTimespan { get; set; }

        public bool ShowTodayTasks
        {
            get { return SelectedTimespan == TimeSpanType.Today; }
            set
            {
                SelectedTimespan = TimeSpanType.Today;
                RefreshViewOptions();
            }
        }

        public bool ShowThisWeekTasks
        {
            get { return SelectedTimespan == TimeSpanType.ThisWeek; }
            set
            {
                SelectedTimespan = TimeSpanType.ThisWeek;
                RefreshViewOptions();
            }
        }

        public bool ShowNextWeekTasks
        {
            get { return SelectedTimespan == TimeSpanType.NextWeek; }
            set
            {
                SelectedTimespan = TimeSpanType.NextWeek;
                RefreshViewOptions();
            }
        }

        public bool ShowPreviousWeekTasks
        {
            get { return SelectedTimespan == TimeSpanType.PreviousWeek; }
            set
            {
                SelectedTimespan = TimeSpanType.PreviousWeek;
                RefreshViewOptions();
            }
        }

        public bool ShowMonthTasks
        {
            get { return SelectedTimespan == TimeSpanType.Month; }
            set
            {
                SelectedTimespan = TimeSpanType.Month;
                RefreshViewOptions();
            }
        }

        public bool ShowFutureTasks
        {
            get { return SelectedTimespan == TimeSpanType.Future; }
            set
            {
                SelectedTimespan = TimeSpanType.Future;
                RefreshViewOptions();
            }
        }

        public bool ShowAllTasks
        {
            get { return SelectedTimespan == TimeSpanType.All; }
            set
            {
                SelectedTimespan = TimeSpanType.All;
                RefreshViewOptions();
            }
        }

        #endregion

        #region Commands

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(AddExecute);
                }
                return _addCommand;
            }
        }

        private RelayCommand _changePriorityCommand;
        public RelayCommand ChangePriorityCommand
        {
            get
            {
                if (_changePriorityCommand == null)
                {
                    _changePriorityCommand = new RelayCommand(ChangePriorityExecute);
                }
                return _changePriorityCommand;
            }
        }

        private RelayCommand<Task> _sortTaskCommand;
        public RelayCommand<Task> SortTaskCommand
        {
            get
            {
                if (_sortTaskCommand == null)
                {
                    _sortTaskCommand = new RelayCommand<Task>(o => CheckIsDoneExecute(o));
                }
                return _sortTaskCommand;
            }
        }

        #endregion

        #region Methods

        private void CheckIsDoneExecute(Task t)
        {
            RefreshTasks();
        }

        public void ChangePriorityExecute()
        {
            if (PriorityColor == System.Windows.Media.Brushes.LimeGreen)
            {
                PriorityColor = System.Windows.Media.Brushes.Yellow;
            }
            else if (PriorityColor == System.Windows.Media.Brushes.Yellow)
            {
                PriorityColor = System.Windows.Media.Brushes.Orange;
            }
            else if (PriorityColor == System.Windows.Media.Brushes.Orange)
            {
                PriorityColor = System.Windows.Media.Brushes.Red;
            }
            else if (PriorityColor == System.Windows.Media.Brushes.Red)
            {
                PriorityColor = System.Windows.Media.Brushes.LimeGreen;
            }
        }

        public void AddExecute()
        {
            if (!string.IsNullOrWhiteSpace(TaskToAdd) && TaskDate != null)
            {
                var t = new Task(this)
                {
                    TaskName = TaskToAdd,
                    TaskDate = TaskDate.Value,
                    PriorityColor = PriorityColor
                };
                AllTasks.Add(t);
                TaskCollection.Add(t);
            }
            RefreshTasks();
            
            //PromptWindow = new PromptWindow("Task Added", "'" + TaskToAdd + "' was added.", PromptType.Ok);
            //PromptWindow.ShowDialog();
            //PromptWindow = null;

            TaskToAdd = null;

        }

        public void RefreshTasks()
        {
            foreach (var t in AllTasks)
            {              
                switch (SelectedTimespan)
                {
                    case TimeSpanType.Today:
                        t.ShowThisTask = (!IsHideCompletedChecked || !t.IsDone) && t.TaskDate.Date == DateTime.Today.Date;
                        break;
                    case TimeSpanType.ThisWeek:
                        if (!t.TaskDate.IsThisWeek())
                        {
                            t.ShowThisTask = false;
                        }
                        else
                        {
                            t.ShowThisTask = (!IsHideCompletedChecked || !t.IsDone);
                        }
                        break;
                    case TimeSpanType.PreviousWeek:
                        if (!t.TaskDate.IsPreviousWeek())
                        {
                            t.ShowThisTask = false;
                        }
                        else
                        {
                            t.ShowThisTask = (!IsHideCompletedChecked || !t.IsDone);
                        }
                        break;
                    case TimeSpanType.NextWeek:
                        if (!t.TaskDate.IsNextWeek())
                        {
                            t.ShowThisTask = false;
                        }
                        else
                        {
                            t.ShowThisTask = (!IsHideCompletedChecked || !t.IsDone);
                        }
                        break;
                    case TimeSpanType.Future:
                        if (!t.TaskDate.IsFutureDate())
                        {
                            t.ShowThisTask = false;
                        }
                        else
                        {
                            t.ShowThisTask = (!IsHideCompletedChecked || !t.IsDone);
                        }
                        break;
                    case TimeSpanType.Month:
                        t.ShowThisTask = (!IsHideCompletedChecked || !t.IsDone) && t.TaskDate.Date.Month == DateTime.Today.Date.Month;
                        break;
                    default:
                        t.ShowThisTask = (!IsHideCompletedChecked || !t.IsDone);
                        break;
                }
            }

            if (_isSortByHighestChecked)
            {
                _taskCollection = new ObservableCollection<Task>(TaskCollection.OrderBy(o => o.IsDone).ThenBy(o => o.Priority).ThenBy(o => o.TaskDate).ToList());
            }
            else
            {
                _taskCollection = new ObservableCollection<Task>(TaskCollection.OrderBy(o => o.IsDone).ThenBy(o => o.TaskDate).ThenBy(o => o.Priority).ToList());
            }
            this.RaisePropertyChanged("TaskCollection");
            NeedsToSave = true;
        }

        public void RefreshRibbon()
        {
            this.RaisePropertyChanged("IsAutoCompletedOff");
            this.RaisePropertyChanged("IsLowCompleted");
            this.RaisePropertyChanged("IsMediumCompleted");
            this.RaisePropertyChanged("IsHighCompleted");
            this.RaisePropertyChanged("IsCriticalCompleted");
            this.RaisePropertyChanged("IsSortByHighestChecked");
            this.RaisePropertyChanged("IsHideCompletedChecked");
            this.RaisePropertyChanged("IsEnableDrag");
        }

        public void RefreshViewOptions()
        {
            this.RaisePropertyChanged("ShowAllTasks");
            this.RaisePropertyChanged("ShowMonthTasks");
            this.RaisePropertyChanged("ShowPreviousWeekTasks");
            this.RaisePropertyChanged("ShowNextWeekTasks");
            this.RaisePropertyChanged("ShowThisWeekTasks");
            this.RaisePropertyChanged("ShowFutureTasks");
            this.RaisePropertyChanged("ShowTodayTasks");
            this.RaisePropertyChanged("TimespanDescription");

            RefreshTasks();
        }

        #endregion

        #region Constructor 

        public TaskViewModel()
        {
            TaskDate = DateTime.Today;
            SelectedTimespan = TimeSpanType.ThisWeek;
            SelectedPriorityCompleted = PriorityType.None;
            PriorityColor = System.Windows.Media.Brushes.LimeGreen;

            AllTasks = new List<Task>();

            reminderTimer.Interval = TimeSpan.FromSeconds(1);
            reminderTimer.Tick += reminderTimer_Tick;
            reminderTimer.Start();
            this.RaisePropertyChanged("CurrentTime");
        }

        #endregion

        System.Timers.Timer timer = new System.Timers.Timer(2000);
        void reminderTimer_Tick(object sender, EventArgs e)
        {
            this.RaisePropertyChanged("CurrentTime");
            if (DateTime.Now.Second < 6)
            {
                reminderTimer.Interval = TimeSpan.FromMinutes(1);
                var timeList = TaskCollection.Where(i => i.TaskDate.Date >= DateTime.Today.Date && !string.IsNullOrWhiteSpace(i.ReminderTime) && !i.IsDone).ToList();
                foreach (var nextTime in timeList.OrderBy(i => i.TaskDate))
                {
                    int hour = 12;
                    if (int.TryParse(nextTime.ReminderTime.Substring(0, nextTime.ReminderTime.IndexOf(":")), out hour))
                    {
                        hour = nextTime.ReminderTime.Substring(5, 2) == "PM" ? hour + 12 : hour == 12 ? 0 : hour;
                        hour = hour == 24 ? 0 : hour;
                    }
                    int minute = 0;
                    int.TryParse(nextTime.ReminderTime.Substring(nextTime.ReminderTime.IndexOf(":") + 1, 2), out minute);
                    int totalMinutesNow = (DateTime.Now.Hour * 60) + DateTime.Now.Minute;
                    int totalMinutesTask = (hour * 60) + minute;
                    int minutesLeft = int.Parse((totalMinutesTask - totalMinutesNow).ToString("00"));

                    if (minutesLeft < 61 && minutesLeft > -1)
                    {
                        if (minutesLeft % 5 == 0 || minutesLeft < 6)
                        {
                            string occursWhen = minutesLeft == 0 ? "NOW!!" :
                                minutesLeft == 1 ? "in " + minutesLeft + " minute." : "in " + minutesLeft + " minutes.";
                            string messageText = "'" + nextTime.TaskName +
                                "' occurs " + occursWhen;
                            string question = "\r\n\nSnooze Reminder or Cancel Reminder?";

                            //PromptWindow = new PromptWindow("Task Reminder", messageText + question, PromptType.SnoozeCancel);
                            //PromptWindow.ShowDialog();
                            //if (PromptWindow.Response == PromptResponse.Cancel)
                            //{
                            //    nextTime.ShowReminder = false;
                            //    nextTime.HasReminderTime = false;
                            //    nextTime.ReminderTime = null;
                            //    RefreshTasks();
                            //}
                            timer.Elapsed += timer_Elapsed;
                            timer.Start();
                            if (!HungReminders.Any(a => a.Item1 == nextTime))
                                PendingReminders.Add(new Tuple<Task, string>(nextTime, messageText + question));
                        }
                    }
                }
            }
            else
            {
                reminderTimer.Interval = TimeSpan.FromSeconds(1);
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (PendingReminders.Count > 0)
            {
                var t = PendingReminders.First();
                PendingReminders.Remove(t);
                HungReminders.Add(t);
                timer.Elapsed -= timer_Elapsed;
                timer.Stop();

                if (System.Windows.MessageBox.Show(t.Item2, "Task Reminder",
                    MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    t.Item1.ShowReminder = false;
                    t.Item1.HasReminderTime = false;
                    t.Item1.ReminderTime = null;
                    RefreshTasks();
                    HungReminders.Remove(t);
                }
                else { HungReminders.Remove(t); };

            }
        }
        List<Tuple<Task, string>> PendingReminders = new List<Tuple<Task, string>>();
        List<Tuple<Task, string>> HungReminders = new List<Tuple<Task, string>>();

        public void DragOver(IDropInfo dropInfo)
        {
            Task sourceItem = dropInfo.Data as Task;
            Task targetItem = dropInfo.TargetItem as Task;

            if (sourceItem != null && targetItem != null && sourceItem != targetItem && !sourceItem.IsEditable)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            Task sourceItem = dropInfo.Data as Task;
            Task targetItem = dropInfo.TargetItem as Task;

            if (sourceItem != null && targetItem != null)
            {
                int sourceIndex = TaskCollection.IndexOf(sourceItem);
                int targetIndex = TaskCollection.IndexOf(targetItem);

                //Dragging down the list
                if (sourceIndex < targetIndex)
                {
                    if (targetIndex == TaskCollection.Count - 1)
                    {
                        sourceItem.PriorityColor = System.Windows.Media.Brushes.LimeGreen; // lowest priority color
                    }
                    else if (targetIndex < TaskCollection.Count - 1)
                    {
                        if (TaskCollection[targetIndex + 1].IsDone)
                        {
                            sourceItem.PriorityColor = System.Windows.Media.Brushes.LimeGreen;
                        }
                        else
                        {
                            sourceItem.PriorityColor = targetItem.PriorityColor;
                        }
                    }
                    else
                    {
                        sourceItem.PriorityColor = targetItem.PriorityColor;
                    }
                }
                //Dragging up the list
                else if (sourceIndex > targetIndex)
                {
                    if (targetIndex == 0)
                    {
                        sourceItem.PriorityColor = System.Windows.Media.Brushes.Red;
                    }
                    else if ((targetIndex - 1) != -1 && TaskCollection[targetIndex - 1].Priority < targetItem.Priority)
                    {
                        sourceItem.PriorityColor = TaskCollection[targetIndex - 1].PriorityColor;
                    }
                    else
                    {
                        sourceItem.PriorityColor = targetItem.PriorityColor;
                    }
                }

                TaskCollection.Remove(sourceItem);
                TaskCollection.Insert(targetIndex, sourceItem);

                RefreshTasks();
            }
        }
    }

    public enum TimeSpanType
    {
        [Description("Today's Tasks")]
        Today = 1,
        [Description("This Week's Tasks")]
        ThisWeek = 2,
        [Description("Previous Week's Tasks")]
        PreviousWeek = 3,
        [Description("Next Week's Tasks")]
        NextWeek = 4,
        [Description("This Month's Tasks")]
        Month = 5,
        [Description("Future Tasks")]
        Future = 6,
        [Description("All Tasks")]
        All = 7
    }
}

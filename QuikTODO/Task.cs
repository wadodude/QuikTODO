using System;
using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace QuikTODO
{
    public class Task : ViewModelBase
    {
        #region Properties

        public int PreviousIndex { get; set; }

        public string TaskName { get; set; }
        public DateTime TaskDate { get; set; }
        public SolidColorBrush PriorityColor { get; set; }
        public SolidColorBrush OverdueColor
        {
            get
            {
                return TaskDate < DateTime.Today ?
                    System.Windows.Media.Brushes.Red :
                    System.Windows.Media.Brushes.Black;
            }
        }
        public bool IsEditable { get; set; }

        private bool _showThisTask;
        public bool ShowThisTask
        {
            get { return _showThisTask; }
            set
            {
                _showThisTask = value;
                this.RaisePropertyChanged("ShowThisTask");
            }
        }

        private bool _showReminder;
        public bool ShowReminder
        {
            get { return _showReminder; }
            set
            {
                _showReminder = value;
                if (_showReminder == true)
                {
                    if (_reminderTime == null)
                        ReminderTime = "12:00AM";
                }
                this.RaisePropertyChanged("ShowReminder");
            }
        }

        public string ReminderHeader
        {
            get { return HasReminderTime ? "Edit Reminder" : "Add Reminder"; }
        }

        private bool _hasReminderTime;
        public bool HasReminderTime
        {
            get { return _hasReminderTime; }
            set
            {
                _hasReminderTime = value;
                this.RaisePropertyChanged("ReminderHeader");
                this.RaisePropertyChanged("HasReminderTime");
            }
        }

        public PriorityType Priority
        {
            get
            {
                return PriorityColor == System.Windows.Media.Brushes.LimeGreen ? PriorityType.Low :
                    PriorityColor == System.Windows.Media.Brushes.Yellow ? PriorityType.Medium :
                    PriorityColor == System.Windows.Media.Brushes.Orange ? PriorityType.High :
                    PriorityColor == System.Windows.Media.Brushes.Red ? PriorityType.Critical : PriorityType.Critical;
            }
        }

        public void ChangePriority()
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

        private bool _isDone;
        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                _isDone = value;
                if (_isDone && !Parent.IsAutoCompletedOff)
                {
                    if (Parent.IsCriticalCompleted)
                    {
                        AssignPriority(PriorityType.Critical);
                    }
                    else if (Parent.IsHighCompleted)
                    {
                        AssignPriority(PriorityType.High);
                    }
                    else if (Parent.IsMediumCompleted)
                    {
                        AssignPriority(PriorityType.Medium);
                    }
                    else if (Parent.IsLowCompleted)
                    {
                        AssignPriority(PriorityType.Low);
                    }
                }
                this.RaisePropertyChanged("IsDone");
            }
        }
        
        private string _reminderTime;
        public string ReminderTime
        {
            get { return _reminderTime; }
            set
            {
                _reminderTime = value;
                this.RaisePropertyChanged("ReminderTime");
            }
        }

        private TaskViewModel Parent { get; set; }

        #endregion

        public Task(TaskViewModel parent) { Parent = parent; }

        private void AssignPriority(PriorityType priorityType)
        {
            switch (priorityType)
            {
                case PriorityType.Critical:
                    PriorityColor = System.Windows.Media.Brushes.Red;
                    break;
                case PriorityType.High:
                    PriorityColor = System.Windows.Media.Brushes.Orange;
                    break;
                case PriorityType.Medium:
                    PriorityColor = System.Windows.Media.Brushes.Yellow;
                    break;
                case PriorityType.Low:
                    PriorityColor = System.Windows.Media.Brushes.LimeGreen;
                    break;
            }
        }

    }
}

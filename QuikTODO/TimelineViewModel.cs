using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace QuikTODO
{
    public class TimelineViewModel : ViewModelBase
    {
        #region Properties

        private int _sliderValue;
        public int SliderValue
        {
            get { return _sliderValue; }
            set
            {
                _sliderValue = value;
                this.RaisePropertyChanged("SliderValue");
                this.RaisePropertyChanged("SliderTime");
            }
        }
        public string SliderTime
        {
            get 
            {
                var t = TimeSpan.FromMinutes(SliderValue);
                int h = t.Hours > 12 ? t.Hours - 12 : t.Hours;
                if (h == 0) { h = 12; }
                return h.ToString("00") + ":" + t.Minutes.ToString("00") + (t.Hours > 12 ? "PM" : "AM");
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

        #endregion

        public TimelineViewModel(ObservableCollection<Task> tasks)
        {
            _taskCollection = tasks;
            _sliderValue = (int)DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            this.RaisePropertyChanged("TaskCollection");
        }
    }
}

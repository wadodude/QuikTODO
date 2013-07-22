using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuikTODO
{
    public class PromptViewModel
    {
        public string PromptTitle { get; set; }
        public string PromptMessage { get; set; }
        public bool IsYesNoCancel { get { return PromptType == PromptType.YesNoCancel; } }
        public bool IsYesNo { get { return PromptType == PromptType.YesNo; } }
        public bool IsReminder { get { return PromptType == PromptType.SnoozeCancel; } }
        public bool IsInformation { get { return PromptType == PromptType.Ok; } }
        public bool IsOkCancel { get { return PromptType == PromptType.OkCancel; } }

        public PromptType PromptType { get; set; }

        public PromptViewModel(string title, string message, PromptType promptType)
        {
            PromptType = promptType;
            PromptTitle = title;
            PromptMessage = message;
        }
    }

    public enum PromptType
    {
        YesNo = 1,
        YesNoCancel =2,
        Ok = 3,
        OkCancel = 4,
        SnoozeCancel = 5
    }

    public enum PromptResponse
    {
        NotSpecified = 0,
        Ok = 1,
        Yes = 2,
        No = 3,
        Snooze = 4,
        Cancel = 5
    }
}

using System;

namespace AgileProjectManager.Model.Entities
{
    public class TaskItem
    {
        public string TaskID { get; set; }
        public string StoryID { get; set; }
        public string State { get; set; }
        public string Priority { get; set; }
        public double PlannedTime { get; set; }
        public double ActualTime { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string DifficultyLevel { get; set; }
        public string CategoryLabels { get; set; }

        public TaskItem() { }

        public TaskItem(string taskID, string storyID, string state, string priority,
            double plannedTime, double actualTime, DateTime? plannedStartDate,
            DateTime? actualEndDate, string difficultyLevel, string categoryLabels)
        {
            TaskID = taskID;
            StoryID = storyID;
            State = state;
            Priority = priority;
            PlannedTime = plannedTime;
            ActualTime = actualTime;
            PlannedStartDate = plannedStartDate;
            ActualEndDate = actualEndDate;
            DifficultyLevel = difficultyLevel;
            CategoryLabels = categoryLabels;
        }

        public override string ToString()
        {
            return $"{TaskID} | {Priority} | {State}";
        }
    }
}
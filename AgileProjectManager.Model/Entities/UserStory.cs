namespace AgileProjectManager.Model.Entities
{
    public class UserStory
    {
        public string StoryID { get; set; }
        public string ProjectID { get; set; }
        public string Content { get; set; }
        public string PriorityLevel { get; set; }
        public string State { get; set; }

        public UserStory() { }

        public UserStory(string storyID, string projectID, string content, string priorityLevel, string state)
        {
            StoryID = storyID;
            ProjectID = projectID;
            Content = content;
            PriorityLevel = priorityLevel;
            State = state;
        }

        public override string ToString()
        {
            return $"{StoryID} | {Content} | {State}";
        }
    }
}
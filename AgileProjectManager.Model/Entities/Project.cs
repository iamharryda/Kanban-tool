namespace AgileProjectManager.Model.Entities
{
    public class Project
    {
        public string ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Project() { }

        public Project(string projectID, string name, string description)
        {
            ProjectID = projectID;
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            return $"{ProjectID} - {Name}";
        }
    }
}
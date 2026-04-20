namespace AgileProjectManager.Model.Entities
{
    public class Person
    {
        public string PersonID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public Person() { }

        public Person(string personID, string name, string role)
        {
            PersonID = personID;
            Name = name;
            Role = role;
        }

        public override string ToString()
        {
            return $"{PersonID} - {Name} ({Role})";
        }
    }
}
using System.Collections.Generic;
using AgileProjectManager.Model.Entities;
using MySql.Data.MySqlClient;

namespace AgileProjectManager.Model.DataAccess
{
    public class PersonRepository
    {
        private readonly DbConnectionFactory _factory;

        public PersonRepository()
        {
            _factory = new DbConnectionFactory();
        }

        public void AddPerson(Person person)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO Person(personID, name, role)
                               VALUES(@personID, @name, @role)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@personID", person.PersonID);
                    cmd.Parameters.AddWithValue("@name", person.Name);
                    cmd.Parameters.AddWithValue("@role", person.Role);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Person GetPersonById(string personID)
        {
            Person person = null;

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT personID, name, role
                               FROM Person
                               WHERE personID = @personID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@personID", personID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            person = new Person(
                                reader.GetString("personID"),
                                reader.GetString("name"),
                                reader.GetString("role")
                            );
                        }
                    }
                }
            }

            return person;
        }

        public List<Person> GetAllPersons()
        {
            List<Person> persons = new List<Person>();

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT personID, name, role FROM Person";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        persons.Add(new Person(
                            reader.GetString("personID"),
                            reader.GetString("name"),
                            reader.GetString("role")
                        ));
                    }
                }
            }

            return persons;
        }

        public void LinkPersonToProject(string projectID, string personID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO ProjectPerson(projectID, personID)
                               VALUES(@projectID, @personID)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    cmd.Parameters.AddWithValue("@personID", personID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool IsPersonLinkedToProject(string projectID, string personID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT COUNT(*)
                               FROM ProjectPerson
                               WHERE projectID = @projectID AND personID = @personID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    cmd.Parameters.AddWithValue("@personID", personID);

                    int count = System.Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public void AssignPersonToTask(string taskID, string personID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO TaskPerson(taskID, personID)
                               VALUES(@taskID, @personID)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", taskID);
                    cmd.Parameters.AddWithValue("@personID", personID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool IsPersonAssignedToTask(string taskID, string personID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT COUNT(*)
                               FROM TaskPerson
                               WHERE taskID = @taskID AND personID = @personID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", taskID);
                    cmd.Parameters.AddWithValue("@personID", personID);

                    int count = System.Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public void RemovePersonFromTask(string taskID, string personID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"DELETE FROM TaskPerson
                       WHERE taskID = @taskID AND personID = @personID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", taskID);
                    cmd.Parameters.AddWithValue("@personID", personID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Person> GetPersonsByTaskId(string taskID)
        {
            List<Person> persons = new List<Person>();

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT p.personID, p.name, p.role
                               FROM Person p
                               INNER JOIN TaskPerson tp ON p.personID = tp.personID
                               WHERE tp.taskID = @taskID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", taskID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            persons.Add(new Person(
                                reader.GetString("personID"),
                                reader.GetString("name"),
                                reader.GetString("role")
                            ));
                        }
                    }
                }
            }

            return persons;
        }
        public void RemoveAllPersonsFromTasksByStoryId(string storyID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = @"DELETE tp FROM TaskPerson tp
                       INNER JOIN TaskItem t ON tp.taskID = t.taskID
                       WHERE t.storyID = @storyID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@storyID", storyID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdatePerson(string personID, string name, string role)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = "UPDATE Person SET name=@name, role=@role WHERE personID=@personID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@personID", personID);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletePerson(string personID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Person WHERE personID=@personID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@personID", personID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemovePersonFromAllTasks(string personID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = "DELETE FROM TaskPerson WHERE personID=@personID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@personID", personID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemovePersonFromAllProjects(string personID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = "DELETE FROM ProjectPerson WHERE personID=@personID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@personID", personID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void RemoveAllPersonsFromTask(string taskID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = "DELETE FROM TaskPerson WHERE taskID=@taskID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", taskID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
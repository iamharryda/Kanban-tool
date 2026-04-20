using System.Collections.Generic;
using AgileProjectManager.Model.Entities;
using MySql.Data.MySqlClient;

namespace AgileProjectManager.Model.DataAccess
{
    public class ProjectRepository
    {
        private readonly DbConnectionFactory _factory;

        public ProjectRepository()
        {
            _factory = new DbConnectionFactory();
        }

        public void AddProject(Project project)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO Project(projectID, name, description)
                               VALUES(@projectID, @name, @description)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@projectID", project.ProjectID);
                    cmd.Parameters.AddWithValue("@name", project.Name);
                    cmd.Parameters.AddWithValue("@description", project.Description);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = "SELECT projectID, name, description FROM Project";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        projects.Add(new Project(
                            reader.GetString("projectID"),
                            reader.GetString("name"),
                            reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description")
                        ));
                    }
                }
            }

            return projects;
        }

        public Project GetProjectById(string projectID)
        {
            Project project = null;

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT projectID, name, description
                               FROM Project
                               WHERE projectID = @projectID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@projectID", projectID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            project = new Project(
                                reader.GetString("projectID"),
                                reader.GetString("name"),
                                reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description")
                            );
                        }
                    }
                }
            }

            return project;
        }
        public void UpdateProject(string projectID, string name, string description)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = @"UPDATE Project SET name=@name, description=@description 
                       WHERE projectID=@projectID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProject(string projectID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Project WHERE projectID=@projectID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@projectID", projectID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
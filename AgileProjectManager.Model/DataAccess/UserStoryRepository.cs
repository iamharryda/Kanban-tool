using System.Collections.Generic;
using AgileProjectManager.Model.Entities;
using MySql.Data.MySqlClient;

namespace AgileProjectManager.Model.DataAccess
{
    public class UserStoryRepository
    {
        private readonly DbConnectionFactory _factory;

        public UserStoryRepository()
        {
            _factory = new DbConnectionFactory();
        }

        public void AddUserStory(UserStory userStory)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO UserStory
                               (storyID, projectID, content, priorityLevel, state)
                               VALUES
                               (@storyID, @projectID, @content, @priorityLevel, @state)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@storyID", userStory.StoryID);
                    cmd.Parameters.AddWithValue("@projectID", userStory.ProjectID);
                    cmd.Parameters.AddWithValue("@content", userStory.Content);
                    cmd.Parameters.AddWithValue("@priorityLevel", userStory.PriorityLevel);
                    cmd.Parameters.AddWithValue("@state", userStory.State);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public UserStory GetUserStoryById(string storyID)
        {
            UserStory story = null;

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT storyID, projectID, content, priorityLevel, state
                               FROM UserStory
                               WHERE storyID = @storyID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@storyID", storyID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            story = new UserStory(
                                reader.GetString("storyID"),
                                reader.GetString("projectID"),
                                reader.GetString("content"),
                                reader.IsDBNull(reader.GetOrdinal("priorityLevel")) ? "" : reader.GetString("priorityLevel"),
                                reader.GetString("state")
                            );
                        }
                    }
                }
            }

            return story;
        }

        public List<UserStory> GetUserStoriesByProject(string projectID)
        {
            List<UserStory> stories = new List<UserStory>();

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT storyID, projectID, content, priorityLevel, state
                               FROM UserStory
                               WHERE projectID = @projectID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@projectID", projectID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stories.Add(new UserStory(
                                reader.GetString("storyID"),
                                reader.GetString("projectID"),
                                reader.GetString("content"),
                                reader.IsDBNull(reader.GetOrdinal("priorityLevel")) ? "" : reader.GetString("priorityLevel"),
                                reader.GetString("state")
                            ));
                        }
                    }
                }
            }

            return stories;
        }

        public void UpdateUserStoryState(string storyID, string newState)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"UPDATE UserStory
                       SET state = @state
                       WHERE storyID = @storyID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@state", newState);
                    cmd.Parameters.AddWithValue("@storyID", storyID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<UserStory> GetDependencies(string storyID)
        {
            List<UserStory> dependencies = new List<UserStory>();

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT us.storyID, us.projectID, us.content, us.priorityLevel, us.state
                       FROM UserStory us
                       INNER JOIN StoryDependency sd 
                       ON us.storyID = sd.dependsOnStoryID
                       WHERE sd.storyID = @storyID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@storyID", storyID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dependencies.Add(new UserStory(
                                reader.GetString("storyID"),
                                reader.GetString("projectID"),
                                reader.GetString("content"),
                                reader.IsDBNull(reader.GetOrdinal("priorityLevel")) ? "" : reader.GetString("priorityLevel"),
                                reader.GetString("state")
                            ));
                        }
                    }
                }
            }

            return dependencies;
        }

        public List<UserStory> GetAllUserStories()
        {
            List<UserStory> stories = new List<UserStory>();

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT storyID, projectID, content, priorityLevel, state
                               FROM UserStory";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stories.Add(new UserStory(
                            reader.GetString("storyID"),
                            reader.GetString("projectID"),
                            reader.GetString("content"),
                            reader.IsDBNull(reader.GetOrdinal("priorityLevel")) ? "" : reader.GetString("priorityLevel"),
                            reader.GetString("state")
                        ));
                    }
                }
            }

            return stories;
        }
        public void DeleteUserStory(string storyID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = "DELETE FROM UserStory WHERE storyID = @storyID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@storyID", storyID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateUserStory(string storyID, string content, string priorityLevel)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = @"UPDATE UserStory SET content=@content, priorityLevel=@priorityLevel 
                       WHERE storyID=@storyID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@storyID", storyID);
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.Parameters.AddWithValue("@priorityLevel", priorityLevel);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
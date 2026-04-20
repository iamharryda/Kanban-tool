using System;
using System.Collections.Generic;
using AgileProjectManager.Model.Entities;
using MySql.Data.MySqlClient;

namespace AgileProjectManager.Model.DataAccess
{
    public class TaskRepository
    {
        private readonly DbConnectionFactory _factory;

        public TaskRepository()
        {
            _factory = new DbConnectionFactory();
        }

        public void AddTask(TaskItem task)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO TaskItem
                               (taskID, storyID, state, priority, plannedTime, actualTime,
                                plannedStartDate, actualEndDate, difficultyLevel, categoryLabels)
                               VALUES
                               (@taskID, @storyID, @state, @priority, @plannedTime, @actualTime,
                                @plannedStartDate, @actualEndDate, @difficultyLevel, @categoryLabels)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", task.TaskID);
                    cmd.Parameters.AddWithValue("@storyID", task.StoryID);
                    cmd.Parameters.AddWithValue("@state", task.State);
                    cmd.Parameters.AddWithValue("@priority", task.Priority);
                    cmd.Parameters.AddWithValue("@plannedTime", task.PlannedTime);
                    cmd.Parameters.AddWithValue("@actualTime", task.ActualTime);
                    cmd.Parameters.AddWithValue("@plannedStartDate", (object?)task.PlannedStartDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@actualEndDate", (object?)task.ActualEndDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@difficultyLevel", task.DifficultyLevel);
                    cmd.Parameters.AddWithValue("@categoryLabels", task.CategoryLabels);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public TaskItem GetTaskById(string taskID)
        {
            TaskItem task = null;

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT taskID, storyID, state, priority, plannedTime, actualTime,
                                      plannedStartDate, actualEndDate, difficultyLevel, categoryLabels
                               FROM TaskItem
                               WHERE taskID = @taskID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", taskID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            task = new TaskItem(
                                reader.GetString("taskID"),
                                reader.GetString("storyID"),
                                reader.GetString("state"),
                                reader.IsDBNull(reader.GetOrdinal("priority")) ? "" : reader.GetString("priority"),
                                reader.IsDBNull(reader.GetOrdinal("plannedTime")) ? 0 : Convert.ToDouble(reader["plannedTime"]),
                                reader.IsDBNull(reader.GetOrdinal("actualTime")) ? 0 : Convert.ToDouble(reader["actualTime"]),
                                reader.IsDBNull(reader.GetOrdinal("plannedStartDate")) ? null : reader.GetDateTime("plannedStartDate"),
                                reader.IsDBNull(reader.GetOrdinal("actualEndDate")) ? null : reader.GetDateTime("actualEndDate"),
                                reader.IsDBNull(reader.GetOrdinal("difficultyLevel")) ? "" : reader.GetString("difficultyLevel"),
                                reader.IsDBNull(reader.GetOrdinal("categoryLabels")) ? "" : reader.GetString("categoryLabels")
                            );
                        }
                    }
                }
            }

            return task;
        }

        public List<TaskItem> GetTasksByStoryId(string storyID)
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT taskID, storyID, state, priority, plannedTime, actualTime,
                                      plannedStartDate, actualEndDate, difficultyLevel, categoryLabels
                               FROM TaskItem
                               WHERE storyID = @storyID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@storyID", storyID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new TaskItem(
                                reader.GetString("taskID"),
                                reader.GetString("storyID"),
                                reader.GetString("state"),
                                reader.IsDBNull(reader.GetOrdinal("priority")) ? "" : reader.GetString("priority"),
                                reader.IsDBNull(reader.GetOrdinal("plannedTime")) ? 0 : Convert.ToDouble(reader["plannedTime"]),
                                reader.IsDBNull(reader.GetOrdinal("actualTime")) ? 0 : Convert.ToDouble(reader["actualTime"]),
                                reader.IsDBNull(reader.GetOrdinal("plannedStartDate")) ? null : reader.GetDateTime("plannedStartDate"),
                                reader.IsDBNull(reader.GetOrdinal("actualEndDate")) ? null : reader.GetDateTime("actualEndDate"),
                                reader.IsDBNull(reader.GetOrdinal("difficultyLevel")) ? "" : reader.GetString("difficultyLevel"),
                                reader.IsDBNull(reader.GetOrdinal("categoryLabels")) ? "" : reader.GetString("categoryLabels")
                            ));
                        }
                    }
                }
            }

            return tasks;
        }

        public void UpdateTaskPriority(string taskID, string newPriority)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"UPDATE TaskItem
                       SET priority = @priority
                       WHERE taskID = @taskID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@priority", newPriority);
                    cmd.Parameters.AddWithValue("@taskID", taskID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTaskState(string taskID, string newState)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"UPDATE TaskItem
                       SET state = @state
                       WHERE taskID = @taskID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@state", newState);
                    cmd.Parameters.AddWithValue("@taskID", taskID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<TaskItem> GetAllTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();

                string sql = @"SELECT taskID, storyID, state, priority, plannedTime, actualTime,
                                      plannedStartDate, actualEndDate, difficultyLevel, categoryLabels
                               FROM TaskItem";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskItem(
                            reader.GetString("taskID"),
                            reader.GetString("storyID"),
                            reader.GetString("state"),
                            reader.IsDBNull(reader.GetOrdinal("priority")) ? "" : reader.GetString("priority"),
                            reader.IsDBNull(reader.GetOrdinal("plannedTime")) ? 0 : Convert.ToDouble(reader["plannedTime"]),
                            reader.IsDBNull(reader.GetOrdinal("actualTime")) ? 0 : Convert.ToDouble(reader["actualTime"]),
                            reader.IsDBNull(reader.GetOrdinal("plannedStartDate")) ? null : reader.GetDateTime("plannedStartDate"),
                            reader.IsDBNull(reader.GetOrdinal("actualEndDate")) ? null : reader.GetDateTime("actualEndDate"),
                            reader.IsDBNull(reader.GetOrdinal("difficultyLevel")) ? "" : reader.GetString("difficultyLevel"),
                            reader.IsDBNull(reader.GetOrdinal("categoryLabels")) ? "" : reader.GetString("categoryLabels")
                        ));
                    }
                }
            }

            return tasks;
        }
        public void DeleteTasksByStoryId(string storyID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = "DELETE FROM TaskItem WHERE storyID = @storyID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@storyID", storyID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateTask(TaskItem task)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = @"UPDATE TaskItem SET priority=@priority, plannedTime=@plannedTime,
                       actualTime=@actualTime, plannedStartDate=@plannedStartDate,
                       actualEndDate=@actualEndDate, difficultyLevel=@difficultyLevel,
                       categoryLabels=@categoryLabels WHERE taskID=@taskID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", task.TaskID);
                    cmd.Parameters.AddWithValue("@priority", task.Priority);
                    cmd.Parameters.AddWithValue("@plannedTime", task.PlannedTime);
                    cmd.Parameters.AddWithValue("@actualTime", task.ActualTime);
                    cmd.Parameters.AddWithValue("@plannedStartDate", (object?)task.PlannedStartDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@actualEndDate", (object?)task.ActualEndDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@difficultyLevel", task.DifficultyLevel);
                    cmd.Parameters.AddWithValue("@categoryLabels", task.CategoryLabels);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(string taskID)
        {
            using (MySqlConnection conn = _factory.CreateConnection())
            {
                conn.Open();
                string sql = "DELETE FROM TaskItem WHERE taskID=@taskID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskID", taskID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
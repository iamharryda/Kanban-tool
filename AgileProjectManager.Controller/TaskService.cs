using System;
using System.Collections.Generic;
using AgileProjectManager.Model.DataAccess;
using AgileProjectManager.Model.Entities;

namespace AgileProjectManager.Controller
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;
        private readonly UserStoryRepository _userStoryRepository;
        private readonly PersonRepository _personRepository;


        public TaskService()
        {
            _taskRepository = new TaskRepository();
            _userStoryRepository = new UserStoryRepository();
            _personRepository = new PersonRepository();
        }

        public void AddTask(string taskID, string storyID, string priority, double plannedTime,
            string difficultyLevel, string categoryLabels)
        {
            UserStory story = _userStoryRepository.GetUserStoryById(storyID);
            if (story == null)
            {
                throw new Exception("User story does not exist.");
            }

            if (story.State == "done")
            {
                throw new Exception("Cannot add a task to a completed user story.");
            }

            TaskItem existing = _taskRepository.GetTaskById(taskID);
            if (existing != null)
            {
                throw new Exception("Task ID already exists.");
            }

            TaskItem task = new TaskItem(
                taskID,
                storyID,
                "to be done",
                priority,
                plannedTime,
                0,
                null,
                null,
                difficultyLevel,
                categoryLabels
            );

            _taskRepository.AddTask(task);
        }

        public List<TaskItem> GetTasksByStoryId(string storyID)
        {
            UserStory story = _userStoryRepository.GetUserStoryById(storyID);
            if (story == null)
            {
                throw new Exception("User story does not exist.");
            }

            return _taskRepository.GetTasksByStoryId(storyID);
        }

        public List<TaskItem> GetAllTasks()
        {
            return _taskRepository.GetAllTasks();
        }

        public void ChangeTaskPriority(string taskID, string newPriority)
        {
            TaskItem task = _taskRepository.GetTaskById(taskID);
            if (task == null)
            {
                throw new Exception("Task does not exist.");
            }

            if (string.IsNullOrWhiteSpace(newPriority))
            {
                throw new Exception("Priority cannot be empty.");
            }

            _taskRepository.UpdateTaskPriority(taskID, newPriority);
        }
        public void ChangeTaskState(string taskID, string newState)
        {
            TaskItem task = _taskRepository.GetTaskById(taskID);
            if (task == null)
            {
                throw new Exception("Task does not exist.");
            }

            if (!IsValidTaskState(newState))
            {
                throw new Exception("Invalid task state. Use: to be done, in process, done.");
            }

            UserStory story = _userStoryRepository.GetUserStoryById(task.StoryID);
            if (story == null)
            {
                throw new Exception("Related user story does not exist.");
            }

            if (story.State != "in sprint")
            {
                throw new Exception("Task state can only be changed when the user story is in sprint.");
            }

            if (!IsValidTaskTransition(task.State, newState))
            {
                throw new Exception("Invalid task state transition.");
            }

            _taskRepository.UpdateTaskState(taskID, newState);
        }

        private bool IsValidTaskState(string state)
        {
            return state == "to be done" ||
                   state == "in process" ||
                   state == "done";
        }

        private bool IsValidTaskTransition(string currentState, string newState)
        {
            if (currentState == newState)
                return true;

            if (currentState == "to be done" && newState == "in process")
                return true;

            if (currentState == "in process" && newState == "to be done")
                return true;

            if (currentState == "in process" && newState == "done")
                return true;

            if (currentState == "done" && newState == "in process")
                return true;

            return false;
        }
        public string GenerateTaskReport(string taskID)
        {
            TaskItem task = _taskRepository.GetTaskById(taskID);
            if (task == null)
                throw new Exception("Task does not exist.");

            UserStory story = _userStoryRepository.GetUserStoryById(task.StoryID);
            List<Person> persons = _personRepository.GetPersonsByTaskId(taskID);

            string report = "";
            report += "===== TASK REPORT =====\n";
            report += $"Task ID       : {task.TaskID}\n";
            report += $"State         : {task.State}\n";
            report += $"Priority      : {task.Priority}\n";
            report += $"Difficulty    : {task.DifficultyLevel}\n";
            report += $"Category      : {task.CategoryLabels}\n";
            report += $"Planned Time  : {task.PlannedTime}\n";
            report += $"Actual Time   : {task.ActualTime}\n";
            report += $"Planned Start : {(task.PlannedStartDate.HasValue ? task.PlannedStartDate.Value.ToShortDateString() : "N/A")}\n";
            report += $"Actual End    : {(task.ActualEndDate.HasValue ? task.ActualEndDate.Value.ToShortDateString() : "N/A")}\n";
            report += $"\nLinked Story  : {story?.StoryID} | {story?.Content} | {story?.State}\n";
            report += "\nAssigned Persons:\n";

            if (persons.Count == 0)
                report += "  No persons assigned.\n";
            else
                foreach (Person p in persons)
                    report += $"  {p.PersonID} | {p.Name} | {p.Role}\n";

            report += "=======================\n";
            return report;
        }
        public void UpdateTask(string taskID, string priority, double plannedTime,
    double actualTime, string difficultyLevel, string categoryLabels)
        {
            TaskItem task = _taskRepository.GetTaskById(taskID);
            if (task == null)
                throw new Exception("Task does not exist.");

            task.Priority = priority;
            task.PlannedTime = plannedTime;
            task.ActualTime = actualTime;
            task.DifficultyLevel = difficultyLevel;
            task.CategoryLabels = categoryLabels;

            _taskRepository.UpdateTask(task);
        }

        public void DeleteTask(string taskID)
        {
            TaskItem task = _taskRepository.GetTaskById(taskID);
            if (task == null)
                throw new Exception("Task does not exist.");
            // Remove person assignments first
            _personRepository.RemoveAllPersonsFromTask(taskID);
            _taskRepository.DeleteTask(taskID);
        }
        public string CheckSprintFeasibility(string storyID, double teamCapacity)
        {
            UserStory story = _userStoryRepository.GetUserStoryById(storyID);
            if (story == null)
                throw new Exception("User story does not exist.");
            if (story.State != "in sprint")
                throw new Exception("User story is not in sprint.");

            List<TaskItem> tasks = _taskRepository.GetTasksByStoryId(storyID);

            double totalPlanned = 0;
            foreach (TaskItem t in tasks)
                totalPlanned += t.PlannedTime;

            string result = "";
            result += "===== SPRINT FEASIBILITY =====\n";
            result += $"Story ID       : {storyID}\n";
            result += $"Total Tasks    : {tasks.Count}\n";
            result += $"Total Planned  : {totalPlanned} hrs\n";
            result += $"Team Capacity  : {teamCapacity} hrs\n";
            result += $"Status         : {(totalPlanned <= teamCapacity ? "FEASIBLE ✓" : "OVER CAPACITY ✗")}\n";
            result += "==============================\n";
            return result;
        }
    }
}
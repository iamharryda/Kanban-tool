using System;
using System.Collections.Generic;
using AgileProjectManager.Model.DataAccess;
using AgileProjectManager.Model.Entities;

namespace AgileProjectManager.Controller
{
    public class UserStoryService
    {
        private readonly UserStoryRepository _userStoryRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly TaskRepository _taskRepository;
        private readonly PersonRepository _personRepository;

        public UserStoryService()
        {
            _userStoryRepository = new UserStoryRepository();
            _projectRepository = new ProjectRepository();
            _taskRepository = new TaskRepository();
            _personRepository = new PersonRepository();
        }

        public void AddUserStory(string storyID, string projectID, string content, string priorityLevel)
        {
            Project project = _projectRepository.GetProjectById(projectID);
            if (project == null)
            {
                throw new Exception("Project does not exist.");
            }

            UserStory existing = _userStoryRepository.GetUserStoryById(storyID);
            if (existing != null)
            {
                throw new Exception("User story ID already exists.");
            }

            UserStory story = new UserStory(
                storyID,
                projectID,
                content,
                priorityLevel,
                "project backlog"
            );

            _userStoryRepository.AddUserStory(story);
        }

        public List<UserStory> GetUserStoriesByProject(string projectID)
        {
            Project project = _projectRepository.GetProjectById(projectID);
            if (project == null)
            {
                throw new Exception("Project does not exist.");
            }

            return _userStoryRepository.GetUserStoriesByProject(projectID);
        }

        public List<UserStory> GetAllUserStories()
        {
            return _userStoryRepository.GetAllUserStories();
        }
        public void ChangeUserStoryState(string storyID, string newState)
        {
            UserStory story = _userStoryRepository.GetUserStoryById(storyID);
            if (story == null)
            {
                throw new Exception("User story does not exist.");
            }

            if (!IsValidUserStoryState(newState))
            {
                throw new Exception("Invalid user story state. Use: project backlog, in sprint, done.");
            }

            if (!IsValidUserStoryTransition(story.State, newState))
            {
                throw new Exception("Invalid user story state transition.");
            }

            if (story.State == "project backlog" && newState == "in sprint")
            {
                List<UserStory> dependencies = _userStoryRepository.GetDependencies(storyID);

                foreach (UserStory dependency in dependencies)
                {
                    if (dependency.State != "in sprint" && dependency.State != "done")
                    {
                        throw new Exception("Cannot move to sprint. One or more dependency stories are still in project backlog.");
                    }
                }

                List<TaskItem> tasks = _taskRepository.GetTasksByStoryId(storyID);

                foreach (TaskItem task in tasks)
                {
                    _taskRepository.UpdateTaskState(task.TaskID, "to be done");
                }
            }

            if (newState == "done")
            {
                List<TaskItem> tasks = _taskRepository.GetTasksByStoryId(storyID);

                foreach (TaskItem task in tasks)
                {
                    if (task.State != "done")
                    {
                        throw new Exception("Cannot move user story to done. All linked tasks must be done first.");
                    }
                }
            }

            _userStoryRepository.UpdateUserStoryState(storyID, newState);
        }

        private bool IsValidUserStoryState(string state)
        {
            return state == "project backlog" ||
                   state == "in sprint" ||
                   state == "done";
        }

        private bool IsValidUserStoryTransition(string currentState, string newState)
        {
            if (currentState == newState)
                return true;

            if (currentState == "project backlog" && newState == "in sprint")
                return true;

            if (currentState == "in sprint" && newState == "project backlog")
                return true;

            if (currentState == "in sprint" && newState == "done")
                return true;

            if (currentState == "done" && newState == "in sprint")
                return true;

            return false;
        }
        public void RemoveUserStory(string storyID)
        {
            UserStory story = _userStoryRepository.GetUserStoryById(storyID);
            if (story == null)
                throw new Exception("User story does not exist.");

            // Clear all person assignments from all tasks of this story
            _personRepository.RemoveAllPersonsFromTasksByStoryId(storyID);

            // Delete all tasks linked to this story
            _taskRepository.DeleteTasksByStoryId(storyID);

            // Delete the user story itself
            _userStoryRepository.DeleteUserStory(storyID);
        }
        public void UpdateUserStory(string storyID, string content, string priorityLevel)
        {
            UserStory story = _userStoryRepository.GetUserStoryById(storyID);
            if (story == null)
                throw new Exception("User story does not exist.");
            _userStoryRepository.UpdateUserStory(storyID, content, priorityLevel);
        }
    }
}
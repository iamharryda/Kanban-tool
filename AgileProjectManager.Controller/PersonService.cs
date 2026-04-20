using System;
using System.Collections.Generic;
using AgileProjectManager.Model.DataAccess;
using AgileProjectManager.Model.Entities;

namespace AgileProjectManager.Controller
{
    public class PersonService
    {
        private readonly PersonRepository _personRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly TaskRepository _taskRepository;
        private readonly UserStoryRepository _userStoryRepository;

        public PersonService()
        {
            _personRepository = new PersonRepository();
            _projectRepository = new ProjectRepository();
            _taskRepository = new TaskRepository();
            _userStoryRepository = new UserStoryRepository();
        }

        public void AddPerson(string personID, string name, string role)
        {
            Person existing = _personRepository.GetPersonById(personID);
            if (existing != null)
            {
                throw new Exception("Person ID already exists.");
            }

            Person person = new Person(personID, name, role);
            _personRepository.AddPerson(person);
        }

        public List<Person> GetAllPersons()
        {
            return _personRepository.GetAllPersons();
        }

        public void LinkPersonToProject(string projectID, string personID)
        {
            Project project = _projectRepository.GetProjectById(projectID);
            if (project == null)
            {
                throw new Exception("Project does not exist.");
            }

            Person person = _personRepository.GetPersonById(personID);
            if (person == null)
            {
                throw new Exception("Person does not exist.");
            }

            if (_personRepository.IsPersonLinkedToProject(projectID, personID))
            {
                throw new Exception("Person is already linked to this project.");
            }

            _personRepository.LinkPersonToProject(projectID, personID);
        }

        public void AssignPersonToTask(string taskID, string personID)
        {
            TaskItem task = _taskRepository.GetTaskById(taskID);
            if (task == null)
            {
                throw new Exception("Task does not exist.");
            }

            Person person = _personRepository.GetPersonById(personID);
            if (person == null)
            {
                throw new Exception("Person does not exist.");
            }

            UserStory story = _userStoryRepository.GetUserStoryById(task.StoryID);
            if (story == null)
            {
                throw new Exception("Related user story does not exist.");
            }

            if (!_personRepository.IsPersonLinkedToProject(story.ProjectID, personID))
            {
                throw new Exception("Person is not linked to the project of this task.");
            }

            if (_personRepository.IsPersonAssignedToTask(taskID, personID))
            {
                throw new Exception("Person is already assigned to this task.");
            }

            _personRepository.AssignPersonToTask(taskID, personID);
        }

        public void RemovePersonFromTask(string taskID, string personID)
        {
            TaskItem task = _taskRepository.GetTaskById(taskID);
            if (task == null)
            {
                throw new Exception("Task does not exist.");
            }

            Person person = _personRepository.GetPersonById(personID);
            if (person == null)
            {
                throw new Exception("Person does not exist.");
            }

            if (!_personRepository.IsPersonAssignedToTask(taskID, personID))
            {
                throw new Exception("Person is not assigned to this task.");
            }

            _personRepository.RemovePersonFromTask(taskID, personID);
        }
        public List<Person> GetPersonsByTaskId(string taskID)
        {
            TaskItem task = _taskRepository.GetTaskById(taskID);
            if (task == null)
            {
                throw new Exception("Task does not exist.");
            }

            return _personRepository.GetPersonsByTaskId(taskID);
        }
        public void UpdatePerson(string personID, string name, string role)
        {
            Person existing = _personRepository.GetPersonById(personID);
            if (existing == null)
                throw new Exception("Person does not exist.");
            _personRepository.UpdatePerson(personID, name, role);
        }

        public void DeletePerson(string personID)
        {
            Person existing = _personRepository.GetPersonById(personID);
            if (existing == null)
                throw new Exception("Person does not exist.");
            // Clear all references first
            _personRepository.RemovePersonFromAllTasks(personID);
            _personRepository.RemovePersonFromAllProjects(personID);
            _personRepository.DeletePerson(personID);
        }
    }
}
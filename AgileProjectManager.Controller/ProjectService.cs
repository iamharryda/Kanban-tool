using System.Collections.Generic;
using AgileProjectManager.Model.DataAccess;
using AgileProjectManager.Model.Entities;

namespace AgileProjectManager.Controller
{
    public class ProjectService
    {
        private readonly ProjectRepository _projectRepository;

        public ProjectService()
        {
            _projectRepository = new ProjectRepository();
        }

        public void AddProject(string projectID, string name, string description)
        {
            Project existing = _projectRepository.GetProjectById(projectID);
            if (existing != null)
            {
                throw new System.Exception("Project ID already exists.");
            }

            Project project = new Project(projectID, name, description);
            _projectRepository.AddProject(project);
        }

        public List<Project> GetAllProjects()
        {
            return _projectRepository.GetAllProjects();
        }
        public void UpdateProject(string projectID, string name, string description)
        {
            Project existing = _projectRepository.GetProjectById(projectID);
            if (existing == null)
                throw new Exception("Project does not exist.");
            _projectRepository.UpdateProject(projectID, name, description);
        }

        public void DeleteProject(string projectID)
        {
            Project existing = _projectRepository.GetProjectById(projectID);
            if (existing == null)
                throw new Exception("Project does not exist.");
            _projectRepository.DeleteProject(projectID);
        }
    }
}
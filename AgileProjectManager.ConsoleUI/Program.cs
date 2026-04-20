using System;
using AgileProjectManager.Controller;
using AgileProjectManager.Model.Entities;

namespace AgileProjectManager.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProjectService projectService = new ProjectService();
            UserStoryService userStoryService = new UserStoryService();
            TaskService taskService = new TaskService();
            PersonService personService = new PersonService();

            while (true)
            {
                PrintHeader("AGILE PROJECT MANAGER");

                PrintSection("PROJECT");
                PrintOption("1", "Add Project");
                PrintOption("2", "Show All Projects");
                PrintOption("20", "Edit Project");
                PrintOption("21", "Delete Project");

                PrintSection("USER STORY");
                PrintOption("3", "Add User Story");
                PrintOption("4", "Show User Stories By Project");
                PrintOption("5", "Show All User Stories");
                PrintOption("16", "Change User Story State");
                PrintOption("18", "Remove User Story");
                PrintOption("22", "Edit User Story");

                PrintSection("TASK");
                PrintOption("6", "Add Task");
                PrintOption("7", "Show Tasks By User Story");
                PrintOption("8", "Show All Tasks");
                PrintOption("14", "Change Task Priority");
                PrintOption("17", "Change Task State");
                PrintOption("19", "Task Report");
                PrintOption("23", "Edit Task");
                PrintOption("24", "Delete Task");
                PrintOption("25", "Check Sprint Feasibility");

                PrintSection("PERSON");
                PrintOption("9", "Add Person");
                PrintOption("10", "Show All Persons");
                PrintOption("11", "Link Person To Project");
                PrintOption("12", "Assign Person To Task");
                PrintOption("13", "Show Persons By Task");
                PrintOption("15", "Remove Person From Task");
                PrintOption("26", "Edit Person");
                PrintOption("27", "Delete Person");

                PrintDivider();
                PrintOption("0", "Exit");
                PrintDivider();

                Console.Write("  Choose: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddProject(projectService); break;
                        case "2": ShowAllProjects(projectService); break;
                        case "3": AddUserStory(userStoryService); break;
                        case "4": ShowUserStoriesByProject(userStoryService); break;
                        case "5": ShowAllUserStories(userStoryService); break;
                        case "6": AddTask(taskService); break;
                        case "7": ShowTasksByUserStory(taskService); break;
                        case "8": ShowAllTasks(taskService); break;
                        case "9": AddPerson(personService); break;
                        case "10": ShowAllPersons(personService); break;
                        case "11": LinkPersonToProject(personService); break;
                        case "12": AssignPersonToTask(personService); break;
                        case "13": ShowPersonsByTask(personService); break;
                        case "14": ChangeTaskPriority(taskService); break;
                        case "15": RemovePersonFromTask(personService); break;
                        case "16": ChangeUserStoryState(userStoryService); break;
                        case "17": ChangeTaskState(taskService); break;
                        case "18": RemoveUserStory(userStoryService); break;
                        case "19": TaskReport(taskService); break;
                        case "20": EditProject(projectService); break;
                        case "21": DeleteProject(projectService); break;
                        case "22": EditUserStory(userStoryService); break;
                        case "23": EditTask(taskService); break;
                        case "24": DeleteTask(taskService); break;
                        case "25": CheckSprintFeasibility(taskService); break;
                        case "26": EditPerson(personService); break;
                        case "27": DeletePerson(personService); break;
                        case "0": return;
                        default: PrintError("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    PrintError(ex.Message);
                }

                Console.WriteLine();
            }
        }

        // ─── Display Helpers ───────────────────────────────────────────
        static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine("  ╔══════════════════════════════════╗");
            Console.WriteLine($"  ║  {title,-34}║");
            Console.WriteLine("  ╚══════════════════════════════════╝");
        }

        static void PrintSection(string name)
        {
            Console.WriteLine($"\n  -- {name} --");
        }

        static void PrintOption(string number, string label)
        {
            Console.WriteLine($"  [{number,2}] {label}");
        }

        static void PrintDivider()
        {
            Console.WriteLine("  ──────────────────────────────────────");
        }

        static void PrintSuccess(string msg)
        {
            Console.WriteLine($"  ✓ {msg}");
        }

        static void PrintError(string msg)
        {
            Console.WriteLine($"  ✗ Error: {msg}");
        }

        // ─── Project ───────────────────────────────────────────────────
        static void AddProject(ProjectService projectService)
        {
            Console.Write("  Project ID   : "); string projectID = Console.ReadLine();
            Console.Write("  Name         : "); string name = Console.ReadLine();
            Console.Write("  Description  : "); string description = Console.ReadLine();
            projectService.AddProject(projectID, name, description);
            PrintSuccess("Project added.");
        }

        static void ShowAllProjects(ProjectService projectService)
        {
            var projects = projectService.GetAllProjects();
            if (projects.Count == 0) { Console.WriteLine("  No projects found."); return; }
            PrintDivider();
            Console.WriteLine($"  {"ID",-12} {"Name",-20} {"Description"}");
            PrintDivider();
            foreach (Project p in projects)
                Console.WriteLine($"  {p.ProjectID,-12} {p.Name,-20} {p.Description}");
            PrintDivider();
        }

        static void EditProject(ProjectService projectService)
        {
            Console.Write("  Project ID   : "); string projectID = Console.ReadLine();
            Console.Write("  New Name     : "); string name = Console.ReadLine();
            Console.Write("  New Desc     : "); string description = Console.ReadLine();
            projectService.UpdateProject(projectID, name, description);
            PrintSuccess("Project updated.");
        }

        static void DeleteProject(ProjectService projectService)
        {
            Console.Write("  Project ID   : "); string projectID = Console.ReadLine();
            projectService.DeleteProject(projectID);
            PrintSuccess("Project deleted.");
        }

        // ─── User Story ────────────────────────────────────────────────
        static void AddUserStory(UserStoryService userStoryService)
        {
            Console.Write("  Story ID     : "); string storyID = Console.ReadLine();
            Console.Write("  Project ID   : "); string projectID = Console.ReadLine();
            Console.Write("  Content      : "); string content = Console.ReadLine();
            Console.Write("  Priority     : "); string priorityLevel = Console.ReadLine();
            userStoryService.AddUserStory(storyID, projectID, content, priorityLevel);
            PrintSuccess("User story added.");
        }

        static void ShowUserStoriesByProject(UserStoryService userStoryService)
        {
            Console.Write("  Project ID   : "); string projectID = Console.ReadLine();
            var stories = userStoryService.GetUserStoriesByProject(projectID);
            if (stories.Count == 0) { Console.WriteLine("  No user stories found."); return; }
            PrintDivider();
            Console.WriteLine($"  {"StoryID",-10} {"Priority",-10} {"State",-16} {"Content"}");
            PrintDivider();
            foreach (UserStory s in stories)
                Console.WriteLine($"  {s.StoryID,-10} {s.PriorityLevel,-10} {s.State,-16} {s.Content}");
            PrintDivider();
        }

        static void ShowAllUserStories(UserStoryService userStoryService)
        {
            var stories = userStoryService.GetAllUserStories();
            if (stories.Count == 0) { Console.WriteLine("  No user stories found."); return; }
            PrintDivider();
            Console.WriteLine($"  {"StoryID",-10} {"ProjectID",-12} {"Priority",-10} {"State",-16} {"Content"}");
            PrintDivider();
            foreach (UserStory s in stories)
                Console.WriteLine($"  {s.StoryID,-10} {s.ProjectID,-12} {s.PriorityLevel,-10} {s.State,-16} {s.Content}");
            PrintDivider();
        }

        static void ChangeUserStoryState(UserStoryService userStoryService)
        {
            Console.Write("  Story ID     : "); string storyID = Console.ReadLine();
            Console.WriteLine("  States: project backlog | in sprint | done");
            Console.Write("  New State    : "); string newState = Console.ReadLine();
            userStoryService.ChangeUserStoryState(storyID, newState);
            PrintSuccess("User story state changed.");
        }

        static void RemoveUserStory(UserStoryService userStoryService)
        {
            Console.Write("  Story ID     : "); string storyID = Console.ReadLine();
            userStoryService.RemoveUserStory(storyID);
            PrintSuccess("User story and all linked tasks removed.");
        }

        static void EditUserStory(UserStoryService userStoryService)
        {
            Console.Write("  Story ID     : "); string storyID = Console.ReadLine();
            Console.Write("  New Content  : "); string content = Console.ReadLine();
            Console.Write("  New Priority : "); string priority = Console.ReadLine();
            userStoryService.UpdateUserStory(storyID, content, priority);
            PrintSuccess("User story updated.");
        }

        // ─── Task ──────────────────────────────────────────────────────
        static void AddTask(TaskService taskService)
        {
            Console.Write("  Task ID      : "); string taskID = Console.ReadLine();
            Console.Write("  Story ID     : "); string storyID = Console.ReadLine();
            Console.Write("  Priority     : "); string priority = Console.ReadLine();
            Console.Write("  Planned Time : "); double plannedTime = Convert.ToDouble(Console.ReadLine());
            Console.Write("  Difficulty   : "); string difficultyLevel = Console.ReadLine();
            Console.Write("  Category     : "); string categoryLabels = Console.ReadLine();
            taskService.AddTask(taskID, storyID, priority, plannedTime, difficultyLevel, categoryLabels);
            PrintSuccess("Task added.");
        }

        static void ShowTasksByUserStory(TaskService taskService)
        {
            Console.Write("  Story ID     : "); string storyID = Console.ReadLine();
            var tasks = taskService.GetTasksByStoryId(storyID);
            if (tasks.Count == 0) { Console.WriteLine("  No tasks found."); return; }
            PrintDivider();
            Console.WriteLine($"  {"TaskID",-10} {"Priority",-10} {"State",-14} {"Planned",-10} {"Actual"}");
            PrintDivider();
            foreach (TaskItem t in tasks)
                Console.WriteLine($"  {t.TaskID,-10} {t.Priority,-10} {t.State,-14} {t.PlannedTime,-10} {t.ActualTime}");
            PrintDivider();
        }

        static void ShowAllTasks(TaskService taskService)
        {
            var tasks = taskService.GetAllTasks();
            if (tasks.Count == 0) { Console.WriteLine("  No tasks found."); return; }
            PrintDivider();
            Console.WriteLine($"  {"TaskID",-10} {"StoryID",-10} {"Priority",-10} {"State",-14} {"Planned",-10} {"Actual"}");
            PrintDivider();
            foreach (TaskItem t in tasks)
                Console.WriteLine($"  {t.TaskID,-10} {t.StoryID,-10} {t.Priority,-10} {t.State,-14} {t.PlannedTime,-10} {t.ActualTime}");
            PrintDivider();
        }

        static void ChangeTaskPriority(TaskService taskService)
        {
            Console.Write("  Task ID      : "); string taskID = Console.ReadLine();
            Console.Write("  New Priority : "); string newPriority = Console.ReadLine();
            taskService.ChangeTaskPriority(taskID, newPriority);
            PrintSuccess("Task priority changed.");
        }

        static void ChangeTaskState(TaskService taskService)
        {
            Console.Write("  Task ID      : "); string taskID = Console.ReadLine();
            Console.WriteLine("  States: to be done | in process | done");
            Console.Write("  New State    : "); string newState = Console.ReadLine();
            taskService.ChangeTaskState(taskID, newState);
            PrintSuccess("Task state changed.");
        }

        static void TaskReport(TaskService taskService)
        {
            Console.Write("  Task ID      : "); string taskID = Console.ReadLine();
            string report = taskService.GenerateTaskReport(taskID);
            Console.WriteLine(report);
        }

        static void EditTask(TaskService taskService)
        {
            Console.Write("  Task ID      : "); string taskID = Console.ReadLine();
            Console.Write("  New Priority : "); string priority = Console.ReadLine();
            Console.Write("  Planned Time : "); double plannedTime = Convert.ToDouble(Console.ReadLine());
            Console.Write("  Actual Time  : "); double actualTime = Convert.ToDouble(Console.ReadLine());
            Console.Write("  Difficulty   : "); string difficulty = Console.ReadLine();
            Console.Write("  Category     : "); string category = Console.ReadLine();
            taskService.UpdateTask(taskID, priority, plannedTime, actualTime, difficulty, category);
            PrintSuccess("Task updated.");
        }

        static void DeleteTask(TaskService taskService)
        {
            Console.Write("  Task ID      : "); string taskID = Console.ReadLine();
            taskService.DeleteTask(taskID);
            PrintSuccess("Task deleted.");
        }

        static void CheckSprintFeasibility(TaskService taskService)
        {
            Console.Write("  Story ID     : "); string storyID = Console.ReadLine();
            Console.Write("  Team Capacity (hrs): "); double capacity = Convert.ToDouble(Console.ReadLine());
            string result = taskService.CheckSprintFeasibility(storyID, capacity);
            Console.WriteLine(result);
        }

        // ─── Person ────────────────────────────────────────────────────
        static void AddPerson(PersonService personService)
        {
            Console.Write("  Person ID    : "); string personID = Console.ReadLine();
            Console.Write("  Name         : "); string name = Console.ReadLine();
            Console.Write("  Role         : "); string role = Console.ReadLine();
            personService.AddPerson(personID, name, role);
            PrintSuccess("Person added.");
        }

        static void ShowAllPersons(PersonService personService)
        {
            var persons = personService.GetAllPersons();
            if (persons.Count == 0) { Console.WriteLine("  No persons found."); return; }
            PrintDivider();
            Console.WriteLine($"  {"PersonID",-12} {"Name",-20} {"Role"}");
            PrintDivider();
            foreach (Person p in persons)
                Console.WriteLine($"  {p.PersonID,-12} {p.Name,-20} {p.Role}");
            PrintDivider();
        }

        static void LinkPersonToProject(PersonService personService)
        {
            Console.Write("  Project ID   : "); string projectID = Console.ReadLine();
            Console.Write("  Person ID    : "); string personID = Console.ReadLine();
            personService.LinkPersonToProject(projectID, personID);
            PrintSuccess("Person linked to project.");
        }

        static void AssignPersonToTask(PersonService personService)
        {
            Console.Write("  Task ID      : "); string taskID = Console.ReadLine();
            Console.Write("  Person ID    : "); string personID = Console.ReadLine();
            personService.AssignPersonToTask(taskID, personID);
            PrintSuccess("Person assigned to task.");
        }

        static void ShowPersonsByTask(PersonService personService)
        {
            Console.Write("  Task ID      : "); string taskID = Console.ReadLine();
            var persons = personService.GetPersonsByTaskId(taskID);
            if (persons.Count == 0) { Console.WriteLine("  No persons assigned."); return; }
            PrintDivider();
            Console.WriteLine($"  {"PersonID",-12} {"Name",-20} {"Role"}");
            PrintDivider();
            foreach (Person p in persons)
                Console.WriteLine($"  {p.PersonID,-12} {p.Name,-20} {p.Role}");
            PrintDivider();
        }

        static void RemovePersonFromTask(PersonService personService)
        {
            Console.Write("  Task ID      : "); string taskID = Console.ReadLine();
            Console.Write("  Person ID    : "); string personID = Console.ReadLine();
            personService.RemovePersonFromTask(taskID, personID);
            PrintSuccess("Person removed from task.");
        }

        static void EditPerson(PersonService personService)
        {
            Console.Write("  Person ID    : "); string personID = Console.ReadLine();
            Console.Write("  New Name     : "); string name = Console.ReadLine();
            Console.Write("  New Role     : "); string role = Console.ReadLine();
            personService.UpdatePerson(personID, name, role);
            PrintSuccess("Person updated.");
        }

        static void DeletePerson(PersonService personService)
        {
            Console.Write("  Person ID    : "); string personID = Console.ReadLine();
            personService.DeletePerson(personID);
            PrintSuccess("Person deleted (removed from all tasks and projects).");
        }
    }
}
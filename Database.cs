using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Console;

namespace Cyberawarenesbot
{
    internal class Database
    {
        public class TaskDatabase
        {
            private readonly string _filePath;
            private List<Task> _tasks;

            public TaskDatabase(string filePath = "tasks.json")
            {
                _filePath = filePath;
                LoadTasks();
            }

            [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Deserialize<TValue>(String, JsonSerializerOptions)")]
            private void LoadTasks()
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _tasks = JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
                }
                else
                {
                    _tasks = new List<Task>();
                }
            }

            [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Serialize<TValue>(TValue, JsonSerializerOptions)")]
            private void SaveTasks()
            {
                string json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }

            public void AddTask(Task task)
            {
                _tasks.Add(task);
                SaveTasks();
                WriteLine($"Task '{task.Title}' added successfully.");
            }

            public List<Task> GetAllTasks()
            {
                return new List<Task>(_tasks); 
            }

            public void DeleteTask(Guid taskId)
            {
                int initialCount = _tasks.Count;
                _tasks.RemoveAll(t => t.Id == taskId);
                if (_tasks.Count < initialCount)
                {
                    SaveTasks();
                    WriteLine($"Task with ID '{taskId}' deleted successfully.");
                }
                else
                {
                    WriteLine($"Task with ID '{taskId}' not found.");
                }
            }

            public void MarkTaskAsCompleted(Guid taskId)
            {
                Task taskToUpdate = _tasks.Find(t => t.Id == taskId);
                if (taskToUpdate != null)
                {
                    taskToUpdate.IsCompleted = true;
                    SaveTasks();
                    WriteLine(value: $"Task '{taskToUpdate.Title}' marked as completed.");
                }
                else
                {
                    WriteLine($"Task with ID '{taskId}' not found.");
                }
            }
        }

        public class Program
        {
            public static void Main(string[] args)
            {
                TaskDatabase db = new TaskDatabase();

                
                db.AddTask(new Task
                {
                    Title = "Set up two factor authentication",
                    Description = "Enable 2FA on all critical accounts.",
                    Reminder = DateTime.Now.AddDays(7)
                });

                db.AddTask(new Task
                {
                    Title = "Review account privacy settings",
                    Description = "Check privacy settings on social media and email accounts.",
                    Reminder = DateTime.Now.AddDays(14)
                });

                db.AddTask(new Task
                {
                    Title = "Update passwords",
                    Description = "Change passwords for less secure accounts."
                });

                
                WriteLine("\n--- All Tasks ---");
                foreach (var task in db.GetAllTasks())
                {
                   WriteLine($"ID: {task.Id}\nTitle: {task.Title}\nDescription: {task.Description}\nReminder: {(task.Reminder.HasValue ? task.Reminder.Value.ToShortDateString() : "None")}\nCompleted: {task.IsCompleted}\n");
                }

                
                if (db.GetAllTasks().Count > 0)
                {
                    Guid firstTaskId = db.GetAllTasks()[0].Id;
                    db.MarkTaskAsCompleted(firstTaskId);
                }


                if (db.GetAllTasks().Count > 0)
                {
                    Guid lastTaskId = db.GetAllTasks()[db.GetAllTasks().Count - 1].Id;
                    db.DeleteTask(lastTaskId);
                }

                WriteLine("\n--- Tasks After Operations ---");
                foreach (var task in db.GetAllTasks())
                {
                    WriteLine($"ID: {task.Id}\nTitle: {task.Title}\nDescription: {task.Description}\nReminder: {(task.Reminder.HasValue ? task.Reminder.Value.ToShortDateString() : "None")}\nCompleted: {task.IsCompleted}\n");
                }
            }
        }
    }
}


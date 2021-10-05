using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            Console.WriteLine(IncreaseSalaries(context));            

        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Where(x =>
                    x.Department.Name == "Engineering" ||
                    x.Department.Name == "Tool Design" ||
                    x.Department.Name == "Marketing" ||
                    x.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName);
            foreach (var employee in employees.ToList())
            {
                Console.Write("Before "+employee.Salary);
                employee.Salary += employee.Salary * 12 / 100;
                Console.WriteLine(" After "+employee.Salary);
            }

            context.SaveChanges();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees.ToList())
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            var latestProjects = context
                .Projects
                .OrderByDescending(x => x.StartDate)
                .Select(x => new
                {
                    ProjectName=x.Name,
                    ProjectDesc=x.Description,
                    ProjectStart=x.StartDate
                })
                .Take(10)                
                .OrderBy(x=>x.ProjectName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var project in latestProjects)
            {
                sb.AppendLine(project.ProjectName);
                sb.AppendLine(project.ProjectDesc);
                sb.AppendLine(project.ProjectStart.ToString("M/d/yyyy h:mm:ss tt"));
            }
            
            return sb.ToString().TrimEnd();
        }       
    }


}

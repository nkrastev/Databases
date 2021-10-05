using SoftUni.Data;
using SoftUni.Models;
using System;
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
            using (context)
            {                
                Console.WriteLine(GetLatestProjects(context));
            }

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

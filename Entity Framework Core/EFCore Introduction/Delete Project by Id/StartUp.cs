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
            Console.WriteLine(DeleteProjectById(context));            

        }
        public static string DeleteProjectById(SoftUniContext context)
        {            
            //delete references before removing from project            
            foreach (var item in context.EmployeesProjects)
            {
                if (item.ProjectId==2)
                {
                    context.EmployeesProjects.Remove(item);
                }
            }
            context.SaveChanges();

            //Find > gets the entity with specific primary key
            var project = context.Projects.Find(2);
            context.Projects.Remove(project);
            context.SaveChanges();

            var projects = context.Projects.ToList();           
            StringBuilder sb = new StringBuilder();
            foreach (var item in projects)
            {
                sb.AppendLine($"{item.Name}");
            }

            return sb.ToString().TrimEnd();
        }
     
    }


}

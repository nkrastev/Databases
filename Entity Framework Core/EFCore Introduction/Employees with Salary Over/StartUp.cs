using SoftUni.Data;
using System;
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
                //Task 3
                //Console.WriteLine(GetEmployeesFullInformation(context));

                //Task 4
                Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
            }                   

        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees.OrderBy(x => x.EmployeeId).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} {item.MiddleName} {item.JobTitle} {item.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees.Where(x=>x.Salary>50000).OrderBy(x=>x.FirstName).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} - {item.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }
    }

    
}

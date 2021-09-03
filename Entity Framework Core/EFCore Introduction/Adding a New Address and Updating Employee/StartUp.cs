using SoftUni.Data;
using SoftUni.Models;
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
                //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));

                //Task 5
                //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));

                //Task 6
                Console.WriteLine(AddNewAddressToEmployee(context));
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

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {            
            var employees = context.Employees
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Department,
                    x.Salary
                })
                .Where(x => x.Department.Name == "Research and Development")
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} from {item.Department.Name} - ${item.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var theAddress = new Address();
            theAddress.TownId = 4;
            theAddress.AddressText = "Vitoshka 15";

            context.Addresses.Add(theAddress);
            context.SaveChanges();

            var employee = context
                .Employees               
                .Where(x=>x.LastName=="Nakov")
                .First();

            //TODO if there are more than 1 Nakov???           

            employee.Address = theAddress;
            context.SaveChanges();
            

            var employees = context
                .Employees
                .OrderByDescending(x => x.AddressId)                
                .Take(10)
                .Select(x => new { x.Address.AddressText, x.AddressId })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.AddressText}");
            }
            
            return sb.ToString().TrimEnd();            
        }

        
    }

    
}

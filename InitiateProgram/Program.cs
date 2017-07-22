using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace InitiateProgram
{
    class Program
    {
        private static List<Student> listOfStudents;
        private static DirectoryInfo directory = new DirectoryInfo(@"d:\StudentData");
        private static StreamWriter logWriter;

        static void Main(string[] args)
        {
            if (directory.Exists) Console.WriteLine("Directory already exists.");
            else
            {
                directory.Create();
                Console.WriteLine("New directory created.");
            }

            listOfStudents = new List<Student>();
            InitializeListOfStudents();

            int choice = 0;
            while (choice != -1)
            {
                Console.WriteLine("What do you want to do?\n1.Add a new student\n2.View all records\n3.View a record" +
                    "\n4.Update a record\n5.View log");
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        if (AddStudent())
                        {
                            Console.WriteLine("Student added successfully.");
                        }
                        break;
                    case 2:
                        foreach (var student in listOfStudents)
                        {
                            Console.WriteLine(student.ToString());
                        }
                        logWriter = File.AppendText(@"d:\StudentData\logs.log");
                        logWriter.WriteLine("{0}:Displayed all students information.", DateTime.Now.ToString());
                        logWriter.Close();
                        break;
                    case 3:
                        while (true)
                        {
                            Console.Write("Enter Student's full name: ");
                            string fullName = Console.ReadLine();
                            if (!Regex.IsMatch(fullName, "^[a-zA-Z]+ [a-zA-Z]$"))
                            {
                                Console.WriteLine("Invalid name.");
                                continue;
                            }
                            foreach (var student in listOfStudents)
                            {
                                if (student.GetFullName().Equals(fullName))
                                {
                                    Console.WriteLine(student.ToString());
                                }
                                break;
                            }
                            Console.WriteLine("Student not found.");
                            logWriter = File.AppendText(@"d:\StudentData\logs.log");
                            logWriter.WriteLine("{1}:Tried to access records of student: {0}", fullName, DateTime.Now.ToString());
                            logWriter.Close();
                            break;
                        }
                        break;
                    case 4:
                        while (true)
                        {
                            Console.Write("Enter Student's full name: ");
                            string fullName = Console.ReadLine();
                            if (!Regex.IsMatch(fullName, "^[a-zA-Z]+ [a-zA-Z]$"))
                            {
                                Console.WriteLine("Invalid name.");
                                continue;
                            }
                            bool found = false;
                            foreach (var student in listOfStudents)
                            {
                                if (student.GetFullName().Equals(fullName))
                                {
                                    DeleteStudent(fullName);
                                    listOfStudents.Remove(student);
                                    if (AddStudent())
                                    {
                                        Console.WriteLine("Student Updated successfully.");
                                    }
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                Console.WriteLine("Student not found.");
                            logWriter = File.AppendText(@"d:\StudentData\logs.log");
                            logWriter.WriteLine("{1}:Updated records of student: {0}", fullName, DateTime.Now.ToString());
                            logWriter.Close();
                            break;
                        }
                        break;
                    case 5:
                        using (StreamReader sr = new StreamReader(@"d:\StudentData\logs.log"))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                Console.WriteLine(line);
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

            }

            logWriter.Close();

        }

        private static void DeleteStudent(string fullName)
        {
            File.Delete(@"d:\StudentData\" + fullName);
            logWriter = File.AppendText(@"d:\StudentData\logs.log");
            logWriter.WriteLine("{1}:Deleted file of student: {0}", fullName, DateTime.Now.ToString());
            logWriter.Close();
        }

        private static bool AddStudent()
        {
            Student student = new Student();
            while (true)
            {
                Console.Write("Enter student's full name: ");
                string fullName = Console.ReadLine();
                if (!Regex.IsMatch(fullName, "^[a-zA-Z]+ [a-zA-Z]+$"))
                {
                    Console.WriteLine("Invalid name.");
                    continue;
                }
                student.FirstName = fullName.Split()[0];
                student.LastName = fullName.Split()[1];
                break;
            }
            foreach (var s in listOfStudents)
            {
                if (s.GetFullName().Equals(student.GetFullName()))
                {
                    Console.WriteLine("Student already present.");
                    return false;
                }
            }
            while (true)
            {
                Console.Write("Enter student's mobile Number: ");
                string mobileNo = Console.ReadLine();
                if (!Regex.IsMatch(mobileNo, "^[0-9]{5,10}$"))
                {
                    Console.WriteLine("Invalid mobile number.");
                    continue;
                }
                student.MobileNo = mobileNo;
                break;
            }
            while (true)
            {
                Console.Write("Enter student's e-mail id: ");
                string mail = Console.ReadLine();
                if (!Regex.IsMatch(mail, @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-z]{2,3}$"))
                {
                    Console.WriteLine("Invalid e-mail id.");
                    continue;
                }
                student.EmailId = mail;
                break;
            }
            Console.Write("Enter student's address: ");
            student.Address = Console.ReadLine();
            while (true)
            {
                Console.Write("Enter student's DOB (ddMMyyyy)");
                string dob = Console.ReadLine();
                if (!Regex.IsMatch(dob, @"^[0-3][0-9][01][0-9][12][0-9]{3}$"))
                {
                    Console.WriteLine("Invalid date of birth");
                    continue;
                }
                student.DateOfBirth = DateTime.ParseExact(dob, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                break;
            }
            Console.Write("Enter student's course: ");
            student.CourseTitle = Console.ReadLine();
            while (true)
            {
                Console.Write("Enter student's mentor's name: ");
                string mentor = Console.ReadLine();
                if (!Regex.IsMatch(mentor, "^[a-zA-Z]+ [a-zA-Z]+$"))
                {
                    Console.WriteLine("Invalid Name.");
                    continue;
                }
                student.MentorName = mentor;
                break;
            }
            while (true)
            {
                Console.Write("Enter student's emergency contact number: ");
                string mobileNo = Console.ReadLine();
                if (!Regex.IsMatch(mobileNo, "^[0-9]{5,10}$"))
                {
                    Console.WriteLine("Invalid mobile number.");
                    continue;
                }
                student.EmergencyCotact = mobileNo;
                break;
            }

            using (StreamWriter sw = new StreamWriter(@"d:\StudentData\" + student.GetFullName() + ".txt"))
            {
                sw.WriteLine(student.FirstName);
                sw.WriteLine(student.LastName);
                sw.WriteLine(student.MobileNo);
                sw.WriteLine(student.EmailId);
                sw.WriteLine(student.Address);
                sw.WriteLine(student.DateOfBirth.ToString("ddMMyyyy"));
                sw.WriteLine(student.CourseTitle);
                sw.WriteLine(student.MentorName);
                sw.WriteLine(student.EmergencyCotact);
            }

            listOfStudents.Add(student);
            logWriter = File.AppendText(@"d:\StudentData\logs.log");
            logWriter.WriteLine("{1}:Added a student - {0}", student.GetFullName(), DateTime.Now.ToString());
            logWriter.Close();
            return true;
        }

        private static void InitializeListOfStudents()
        {
            FileInfo[] files = directory.GetFiles("*.txt", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(@"d:\StudentData\" + file.Name))
                {
                    Student student = new Student();
                    student.FirstName = sr.ReadLine();
                    student.LastName = sr.ReadLine();
                    student.MobileNo = sr.ReadLine();
                    student.EmailId = sr.ReadLine();
                    student.Address = sr.ReadLine();
                    student.DateOfBirth = DateTime.ParseExact(sr.ReadLine(), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                    student.CourseTitle = sr.ReadLine();
                    student.MentorName = sr.ReadLine();
                    student.EmergencyCotact = sr.ReadLine();
                    listOfStudents.Add(student);
                }
            }
        }
    }
}

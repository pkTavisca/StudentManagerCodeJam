using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace InitiateProgram
{
    class Program
    {
        private static List<Student> listOfStudents;
        private static string directoryPath = @"c:\StudentData\";
        private static DirectoryInfo directory = new DirectoryInfo(directoryPath);
        private static StreamWriter logWriter;
        private static string logPath = directoryPath + "logs.log";

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
                Console.WriteLine("\nWhat do you want to do?\n1.Add a new student\n2.View all records\n3.View a record" +
                    "\n4.Update a record\n5.View log");
                int.TryParse(Console.ReadLine(), out choice);
                Console.WriteLine();
                switch (choice)
                {
                    //adding a student
                    case 1:
                        if (AddStudent())
                        {
                            Console.WriteLine("Student added successfully.");
                        }
                        break;

                    //displaying all records
                    case 2:
                        foreach (var student in listOfStudents)
                        {
                            Console.WriteLine("\n" + student.ToString());
                        }

                        WriteToLog("\nDisplayed all students information.");
                        break;

                    //displaying a particular record
                    case 3:
                        while (true)
                        {
                            Console.Write("Enter Student's full name: ");
                            string fullName = Console.ReadLine();
                            if (!Student.IsNameValid(fullName))
                            {
                                Console.WriteLine("Invalid name.");
                                continue;
                            }
                            bool found = false;
                            foreach (var student in listOfStudents)
                            {
                                if (student.GetFullName().Equals(fullName))
                                {
                                    Console.WriteLine(student.ToString());
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                Console.WriteLine("Student not found.");
                            WriteToLog(string.Format("Tried to access records of student: {0}. {1}", fullName, found ? "Success" : "Failure"));
                            break;
                        }
                        break;

                    //updating a student's record
                    case 4:
                        while (true)
                        {
                            Console.Write("Enter Student's full name: ");
                            string fullName = Console.ReadLine();
                            if (!Student.IsNameValid(fullName))
                            {
                                Console.WriteLine("Invalid name.");
                                continue;
                            }
                            bool found = false;
                            foreach (var student in listOfStudents)
                            {
                                if (student.GetFullName().Equals(fullName))
                                {
                                    if (UpdateStudent(student))
                                    {
                                        Console.WriteLine("Student Updated successfully.");
                                    }
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                Console.WriteLine("Student not found.");
                            break;
                        }
                        break;

                    //displying all logs
                    case 5:
                        using (StreamReader sr = new StreamReader(logPath))
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

        private static bool UpdateStudent(Student student)
        {
            int choice = -1;
            while (choice != 0)
            {
                Console.WriteLine("What do you want to update? Enter 0 to Exit.\n1.Name\n2.Mobile Number\n" +
                    "3.Email Id\n4.Address\n5.Date of Birth" +
                    "\n6.Course Tile\n7.Mentor's Name\n8.Emergency Contact");
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        string prevName = student.GetFullName();
                        string fullName = Console.ReadLine();
                        if (Student.IsNameValid(fullName))
                        {
                            DeleteStudent(student);
                            student.FirstName = fullName.Split(' ')[0];
                            student.LastName = fullName.Split(' ')[1];
                            WriteStudentToFile(student);
                            WriteToLog(string.Format("Updated name of {0} to {1}", prevName, fullName));
                            Console.WriteLine("Updated name of {0} to {1}", prevName, fullName);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Name.");
                            continue;
                        }
                        break;
                    case 2:
                        string prevMob = student.MobileNo;
                        string mobile = Console.ReadLine();
                        if (Student.IsMobileNoValid(mobile))
                        {
                            student.MobileNo = mobile;
                            WriteStudentToFile(student);
                            WriteToLog(string.Format("Updated mobile number from {0} to {1}", prevMob, mobile));
                            Console.WriteLine("Updated mobile number from {0} to {1}", prevMob, mobile);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Mobile Number.");
                            continue;
                        }
                        break;
                    case 3:
                        string prevEmail = student.EmailId;
                        string email = Console.ReadLine();
                        if (Student.IsEmailValid(email))
                        {
                            student.EmailId = email;
                            WriteStudentToFile(student);
                            WriteToLog(string.Format("Updated email from {0} to {1}", prevEmail, email));
                            Console.WriteLine("Updated email from {0} to {1}", prevEmail, email);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Email.");
                            continue;
                        }
                        break;
                    case 4:
                        string prevAddress = student.Address;
                        string address = Console.ReadLine();
                        student.Address = address;
                        WriteStudentToFile(student);
                        WriteToLog(string.Format("Updated address from {0} to {1}", prevAddress, address));
                        Console.WriteLine("Updated address from {0} to {1}", prevAddress, address);
                        break;
                    case 5:
                        DateTime prevDob = student.DateOfBirth;
                        string dob = Console.ReadLine();
                        if (Student.IsDateValid(dob))
                        {
                            student.DateOfBirth = DateTime.ParseExact(dob, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                            WriteStudentToFile(student);
                            WriteToLog(string.Format("Updated date of birth from {0} to {1}", prevDob, dob));
                            Console.WriteLine("Updated date of birth from {0} to {1}", prevDob, dob);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Date of Birth.");
                            continue;
                        }
                        break;
                    case 6:
                        string prevCourse = student.CourseTitle;
                        string newCourse = Console.ReadLine();
                        student.CourseTitle = newCourse;
                        WriteStudentToFile(student);
                        WriteToLog(string.Format("Updated course from {0} to {1}", prevCourse, newCourse));
                        Console.WriteLine("Updated course from {0} to {1}", prevCourse, newCourse);
                        break;
                    case 7:
                        string prevMentor = student.MentorName;
                        string mentor = Console.ReadLine();
                        if (Student.IsNameValid(mentor))
                        {
                            student.MentorName = mentor;
                            WriteStudentToFile(student);
                            WriteToLog(string.Format("Updated name of mentor from {0} to {1}", prevMentor, mentor));
                            Console.WriteLine("Updated name of mentor from {0} to {1}", prevMentor, mentor);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Mentor's Name.");
                            continue;
                        }
                        break;
                    case 8:
                        string prevEmerNo = student.EmergencyCotact;
                        string emerNo = Console.ReadLine();
                        if (Student.IsMobileNoValid(emerNo))
                        {
                            student.EmergencyCotact = emerNo;
                            WriteStudentToFile(student);
                            WriteToLog(string.Format("Updated emergency contact from {0} to {1}", prevEmerNo, emerNo));
                            Console.WriteLine("Updated emergency contact from {0} to {1}", prevEmerNo, emerNo);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Mobile Number.");
                            continue;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid Choice.");
                        break;
                }
            }

            return true;
        }

        private static void DeleteStudent(Student student)
        {
            File.Delete(directoryPath + student.GetFullName() + ".txt");
            WriteToLog(string.Format("Deleted record of {0}",student.GetFullName()));
        }

        private static bool AddStudent()
        {
            Student student = new Student();
            while (true)
            {
                Console.Write("Enter student's full name: ");
                string fullName = Console.ReadLine();
                if (!Student.IsNameValid(fullName))
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
                if (!Student.IsMobileNoValid(mobileNo))
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
                if (!Student.IsEmailValid(mail))
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
                if (!Student.IsDateValid(dob))
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
                if (!Student.IsNameValid(mentor))
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
                if (!Student.IsMobileNoValid(mobileNo))
                {
                    Console.WriteLine("Invalid mobile number.");
                    continue;
                }
                student.EmergencyCotact = mobileNo;
                break;
            }

            WriteStudentToFile(student);

            listOfStudents.Add(student);
            WriteToLog(string.Format("Added a student - {0}", student.GetFullName()));

            return true;
        }

        private static void WriteStudentToFile(Student student)
        {
            File.WriteAllText(directoryPath + student.GetFullName() + ".txt", JsonConvert.SerializeObject(student));
        }

        private static void InitializeListOfStudents()
        {
            FileInfo[] files = directory.GetFiles("*.txt", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    Student student = new Student();
                    string contents = sr.ReadLine();
                    student = JsonConvert.DeserializeObject<Student>(contents);
                    listOfStudents.Add(student);
                }
            }
        }

        private static void WriteToLog(string log)
        {
            using (logWriter = File.AppendText(logPath))
            {
                logWriter.WriteLine("{0}:{1}", DateTime.Now.ToString(), log);
            }
        }
    }
}

using System;
using System.Text.RegularExpressions;

namespace InitiateProgram
{
    class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CourseTitle { get; set; }
        public string MentorName { get; set; }
        public string EmergencyCotact { get; set; }

        public string GetFullName()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }

        override
        public string ToString()
        {
            return string.Format("Name: {0}\tMobile:{1}\tEmail:{2}\tAddress:{3}\tDOB:{4}\tCourse:{5}\tMentor:{6}\tEmergency Contact:{7}",
                GetFullName(), MobileNo, EmailId, Address, DateOfBirth.ToString("ddMMyyyy"), CourseTitle, MentorName, EmergencyCotact);
        }

        public static bool IsNameValid(string name)
        {
            if (Regex.IsMatch(name, "^[a-zA-Z]+ [a-zA-Z]+$"))
                return true;
            return false;
        }

        public static bool IsMobileNoValid(string mobileNo)
        {
            if (Regex.IsMatch(mobileNo, "^[0-9]{5,10}$"))
                return true;
            return false;
        }

        public static bool IsEmailValid(string email)
        {
            if (Regex.IsMatch(email, @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-z]{2,3}$"))
                return true;
            return false;
        }

        public static bool IsDateValid(string dob)
        {
            if (!Regex.IsMatch(dob, @"^[0-3][0-9][01][0-9][12][0-9]{3}$"))
                return false;
            try
            {
                var dateTime = DateTime.ParseExact(dob, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}

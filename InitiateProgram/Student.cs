using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

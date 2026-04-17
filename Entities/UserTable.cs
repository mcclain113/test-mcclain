using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class UserTable
    {
        public UserTable()
        {
            InverseInstructor = new HashSet<UserTable>();
            UserFacilities = new HashSet<UserFacility>();
            UserPrograms = new HashSet<UserProgram>();
            UserSecurityQuestions = new HashSet<UserSecurityQuestion>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProgramEnrolledIn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LastModified { get; set; }
        public string AspNetUsersId { get; set; }
        public int? InstructorId { get; set; }

        public virtual AspNetUser AspNetUsers { get; set; }
        public virtual UserTable Instructor { get; set; }
        public virtual ICollection<UserTable> InverseInstructor { get; set; }
        public virtual ICollection<Request> RequestCompletedByNavigations { get; set; } = new List<Request>();

        public virtual ICollection<Request> RequestEnteredByNavigations { get; set; } = new List<Request>();
        public virtual ICollection<UserFacility> UserFacilities { get; set; }
        public virtual ICollection<UserProgram> UserPrograms { get; set; }
        public virtual ICollection<UserSecurityQuestion> UserSecurityQuestions { get; set; }
    }
}

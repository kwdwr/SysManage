using System;

namespace SyllabusManager.App.Models
{
    // LO4: Inheritance - Base User class
    public abstract class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; } // e.g., "CE", "SE"
        public string Password { get; set; }

        public User(string id, string name, string department)
        {
            Id = id;
            Name = name;
            Department = department;
            Password = "123"; // Default password
        }

        public abstract string GetRoleDescription();
    }

    public class InstructorUser : User
    {
        public InstructorUser(string id, string name, string department)
            : base(id, name, department) { }

        public override string GetRoleDescription() => "Instructor - Can create and edit syllabi.";
    }

    public class HeadOfDepartmentUser : User
    {
        public HeadOfDepartmentUser(string id, string name, string department)
            : base(id, name, department) { }

        public override string GetRoleDescription() => "Head of Department - Can manage all syllabi and receives notifications.";
    }

    public class AdminUser : User
    {
        public AdminUser(string id, string name, string department)
            : base(id, name, department) { }

        public override string GetRoleDescription() => "Admin - Implementation System Management.";
    }
}

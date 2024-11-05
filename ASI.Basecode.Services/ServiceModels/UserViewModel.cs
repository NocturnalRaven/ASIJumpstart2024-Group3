using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Services.ServiceModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "This is required")]
        public int Role { get; set; }

        // Computed property for display based on the Role
        public string DisplayRole => GetRoleDisplayName(Role);

        [Required(ErrorMessage = "This is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required(ErrorMessage = "This is required")]
        public string UserCode { get; set; }

        [Required(ErrorMessage = "This is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        // Method to get the role display name
        private string GetRoleDisplayName(int role)
        {
            return role switch
            {
                9 => "Super Admin",
                1 => "Admin",
                2 => "Staff",
                3 => "User",
                _ => "Unknown"
            };
        }
    }

    public class UserListViewModel
    {
        [DisplayName("ID")]
        public string IdFilter { get; set; }

        [Display(Name = "First Name")]
        public string FirstNameFilter { get; set; }

        public IEnumerable<UserViewModel> dataList { get; set; }
    }

    public class UserPageViewModel
    {
        public UserListViewModel UserList { get; set; }
        public UserViewModel NewUser { get; set; }
    }
}

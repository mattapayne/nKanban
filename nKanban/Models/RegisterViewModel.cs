using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace nKanban.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name cannot be more than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Email cannot be more than 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters and no more than 20 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password confirmation must be at least 6 characters and no more than 20 characters.")]
        [Compare("Password", ErrorMessage = "Password confirmation must match Password.")]
        public string PasswordConfirmation { get; set; }

        [Display(Name = "Organization")]
        [StringLength(100, ErrorMessage = "Organization name cannot be more than 100 characters.")]
        public string OrganizationName { get; set; }

        [Display(Name = "Address One")]
        [StringLength(255, ErrorMessage = "Address one cannot be more than 255 characters.")]
        public string Address1 { get; set; }

        [Display(Name = "Address Two")]
        [StringLength(255, ErrorMessage = "Address two cannot be more than 255 characters.")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        [StringLength(100, ErrorMessage = "City cannot be more than 100 characters.")]
        public string City { get; set; }

        [Display(Name = "Province/State")]
        public Guid? ProvinceId { get; set; }

        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        [Display(Name = "Postal/Zip Code")]
        [StringLength(20, ErrorMessage = "Postal/Zip code cannot be more than 20 characters.")]
        public string PostalCode { get; set; }

        //properties used for New
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Provinces { get; set; }

        public RegisterViewModel()
        {
            Countries = Enumerable.Empty<SelectListItem>();
            Provinces = Enumerable.Empty<SelectListItem>();
        }
    }
}
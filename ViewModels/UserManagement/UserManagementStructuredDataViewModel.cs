using System.Collections.Generic;
using IS_Proj_HIT.Entities;
using Microsoft.AspNetCore.Identity;

namespace  IS_Proj_HIT.ViewModels.UserManagement
{
  
  public class UserManagementStructuredDataViewModel
  {
    public Address Address {get; set;}
    public AddressState AddressState {get; set;}


    public Country Country {get; set;}
    public Facility Facility {get; set;}
    public IdentityRole IdentityRole { get; set; }
    public AspNetPermission AspNetPermission { get; set; }
    public bool IsPermissionGroup {get; set;}
    public IS_Proj_HIT.Entities.Program Program {get; set;}
    public ProgramFacility ProgramFacility {get; set;}
    public List<UserFacility> UserFacilities {get; set;}
    public SecurityQuestion SecurityQuestion {get; set;}
    public List<UserTable> Users { get; set; }
    public List<IdentityRole> Roles { get; set; }


    // these two properties are used in managing RolePermissions 
    public string RoleName {get; set;}
    public List<string> Permissions {get; set;}

    public UserManagementStructuredDataViewModel()
    {
      Users = new List<UserTable>();
      Permissions = new List<string>();
      Roles = new List<IdentityRole>();
      UserFacilities = new List<UserFacility>();
    }

    public UserManagementStructuredDataViewModel(Facility facility, Address address){
      this.Facility = facility;
      this.Address = address;
    }

    public UserManagementStructuredDataViewModel(Facility facility, Address address, AddressState addressState, Country country){
      this.Facility = facility;
      this.Address = address;
      this.AddressState = addressState;
      this.Country = country;
    }

    public UserManagementStructuredDataViewModel(IdentityRole identityRole)
    {
      this.IdentityRole = identityRole;
      Users = new List<UserTable>();
    }

    public UserManagementStructuredDataViewModel(AspNetPermission aspNetPermission)
    {
      this.AspNetPermission = aspNetPermission;
    }

    public UserManagementStructuredDataViewModel(IS_Proj_HIT.Entities.Program program)
    {
      this.Program = program;
      Users = new List<UserTable>();
    }

    public UserManagementStructuredDataViewModel(ProgramFacility programFacility)
    {
      ProgramFacility = programFacility;
      Facility = programFacility.Facility;
      Program = programFacility.Program;
    } 

    public UserManagementStructuredDataViewModel(SecurityQuestion securityQuestion)
    {
      this.SecurityQuestion = securityQuestion;
    }

  }
}
using IS_Proj_HIT.Entities;

namespace  IS_Proj_HIT.ViewModels.PhysicianStructuredData
{
  
  public class PhysicianStructuredDataViewModel
  {
    public Physician Physician {get; set;}  
    public PhysicianRole PhysicianRole {get; set;}
    public ProviderType ProviderType {get; set;}
    public Specialty Specialty {get; set;}
    public ProviderStatus ProviderStatus {get; set;}
    public Address Address {get; set;}
    public AddressState AddressState {get; set;}
    public Country Country {get; set;}

    // General Constructors
    public PhysicianStructuredDataViewModel(){}

    public PhysicianStructuredDataViewModel(Physician physician){
        this.Physician = physician;
    }

    public PhysicianStructuredDataViewModel(Physician physician, Address address){
        this.Physician = physician;
        this.Address = address;
    }

    public PhysicianStructuredDataViewModel(Physician physician, Address address, AddressState addressState, Country country, Specialty specialty, ProviderStatus providerStatus, ProviderType providerType){
        this.Physician = physician;
        this.Address = address;
        this.AddressState = addressState;
        this.Country = country;
        this.Specialty = specialty;
        this.ProviderType = providerType;
        this.ProviderStatus = providerStatus;
    }

    public PhysicianStructuredDataViewModel(PhysicianRole physicianRole){
        this.PhysicianRole = physicianRole;
    }

    public PhysicianStructuredDataViewModel(ProviderType providerType){
        this.ProviderType = providerType;
    }
    public PhysicianStructuredDataViewModel(Specialty specialty){
        this.Specialty = specialty;
    }


    public PhysicianStructuredDataViewModel(Address address){
        this.Address = address;
    }

  }

}
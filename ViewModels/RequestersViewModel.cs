using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels
{
  
  public class RequestersViewModel
  {
    public Requester Requester {get; set;}
    public RequesterType RequesterType {get; set;}
    public RequesterStatus RequesterStatus {get; set;}
    public Address Address {get; set;}
    public AddressState AddressState {get; set;}
    public Country Country {get; set;}

    // general constructors
    public RequestersViewModel(){}

    public RequestersViewModel(Requester requester){
        this.Requester = requester;
    }

    public RequestersViewModel(Requester requester, Address address, RequesterStatus requesterStatus){
        this.Requester = requester;
        this.Address = address;
        this.RequesterStatus = requesterStatus;
    }

    public RequestersViewModel(Requester requester, Address address, AddressState addressState, Country country, RequesterStatus requesterStatus, RequesterType requesterType){
        this.Requester = requester;
        this.Address = address;
        this.AddressState = addressState;
        this.Country = country;
        this.RequesterType = requesterType;
        this.RequesterStatus = requesterStatus;
    }

  }
}
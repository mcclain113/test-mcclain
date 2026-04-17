using System.Drawing;
using IS_Proj_HIT.Entities;

namespace  IS_Proj_HIT.ViewModels
{
  
  public class EncounterStructuredDataViewModel
  {
    public Address Address {get; set;}
    public AddressState AddressState {get; set;}
    public Country Country {get; set;}

    public AdmitType AdmitType {get; set;} 
    public Department Department {get; set;}
    public Discharge Discharge {get; set;}
    public EncounterType EncounterType {get; set;}
    public PlaceOfServiceOutPatient PlaceOfServiceOutPatient {get; set;}
    public PointOfOrigin PointOfOrigin {get; set;}

    // constructors
    public EncounterStructuredDataViewModel(){}
    public EncounterStructuredDataViewModel(AdmitType admitType){
      this.AdmitType = admitType;
    }

    public EncounterStructuredDataViewModel(Department department){
      this.Department = department;
    }

    public EncounterStructuredDataViewModel(Discharge discharge){
      this.Discharge = discharge;
    }

    public EncounterStructuredDataViewModel(EncounterType encounterType){
      this.EncounterType = encounterType;
    }

    public EncounterStructuredDataViewModel(PlaceOfServiceOutPatient placeOfServiceOutPatient){
      this.PlaceOfServiceOutPatient = placeOfServiceOutPatient;
    }

    public EncounterStructuredDataViewModel(PointOfOrigin pointOfOrigin){
      this.PointOfOrigin = pointOfOrigin;
    }


  }
}
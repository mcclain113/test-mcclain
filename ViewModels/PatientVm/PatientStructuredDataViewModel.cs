using IS_Proj_HIT.Entities;

namespace  IS_Proj_HIT.ViewModels.PatientVm
{
  
  public class PatientStructuredDataViewModel
  {
    public Country Country {get; set;}
    public EducationLevel EducationLevel {get; set;}
    public Ethnicity Ethnicity {get; set;}
    public Gender Gender {get; set;}
    public GenderPronoun GenderPronoun {get; set;}
    public InsuranceProvider InsuranceProvider {get; set;}
    public Language Language {get; set;}
    public LegalStatus LegalStatus {get; set;}
    public MaritalStatus MaritalStatus {get; set;}
    public PreferredContactTime PreferredContactTime {get; set;}
    public PreferredModeOfContact PreferredModeOfContact {get; set;}
    public Race Race {get; set;}
    public Religion Religion {get; set;}
    public Sex Sex {get; set;}

    // constructors
    public PatientStructuredDataViewModel(){}

    public PatientStructuredDataViewModel(Country country)
    {
        this.Country = country;
    }

    public PatientStructuredDataViewModel(EducationLevel educationLevel)
    {
        this.EducationLevel = educationLevel;
    }

    public PatientStructuredDataViewModel(Ethnicity ethnicity)
    {
        this.Ethnicity = ethnicity;
    }

    public PatientStructuredDataViewModel(Gender gender)
    {
        this.Gender = gender;
    }

    public PatientStructuredDataViewModel(GenderPronoun genderPronoun)
    {
        this.GenderPronoun = genderPronoun;
    }

    public PatientStructuredDataViewModel(InsuranceProvider insuranceProvider)
    {
        this.InsuranceProvider = insuranceProvider;
    }

    public PatientStructuredDataViewModel(Language language)
    {
        this.Language = language;
    }

    public PatientStructuredDataViewModel(LegalStatus legalStatus)
    {
        this.LegalStatus = legalStatus;
    }

    public PatientStructuredDataViewModel(MaritalStatus maritalStatus)
    {
        this.MaritalStatus = maritalStatus;
    }

    public PatientStructuredDataViewModel(PreferredContactTime preferredContactTime)
    {
        this.PreferredContactTime = preferredContactTime;
    }

    public PatientStructuredDataViewModel(PreferredModeOfContact preferredModeOfContact)
    {
        this.PreferredModeOfContact = preferredModeOfContact;
    }

    public PatientStructuredDataViewModel(Race race)
    {
        this.Race = race;
    }

    public PatientStructuredDataViewModel(Religion religion)
    {
        this.Religion = religion;
    }

    public PatientStructuredDataViewModel(Sex sex)
    {
        this.Sex = sex;
    }
  }
}

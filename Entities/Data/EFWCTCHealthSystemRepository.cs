using System;
using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.DTOs;
using IS_Proj_HIT.ViewModels;
using IS_Proj_HIT.ViewModels.PatientVm;
using Microsoft.EntityFrameworkCore;

namespace IS_Proj_HIT.Entities.Data
{
    public class EFWCTCHealthSystemRepository : IWCTCHealthSystemRepository
    {
        private readonly WCTCHealthSystemContext _context;

        public EFWCTCHealthSystemRepository(WCTCHealthSystemContext context) => _context = context;
        #region IQueryable
        public IQueryable<AbnormalCondition> AbnormalConditions => _context.AbnormalConditions;
        public IQueryable<Address> Addresses => _context.Addresses;
        public IQueryable<AddressState> AddressStates => _context.AddressStates;
        public IQueryable<AdvancedDirective> AdvancedDirectives => _context.AdvancedDirectives;
        public IQueryable<RequesterType> RequesterTypes => _context.RequesterTypes;
        public IQueryable<RequesterStatus> RequesterStatuses => _context.RequesterStatuses;
        public IQueryable<Requester> Requesters => _context.Requesters;
        public IQueryable<CareSystemAssessment> CareSystemAssessments => _context.CareSystemAssessments;
        public IQueryable<CareSystemType> CareSystemAssessmentTypes => _context.CareSystemTypes;
        public IQueryable<CareSystemParameter> CareSystemParameters => _context.CareSystemParameters;
        public IQueryable<BirthPlaceType> BirthPlaceTypes => _context.BirthPlaceTypes;
        public IQueryable<CharacteristicOfLabor> CharacteristicOfLabors => _context.CharacteristicOfLabors;
        public IQueryable<CongenitalAnomaly> CongenitalAnomalies => _context.CongenitalAnomalies;
        public IQueryable<FetalPresentationAtBirth> FetalPresentationAtBirths => _context.FetalPresentationAtBirths;
        public IQueryable<FinalRouteAndMethodOfDelivery> FinalRouteAndMethodOfDeliveries => _context.FinalRouteAndMethodOfDeliveries;
        public IQueryable<MaternalMorbidity> MaternalMorbidities => _context.MaternalMorbidities;
        public IQueryable<OnsetOfLabor> OnsetOfLabors => _context.OnsetOfLabors;
        public IQueryable<PregnancyInfection> PregnancyInfections => _context.PregnancyInfections;
        public IQueryable<PregnancyRiskFactor> PregnancyRiskFactors => _context.PregnancyRiskFactors;
        public IQueryable<BloodPressureRouteType> BloodPressureRouteTypes => _context.BloodPressureRouteTypes;
        public IQueryable<Bmimethod> BmiMethods => _context.Bmimethods;
        public IQueryable<PcapainAssessment> PainAssessments => _context.PcapainAssessments;
        public IQueryable<PainRating> PainRatings => _context.PainRatings;
        public IQueryable<PainRatingImage> PainRatingImages => _context.PainRatingImages;
        public IQueryable<PainParameter> PainParameters => _context.PainParameters;
        public IQueryable<PainScaleType> PainScaleTypes => _context.PainScaleTypes;
        public IQueryable<O2deliveryType> O2DeliveryTypes => _context.O2deliveryTypes;
        public IQueryable<PulseRouteType> PulseRouteTypes => _context.PulseRouteTypes;
        public IQueryable<TempRouteType> TempRouteTypes => _context.TempRouteTypes;
        public IQueryable<Pcarecord> PcaRecords => _context.Pcarecords;
        public IQueryable<Pcacomment> PcaComments => _context.Pcacomments;
        public IQueryable<PcacommentType> PcaCommentTypes => _context.PcacommentTypes;
        public IQueryable<AdmitType> AdmitTypes => _context.AdmitTypes;
        public IQueryable<Ethnicity> Ethnicities => _context.Ethnicities;
        public IQueryable<Gender> Genders => _context.Genders;
        public IQueryable<GenderPronoun> GenderPronouns => _context.GenderPronouns;
        public IQueryable<AspNetRole> AspNetRoles => _context.AspNetRoles;
        public IQueryable<AspNetUserRole> AspNetUserRoles => _context.AspNetUserRoles;
        public IQueryable<AspNetRolePermission> AspNetRolePermissions => _context.AspNetRolePermissions;
        public IQueryable<AspNetPermission> AspNetPermissions => _context.AspNetPermissions;
        public IQueryable<AspNetUser> AspNetUsers => _context.AspNetUsers;
        public IQueryable<Department> Departments => _context.Departments;
        public IQueryable<EncounterType> EncounterTypes => _context.EncounterTypes;
        public IQueryable<Discharge> Discharges => _context.Discharges;
        public IQueryable<Sex> Sexes => _context.Sexes;
        public IQueryable<Patient> Patients => _context.Patients;
        public IQueryable<Person> Persons => _context.Persons;
        public IQueryable<PersonContactDetail> PersonContactDetails => _context.PersonContactDetails;
        public IQueryable<PersonLanguage> PersonLanguages => _context.PersonLanguages;
        public IQueryable<PersonModeOfContact> PersonModeOfContacts => _context.PersonModeOfContacts;
        public IQueryable<PersonContactTime> PersonContactTimes => _context.PersonContactTimes;
        public IQueryable<PersonRace> PersonRaces => _context.PersonRaces;
        public IQueryable<PlaceOfServiceOutPatient> PlaceOfService => _context.PlaceOfServiceOutPatients;
        public IQueryable<PointOfOrigin> PointOfOrigin => _context.PointOfOrigins;
        public IQueryable<Religion> Religions => _context.Religions;
        public IQueryable<LegalStatus> LegalStatuses => _context.LegalStatuses;
        public IQueryable<MaritalStatus> MaritalStatuses => _context.MaritalStatuses;
        public IQueryable<PatientContactDetail> PatientContactDetails => _context.PatientContactDetails;
        public IQueryable<PatientAlert> PatientAlerts => _context.PatientAlerts;
        public IQueryable<AlertType> AlertTypes => _context.AlertTypes;
        public IQueryable<ClinicalReminder> ClinicalReminders => _context.ClinicalReminders;
        public IQueryable<PatientRestriction> PatientRestrictions => _context.PatientRestrictions;
        public IQueryable<EducationLevel> EducationLevels => _context.EducationLevels;
        public IQueryable<Employment> Employments => _context.Employments;
        public IQueryable<Encounter> Encounters => _context.Encounters;
        public IQueryable<FallRisk> FallRisks => _context.FallRisks;
        public IQueryable<Restriction> Restrictions => _context.Restrictions;
        public IQueryable<PatientFallRisk> PatientFallRisks => _context.PatientFallRisks;
        public IQueryable<Allergen> Allergens => _context.Allergens;
        public IQueryable<Reaction> Reactions => _context.Reactions;
        public IQueryable<PatientAllergy> PatientAllergy => _context.PatientAllergies;
        public IQueryable<EncounterPhysician> EncounterPhysicians => _context.EncounterPhysicians;
        public IQueryable<Facility> Facilities => _context.Facilities;
        public IQueryable<Physician> Physicians => _context.Physicians;
        public IQueryable<PhysicianRole> PhysicianRoles => _context.PhysicianRoles;
        public IQueryable<UserTable> UserTables => _context.UserTables;
        public IQueryable<Language> Languages => _context.Languages;
        public IQueryable<PatientLanguage> PatientLanguages => _context.PatientLanguages;
        public IQueryable<Race> Races => _context.Races;
        public IQueryable<PatientRace> PatientRaces => _context.PatientRaces;
        public IQueryable<IS_Proj_HIT.Entities.Program> Programs => _context.Programs;
        public IQueryable<UserFacility> UserFacilities => _context.UserFacilities;
        public IQueryable<UserProgram> UserPrograms => _context.UserPrograms;
        public IQueryable<ProgramFacility> ProgramFacilities => _context.ProgramFacilities;
        public IQueryable<SecurityQuestion> SecurityQuestions => _context.SecurityQuestions;
        public IQueryable<UserSecurityQuestion> UserSecurityQuestions => _context.UserSecurityQuestions;
        public IQueryable<PhysicianAssessment> PhysicianAssessments => _context.PhysicianAssessments;
        public IQueryable<PhysicianAssessmentAllergy> PhysicianAssessmentAllergies => _context.PhysicianAssessmentAllergies;
        public IQueryable<BodySystemAssessment> BodySystemAssessments => _context.BodySystemAssessments;
        public IQueryable<ExamType> ExamTypes => _context.ExamTypes;
        public IQueryable<BodySystemType> BodySystemTypes => _context.BodySystemTypes;
        public IQueryable<ProgressNote> ProgressNotes => _context.ProgressNotes;
        public IQueryable<NoteType> NoteTypes => _context.NoteTypes;
        public IQueryable<VisitType> VisitTypes => _context.VisitTypes;
        public IQueryable<AdmitOrder> AdmitOrders => _context.AdmitOrders;
        public IQueryable<OrderInfo> OrderInfos => _context.OrderInfos;
        public IQueryable<OrderType> OrderTypes => _context.OrderTypes;
        public IQueryable<PatientAlias> PatientAliases => _context.PatientAliases;
        public IQueryable<Relationship> Relationships => _context.Relationships;
        public IQueryable<Country> Countries => _context.Countries;
        public IQueryable<PreferredModeOfContact> PreferredModesOfContact => _context.PreferredModeOfContacts;
        public IQueryable<PreferredContactTime> PreferredContactTimes => _context.PreferredContactTimes;
        public IQueryable<PatientContactTime> PatientContactTimes => _context.PatientContactTimes;
        public IQueryable<PatientModeOfContact> PatientModeOfContacts => _context.PatientModeOfContacts;
        public IQueryable<PatientEmergencyContact> PatientEmergencyContacts => _context.PatientEmergencyContacts;

        // Added Medication List tables
        public IQueryable<Medication> Medications => _context.Medications;
        public IQueryable<MedicationBrandName> MedicationBrandNames => _context.MedicationBrandNames;
        public IQueryable<MedicationDeliveryRoute> MedicationDeliveryRoutes => _context.MedicationDeliveryRoutes;
        public IQueryable<MedicationDosageForm> MedicationDosageForms => _context.MedicationDosageForms;
        public IQueryable<MedicationGenericName> MedicationGenericNames => _context.MedicationGenericNames;
        public IQueryable<MedicationFrequency> MedicationFrequencies => _context.MedicationFrequencies;
        public IQueryable<PatientMedicationList> PatientMedicationLists => _context.PatientMedicationLists;

        public IQueryable<InsuranceProvider> InsuranceProviders => _context.InsuranceProviders;
        public IQueryable<PatientInsurance> PatientInsurances => _context.PatientInsurances;
        public IQueryable<PatientRepresentative> PatientRepresentatives => _context.PatientRepresentatives;

        //Added Disclosure List Tables
        public IQueryable<Disclosure> Disclosures => _context.Disclosures;
        public IQueryable<DisclosureFee> DisclosureFees => _context.DisclosureFees;
        public IQueryable<DisclosureFeeType> DisclosureFeeTypes => _context.DisclosureFeeTypes;
        public IQueryable<DisclosurePayment> DisclosurePayments => _context.DisclosurePayments;
        public IQueryable<PaymentType> PaymentTypes => _context.PaymentTypes;
        public IQueryable<Request> Requests => _context.Requests;
        public IQueryable<RequestedItem> RequestedItems => _context.RequestedItems;
        public IQueryable<RequestPriority> RequestPriorities => _context.RequestPriorities;
        public IQueryable<RequestPurpose> RequestPurposes => _context.RequestPurposes;
        public IQueryable<DocumentRequested> DocumentRequesteds => _context.DocumentRequesteds;
        public IQueryable<RequestReleaseFormat> RequestReleaseFormats => _context.RequestReleaseFormats;
        public IQueryable<RequestStatus> RequestStatuses => _context.RequestStatuses;
        public IQueryable<RequestStatusReason> RequestStatusReasons => _context.RequestStatusReasons;
        public IQueryable<ItemStatus> ItemStatuses => _context.ItemStatuses;

        //Added Physician Structure Data tables
        public IQueryable<ProviderType> ProviderTypes => _context.ProviderTypes;
        public IQueryable<Specialty> Specialties => _context.Specialties;
        public IQueryable<ProviderStatus> ProviderStatuses => _context.ProviderStatuses;
        public IQueryable<Priority> Priorities => _context.Priorities;
        public IQueryable<ChargeDefinition> ChargeDefinitions => _context.ChargeDefinitions;
        public IQueryable<RevenueCode> RevenueCodes => _context.RevenueCodes;
        public IQueryable<Document> Documents => _context.Documents;
        public IQueryable<DocumentType> DocumentTypes => _context.DocumentTypes;
        public IQueryable<Birth> Births => _context.Births;
        public IQueryable<BirthFather> BirthFathers => _context.BirthFathers;
        public IQueryable<Newborn> Newborns => _context.Newborns;
        public IQueryable<Prenatal> Prenatals => _context.Prenatals;
        public IQueryable<LaborAndDelivery> LaborAndDeliveries => _context.LaborAndDeliveries;


        #endregion IQueryable

        public void AddPatient(Patient patient)
        {
            _context.Add(patient);
            _context.SaveChanges();
        }

        public void DeletePatient(Patient patient)
        {
            _context.Remove(patient);
            _context.SaveChanges();
        }

        public void EditPatient(Patient patient)
        {
            _context.Update(patient);
            _context.SaveChanges();
        }

        public void AddPatientAllergy(PatientAllergy patientAllergy)
        {
            _context.Add(patientAllergy);
            _context.SaveChanges();
        }

        public void EditPatientAllergy(PatientAllergy patientAllergy)
        {
            _context.Update(patientAllergy);
            _context.SaveChanges();
        }

        public void DeletePatientAllergy(PatientAllergy patientAllergy)
        {
            _context.Remove(patientAllergy);
            _context.SaveChanges();
        }

        public void AddPatientContactDetail(PatientContactDetail patientContactDetail)
        {
            _context.Add(patientContactDetail);
            _context.SaveChanges();
        }

        public void AddPerson(Person person)
        {
            _context.Add(person);
            _context.SaveChanges();
        }

        public void EditPerson(Person person)
        {
            Console.WriteLine($"[DEBUG] EditPerson: Saving PersonId={person.PersonId}, FirstName='{person.FirstName}', LastName='{person.LastName}', ReligionId(before)={person.ReligionId}");
            _context.Update(person);
            _context.SaveChanges();
            Console.WriteLine($"[DEBUG] EditPerson: Saved PersonId={person.PersonId}, ReligionId(after)={person.ReligionId}");
        }

        public void DeletePerson(Person person)
        {
            _context.Remove(person);
            _context.SaveChanges();
        }

        public void AddPersonContactDetail(PersonContactDetail pcd)
        {
            _context.Add(pcd);
            _context.SaveChanges();
        }

         public void DeletePersonContactDetail(PersonContactDetail pcd)
        {
            _context.Remove(pcd);
            _context.SaveChanges();
        }

         public void EditPersonContactDetail(PersonContactDetail pcd)
        {
            _context.Update(pcd);
            _context.SaveChanges();
        }

        public void AddPersonContactTime(PersonContactTime pct)
        {
            _context.Add(pct);
            _context.SaveChanges();
        }

        public void DeletePersonContactTime(PersonContactTime pct)
        {
            _context.Remove(pct);
            _context.SaveChanges();
        }

        public void EditPersonContactTime(PersonContactTime pct)
        {
            _context.Update(pct);
            _context.SaveChanges();
        }

        public void AddPersonContactTime(List<PersonContactTime> pctList)
        {
            pctList.ToList().ForEach(a => _context.Add(a));
            _context.SaveChanges();
        }

        public void DeletePersonContactTime(List<PersonContactTime> pctList)
        {
            pctList.ToList().ForEach(a => _context.Remove(a));
            _context.SaveChanges();
        }

        public void AddPersonLanguage(PersonLanguage personLanguage)
        {
            _context.Add(personLanguage);
            _context.SaveChanges();
        }

        public void DeletePersonLanguage(PersonLanguage personLanguage)
        {
            _context.Remove(personLanguage);
            _context.SaveChanges();
        }

        public void EditPersonLanguage(PersonLanguage personLanguage)
        {
            _context.Update(personLanguage);
            _context.SaveChanges();
        }

        public void AddPersonLanguage(List<PersonLanguage> plList)
        {
            plList.ToList().ForEach(a => _context.Add(a));
            _context.SaveChanges();
        }

        public void DeletePersonLanguage(List<PersonLanguage> plList)
        {
            plList.ToList().ForEach(a => _context.Remove(a));
            _context.SaveChanges();
        }

        public void AddPersonModeOfContact(PersonModeOfContact personModeOfContact)
        {
            _context.Add(personModeOfContact);
            _context.SaveChanges();
        }

        public void DeletePersonModeOfContact(PersonModeOfContact personModeOfContact)
        {
            _context.Remove(personModeOfContact);
            _context.SaveChanges();
        }

        public void EditPersonModeOfContact(PersonModeOfContact personModeOfContact)
        {
            _context.Update(personModeOfContact);
            _context.SaveChanges();
        }

        public void AddPersonModeOfContact(List<PersonModeOfContact> pmcList)
        {
            pmcList.ToList().ForEach(a => _context.Add(a));
            _context.SaveChanges();
        }

        public void DeletePersonModeOfContact(List<PersonModeOfContact> pmcList)
        {
            pmcList.ToList().ForEach(a => _context.Remove(a));
            _context.SaveChanges();
        }

        public void AddPersonRace(PersonRace personRace)
        {
            _context.Add(personRace);
            _context.SaveChanges();
        }

        public void EditPersonRace(PersonRace personRace)
        {
            _context.Update(personRace);
            _context.SaveChanges();
        }

        public void DeletePersonRace(PersonRace personRace)
        {
            _context.Remove(personRace);
            _context.SaveChanges();
        }

        public void AddPersonRace(List<PersonRace> prList)
        {
            prList.ToList().ForEach(a => _context.Add(a));
            _context.SaveChanges();
        }

        public void DeletePersonRace(List<PersonRace> prList)
        {
            prList.ToList().ForEach(a => _context.Remove(a));
            _context.SaveChanges();
        }

        public void AddGender(Gender gender)
        {
            _context.Add(gender);
            _context.SaveChanges();
        }

        public void EditGender(Gender gender)
        {
            _context.Update(gender);
            _context.SaveChanges();
        }

        public void DeleteGender(Gender gender)
        {
            _context.Remove(gender);
            _context.SaveChanges();
        }

        public void AddGenderPronoun(GenderPronoun genderPronoun)
        {
            _context.Add(genderPronoun);
            _context.SaveChanges();
        }

        public void EditGenderPronoun(GenderPronoun genderPronoun)
        {
            _context.Update(genderPronoun);
            _context.SaveChanges();
        }

        public void DeleteGenderPronoun(GenderPronoun genderPronoun)
        {
            _context.Remove(genderPronoun);
            _context.SaveChanges();
        }

        public void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = _context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }

        public void EditPatientContactDetail(PatientContactDetail patientContactDetail)
        {
            _context.Update(patientContactDetail);
            _context.SaveChanges();
        }
        public void DeletePatientContactDetail(PatientContactDetail patientContactDetail)
        {
            _context.Remove(patientContactDetail);
            _context.SaveChanges();
        }

        public void AddPreferredModeOfContact(PreferredModeOfContact preferredModeOfContact)
        {
            _context.Add(preferredModeOfContact);
            _context.SaveChanges();
        }

        public void EditPreferredModeOfContact(PreferredModeOfContact preferredModeOfContact)
        {
            _context.Update(preferredModeOfContact);
            _context.SaveChanges();
        }

        public void DeletePreferredModeOfContact(PreferredModeOfContact preferredModeOfContact)
        {
            _context.Remove(preferredModeOfContact);
            _context.SaveChanges();
        }

        public void AddPreferredContactTime(PreferredContactTime preferredContactTime)
        {
            _context.Add(preferredContactTime);
            _context.SaveChanges();
        }

        public void EditPreferredContactTime(PreferredContactTime preferredContactTime)
        {
            _context.Update(preferredContactTime);
            _context.SaveChanges();
        }

        public void DeletePreferredContactTime(PreferredContactTime preferredContactTime)
        {
            _context.Remove(preferredContactTime);
            _context.SaveChanges();
        }

        public void AddReligion(Religion religion)
        {
            _context.Add(religion);
            _context.SaveChanges();
        }

        public void EditReligion(Religion religion)
        {
            _context.Update(religion);
            _context.SaveChanges();
        }

        public void DeleteReligion(Religion religion)
        {
            _context.Remove(religion);
            _context.SaveChanges();
        }

        public void AddSex(Sex sex)
        {
            _context.Add(sex);
            _context.SaveChanges();
        }

        public void EditSex(Sex sex)
        {
            _context.Update(sex);
            _context.SaveChanges();
        }

        public void DeleteSex(Sex sex)
        {
            _context.Remove(sex);
            _context.SaveChanges();
        }

        public void AddEncounter(Encounter encounter)
        {
            _context.Add(encounter);
            _context.SaveChanges();
        }

        public void EditEncounter(Encounter encounter)
        {
            _context.Update(encounter);
            _context.SaveChanges();
        }

        public void AddRequester(Requester requester)
        {
            _context.Add(requester);
            _context.SaveChanges();
        }

        public void EditRequester(Requester requester)
        {
            _context.Update(requester);
            _context.SaveChanges();
        }
        public void DeleteRequester(Requester requester)
        {
            if (requester != null)
            {
                _context.Requesters.Remove(requester);
                _context.SaveChanges();
            }
        }


        public void DeleteEncounter(Encounter encounter)
        {
            var pcas = _context.Pcarecords.Where(p => p.EncounterId == encounter.EncounterId);
            foreach (Pcarecord pca in pcas)
            {
                _context.Remove(pca);
            }
            var encounterPhysicians = _context.EncounterPhysicians.Where(p => p.EncounterId == encounter.EncounterId);
            foreach (EncounterPhysician e in encounterPhysicians)
            {
                _context.Remove(e);
            }
            _context.Remove(encounter);
            _context.SaveChanges();
        }

        public void AddAlert(PatientAlert patientAlert)
        {
            _context.Add(patientAlert);
            _context.SaveChanges();
        }
        public void DeleteAlert(PatientAlert patientAlert)
        {
            _context.Remove(patientAlert);
            _context.SaveChanges();
        }

        public void EditAlert(PatientAlert patientAlert)
        {
            _context.Update(patientAlert);
            _context.SaveChanges();
        }

        public void AddPatientFallRisk(PatientFallRisk patientFallRisk)
        {
            _context.Add(patientFallRisk);
            _context.SaveChanges();            
        }

        public void EditPatientFallRisk(PatientFallRisk patientFallRisk)
        {
            _context.Update(patientFallRisk);
            _context.SaveChanges();            
        }

        public void DeletePatientFallRisk(PatientFallRisk patientFallRisk)
        {
            _context.Remove(patientFallRisk);
            _context.SaveChanges();            
        }

        public void AddPatientRestriction(PatientRestriction patientRestriction)
        {
            _context.Add(patientRestriction);
            _context.SaveChanges();            
        }

        public void EditPatientRestriction(PatientRestriction patientRestriction)
        {
            _context.Update(patientRestriction);
            _context.SaveChanges();            
        }

        public void DeletePatientRestriction(PatientRestriction patientRestriction)
        {
            _context.Remove(patientRestriction);
            _context.SaveChanges();            
        }

        public void AddAlertType(AlertType alertType)
        {
            _context.Add(alertType);
            _context.SaveChanges();
        }

        public void EditAlertType(AlertType alertType)
        {
            _context.Update(alertType);
            _context.SaveChanges();
        }

        public void DeleteAlertType(AlertType alertType)
        {
            _context.Remove(alertType);
            _context.SaveChanges();
        }

        public void AddAdvancedDirective(AdvancedDirective advancedDirective)
        {
            _context.Add(advancedDirective);
            _context.SaveChanges();
        }

        public void EditAdvancedDirective(AdvancedDirective advancedDirective)
        {
            _context.Update(advancedDirective);
            _context.SaveChanges();
        }

        public void DeleteAdvancedDirective(AdvancedDirective advancedDirective)
        {
            _context.Remove(advancedDirective);
            _context.SaveChanges();
        }

        public void AddAllergen(Allergen allergen)
        {
            _context.Add(allergen);
            _context.SaveChanges();
        }

        public void EditAllergen(Allergen allergen)
        {
            _context.Update(allergen);
            _context.SaveChanges();
        }

        public void DeleteAllergen(Allergen allergen)
        {
            _context.Remove(allergen);
            _context.SaveChanges();
        }

        public void AddFallRisk(FallRisk fallRisk)
        {
            _context.Add(fallRisk);
            _context.SaveChanges();
        }

        public void EditFallRisk(FallRisk fallRisk)
        {
            _context.Update(fallRisk);
            _context.SaveChanges();
        }

        public void DeleteFallRisk(FallRisk fallRisk)
        {
            _context.Remove(fallRisk);
            _context.SaveChanges();
        }

        public void AddReaction(Reaction reaction)
        {
            _context.Add(reaction);
            _context.SaveChanges();
        }

        public void EditReaction(Reaction reaction)
        {
            _context.Update(reaction);
            _context.SaveChanges();
        }

        public void DeleteReaction(Reaction reaction)
        {
            _context.Remove(reaction);
            _context.SaveChanges();
        }

        public void AddClinicalReminder(ClinicalReminder clinicalReminder)
        {
            _context.Add(clinicalReminder);
            _context.SaveChanges();
        }

        public void EditClinicalReminder(ClinicalReminder clinicalReminder)
        {
            _context.Update(clinicalReminder);
            _context.SaveChanges();
        }

        public void DeleteClinicalReminder(ClinicalReminder clinicalReminder)
        {
            _context.Remove(clinicalReminder);
            _context.SaveChanges();
        }

        public void AddRestriction(Restriction restriction)
        {
            _context.Add(restriction);
            _context.SaveChanges();
        }

        public void EditRestriction(Restriction restriction)
        {
            _context.Update(restriction);
            _context.SaveChanges();
        }

        public void DeleteRestriction(Restriction restriction)
        {
            _context.Remove(restriction);
            _context.SaveChanges();
        }

        public void AddEducationLevel(EducationLevel educationLevel)
        {
            _context.Add(educationLevel);
            _context.SaveChanges();
        }

        public void EditEducationLevel(EducationLevel educationLevel)
        {
            _context.Update(educationLevel);
            _context.SaveChanges();
        }

        public void DeleteEducationLevel(EducationLevel educationLevel)
        {
            _context.Remove(educationLevel);
            _context.SaveChanges();
        }

        public void AddEthnicity(Ethnicity ethnicity)
        {
            _context.Add(ethnicity);
            _context.SaveChanges();
        }

        public void EditEthnicity(Ethnicity ethnicity)
        {
            _context.Update(ethnicity);
            _context.SaveChanges();
        }

        public void DeleteEthnicity(Ethnicity ethnicity)
        {
            _context.Remove(ethnicity);
            _context.SaveChanges();
        }

        public void AddEmployment(Employment employment)
        {
            _context.Add(employment);
            _context.SaveChanges();
        }

        public void EditEmployment(Employment employment)
        {
            _context.Update(employment);
            _context.SaveChanges();
        }

        public void DeleteEmployment(Employment employment)
        {
            _context.Remove(employment);
            _context.SaveChanges();
        }

        public void AddUser(UserTable userTable)
        {
            _context.Add(userTable);
            _context.SaveChanges();
        }

        public void DeleteUser(UserTable userTable)
        {
            _context.Remove(userTable);
            _context.SaveChanges();
        }

        public void EditUser(UserTable userTable)
        {
            _context.Update(userTable);
            _context.SaveChanges();
        }

        public void AddPcaRecord(Pcarecord pca)
        {
            _context.Add(pca);
            _context.SaveChanges();
        }

        public void DeletePcaRecord(Pcarecord pca)
        {
            foreach (Pcacomment comment in pca.Pcacomments)
            {
                _context.Remove(comment);
            }
            _context.Remove(pca);
            _context.SaveChanges();
        }

        public void EditPcaRecord(Pcarecord pca)
        {
            _context.Update(pca);
            _context.SaveChanges();
        }

        public void AddPcaComment(Pcacomment com)
        {
            _context.Add(com);
            _context.SaveChanges();
        }

        public void DeletePcaComment(Pcacomment com)
        {
            _context.Remove(com);
            _context.SaveChanges();
        }

        public void EditPcaComment(Pcacomment com)
        {
            _context.Update(com);
            _context.SaveChanges();
        }
        public void AddSystemAssessment(CareSystemAssessment csa)
        {
            _context.Add(csa);
            _context.SaveChanges();
        }

        public void AddSystemAssessments(IList<CareSystemAssessment> csaList)
        {
            csaList.ToList().ForEach(a => _context.Add(a));
            _context.SaveChanges();
        }

        public void DeleteSystemAssessment(CareSystemAssessment csa)
        {
            _context.Remove(csa);
            _context.SaveChanges();
        }

        public void EditSystemAssessment(CareSystemAssessment csa)
        {
            _context.Update(csa);
            _context.SaveChanges();
        }

        public void AddPainAssessment(PcapainAssessment pa)
        {
            _context.Add(pa);
            _context.SaveChanges();
        }

        public void DeletePainAssessment(PcapainAssessment pa)
        {
            _context.Remove(pa);
            _context.SaveChanges();
        }

        public void EditPainAssessment(PcapainAssessment pa)
        {
            _context.Update(pa);
            _context.SaveChanges();
        }

        public void AddLanguage(Language language)
        {
            _context.Add(language);
            _context.SaveChanges();
        }

        public void EditLanguage(Language language)
        {
            _context.Update(language);
            _context.SaveChanges();
        }

        public void DeleteLanguage(Language language)
        {
            _context.Remove(language);
            _context.SaveChanges();
        }

        public void AddPatientLanguage(PatientLanguage patientLanguage)
        {
            _context.Add(patientLanguage);
            _context.SaveChanges();
        }

        public void EditPatientLanguage(PatientLanguage patientLanguage)
        {
            _context.Update(patientLanguage);
            _context.SaveChanges();
        }

        public void DeletePatientLanguage(PatientLanguage patientLanguage)
        {
            _context.Remove(patientLanguage);
            _context.SaveChanges();
        }

        public void AddRace(Race race)
        {
            _context.Add(race);
            _context.SaveChanges();
        }

        public void EditRace(Race race)
        {
            _context.Update(race);
            _context.SaveChanges();
        }

        public void DeleteRace(Race race)
        {
            _context.Remove(race);
            _context.SaveChanges();
        }

        public void AddPatientRace(PatientRace race)
        {
            _context.Add(race);
            _context.SaveChanges();
        }

        public void EditPatientRace(PatientRace race)
        {
            _context.Update(race);
            _context.SaveChanges();
        }

        public void DeletePatientRace(PatientRace race)
        {
            _context.Remove(race);
            _context.SaveChanges();
        }

        public void AddAddress(Address address)
        {
            _context.Add(address);
            _context.SaveChanges();
        }

        public void EditAddress(Address address)
        {
            _context.Update(address);
            _context.SaveChanges();
        }

        public void DeleteAddress(Address address)
        {
            _context.Remove(address);
            _context.SaveChanges();
        }

        public void AddAddressState(AddressState addressState)
        {
            _context.Add(addressState);
            _context.SaveChanges();
        }

        public void EditAddressState(AddressState addressState)
        {
            _context.Update(addressState);
            _context.SaveChanges();
        }

        public void DeleteAddressState(AddressState addressState)
        {
            _context.Remove(addressState);
            _context.SaveChanges();
        }

        public void AddCountry(Country country)
        {
            _context.Add(country);
            _context.SaveChanges();
        }

        public void EditCountry(Country country)
        {
            _context.Update(country);
            _context.SaveChanges();
        }

        public void DeleteCountry(Country country)
        {
            _context.Remove(country);
            _context.SaveChanges();
        }

        public void AddAdmitType(AdmitType admitType)
        {
            _context.Add(admitType);
            _context.SaveChanges();
        }

        public void EditAdmitType(AdmitType admitType)
        {
            _context.Update(admitType);
            _context.SaveChanges();
        }

        public void DeleteAdmitType(AdmitType admitType)
        {
            _context.Remove(admitType);
            _context.SaveChanges();
        }

        public void AddDepartment(Department department)
        {
            _context.Add(department);
            _context.SaveChanges();
        }

        public void EditDepartment(Department department)
        {
            _context.Update(department);
            _context.SaveChanges();
        }

        public void DeleteDepartment(Department department)
        {
            _context.Remove(department);
            _context.SaveChanges();
        }

        public void AddDischarge(Discharge discharge)
        {
            _context.Add(discharge);
            _context.SaveChanges();
        }

        public void EditDischarge(Discharge discharge)
        {
            _context.Update(discharge);
            _context.SaveChanges();
        }

        public void DeleteDischarge(Discharge discharge)
        {
            _context.Remove(discharge);
            _context.SaveChanges();
        }

        public void AddEncounterType(EncounterType encounterType)
        {
            _context.Add(encounterType);
            _context.SaveChanges();
        }

        public void EditEncounterType(EncounterType encounterType)
        {
            _context.Update(encounterType);
            _context.SaveChanges();
        }

        public void DeleteEncounterType(EncounterType encounterType)
        {
            _context.Remove(encounterType);
            _context.SaveChanges();
        }

        public void AddFacility(Facility facility)
        {
            _context.Add(facility);
            _context.SaveChanges();
        }

        public void EditFacility(Facility facility)
        {
            _context.Update(facility);
            _context.SaveChanges();
        }

        public void DeleteFacility(Facility facility)
        {
            _context.Remove(facility);
            _context.SaveChanges();
        }

        public Facility SaveOrUpdateFacility(Facility facility)
        {
            if (facility.FacilityId == 0)
            {
                _context.Facilities.Add(facility);
            }
            else
            {
                _context.Facilities.Update(facility);
            }
            _context.SaveChanges();
            return facility;
        }




        public void AddPlaceOfService(PlaceOfServiceOutPatient placeofServiceOutPatient)
        {
            _context.Add(placeofServiceOutPatient);
            _context.SaveChanges();
        }

        public void EditPlaceOfService(PlaceOfServiceOutPatient placeofServiceOutPatient)
        {
            _context.Update(placeofServiceOutPatient);
            _context.SaveChanges();
        }

        public void DeletePlaceOfService(PlaceOfServiceOutPatient placeofServiceOutPatient)
        {
            _context.Remove(placeofServiceOutPatient);
            _context.SaveChanges();
        }

        public void AddPointOfOrigin(PointOfOrigin pointOfOrigin)
        {
            _context.Add(pointOfOrigin);
            _context.SaveChanges();
        }

        public void EditPointOfOrigin(PointOfOrigin pointOfOrigin)
        {
            _context.Update(pointOfOrigin);
            _context.SaveChanges();
        }

        public void DeletePointOfOrigin(PointOfOrigin pointOfOrigin)
        {
            _context.Remove(pointOfOrigin);
            _context.SaveChanges();
        }

        public void AddProgram(Program program)
        {
            _context.Add(program);
            _context.SaveChanges();
        }

        public void DeleteProgram(Program program)
        {
            _context.Remove(program);
            _context.SaveChanges();
        }

        public void EditProgram(Program program)
        {
            _context.Update(program);
            _context.SaveChanges();
        }


        public void AddUserFacility(UserFacility userFacility)
        {
            _context.Add(userFacility);
            _context.SaveChanges();
        }

        public void DeleteUserFacility(UserFacility userFacility)
        {
            _context.Remove(userFacility);
            _context.SaveChanges();
        }

        public void EditUserFacility(UserFacility userFacility)
        {
            _context.Update(userFacility);
            _context.SaveChanges();
        }

        public void AddUserProgram(UserProgram userProgram)
        {
            _context.Add(userProgram);
            _context.SaveChanges();
        }

        public void DeleteUserProgram(UserProgram userProgram)
        {
            _context.Remove(userProgram);
            _context.SaveChanges();
        }

        public void EditUserProgram(UserProgram userProgram)
        {
            _context.Update(userProgram);
            _context.SaveChanges();
        }

        public void AddProgramFacility(ProgramFacility programFacility)
        {
            _context.Add(programFacility);
            _context.SaveChanges();
        }

        public void DeleteProgramFacility(ProgramFacility programFacility)
        {
            _context.Remove(programFacility);
            _context.SaveChanges();
        }

        public void EditProgramFacility(ProgramFacility programFacility)
        {
            _context.Update(programFacility);
            _context.SaveChanges();
        }

        public void AddUserSecurityQuestion(UserSecurityQuestion userSecurityQuestion)
        {
            _context.Add(userSecurityQuestion);
            _context.SaveChanges();
        }
        public void DeleteUserSecurityQuestion(UserSecurityQuestion userSecurityQuestion)
        {
            _context.Remove(userSecurityQuestion);
            _context.SaveChanges();
        }

        public void AddSecurityQuestion(SecurityQuestion securityQuestion)
        {
            _context.Add(securityQuestion);
            _context.SaveChanges();
        }

        public void EditSecurityQuestion(SecurityQuestion securityQuestion)
        {
            _context.Update(securityQuestion);
            _context.SaveChanges();
        }

        public void DeleteSecurityQuestion(SecurityQuestion securityQuestion)
        {
            _context.Remove(securityQuestion);
            _context.SaveChanges();
        }

        public void AddPhysicianAssessment(PhysicianAssessment assessment)
        {
            _context.Add(assessment);
            _context.SaveChanges();
        }

        public void EditPhysicianAssessment(PhysicianAssessment assessment)
        {
            _context.Update(assessment);
            _context.SaveChanges();
        }

        public void AddPhysicianAssessmentAllergy(PhysicianAssessmentAllergy assessmentAllergy)
        {
            _context.Add(assessmentAllergy);
            _context.SaveChanges();
        }

        public void EditPhysicianAssessmentAllergy(PhysicianAssessmentAllergy assessmentAllergy)
        {
            _context.Update(assessmentAllergy);
            _context.SaveChanges();
        }

        public void DeletePhysicianAssessmentAllergy(PhysicianAssessmentAllergy assessmentAllergy)
        {
            _context.Remove(assessmentAllergy);
            _context.SaveChanges();
        }

        public void AddBodySystemAssessment(BodySystemAssessment bsa)
        {
            _context.Add(bsa);
            _context.SaveChanges();
        }

        public void EditBodySystemAssessment(BodySystemAssessment bsa)
        {
            _context.Update(bsa);
            _context.SaveChanges();
        }

        public void AddBodySystemAssessments(IList<BodySystemAssessment> bsaList)
        {
            bsaList.ToList().ForEach(a => _context.Add(a));
            _context.SaveChanges();
        }

        public void AddPhysicianAssessmentAllergy(List<PhysicianAssessmentAllergyDto> paaList)
        {
            paaList.ToList().ForEach(a => _context.Add(a));
            _context.SaveChanges();
        }

        public void DeletePhysicianAssessmentAllergy(List<PhysicianAssessmentAllergy> paaList)
        {
            paaList.ToList().ForEach(a => _context.Remove(a));
            _context.SaveChanges();
        }

        public void AddProgressNote(ProgressNote progressNote)
        {
            _context.Add(progressNote);
            _context.SaveChanges();
        }

        public void EditProgressNote(ProgressNote progressNote)
        {
            _context.Update(progressNote);
            _context.SaveChanges();
        }
        public void DeleteProgressNote(ProgressNote progressNote)
        {
            _context.Remove(progressNote);
            _context.SaveChanges();
        }

        public void AddVisitType(VisitType visitType)
        {
            _context.Add(visitType);
            _context.SaveChanges();
        }

        public void EditVisitType(VisitType visitType)
        {
            _context.Update(visitType);
            _context.SaveChanges();
        }

        public void DeleteVisitType(VisitType visitType)
        {
            _context.Remove(visitType);
            _context.SaveChanges();
        }

        public void AddNoteType(NoteType noteType)
        {
            _context.Add(noteType);
            _context.SaveChanges();
        }

        public void EditNoteType(NoteType noteType)
        {
            _context.Update(noteType);
            _context.SaveChanges();
        }

        public void DeleteNoteType(NoteType noteType)
        {
            _context.Remove(noteType);
            _context.SaveChanges();
        }

        public void AddOrderInfo(OrderInfo orderInfo)
        {
            _context.Add(orderInfo);
            _context.SaveChanges();
        }

        public void EditOrderInfo(OrderInfo orderInfo)
        {
            _context.Update(orderInfo);
            _context.SaveChanges();
        }

        public void DeleteOrderInfo(OrderInfo orderInfo)
        {
            _context.Remove(orderInfo);
            _context.SaveChanges();
        }

        public void PhysicianDischargeEdit(Encounter encounter)
        {
            _context.Update(encounter);
            _context.SaveChanges();
        }

        public void AddPatientAlias(PatientAlias patientAlias)
        {
            _context.Add(patientAlias);
            _context.SaveChanges();
        }
        public void EditPatientAlias(PatientAlias patientAlias)
        {
            _context.Update(patientAlias);
            _context.SaveChanges();
        }
        public void DeletePatientAlias(PatientAlias patientAlias)
        {
            _context.Remove(patientAlias);
            _context.SaveChanges();
        }

        public void AddRelationship(Relationship relationship)
        {
            _context.Add(relationship);
            _context.SaveChanges();
        }
        public void EditRelationship(Relationship relationship)
        {
            _context.Update(relationship);
            _context.SaveChanges();
        }
        public void DeleteRelationship(Relationship relationship)
        {
            _context.Remove(relationship);
            _context.SaveChanges();
        }

        public void AddPatientModeOfContact(PatientModeOfContact patientModeOfContact)
        {
            _context.Add(patientModeOfContact);
            _context.SaveChanges();
        }
        public void EditPatientModeOfContact(PatientModeOfContact patientModeOfContact)
        {
            _context.Update(patientModeOfContact);
            _context.SaveChanges();
        }
        public void DeletePatientModeOfContact(PatientModeOfContact patientModeOfContact)
        {
            _context.Remove(patientModeOfContact);
            _context.SaveChanges();
        }

        public void AddPatientContactTime(PatientContactTime patientContactTime)
        {
            _context.Add(patientContactTime);
            _context.SaveChanges();
        }
        public void EditPatientContactTime(PatientContactTime patientContactTime)
        {
            _context.Update(patientContactTime);
            _context.SaveChanges();
        }
        public void DeletePatientContactTime(PatientContactTime patientContactTime)
        {
            _context.Remove(patientContactTime);
            _context.SaveChanges();
        }

        public void AddPatientEmergencyContact(PatientEmergencyContact patientEmergencyContact)
        {
            _context.Add(patientEmergencyContact);
            _context.SaveChanges();
        }
        public void EditPatientEmergencyContact(PatientEmergencyContact patientEmergencyContact)
        {
            _context.Update(patientEmergencyContact);
            _context.SaveChanges();
        }
        public void DeletePatientEmergencyContact(PatientEmergencyContact patientEmergencyContact)
        {
            _context.Remove(patientEmergencyContact);
            _context.SaveChanges();
        }

        public void AddInsuranceProvider(InsuranceProvider insuranceProvider)
        {
            _context.Add(insuranceProvider);
            _context.SaveChanges();
        }

        public void EditInsuranceProvider(InsuranceProvider insuranceProvider)
        {
            _context.Update(insuranceProvider);
            _context.SaveChanges();
        }

        public void DeleteInsuranceProvider(InsuranceProvider insuranceProvider)
        {
            _context.Remove(insuranceProvider);
            _context.SaveChanges();
        }

        public void AddPatientInsurance(PatientInsurance patientInsurance)
        {
            _context.Add(patientInsurance);
            _context.SaveChanges();
        }

        public void EditPatientInsurance(PatientInsurance patientInsurance)
        {
            _context.Update(patientInsurance);
            _context.SaveChanges();
        }

        public void AddLegalStatus(LegalStatus legalStatus)
        {
            _context.Add(legalStatus);
            _context.SaveChanges();
        }
        public void EditLegalStatus(LegalStatus legalStatus)
        {
            _context.Update(legalStatus);
            _context.SaveChanges();
        }
        public void DeleteLegalStatus(LegalStatus legalStatus)
        {
            _context.Remove(legalStatus);
            _context.SaveChanges();
        }

        public void AddMaritalStatus(MaritalStatus maritalStatus)
        {
            _context.Add(maritalStatus);
            _context.SaveChanges();
        }

        public void EditMaritalStatus(MaritalStatus maritalStatus)
        {
            _context.Update(maritalStatus);
            _context.SaveChanges();
        }

        public void DeleteMaritalStatus(MaritalStatus maritalStatus)
        {
            _context.Remove(maritalStatus);
            _context.SaveChanges();
        }

        public void AddMedication(Medication meds)
        {
            _context.Add(meds);
            _context.SaveChanges();
        }

        public void DeleteMedication(Medication meds)
        {
            _context.Remove(meds);
            _context.SaveChanges();
        }

        public void EditMedication(Medication meds)
        {
            _context.Update(meds);
            _context.SaveChanges();
        }

        public void AddMedicationBrandName(MedicationBrandName brandName)
        {
            _context.Add(brandName);
            _context.SaveChanges();
        }

        public void DeleteMedicationBrandName(MedicationBrandName brandName)
        {
            _context.Remove(brandName);
            _context.SaveChanges();
        }

        public void EditMedicationBrandName(MedicationBrandName brandName)
        {
            _context.Update(brandName);
            _context.SaveChanges();
        }

        public void AddMedicationGenericName(MedicationGenericName genericName)
        {
            _context.Add(genericName);
            _context.SaveChanges();
        }

        public void DeleteMedicationGenericName(MedicationGenericName genericName)
        {
            _context.Remove(genericName);
            _context.SaveChanges();
        }

        public void EditMedicationGenericName(MedicationGenericName genericName)
        {
            _context.Update(genericName);
            _context.SaveChanges();
        }

        public void AddMedicationDosageForm(MedicationDosageForm dosageForm)
        {
            _context.Add(dosageForm);
            _context.SaveChanges();
        }

        public void DeleteMedicationDosageForm(MedicationDosageForm dosageForm)
        {
            _context.Remove(dosageForm);
            _context.SaveChanges();
        }

        public void EditMedicationDosageForm(MedicationDosageForm dosageForm)
        {
            _context.Update(dosageForm);
            _context.SaveChanges();
        }

        public void AddMedicationDeliveryRoute(MedicationDeliveryRoute deliveryRoute)
        {
            _context.Add(deliveryRoute);
            _context.SaveChanges();
        }

        public void DeleteMedicationDeliveryRoute(MedicationDeliveryRoute deliveryRoute)
        {
            _context.Remove(deliveryRoute);
            _context.SaveChanges();
        }

        public void EditMedicationDeliveryRoute(MedicationDeliveryRoute deliveryRoute)
        {
            _context.Update(deliveryRoute);
            _context.SaveChanges();
        }

        public void AddMedicationFrequency(MedicationFrequency frequency)
        {
            _context.Add(frequency);
            _context.SaveChanges();
        }

        public void DeleteMedicationFrequency(MedicationFrequency frequency)
        {
            _context.Remove(frequency);
            _context.SaveChanges();
        }

        public void EditMedicationFrequency(MedicationFrequency frequency)
        {
            _context.Update(frequency);
            _context.SaveChanges();
        }

        //this will need a patient medication add and edit/delete

        public void AddPatientMedicationList(PatientMedicationList pml)
        {
            _context.Add(pml);
            _context.SaveChanges();
        }

        public void DeletePatientMedicationList(PatientMedicationList pml)
        {
            _context.Remove(pml);
            _context.SaveChanges();
        }

        public void EditPatientMedicationList(PatientMedicationList pml)
        {
            _context.Update(pml);
            _context.SaveChanges();
        }

        public void AddRequest(Request request)
        {
            _context.Add(request);
            _context.SaveChanges();
        }

        public void DeleteRequest(Request request)
        {
            _context.Remove(request);
            _context.SaveChanges();
        }
        public void EditRequest(Request request)
        {
            _context.Update(request);
            _context.SaveChanges();
        }

        public void AddRequestPurpose(RequestPurpose requestPurpose)
        {
            _context.Add(requestPurpose);
            _context.SaveChanges();
        }

        public void DeleteRequestPurpose(RequestPurpose requestPurpose)
        {
            _context.Remove(requestPurpose);
            _context.SaveChanges();
        }

        public void EditRequestPurpose(RequestPurpose requestPurpose)
        {
            _context.Update(requestPurpose);
            _context.SaveChanges();
        }
        public void AddRequestPriority(RequestPriority requestPriority)
        {
            _context.Add(requestPriority);
            _context.SaveChanges();
        }

        public void DeleteRequestPriority(RequestPriority requestPriority)
        {
            _context.Remove(requestPriority);
            _context.SaveChanges();
        }

        public void EditRequestPriority(RequestPriority requestPriority)
        {
            _context.Update(requestPriority);
            _context.SaveChanges();
        }

        public void AddDocumentRequested(DocumentRequested documentRequested)
        {
            _context.Add(documentRequested);
            _context.SaveChanges();
        }

        public void DeleteDocumentRequested(DocumentRequested documentRequested)
        {
            _context.Remove(documentRequested);
            _context.SaveChanges();
        }

        public void EditDocumentRequested(DocumentRequested documentRequested)
        {
            _context.Update(documentRequested);
            _context.SaveChanges();
        }

        public void AddRequestReleaseFormat(RequestReleaseFormat requestReleaseFormat)
        {
            _context.Add(requestReleaseFormat);
            _context.SaveChanges();
        }

        public void DeleteRequestReleaseFormat(RequestReleaseFormat requestReleaseFormat)
        {
            _context.Remove(requestReleaseFormat);
            _context.SaveChanges();
        }

        public void EditRequestReleaseFormat(RequestReleaseFormat requestReleaseFormat)
        {
            _context.Update(requestReleaseFormat);
            _context.SaveChanges();
        }

        public void AddRequestStatus(RequestStatus requestStatus)
        {
            _context.Add(requestStatus);
            _context.SaveChanges();
        }

        public void DeleteRequestStatus(RequestStatus requestStatus)
        {
            _context.Remove(requestStatus);
            _context.SaveChanges();
        }

        public void EditRequestStatus(RequestStatus requestStatus)
        {
            _context.Update(requestStatus);
            _context.SaveChanges();
        }

        public void AddRequestStatusReason(RequestStatusReason requestStatusReason)
        {
            _context.Add(requestStatusReason);
            _context.SaveChanges();
        }

        public void DeleteRequestStatusReason(RequestStatusReason requestStatusReason)
        {
            _context.Remove(requestStatusReason);
            _context.SaveChanges();
        }

        public void EditRequestStatusReason(RequestStatusReason requestStatusReason)
        {
            _context.Update(requestStatusReason);
            _context.SaveChanges();
        }

        public void AddItemStatus(ItemStatus itemStatus)
        {
            _context.Add(itemStatus);
            _context.SaveChanges();
        }

        public void DeleteItemStatus(ItemStatus itemStatus)
        {
            _context.Remove(itemStatus);
            _context.SaveChanges();
        }

        public void EditItemStatus(ItemStatus itemStatus)
        {
            _context.Update(itemStatus);
            _context.SaveChanges();
        }

        public void AddRequestedItem(RequestedItem requestedItem)
        {
            _context.Add(requestedItem);
            _context.SaveChanges();
        }
        public void EditRequestedItem(RequestedItem requestedItem)
        {
            _context.Update(requestedItem);
            _context.SaveChanges();
        }
        public void DeleteRequestedItem(RequestedItem requestedItem)
        {
            _context.Remove(requestedItem);
            _context.SaveChanges();
        }
        public void AddPatientRepresentative(PatientRepresentative patientRepresentative)
        {
            _context.Add(patientRepresentative);
            _context.SaveChanges();
        }
        public void EditPatientRepresentative(PatientRepresentative patientRepresentative)
        {
            _context.Update(patientRepresentative);
            _context.SaveChanges();
        }
        public void DeletePatientRepresentative(PatientRepresentative patientRepresentative)
        {
            _context.Remove(patientRepresentative);
            _context.SaveChanges();
        }
        public void AddDisclosure(Disclosure disclosure)
        {
            _context.Add(disclosure);
            _context.SaveChanges();
        }
        public void EditDisclosure(Disclosure disclosure)
        {
            _context.Update(disclosure);
            _context.SaveChanges();
        }
        public void DeleteDisclosure(Disclosure disclosure)
        {
            _context.Remove(disclosure);
            _context.SaveChanges();
        }

        public void AddDisclosureFee(DisclosureFee disclosureFee)
        {
            _context.Add(disclosureFee);
            _context.SaveChanges();
        }

        public void EditDisclosureFee(DisclosureFee disclosureFee)
        {
            _context.Update(disclosureFee);
            _context.SaveChanges();
        }

        public void DeleteDisclosureFee(DisclosureFee disclosureFee)
        {
            _context.Remove(disclosureFee);
            _context.SaveChanges();
        }

        public void AddDisclosureFeeType(DisclosureFeeType disclosureFeeType)
        {
            _context.Add(disclosureFeeType);
            _context.SaveChanges();
        }

        public void EditDisclosureFeeType(DisclosureFeeType disclosureFeeType)
        {
            _context.Update(disclosureFeeType);
            _context.SaveChanges();
        }

        public void DeleteDisclosureFeeType(DisclosureFeeType disclosureFeeType)
        {
            _context.Remove(disclosureFeeType);
            _context.SaveChanges();
        }

        public void AddDisclosurePayment(DisclosurePayment disclosurePayment)
        {
            _context.Add(disclosurePayment);
            _context.SaveChanges();
        }
        public void EditDisclosurePayment(DisclosurePayment disclosurePayment)
        {
            _context.Update(disclosurePayment);
            _context.SaveChanges();
        }
        public void DeleteDisclosurePayment(DisclosurePayment disclosurePayment)
        {
            _context.Remove(disclosurePayment);
            _context.SaveChanges();
        }

        public void AddPaymentType(PaymentType paymentType)
        {
            _context.Add(paymentType);
            _context.SaveChanges();
        }

        public void EditPaymentType(PaymentType paymentType)
        {
            _context.Update(paymentType);
            _context.SaveChanges();
        }

        public void DeletePaymentType(PaymentType paymentType)
        {
            _context.Remove(paymentType);
            _context.SaveChanges();
        }

        public void AddPhysician(Physician physician)
        {
            _context.Add(physician);
            _context.SaveChanges();
        }

        public void EditPhysician(Physician physician)
        {
            _context.Update(physician);
            _context.SaveChanges();
        }

        public void DeletePhysician(Physician physician)
        {
            _context.Remove(physician);
            _context.SaveChanges();
        }

        public void AddPhysicianRole(PhysicianRole physicianRole)
        {
            _context.Add(physicianRole);
            _context.SaveChanges();
        }

        public void EditPhysicianRole(PhysicianRole physicianRole)
        {
            _context.Update(physicianRole);
            _context.SaveChanges();
        }

        public void DeletePhysicianRole(PhysicianRole physicianRole)
        {
            _context.Remove(physicianRole);
            _context.SaveChanges();
        }

        public void AddProviderType(ProviderType providerType)
        {
            _context.Add(providerType);
            _context.SaveChanges();
        }

        public void EditProviderType(ProviderType providerType)
        {
            _context.Update(providerType);
            _context.SaveChanges();
        }

        public void DeleteProviderType(ProviderType providerType)
        {
            _context.Remove(providerType);
            _context.SaveChanges();
        }

        public void AddSpecialty(Specialty specialty)
        {
            _context.Add(specialty);
            _context.SaveChanges();
        }

        public void EditSpecialty(Specialty specialty)
        {
            _context.Update(specialty);
            _context.SaveChanges();
        }

        public void DeleteSpecialty(Specialty specialty)
        {
            _context.Remove(specialty);
            _context.SaveChanges();
        }

        public void AddProviderStatus(ProviderStatus providerStatus)
        {
            _context.Add(providerStatus);
            _context.SaveChanges();
        }

        public void EditProviderStatus(ProviderStatus providerStatus)
        {
            _context.Update(providerStatus);
            _context.SaveChanges();
        }

        public void DeleteProviderStatus(ProviderStatus providerStatus)
        {
            _context.Remove(providerStatus);
            _context.SaveChanges();
        }

        public void AddPriority(Priority priority)
        {
            _context.Add(priority);
            _context.SaveChanges();
        }

        public void EditPriority(Priority priority)
        {
            _context.Update(priority);
            _context.SaveChanges();
        }

        public void DeletePriority(Priority priority)
        {
            _context.Remove(priority);
            _context.SaveChanges();
        }

        public void AddChargeDefinition(ChargeDefinition chargeDefinition)
        {
            _context.Add(chargeDefinition);
            _context.SaveChanges();
        }

        public void EditChargeDefinition(ChargeDefinition chargeDefinition)
        {
            _context.Update(chargeDefinition);
            _context.SaveChanges();
        }

        public void DeleteChargeDefinition(ChargeDefinition chargeDefinition)
        {
            _context.Remove(chargeDefinition);
            _context.SaveChanges();
        }

        public void AddRevenueCode(RevenueCode revenueCode)
        {
            _context.Add(revenueCode);
            _context.SaveChanges();
        }

        public void EditRevenueCode(RevenueCode revenueCode)
        {
            _context.Update(revenueCode);
            _context.SaveChanges();
        }

        public void DeleteRevenueCode(RevenueCode revenueCode)
        {
            _context.Remove(revenueCode);
            _context.SaveChanges();
        }

        public void AddRequesterType(RequesterType requesterType)
        {
            _context.Add(requesterType);
            _context.SaveChanges();
        }

        public void EditRequesterType(RequesterType requesterType)
        {
            _context.Update(requesterType);
            _context.SaveChanges();
        }

        public void DeleteRequesterType(RequesterType requesterType)
        {
            _context.Remove(requesterType);
            _context.SaveChanges();
        }

        public void AddRequesterStatus(RequesterStatus requesterStatus)
        {
            _context.Add(requesterStatus);
            _context.SaveChanges();
        }

        public void EditRequesterStatus(RequesterStatus requesterStatus)
        {
            _context.Update(requesterStatus);
            _context.SaveChanges();
        }

        public void DeleteRequesterStatus(RequesterStatus requesterStatus)
        {
            _context.Remove(requesterStatus);
            _context.SaveChanges();
        }

        public void AddAspNetPermission(AspNetPermission aspNetPermission)
        {
            _context.Add(aspNetPermission);
            _context.SaveChanges();
        }

        public void EditAspNetPermission(AspNetPermission aspNetPermission)
        {
            _context.Update(aspNetPermission);
            _context.SaveChanges();
        }
        public void DeleteAspNetPermission(AspNetPermission aspNetPermission)
        {
            _context.Remove(aspNetPermission);
            _context.SaveChanges();
        }

        public void AddAspNetRolePermission(AspNetRolePermission aspNetRolePermission)
        {
            _context.Add(aspNetRolePermission);
            _context.SaveChanges();
        }

        public void EditAspNetRolePermission(AspNetRolePermission aspNetRolePermission)
        {
            _context.Update(aspNetRolePermission);
            _context.SaveChanges();
        }

        public void DeleteAspNetRolePermission(AspNetRolePermission aspNetRolePermission)
        {
            _context.Remove(aspNetRolePermission);
            _context.SaveChanges();
        }

        public void AddBloodPressureRouteType(BloodPressureRouteType bloodPressureRouteType)
        {
            _context.Add(bloodPressureRouteType);
            _context.SaveChanges();
        }

        public void EditBloodPressureRouteType(BloodPressureRouteType bloodPressureRouteType)
        {
            _context.Update(bloodPressureRouteType);
            _context.SaveChanges();
        }

        public void DeleteBloodPressureRouteType(BloodPressureRouteType bloodPressureRouteType)
        {
            _context.Remove(bloodPressureRouteType);
            _context.SaveChanges();
        }

        public void AddBmiMethod(Bmimethod bmiMethod)
        {
            _context.Add(bmiMethod);
            _context.SaveChanges();
        }

        public void EditBmiMethod(Bmimethod bmiMethod)
        {
            _context.Update(bmiMethod);
            _context.SaveChanges();
        }
        public void DeleteBmiMethod(Bmimethod bmiMethod)
        {
            _context.Remove(bmiMethod);
            _context.SaveChanges();
        }

        public void AddCareSystemType(CareSystemType careSystemType)
        {
            _context.Add(careSystemType);
            _context.SaveChanges();
        }


        public void EditCareSystemType(CareSystemType careSystemType)
        {
            _context.Update(careSystemType);
            _context.SaveChanges();
        }

        public void DeleteCareSystemType(CareSystemType careSystemType)
        {
            _context.Remove(careSystemType);
            _context.SaveChanges();
        }

        public void AddO2DeliveryType(O2deliveryType o2DeliveryType)
        {
            _context.Add(o2DeliveryType);
            _context.SaveChanges();
        }

        public void EditO2DeliveryType(O2deliveryType o2DeliveryType)
        {
            _context.Update(o2DeliveryType);
            _context.SaveChanges();
        }

        public void DeleteO2DeliveryType(O2deliveryType o2DeliveryType)
        {
            _context.Remove(o2DeliveryType);
            _context.SaveChanges();
        }

        public void AddPainScaleType(PainScaleType painScaleType)
        {
            _context.Add(painScaleType);
            _context.SaveChanges();
        }


        public void EditPainScaleType(PainScaleType painScaleType)
        {
            _context.Update(painScaleType);
            _context.SaveChanges();
        }

        public void DeletePainScaleType(PainScaleType painScaleType)
        {
            _context.Remove(painScaleType);
            _context.SaveChanges();
        }

        public void AddPcaCommentType(PcacommentType pcacommentType)
        {
            _context.Add(pcacommentType);
            _context.SaveChanges();
        }

        public void EditPcaCommentType(PcacommentType pcacommentType)
        {
            _context.Update(pcacommentType);
            _context.SaveChanges();
        }

        public void DeletePcaCommentType(PcacommentType pcacommentType)
        {
            _context.Remove(pcacommentType);
            _context.SaveChanges();
        }

        public void AddPulseRouteType(PulseRouteType pulseRouteType)
        {
            _context.Add(pulseRouteType);
            _context.SaveChanges();
        }

        public void EditPulseRouteType(PulseRouteType pulseRouteType)
        {
            _context.Update(pulseRouteType);
            _context.SaveChanges();
        }

        public void DeletePulseRouteType(PulseRouteType pulseRouteType)
        {
            _context.Remove(pulseRouteType);
            _context.SaveChanges();
        }

        public void AddTempRouteType(TempRouteType tempRouteType)
        {
            _context.Add(tempRouteType);
            _context.SaveChanges();
        }

        public void EditTempRouteType(TempRouteType tempRouteType)
        {
            _context.Update(tempRouteType);
            _context.SaveChanges();
        }

        public void DeleteTempRouteType(TempRouteType tempRouteType)
        {
            _context.Remove(tempRouteType);
            _context.SaveChanges();
        }

        public void AddCareSystemParameter(CareSystemParameter careSystemParameter)
        {
            _context.Add(careSystemParameter);
            _context.SaveChanges();
        }

        public void EditCareSystemParameter(CareSystemParameter careSystemParameter)
        {
            _context.Update(careSystemParameter);
            _context.SaveChanges();
        }

        public void DeleteCareSystemParameter(CareSystemParameter careSystemParameter)
        {
            _context.Remove(careSystemParameter);
            _context.SaveChanges();
        }

        public void AddPainParameter(PainParameter painParameter)
        {
            _context.Add(painParameter);
            _context.SaveChanges();
        }

        public void EditPainParameter(PainParameter painParameter)
        {
            _context.Update(painParameter);
            _context.SaveChanges();
        }

        public void DeletePainParameter(PainParameter painParameter)
        {
            _context.Remove(painParameter);
            _context.SaveChanges();
        }

        public void AddPainRating(PainRating painRating)
        {
            _context.Add(painRating);
            _context.SaveChanges();
        }

        public void EditPainRating(PainRating painRating)
        {
            _context.Update(painRating);
            _context.SaveChanges();
        }
        public void DeletePainRating(PainRating painRating)
        {
            _context.Remove(painRating);
            _context.SaveChanges();
        }

        public void AddDocument(Document document)
        {
            _context.Add(document);
            _context.SaveChanges();
        }
        public void EditDocument(Document document)
        {
            _context.Update(document);
            _context.SaveChanges();
        }
        public void DeleteDocument(Document document)
        {
            _context.Remove(document);
            _context.SaveChanges();
        }
        public void AddDocumentType(DocumentType documentType)
        {
            _context.Add(documentType);
            _context.SaveChanges();
        }

        public void EditDocumentType(DocumentType documentType)
        {
            _context.Update(documentType);
            _context.SaveChanges();
        }

        public void DeleteDocumentType(DocumentType documentType)
        {
            _context.Remove(documentType);
            _context.SaveChanges();
        }

        public void AddAbnormalCondition(AbnormalCondition abnormalCondition)
        {
            _context.Add(abnormalCondition);
            _context.SaveChanges();
        }

        public void EditAbnormalCondition(AbnormalCondition abnormalCondition)
        {
            _context.Update(abnormalCondition);
            _context.SaveChanges();
        }

        public void DeleteAbnormalCondition(AbnormalCondition abnormalCondition)
        {
            _context.Remove(abnormalCondition);
            _context.SaveChanges();
        }

        public void AddBirthPlaceType(BirthPlaceType birthPlaceType)
        {
            _context.Add(birthPlaceType);
            _context.SaveChanges();
        }

        public void EditBirthPlaceType(BirthPlaceType birthPlaceType)
        {
            _context.Update(birthPlaceType);
            _context.SaveChanges();
        }

        public void DeleteBirthPlaceType(BirthPlaceType birthPlaceType)
        {
            _context.Remove(birthPlaceType);
            _context.SaveChanges();
        }

        public void AddCharacteristicOfLabor(CharacteristicOfLabor characteristicOfLabor)
        {
            _context.Add(characteristicOfLabor);
            _context.SaveChanges();
        }

        public void EditCharacteristicOfLabor(CharacteristicOfLabor characteristicOfLabor)
        {
            _context.Update(characteristicOfLabor);
            _context.SaveChanges();
        }

        public void DeleteCharacteristicOfLabor(CharacteristicOfLabor characteristicOfLabor)
        {
            _context.Remove(characteristicOfLabor);
            _context.SaveChanges();
        }

        public void AddCongenitalAnomaly(CongenitalAnomaly congenitalAnomaly)
        {
            _context.Add(congenitalAnomaly);
            _context.SaveChanges();
        }

        public void EditCongenitalAnomaly(CongenitalAnomaly congenitalAnomaly)
        {
            _context.Update(congenitalAnomaly);
            _context.SaveChanges();
        }

        public void DeleteCongenitalAnomaly(CongenitalAnomaly congenitalAnomaly)
        {
            _context.Remove(congenitalAnomaly);
            _context.SaveChanges();
        }

        public void AddFetalPresentationAtBirth(FetalPresentationAtBirth fetalPresentationAtBirth)
        {
            _context.Add(fetalPresentationAtBirth);
            _context.SaveChanges();
        }

        public void EditFetalPresentationAtBirth(FetalPresentationAtBirth fetalPresentationAtBirth)
        {
            _context.Update(fetalPresentationAtBirth);
            _context.SaveChanges();
        }

        public void DeleteFetalPresentationAtBirth(FetalPresentationAtBirth fetalPresentationAtBirth)
        {
            _context.Remove(fetalPresentationAtBirth);
            _context.SaveChanges();
        }

        public void AddFinalRouteAndMethodOfDelivery(FinalRouteAndMethodOfDelivery finalRouteAndMethodOfDelivery)
        {
            _context.Add(finalRouteAndMethodOfDelivery);
            _context.SaveChanges();
        }

        public void EditFinalRouteAndMethodOfDelivery(FinalRouteAndMethodOfDelivery finalRouteAndMethodOfDelivery)
        {
            _context.Update(finalRouteAndMethodOfDelivery);
            _context.SaveChanges();
        }

        public void DeleteFinalRouteAndMethodOfDelivery(FinalRouteAndMethodOfDelivery finalRouteAndMethodOfDelivery)
        {
            _context.Remove(finalRouteAndMethodOfDelivery);
            _context.SaveChanges();
        }

        public void AddMaternalMorbidity(MaternalMorbidity maternalMorbidity)
        {
            _context.Add(maternalMorbidity);
            _context.SaveChanges();
        }

        public void EditMaternalMorbidity(MaternalMorbidity maternalMorbidity)
        {
            _context.Update(maternalMorbidity);
            _context.SaveChanges();
        }

        public void DeleteMaternalMorbidity(MaternalMorbidity maternalMorbidity)
        {
            _context.Remove(maternalMorbidity);
            _context.SaveChanges();
        }

        public void AddOnsetOfLabor(OnsetOfLabor onsetOfLabor)
        {
            _context.Add(onsetOfLabor);
            _context.SaveChanges();
        }

        public void EditOnsetOfLabor(OnsetOfLabor onsetOfLabor)
        {
            _context.Update(onsetOfLabor);
            _context.SaveChanges();
        }

        public void DeleteOnsetOfLabor(OnsetOfLabor onsetOfLabor)
        {
            _context.Remove(onsetOfLabor);
            _context.SaveChanges();
        }

        public void AddPregnancyInfection(PregnancyInfection pregnancyInfection)
        {
            _context.Add(pregnancyInfection);
            _context.SaveChanges();
        }

        public void EditPregnancyInfection(PregnancyInfection pregnancyInfection)
        {
            _context.Update(pregnancyInfection);
            _context.SaveChanges();
        }

        public void DeletePregnancyInfection(PregnancyInfection pregnancyInfection)
        {
            _context.Remove(pregnancyInfection);
            _context.SaveChanges();
        }

        public void AddPregnancyRiskFactor(PregnancyRiskFactor pregnancyRiskFactor)
        {
            _context.Add(pregnancyRiskFactor);
            _context.SaveChanges();
        }

        public void EditPregnancyRiskFactor(PregnancyRiskFactor pregnancyRiskFactor)
        {
            _context.Update(pregnancyRiskFactor);
            _context.SaveChanges();
        }

        public void DeletePregnancyRiskFactor(PregnancyRiskFactor pregnancyRiskFactor)
        {
            _context.Remove(pregnancyRiskFactor);
            _context.SaveChanges();
        }
        public void AddBirth(Birth birth)
        {
            _context.Add(birth);
            _context.SaveChanges();
        }


        public void EditBirth(Birth birth)
        {
            _context.Update(birth);
            _context.SaveChanges();
        }

        public void DeleteBirth(Birth birth)
        {
            var relatedNewborns = _context.Newborns.Where(n => n.BirthId == birth.BirthId);
            foreach (var newborn in relatedNewborns)
            {
                var relatedLaborAndDeliveries = _context.LaborAndDeliveries.Where(l => l.NewbornId == newborn.NewbornId);
                foreach (var laborAndDelivery in relatedLaborAndDeliveries)
                {
                    _context.Remove(laborAndDelivery);
                }

                _context.Remove(newborn);
            }

            var relatedPrenatals = _context.Prenatals.Where(p => p.BirthId == birth.BirthId);
            foreach (var prenatal in relatedPrenatals)
            {
                _context.Remove(prenatal);
            }

            _context.Remove(birth);
            _context.SaveChanges();
        }

        public Birth SaveOrUpdateBirth(Birth birth)
        {
            if (birth.BirthId == 0)
            {
                _context.Births.Add(birth);
            }
            else
            {
                var existingBirth = _context.Births.FirstOrDefault(b => b.BirthId == birth.BirthId);
                if (existingBirth != null)
                {
                    _context.Entry(existingBirth).CurrentValues.SetValues(birth);
                }
            }

            _context.SaveChanges();
            return birth;
        }
      
        public void AddBirthFather(BirthFather birthFather)
        {
            _context.Add(birthFather);
            _context.SaveChanges();
        }

        public void EditBirthFather(BirthFather birthFather)
        {
            _context.Update(birthFather);
            _context.SaveChanges();
        }

        public void DeleteBirthFather(BirthFather birthFather)
        {
            _context.Remove(birthFather);
            _context.SaveChanges();
        }
        public BirthFather SaveOrUpdateBirthFather(BirthFather birthFather)
        {
            if (birthFather.BirthFatherId == 0)
            {
                _context.BirthFathers.Add(birthFather);
            }
            else
            {
                _context.BirthFathers.Update(birthFather);
            }
            _context.SaveChanges();
            return birthFather;
        }


        public void AddNewborn(Newborn newborn)
        {
            _context.Add(newborn);
            _context.SaveChanges();
        }

        public void EditNewborn(Newborn newborn)
        {
            _context.Update(newborn);
            _context.SaveChanges();
        }

        public void DeleteNewborn(Newborn newborn)
        {
            var relatedLaborAndDeliveries = _context.LaborAndDeliveries.Where(l => l.NewbornId == newborn.NewbornId);
            foreach (var laborAndDelivery in relatedLaborAndDeliveries)
            {
                _context.Remove(laborAndDelivery);
            }

            _context.Remove(newborn);
            _context.SaveChanges();
        }

        public void AddPrenatal(Prenatal prenatal)
        {
            _context.Add(prenatal);
            _context.SaveChanges();
        }

        public void EditPrenatal(Prenatal prenatal)
        {
            _context.Update(prenatal);
            _context.SaveChanges();
        }

        public void DeletePrenatal(Prenatal prenatal)
        {
            _context.Remove(prenatal);
            _context.SaveChanges();
        }

        public Prenatal SaveOrUpdatePrenatal(Prenatal prenatal)
        {

            if (prenatal.PrenatalId == 0)
            {
                _context.Prenatals.Add(prenatal);
            }
            else
            {
                _context.Prenatals.Update(prenatal);
            }

            _context.SaveChanges();

            _context.Entry(prenatal).Collection(p => p.Infections).Load();
            _context.Entry(prenatal).Collection(p => p.RiskFactors).Load();

            return prenatal;
        }


        public void AddLaborAndDelivery(LaborAndDelivery laborAndDelivery)
        {
            _context.Add(laborAndDelivery);
            _context.SaveChanges();
        }

        public void EditLaborAndDelivery(LaborAndDelivery laborAndDelivery)
        {
            _context.Update(laborAndDelivery);
            _context.SaveChanges();
        }

        public void DeleteLaborAndDelivery(LaborAndDelivery laborAndDelivery)
        {
            _context.Remove(laborAndDelivery);
            _context.SaveChanges();
        }

        public LaborAndDelivery SaveOrUpdateLaborAndDelivery (LaborAndDelivery
        laborAndDelivery)
        {
            if (laborAndDelivery.LaborAndDeliveryId == 0)
            {
                _context.LaborAndDeliveries.Add(laborAndDelivery);
            }
            else
            {
                _context.LaborAndDeliveries.Update(laborAndDelivery);
            }
            _context.SaveChanges();
            return laborAndDelivery;
        }
 


        /// <summary>
        /// Returns medications with "amount = pageSize" and starting from "pageNumber * pageSize"
        /// </summary>
        public IQueryable<Medication> MedicationListPages(int pageNumber = 1, int pageSize = 20)
        {
            // Formula to skip for page size of n
            int pageSkip = (pageNumber - 1) * pageSize;

            return _context.Medications
                .Skip(pageSkip)
                .Take(pageSize);
        }
    }
}
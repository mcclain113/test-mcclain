using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.DTOs;
using IS_Proj_HIT.ViewModels;
using IS_Proj_HIT.ViewModels.PatientVm;
using Microsoft.EntityFrameworkCore;

namespace IS_Proj_HIT.Entities.Data
{
    public interface IWCTCHealthSystemRepository
    {
        #region IQueryables
        IQueryable<AbnormalCondition> AbnormalConditions { get; }
        IQueryable<Address> Addresses { get; }
        IQueryable<AddressState> AddressStates { get; }        
        IQueryable<AdvancedDirective> AdvancedDirectives { get; }        
        IQueryable<RequesterType> RequesterTypes { get; }
        IQueryable<RequesterStatus> RequesterStatuses { get; }
        IQueryable<Requester> Requesters { get; }
        IQueryable<CareSystemAssessment> CareSystemAssessments { get; }
        IQueryable<CareSystemType> CareSystemAssessmentTypes { get; }
        IQueryable<CareSystemParameter> CareSystemParameters { get; }
        IQueryable<BirthPlaceType> BirthPlaceTypes { get; }
        IQueryable<CharacteristicOfLabor> CharacteristicOfLabors { get; }
        IQueryable<CongenitalAnomaly> CongenitalAnomalies { get; }
        IQueryable<FetalPresentationAtBirth> FetalPresentationAtBirths { get; }
        IQueryable<FinalRouteAndMethodOfDelivery> FinalRouteAndMethodOfDeliveries { get; }
        IQueryable<MaternalMorbidity> MaternalMorbidities { get; }
        IQueryable<OnsetOfLabor> OnsetOfLabors { get; }
        IQueryable<PregnancyInfection> PregnancyInfections { get; }
        IQueryable<PregnancyRiskFactor> PregnancyRiskFactors { get; }

        IQueryable<BloodPressureRouteType> BloodPressureRouteTypes { get; }
        IQueryable<Bmimethod> BmiMethods { get; }
        IQueryable<PcapainAssessment> PainAssessments { get; }
        IQueryable<PainRating> PainRatings { get; }
        IQueryable<PainRatingImage> PainRatingImages { get; }
        IQueryable<PainParameter> PainParameters { get; }
        IQueryable<PainScaleType> PainScaleTypes { get; }
        IQueryable<O2deliveryType> O2DeliveryTypes { get; }
        IQueryable<PulseRouteType> PulseRouteTypes { get; }
        IQueryable<TempRouteType> TempRouteTypes { get; }
        IQueryable<Pcarecord> PcaRecords { get; }
        IQueryable<Pcacomment> PcaComments { get; }
        IQueryable<PcacommentType> PcaCommentTypes { get; }
        IQueryable<AdmitType> AdmitTypes { get; }
        IQueryable<Ethnicity> Ethnicities { get; }
        IQueryable<Gender> Genders { get; }
        IQueryable<GenderPronoun> GenderPronouns { get; }
        IQueryable<Discharge> Discharges { get; }
        IQueryable<Department> Departments { get; }
        IQueryable<EncounterType> EncounterTypes { get; }
        IQueryable<Sex> Sexes { get; }
        IQueryable<Religion> Religions { get; }
        IQueryable<MaritalStatus> MaritalStatuses { get; }
        IQueryable<Patient> Patients { get; }
        IQueryable<Person> Persons { get; }
        IQueryable<PersonContactDetail> PersonContactDetails { get; }
        IQueryable<PersonLanguage> PersonLanguages { get; }
        IQueryable<PersonModeOfContact> PersonModeOfContacts { get; }
        IQueryable<PersonContactTime> PersonContactTimes { get; }
        IQueryable<PersonRace> PersonRaces { get; }
        IQueryable<PatientContactTime> PatientContactTimes { get; }
        IQueryable<PatientModeOfContact> PatientModeOfContacts { get; }
        IQueryable<PlaceOfServiceOutPatient> PlaceOfService { get; }
        IQueryable<PointOfOrigin> PointOfOrigin { get; }
        IQueryable<EducationLevel> EducationLevels { get; }
        IQueryable<Employment> Employments { get; }
        IQueryable<Encounter> Encounters { get; }
        IQueryable<EncounterPhysician> EncounterPhysicians { get; }
        IQueryable<Facility> Facilities { get; }
        IQueryable<PatientContactDetail> PatientContactDetails { get; }
        IQueryable<Physician> Physicians { get; }
        IQueryable<PhysicianRole> PhysicianRoles { get; }
        IQueryable<PatientAlert> PatientAlerts { get; }
        IQueryable<AlertType> AlertTypes { get; }
        IQueryable<ClinicalReminder> ClinicalReminders { get; }
        IQueryable<PatientRestriction> PatientRestrictions { get; }
        IQueryable<Restriction> Restrictions { get; }
        IQueryable<PatientFallRisk> PatientFallRisks { get; }
        IQueryable<FallRisk> FallRisks { get; }
        IQueryable<Allergen> Allergens { get; }
        IQueryable<Reaction> Reactions { get; }
        IQueryable<PatientAllergy> PatientAllergy { get; }
        IQueryable<UserTable> UserTables { get; }
        IQueryable<Language> Languages { get; }
        IQueryable<PatientLanguage> PatientLanguages { get; }
        IQueryable<Race> Races { get; }
        IQueryable<PatientRace> PatientRaces { get; }
        IQueryable<IS_Proj_HIT.Entities.Program> Programs { get; }
        IQueryable<UserFacility> UserFacilities { get; }
        IQueryable<UserProgram> UserPrograms { get; }
        IQueryable<ProgramFacility> ProgramFacilities { get; }
        IQueryable<SecurityQuestion> SecurityQuestions { get; }
        IQueryable<UserSecurityQuestion> UserSecurityQuestions { get; }
        IQueryable<PhysicianAssessment> PhysicianAssessments { get; }
        IQueryable<PhysicianAssessmentAllergy> PhysicianAssessmentAllergies { get; }
        IQueryable<BodySystemAssessment> BodySystemAssessments { get; }
        IQueryable<ExamType> ExamTypes { get; }
        IQueryable<BodySystemType> BodySystemTypes { get; }
        IQueryable<ProgressNote> ProgressNotes { get; }
        IQueryable<NoteType> NoteTypes { get; }
        IQueryable<VisitType> VisitTypes { get; }
        IQueryable<AdmitOrder> AdmitOrders { get; }
        IQueryable<OrderInfo> OrderInfos { get; }
        IQueryable<OrderType> OrderTypes { get; }
        IQueryable<PatientAlias> PatientAliases { get; }
        IQueryable<Relationship> Relationships { get; }
        IQueryable<LegalStatus> LegalStatuses { get; }
        IQueryable<Medication> Medications { get; }
        IQueryable<PatientMedicationList> PatientMedicationLists { get; }
        IQueryable<MedicationFrequency> MedicationFrequencies { get; }
        IQueryable<MedicationBrandName> MedicationBrandNames { get; }
        IQueryable<MedicationGenericName> MedicationGenericNames { get; }
        IQueryable<MedicationDosageForm> MedicationDosageForms { get; }
        IQueryable<MedicationDeliveryRoute> MedicationDeliveryRoutes { get; }
        IQueryable<Country> Countries { get; }
        IQueryable<PreferredModeOfContact> PreferredModesOfContact { get; }
        IQueryable<PreferredContactTime> PreferredContactTimes { get; }
        IQueryable<PatientEmergencyContact> PatientEmergencyContacts { get; }
        IQueryable<InsuranceProvider> InsuranceProviders { get; }
        IQueryable<PatientInsurance> PatientInsurances { get; }
        IQueryable<PatientRepresentative> PatientRepresentatives { get; }
        IQueryable<Disclosure> Disclosures { get; }
        IQueryable<DisclosureFee> DisclosureFees { get; }
        IQueryable<DisclosureFeeType> DisclosureFeeTypes { get; }
        IQueryable<DisclosurePayment> DisclosurePayments { get; }
        IQueryable<PaymentType> PaymentTypes { get; }
        IQueryable<Request> Requests { get; }
        IQueryable<RequestedItem> RequestedItems { get; }
        IQueryable<RequestPurpose> RequestPurposes { get; }
        IQueryable<RequestPriority> RequestPriorities { get; }
        IQueryable<DocumentRequested> DocumentRequesteds { get; }
        IQueryable<RequestReleaseFormat> RequestReleaseFormats { get; }
        IQueryable<RequestStatus> RequestStatuses { get; }
        IQueryable<RequestStatusReason> RequestStatusReasons { get; }
        IQueryable<ProviderType> ProviderTypes { get; }
        IQueryable<Specialty> Specialties { get; }
        IQueryable<ProviderStatus> ProviderStatuses { get; }
        IQueryable<Priority> Priorities { get; }
        IQueryable<ItemStatus> ItemStatuses { get; }
        IQueryable<RevenueCode> RevenueCodes { get; }
        IQueryable<ChargeDefinition> ChargeDefinitions { get; }
        IQueryable<AspNetPermission> AspNetPermissions { get; }
        IQueryable<AspNetRolePermission> AspNetRolePermissions { get; }
        IQueryable<AspNetRole> AspNetRoles { get; }
        IQueryable<AspNetUserRole> AspNetUserRoles { get; }
        IQueryable<Document> Documents { get; }
        IQueryable<DocumentType> DocumentTypes { get; }
        IQueryable<Birth> Births { get; }
        IQueryable<BirthFather> BirthFathers { get; }
        IQueryable<Newborn> Newborns { get; }
        IQueryable<Prenatal> Prenatals { get; }
        IQueryable<LaborAndDelivery> LaborAndDeliveries { get; }

        #endregion
        void AddAlertType(AlertType alertType);
        void EditAlertType(AlertType alertType);
        void DeleteAlertType(AlertType alertType);

        void AddAdmitType(AdmitType admitType);
        void EditAdmitType(AdmitType admitType);
        void DeleteAdmitType(AdmitType admitType);

        void AddAdvancedDirective(AdvancedDirective advancedDirective);
        void EditAdvancedDirective(AdvancedDirective advancedDirective);
        void DeleteAdvancedDirective(AdvancedDirective advancedDirective);

        void AddAllergen(Allergen allergen);
        void EditAllergen(Allergen allergen);
        void DeleteAllergen(Allergen allergen);

        void AddFallRisk(FallRisk fallRisk);
        void EditFallRisk(FallRisk fallRisk);
        void DeleteFallRisk(FallRisk fallRisk);

        void AddRestriction(Restriction restriction);
        void EditRestriction(Restriction restriction);
        void DeleteRestriction(Restriction restriction);

        void AddDepartment(Department department);
        void EditDepartment(Department department);
        void DeleteDepartment(Department department);

        void AddDischarge(Discharge discharge);
        void EditDischarge(Discharge discharge);
        void DeleteDischarge(Discharge discharge);

        void AddEncounterType(EncounterType encounterType);
        void EditEncounterType(EncounterType encounterType);
        void DeleteEncounterType(EncounterType encounterType);

        void AddFacility(Facility facility);
        void EditFacility(Facility facility);
        void DeleteFacility(Facility facility);
        Facility SaveOrUpdateFacility (Facility facility);
        

        void AddPlaceOfService(PlaceOfServiceOutPatient placeOfServiceOutPatient);
        void EditPlaceOfService(PlaceOfServiceOutPatient placeOfServiceOutPatient);
        void DeletePlaceOfService(PlaceOfServiceOutPatient placeOfServiceOutPatient);

        void AddPointOfOrigin(PointOfOrigin pointOfOrigin);
        void EditPointOfOrigin(PointOfOrigin pointOfOrigin);
        void DeletePointOfOrigin(PointOfOrigin pointOfOrigin);

        void AddProgramFacility(ProgramFacility programFacility);
        void DeleteProgramFacility(ProgramFacility programFacility);
        void EditProgramFacility(ProgramFacility programFacility);

        void AddUserFacility(UserFacility userFacility);
        void DeleteUserFacility(UserFacility userFacility);
        void EditUserFacility(UserFacility userFacility);

        void AddUserProgram(UserProgram userProgram);
        void DeleteUserProgram(UserProgram userProgram);
        void EditUserProgram(UserProgram userProgram);

        void AddRequester(Requester requester);
        void EditRequester(Requester requester);
        void DeleteRequester(Requester requester);

        void AddProgram(Program program);
        void DeleteProgram(Program program);
        void EditProgram(Program program);

        void AddPatient(Patient patient);
        void DeletePatient(Patient patient);
        void EditPatient(Patient patient);

        void AddPerson(Person person);
        void EditPerson(Person person);
        void DeletePerson(Person person);

        void AddPersonContactDetail(PersonContactDetail pcd);
        void DeletePersonContactDetail(PersonContactDetail pcd);
        void EditPersonContactDetail(PersonContactDetail pcd);

        void AddPersonContactTime(PersonContactTime pct);
        void DeletePersonContactTime(PersonContactTime pct);
        void EditPersonContactTime(PersonContactTime pct);
        void AddPersonContactTime(List<PersonContactTime> pctList);
        void DeletePersonContactTime(List<PersonContactTime> pctList);

        void AddPersonLanguage(PersonLanguage personLanguage);
        void DeletePersonLanguage(PersonLanguage personLanguage);
        void EditPersonLanguage(PersonLanguage personLanguage);
        void AddPersonLanguage(List<PersonLanguage> plList);
        void DeletePersonLanguage(List<PersonLanguage> plList);

        void AddPersonModeOfContact(PersonModeOfContact personModeOfContact);
        void DeletePersonModeOfContact(PersonModeOfContact personModeOfContact);
        void EditPersonModeOfContact(PersonModeOfContact personModeOfContact);
        void AddPersonModeOfContact(List<PersonModeOfContact> pmcList);
        void DeletePersonModeOfContact(List<PersonModeOfContact> pmcList);

        void AddPersonRace(PersonRace personRace);
        void EditPersonRace(PersonRace personRace);
        void DeletePersonRace(PersonRace personRace);
        void AddPersonRace(List<PersonRace> prList);
        void DeletePersonRace(List<PersonRace> prList);

        void AddPatientAllergy(PatientAllergy patientAllergy);
        void EditPatientAllergy(PatientAllergy patientAllergy);
        void DeletePatientAllergy(PatientAllergy patientAllergy);

        void AddGender(Gender gender);
        void EditGender(Gender gender);
        void DeleteGender(Gender gender);

        void AddGenderPronoun(GenderPronoun genderPronoun);
        void EditGenderPronoun(GenderPronoun genderPronoun);
        void DeleteGenderPronoun(GenderPronoun genderPronoun);

        void AddPatientContactDetail(PatientContactDetail patientContactDetail);
        void EditPatientContactDetail(PatientContactDetail patientContactDetail);
        void DeletePatientContactDetail(PatientContactDetail patientContactDetail);

        void AddPreferredModeOfContact(PreferredModeOfContact preferredModeOfContact);
        void EditPreferredModeOfContact(PreferredModeOfContact preferredModeOfContact);
        void DeletePreferredModeOfContact(PreferredModeOfContact preferredModeOfContact);

        void AddPreferredContactTime(PreferredContactTime preferredContactTime);
        void EditPreferredContactTime(PreferredContactTime preferredContactTime);
        void DeletePreferredContactTime(PreferredContactTime preferredContactTime);

        void AddReligion(Religion religion);
        void EditReligion(Religion religion);
        void DeleteReligion(Religion religion);

        void AddSex(Sex sex);
        void EditSex(Sex sex);
        void DeleteSex(Sex sex);

        void AddEncounter(Encounter encounter);
        void EditEncounter(Encounter encounter);
        void DeleteEncounter(Encounter encounter);

        void AddAlert(PatientAlert patientAlert);
        void EditAlert(PatientAlert patientAlert);
        void DeleteAlert(PatientAlert patientAlert);

        void AddPatientFallRisk(PatientFallRisk patientFallRisk);
        void EditPatientFallRisk(PatientFallRisk patientFallRisk);
        void DeletePatientFallRisk(PatientFallRisk patientFallRisk);

        void AddPatientRestriction(PatientRestriction patientRestriction);
        void EditPatientRestriction(PatientRestriction patientRestriction);
        void DeletePatientRestriction(PatientRestriction patientRestriction);

        void AddEducationLevel(EducationLevel educationLevel);
        void EditEducationLevel(EducationLevel educationLevel);
        void DeleteEducationLevel(EducationLevel educationLevel);

        void AddEthnicity(Ethnicity ethnicity);
        void EditEthnicity(Ethnicity ethnicity);
        void DeleteEthnicity(Ethnicity ethnicity);


        void AddEmployment(Employment employment);
        void EditEmployment(Employment employment);
        void DeleteEmployment(Employment employment);

        void AddUser(UserTable userTable);
        void DeleteUser(UserTable userTable);
        void EditUser(UserTable userTable);

        void AddPcaRecord(Pcarecord pca);
        void DeletePcaRecord(Pcarecord pca);
        void EditPcaRecord(Pcarecord pca);

        void AddPcaComment(Pcacomment pca);
        void DeletePcaComment(Pcacomment pca);
        void EditPcaComment(Pcacomment pca);

        void AddSystemAssessment(CareSystemAssessment csa);
        void AddSystemAssessments(IList<CareSystemAssessment> csaList);
        void DeleteSystemAssessment(CareSystemAssessment csa);
        void EditSystemAssessment(CareSystemAssessment csa);

        void AddPainAssessment(PcapainAssessment pa);
        void DeletePainAssessment(PcapainAssessment pa);
        void EditPainAssessment(PcapainAssessment pa);

        void AddLanguage(Language language);
        void EditLanguage(Language language);
        void DeleteLanguage(Language language);

        void AddPatientLanguage(PatientLanguage patientLanguage);
        void EditPatientLanguage(PatientLanguage patientLanguage);
        void DeletePatientLanguage(PatientLanguage patientLanguage);

        void AddRace(Race race);
        void EditRace(Race race);
        void DeleteRace(Race race);

        void AddPatientRace(PatientRace race);
        void EditPatientRace(PatientRace race);
        void DeletePatientRace(PatientRace race);

        void AddAddress(Address address);
        void EditAddress(Address address);
        void DeleteAddress(Address address);

        void AddAddressState(AddressState addressState);
        void EditAddressState(AddressState addressState);
        void DeleteAddressState(AddressState addressState);

        void AddCountry(Country country);
        void EditCountry(Country country);
        void DeleteCountry(Country country);

        void AddUserSecurityQuestion(UserSecurityQuestion userSecurityQuestion);
        void DeleteUserSecurityQuestion(UserSecurityQuestion userSecurityQuestion);

        void AddSecurityQuestion(SecurityQuestion securityQuestion);
        void EditSecurityQuestion(SecurityQuestion securityQuestion);
        void DeleteSecurityQuestion(SecurityQuestion securityQuestion);

        void AddPhysicianAssessment(PhysicianAssessment assessment);
        void EditPhysicianAssessment(PhysicianAssessment assessment);
        void AddBodySystemAssessment(BodySystemAssessment bsa);
        void EditBodySystemAssessment(BodySystemAssessment bsa);
        void AddBodySystemAssessments(IList<BodySystemAssessment> bsaList);

        void AddPhysicianAssessmentAllergy(List<PhysicianAssessmentAllergyDto> paaList);
        void DeletePhysicianAssessmentAllergy(List<PhysicianAssessmentAllergy> paaList);
        void AddPhysicianAssessmentAllergy(PhysicianAssessmentAllergy assessmentAllergy);
        void EditPhysicianAssessmentAllergy(PhysicianAssessmentAllergy assessmentAllergy);
        void DeletePhysicianAssessmentAllergy(PhysicianAssessmentAllergy assessmentAllergy);

        void AddProgressNote(ProgressNote progressNote);
        void EditProgressNote(ProgressNote progressNote);
        void DeleteProgressNote(ProgressNote progressNote);

        void AddVisitType(VisitType visitType);
        void EditVisitType(VisitType visitType);
        void DeleteVisitType(VisitType visitType);

        void AddNoteType(NoteType noteType);
        void EditNoteType(NoteType noteType);
        void DeleteNoteType(NoteType noteType);

        void PhysicianDischargeEdit(Encounter encounter);

        void AddOrderInfo(OrderInfo orderInfo);
        void EditOrderInfo(OrderInfo orderInfo);
        void DeleteOrderInfo(OrderInfo orderInfo);

        void AddPatientAlias(PatientAlias patientAlias);
        void DeletePatientAlias(PatientAlias patientAlias);
        void EditPatientAlias(PatientAlias patientAlias);

        void AddRelationship(Relationship relationship);
        void EditRelationship(Relationship relationship);
        void DeleteRelationship(Relationship relationship);

        void AddLegalStatus(LegalStatus legalStatus);
        void EditLegalStatus(LegalStatus legalStatus);
        void DeleteLegalStatus(LegalStatus legalStatus);

        void AddMaritalStatus(MaritalStatus maritalStatus);
        void EditMaritalStatus(MaritalStatus maritalStatus);
        void DeleteMaritalStatus(MaritalStatus maritalStatus);

        void AddPatientModeOfContact(PatientModeOfContact patientModeOfContact);
        void EditPatientModeOfContact(PatientModeOfContact patientModeOfContact);
        void DeletePatientModeOfContact(PatientModeOfContact patientModeOfContact);

        void AddPatientContactTime(PatientContactTime patientContactTime);
        void EditPatientContactTime(PatientContactTime patientContactTime);
        void DeletePatientContactTime(PatientContactTime patientContactTime);

        void AddPatientEmergencyContact(PatientEmergencyContact patientEmergencyContact);
        void EditPatientEmergencyContact(PatientEmergencyContact patientEmergencyContact);
        void DeletePatientEmergencyContact(PatientEmergencyContact patientEmergencyContact);

        void AddInsuranceProvider(InsuranceProvider insuranceProvider);
        void EditInsuranceProvider(InsuranceProvider insuranceProvider);
        void DeleteInsuranceProvider(InsuranceProvider insuranceProvider);

        void AddPatientInsurance(PatientInsurance patientInsurance);

        void EditPatientInsurance(PatientInsurance patientInsurance);

        void AddMedication(Medication medication);
        void EditMedication(Medication medication);
        void DeleteMedication(Medication medication);
        void AddMedicationBrandName(MedicationBrandName medicationBrandName);
        void EditMedicationBrandName(MedicationBrandName medicationBrandName);
        void DeleteMedicationBrandName(MedicationBrandName medicationBrandName);
        void AddMedicationGenericName(MedicationGenericName medicationGenericName);
        void EditMedicationGenericName(MedicationGenericName medicationGenericName);
        void DeleteMedicationGenericName(MedicationGenericName medicationGenericName);
        void AddMedicationDosageForm(MedicationDosageForm dosageForm);
        void EditMedicationDosageForm(MedicationDosageForm dosageForm);
        void DeleteMedicationDosageForm(MedicationDosageForm dosageForm);
        void AddMedicationDeliveryRoute(MedicationDeliveryRoute deliveryRoute);
        void EditMedicationDeliveryRoute(MedicationDeliveryRoute deliveryRoute);
        void DeleteMedicationDeliveryRoute(MedicationDeliveryRoute deliveryRoute);
        void AddMedicationFrequency(MedicationFrequency frequency);
        void EditMedicationFrequency(MedicationFrequency frequency);
        void DeleteMedicationFrequency(MedicationFrequency frequency);

        //patient medication?
        void AddPatientMedicationList(PatientMedicationList patientMedicationList);
        void EditPatientMedicationList(PatientMedicationList patientMedicationList);
        void DeletePatientMedicationList(PatientMedicationList patientMedicationList);

        // Special Pagination methods
        IQueryable<Medication> MedicationListPages(int pageNumber = 1, int pageSize = 20);

        //Disclosure Request
        void AddRequest(Request request);
        void DeleteRequest(Request request);
        void EditRequest(Request request);

        // Disclosure Request Purposes
        void AddRequestPurpose(RequestPurpose requestPurpose);
        void EditRequestPurpose(RequestPurpose requestPurpose);
        void DeleteRequestPurpose(RequestPurpose requestPurpose);

        //Disclosure Request Priorities
        void AddRequestPriority(RequestPriority requestPriority);
        void DeleteRequestPriority(RequestPriority requestPriority);
        void EditRequestPriority(RequestPriority requestPriority);

        //Disclosure Documents Requested
        void AddDocumentRequested(DocumentRequested documentRequested);
        void DeleteDocumentRequested(DocumentRequested documentRequested);
        void EditDocumentRequested(DocumentRequested documentRequested);

        // Disclosure Request Release Format
        void AddRequestReleaseFormat(RequestReleaseFormat requestReleaseFormat);
        void DeleteRequestReleaseFormat(RequestReleaseFormat requestReleaseFormat);
        void EditRequestReleaseFormat(RequestReleaseFormat requestReleaseFormat);

        // Disclosure Request Status
        void AddRequestStatus(RequestStatus requestStatus);
        void DeleteRequestStatus(RequestStatus requestStatus);
        void EditRequestStatus(RequestStatus requestStatus);

        // Disclosure Request Status Reason
        void AddRequestStatusReason(RequestStatusReason requestStatusReason);
        void DeleteRequestStatusReason(RequestStatusReason requestStatusReason);
        void EditRequestStatusReason(RequestStatusReason requestStatusReason);

        void AddItemStatus(ItemStatus itemStatus);
        void DeleteItemStatus(ItemStatus itemStatus);
        void EditItemStatus(ItemStatus itemStatus);

        // Disclosure Request RequestedItem

        void AddRequestedItem(RequestedItem requestedItem);
        void EditRequestedItem(RequestedItem requestedItem);
        void DeleteRequestedItem(RequestedItem requestedItem);

        // Disclosure Requester Type
        void AddRequesterType(RequesterType requesterType);
        void EditRequesterType(RequesterType requesterType);
        void DeleteRequesterType(RequesterType requesterType);

        // Disclosure Requester Status
        void AddRequesterStatus(RequesterStatus requesterStatus);
        void EditRequesterStatus(RequesterStatus requesterStatus);
        void DeleteRequesterStatus(RequesterStatus requesterStatus);

        // Disclosure PatientRepresentative 

        void AddPatientRepresentative(PatientRepresentative patientRepresentative);
        void EditPatientRepresentative(PatientRepresentative patientRepresentative);
        void DeletePatientRepresentative(PatientRepresentative patientRepresentative);

        // Request Disclosure

        void AddDisclosure(Disclosure disclosure);
        void EditDisclosure(Disclosure disclosure);
        void DeleteDisclosure(Disclosure disclosure);

        void AddDisclosureFee(DisclosureFee disclosureFee);
        void EditDisclosureFee(DisclosureFee disclosureFee);
        void DeleteDisclosureFee(DisclosureFee disclosureFee);

        void AddDisclosureFeeType(DisclosureFeeType disclosureFeeType);
        void EditDisclosureFeeType(DisclosureFeeType disclosureFeeType);
        void DeleteDisclosureFeeType(DisclosureFeeType disclosureFeeType);

        // Request DisclosurePaymentType

        void AddDisclosurePayment(DisclosurePayment disclosurePayment);
        void EditDisclosurePayment(DisclosurePayment disclosurePayment);
        void DeleteDisclosurePayment(DisclosurePayment disclosurePayment);

        void AddPaymentType(PaymentType paymentType);
        void EditPaymentType(PaymentType paymentType);
        void DeletePaymentType(PaymentType paymentType);

        // Physician Structure Data
        void AddPhysician(Physician physician);
        void EditPhysician(Physician physician);
        void DeletePhysician(Physician physician);

        void AddPhysicianRole(PhysicianRole physicianRole);
        void EditPhysicianRole(PhysicianRole physicianRole);
        void DeletePhysicianRole(PhysicianRole physicianRole);

        void AddProviderType(ProviderType providerType);
        void EditProviderType(ProviderType providerType);
        void DeleteProviderType(ProviderType providerType);

        void AddSpecialty(Specialty specialty);
        void EditSpecialty(Specialty specialty);
        void DeleteSpecialty(Specialty specialty);

        void AddProviderStatus(ProviderStatus providerStatus);
        void EditProviderStatus(ProviderStatus providerStatus);
        void DeleteProviderStatus(ProviderStatus providerStatus);

        // Priority used in Physician Orders
        void AddPriority(Priority priority);
        void EditPriority(Priority priority);
        void DeletePriority(Priority priority);

        void AddChargeDefinition(ChargeDefinition chargeDefinition);
        void EditChargeDefinition(ChargeDefinition chargeDefinition);
        void DeleteChargeDefinition(ChargeDefinition chargeDefinition);

        void AddRevenueCode(RevenueCode revenueCode);
        void EditRevenueCode(RevenueCode revenueCode);
        void DeleteRevenueCode(RevenueCode revenueCode);

        void AddAspNetPermission(AspNetPermission aspNetPermission);
        void EditAspNetPermission(AspNetPermission aspNetPermission);
        void DeleteAspNetPermission(AspNetPermission aspNetPermission);

        void AddAspNetRolePermission(AspNetRolePermission aspNetRolePermission);
        void EditAspNetRolePermission(AspNetRolePermission aspNetRolePermission);
        void DeleteAspNetRolePermission(AspNetRolePermission aspNetRolePermission);

        void AddBloodPressureRouteType(BloodPressureRouteType bloodPressureRouteType);
        void EditBloodPressureRouteType(BloodPressureRouteType bloodPressureRouteType);
        void DeleteBloodPressureRouteType(BloodPressureRouteType bloodPressureRouteType);

        void AddBmiMethod(Bmimethod bmiMethod);
        void EditBmiMethod(Bmimethod bmiMethod);
        void DeleteBmiMethod(Bmimethod bmiMethod);

        void AddCareSystemType(CareSystemType careSystemType);
        void EditCareSystemType(CareSystemType careSystemType);
        void DeleteCareSystemType(CareSystemType careSystemType);

        void AddO2DeliveryType(O2deliveryType o2DeliveryType);
        void EditO2DeliveryType(O2deliveryType o2DeliveryType);
        void DeleteO2DeliveryType(O2deliveryType o2DeliveryType);

        void AddPainScaleType(PainScaleType painScaleType);
        void EditPainScaleType(PainScaleType painScaleType);
        void DeletePainScaleType(PainScaleType painScaleType);

        void AddPcaCommentType(PcacommentType pcacommentType);
        void EditPcaCommentType(PcacommentType pcacommentType);
        void DeletePcaCommentType(PcacommentType pcacommentType);

        void AddPulseRouteType(PulseRouteType pulseRouteType);
        void EditPulseRouteType(PulseRouteType pulseRouteType);
        void DeletePulseRouteType(PulseRouteType pulseRouteType);

        void AddTempRouteType(TempRouteType tempRouteType);
        void EditTempRouteType(TempRouteType tempRouteType);
        void DeleteTempRouteType(TempRouteType tempRouteType);

        void AddCareSystemParameter(CareSystemParameter careSystemParameter);
        void EditCareSystemParameter(CareSystemParameter careSystemParameter);
        void DeleteCareSystemParameter(CareSystemParameter careSystemParameter);

        void AddPainParameter(PainParameter painParameter);
        void EditPainParameter(PainParameter painParameter);
        void DeletePainParameter(PainParameter painParameter);

        void AddPainRating(PainRating painRating);
        void EditPainRating(PainRating painRating);
        void DeletePainRating(PainRating painRating);

        void AddDocument(Document document);
        void EditDocument(Document document);
        void DeleteDocument(Document document);

        void AddDocumentType(DocumentType documentType);
        void EditDocumentType(DocumentType documentType);
        void DeleteDocumentType(DocumentType documentType);

        void AddAbnormalCondition(AbnormalCondition abnormalCondition);
        void EditAbnormalCondition(AbnormalCondition abnormalCondition);
        void DeleteAbnormalCondition(AbnormalCondition abnormalCondition);

        void AddBirthPlaceType(BirthPlaceType birthPlaceType);
        void EditBirthPlaceType(BirthPlaceType birthPlaceType);
        void DeleteBirthPlaceType(BirthPlaceType birthPlaceType);

        void AddCharacteristicOfLabor(CharacteristicOfLabor characteristicOfLabor);
        void EditCharacteristicOfLabor(CharacteristicOfLabor characteristicOfLabor);
        void DeleteCharacteristicOfLabor(CharacteristicOfLabor characteristicOfLabor);

        void AddCongenitalAnomaly(CongenitalAnomaly congenitalAnomaly);
        void EditCongenitalAnomaly(CongenitalAnomaly congenitalAnomaly);
        void DeleteCongenitalAnomaly(CongenitalAnomaly congenitalAnomaly);

        void AddFetalPresentationAtBirth(FetalPresentationAtBirth fetalPresentationAtBirth);
        void EditFetalPresentationAtBirth(FetalPresentationAtBirth fetalPresentationAtBirth);
        void DeleteFetalPresentationAtBirth(FetalPresentationAtBirth fetalPresentationAtBirth);

        void AddFinalRouteAndMethodOfDelivery(FinalRouteAndMethodOfDelivery finalRouteAndMethodOfDelivery);
        void EditFinalRouteAndMethodOfDelivery(FinalRouteAndMethodOfDelivery finalRouteAndMethodOfDelivery);
        void DeleteFinalRouteAndMethodOfDelivery(FinalRouteAndMethodOfDelivery finalRouteAndMethodOfDelivery);

        void AddMaternalMorbidity(MaternalMorbidity maternalMorbidity);
        void EditMaternalMorbidity(MaternalMorbidity maternalMorbidity);
        void DeleteMaternalMorbidity(MaternalMorbidity maternalMorbidity);

        void AddOnsetOfLabor(OnsetOfLabor onsetOfLabor);
        void EditOnsetOfLabor(OnsetOfLabor onsetOfLabor);
        void DeleteOnsetOfLabor(OnsetOfLabor onsetOfLabor);

        void AddPregnancyInfection(PregnancyInfection pregnancyInfection);
        void EditPregnancyInfection(PregnancyInfection pregnancyInfection);
        void DeletePregnancyInfection(PregnancyInfection pregnancyInfection);

        void AddPregnancyRiskFactor(PregnancyRiskFactor pregnancyRiskFactor);
        void EditPregnancyRiskFactor(PregnancyRiskFactor pregnancyRiskFactor);
        void DeletePregnancyRiskFactor(PregnancyRiskFactor pregnancyRiskFactor);

        void AddBirth(Birth birth);
        void EditBirth(Birth birth);
        void DeleteBirth(Birth birth);
        Birth SaveOrUpdateBirth(Birth birth);

        void AddBirthFather(BirthFather birthFather);
        void EditBirthFather(BirthFather birthFather);
        void DeleteBirthFather(BirthFather birthFather);
        BirthFather SaveOrUpdateBirthFather(BirthFather birthFather);

        void AddNewborn(Newborn newborn);
        void EditNewborn(Newborn newborn);
        void DeleteNewborn(Newborn newborn);

        void AddPrenatal(Prenatal prenatal);
        void EditPrenatal(Prenatal prenatal);
        void DeletePrenatal(Prenatal prenatal);

        Prenatal SaveOrUpdatePrenatal(Prenatal prenatal);

        void AddLaborAndDelivery(LaborAndDelivery laborAndDelivery);
        void EditLaborAndDelivery(LaborAndDelivery laborAndDelivery);
        void DeleteLaborAndDelivery(LaborAndDelivery laborAndDelivery);
        LaborAndDelivery SaveOrUpdateLaborAndDelivery (LaborAndDelivery laborAndDelivery);
        void AddReaction(Reaction reaction);
        void EditReaction(Reaction reaction);
        void DeleteReaction(Reaction reaction);

        void AddClinicalReminder(ClinicalReminder clinicalReminder);        
        void EditClinicalReminder(ClinicalReminder clinicalReminder);        
        void DeleteClinicalReminder(ClinicalReminder clinicalReminder);        
        void Detach<TEntity>(TEntity entity) where TEntity : class;

    }
}

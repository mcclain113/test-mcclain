using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities.Data
{
    public partial class WCTCHealthSystemContext : DbContext
    {

        public WCTCHealthSystemContext()
        {
        }

        public WCTCHealthSystemContext(DbContextOptions<WCTCHealthSystemContext> options)
            : base(options)
        {

        }

        public virtual DbSet<AbnormalCondition> AbnormalConditions { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AddressState> AddressStates {get; set;}
        public virtual DbSet<AdmitOrder> AdmitOrders { get; set; }
        public virtual DbSet<AdmitType> AdmitTypes { get; set; }
        public virtual DbSet<AdvancedDirective> AdvancedDirectives { get; set; }
        public virtual DbSet<AlertType> AlertTypes { get; set; }
        public virtual DbSet<Allergen> Allergens { get; set; }
        public virtual DbSet<AnestheticType> AnestheticTypes { get; set; }
        public virtual DbSet<AspNetPermission> AspNetPermissions { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRolePermission> AspNetRolePermissions { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Birth> Births { get; set; }
        public virtual DbSet<BirthFather> BirthFathers { get; set; }
        public virtual DbSet<BirthPlaceType> BirthPlaceTypes { get; set; }
        public virtual DbSet<BloodPressureRouteType> BloodPressureRouteTypes { get; set; }
        public virtual DbSet<Bmimethod> Bmimethods { get; set; }
        public virtual DbSet<BodyPart> BodyParts { get; set; }
        public virtual DbSet<BodySystemAssessment> BodySystemAssessments { get; set; }
        public virtual DbSet<BodySystemType> BodySystemTypes { get; set; }
        public virtual DbSet<CareSystemAssessment> CareSystemAssessments { get; set; }
        public virtual DbSet<CareSystemParameter> CareSystemParameters { get; set; }
        public virtual DbSet<CareSystemType> CareSystemTypes { get; set; }
        public virtual DbSet<CharacteristicOfLabor> CharacteristicOfLabors { get; set; }
        public virtual DbSet<ClinicalReminder> ClinicalReminders { get; set; }
        public virtual DbSet<CongenitalAnomaly> CongenitalAnomalies { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Discharge> Discharges { get; set; }
        public virtual DbSet<Disclosure> Disclosures { get; set; }
        public virtual DbSet<DisclosureFee> DisclosureFees { get; set; }
        public virtual DbSet<DisclosureFeeType> DisclosureFeeTypes { get; set; }
        public virtual DbSet<DisclosurePayment> DisclosurePayments { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentRequested> DocumentRequesteds { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<Emar> Emars { get; set; }
        public virtual DbSet<Employment> Employments { get; set; }
        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
        public virtual DbSet<Encounter> Encounters { get; set; }
        public virtual DbSet<EncounterAlert> EncounterAlerts { get; set; }
        public virtual DbSet<EncounterHistory> EncounterHistories { get; set; }
        public virtual DbSet<EncounterPhysician> EncounterPhysicians { get; set; }
        public virtual DbSet<EncounterType> EncounterTypes { get; set; }
        public virtual DbSet<Ethnicity> Ethnicities { get; set; }
        public virtual DbSet<ExamType> ExamTypes { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<FallRisk> FallRisks { get; set; }
        public virtual DbSet<FetalPresentationAtBirth> FetalPresentationAtBirths { get; set; }
        public virtual DbSet<FinalRouteAndMethodOfDelivery> FinalRouteAndMethodOfDeliveries { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<GenderPronoun> GenderPronouns { get; set; }
        public virtual DbSet<InsuranceProvider> InsuranceProviders { get; set; }
        public virtual DbSet<ItemStatus> ItemStatuses { get; set; }
        public virtual DbSet<LaborAndDelivery> LaborAndDeliveries { get; set; }
        public virtual DbSet<PatientInsurance> PatientInsurances { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LegalStatus> LegalStatuses { get; set; }
        public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public virtual DbSet<MaternalMorbidity> MaternalMorbidities { get; set; }
        public virtual DbSet<Newborn> Newborns { get; set; }
        public virtual DbSet<NoteType> NoteTypes { get; set; }
        public virtual DbSet<O2deliveryType> O2deliveryTypes { get; set; }
        public virtual DbSet<OnsetOfLabor> OnsetOfLabors { get; set; }
        public virtual DbSet<OrderInfo> OrderInfos { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<PainParameter> PainParameters { get; set; }
        public virtual DbSet<PainRating> PainRatings { get; set; }
        public virtual DbSet<PainRatingImage> PainRatingImages { get; set; }
        public virtual DbSet<PainScaleType> PainScaleTypes { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientAdvancedDirective> PatientAdvancedDirectives { get; set; }
        public virtual DbSet<PatientAlert> PatientAlerts { get; set; }
        public virtual DbSet<PatientAlias> PatientAliases { get; set; }
        public virtual DbSet<PatientAllergy> PatientAllergies { get; set; }
        public virtual DbSet<PatientClinicalReminder> PatientClinicalReminders { get; set; }
        public virtual DbSet<PatientContactDetail> PatientContactDetails { get; set; }
        public virtual DbSet<PatientContactTime> PatientContactTimes { get; set; }
        public virtual DbSet<PatientEmergencyContact> PatientEmergencyContacts { get; set; }
        public virtual DbSet<PatientFallRisk> PatientFallRisks { get; set; }
        public virtual DbSet<PatientGeneral> PatientGenerals { get; set; }
        public virtual DbSet<PatientLanguage> PatientLanguages { get; set; }
        public virtual DbSet<PatientMedicationList> PatientMedicationLists { get; set; }
        public virtual DbSet<Medication> Medications { get; set; }
        public virtual DbSet<MedicationBrandName> MedicationBrandNames { get; set; }
        public virtual DbSet<MedicationDeliveryRoute> MedicationDeliveryRoutes { get; set; }
        public virtual DbSet<MedicationDosageForm> MedicationDosageForms { get; set; }
        public virtual DbSet<MedicationFrequency> MedicationFrequencies { get; set; }
        public virtual DbSet<MedicationGenericName> MedicationGenericNames { get; set; }
        public virtual DbSet<PatientModeOfContact> PatientModeOfContacts { get; set; }
        public virtual DbSet<PatientRace> PatientRaces { get; set; }
        public virtual DbSet<PatientRepresentative> PatientRepresentatives { get; set; }
        public virtual DbSet<PatientRestriction> PatientRestrictions { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<PersonContactDetail> PersonContactDetails { get; set; }
        public virtual DbSet<PersonContactTime> PersonContactTimes { get; set; }
        public virtual DbSet<PersonLanguage> PersonLanguages { get; set; }
        public virtual DbSet<PersonModeOfContact> PersonModeOfContacts { get; set; }
        public virtual DbSet<PersonRace> PersonRaces { get; set; }
        public virtual DbSet<PaymentPlan> PaymentPlans { get; set; }
        public virtual DbSet<PaymentSource> PaymentSources { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Pcacomment> Pcacomments { get; set; }
        public virtual DbSet<PcacommentType> PcacommentTypes { get; set; }
        public virtual DbSet<PcapainAssessment> PcapainAssessments { get; set; }
        public virtual DbSet<Pcarecord> Pcarecords { get; set; }
        public virtual DbSet<Physician> Physicians { get; set; }
        public virtual DbSet<PhysicianAssessment> PhysicianAssessments { get; set; }
        public virtual DbSet<PhysicianAssessmentAllergy> PhysicianAssessmentAllergies { get; set; }
        public virtual DbSet<PhysicianAssessmentType> PhysicianAssessmentTypes { get; set; }
        public virtual DbSet<PhysicianRole> PhysicianRoles { get; set; }
        public virtual DbSet<PlaceOfServiceOutPatient> PlaceOfServiceOutPatients { get; set; }
        public virtual DbSet<PointOfOrigin> PointOfOrigins { get; set; }
        public virtual DbSet<PreferredContactTime> PreferredContactTimes { get; set; }
        public virtual DbSet<PreferredModeOfContact> PreferredModeOfContacts { get; set; }
        public virtual DbSet<PregnancyInfection> PregnancyInfections { get; set; }
        public virtual DbSet<PregnancyRiskFactor> PregnancyRiskFactors { get; set; }
        public virtual DbSet<Prenatal> Prenatals { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<ProcedureReport> ProcedureReports { get; set; }
        public virtual DbSet<ProcedureReportAnestheticType> ProcedureReportAnestheticTypes { get; set; }
        public virtual DbSet<ProcedureReportPhysician> ProcedureReportPhysicians { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<ProgramFacility> ProgramFacilities { get; set; }
        public virtual DbSet<ProgressNote> ProgressNotes { get; set; }
        public virtual DbSet<ProviderStatus> ProviderStatuses { get; set; }
        public virtual DbSet<ProviderType> ProviderTypes { get; set; }
        public virtual DbSet<PulseRouteType> PulseRouteTypes { get; set; }
        public virtual DbSet<Race> Races { get; set; }
        public virtual DbSet<Reaction> Reactions { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestPriority> RequestPriorities { get; set; }
        public virtual DbSet<RequestPurpose> RequestPurposes { get; set; }
        public virtual DbSet<RequestReleaseFormat> RequestReleaseFormats { get; set; }
        public virtual DbSet<RequestStatus> RequestStatuses { get; set; }
        public virtual DbSet<RequestStatusReason> RequestStatusReasons { get; set; }
        public virtual DbSet<RequestedItem> RequestedItems { get; set; }
        public virtual DbSet<Requester> Requesters { get; set; }
        public virtual DbSet<RequesterStatus> RequesterStatuses { get; set; }
        public virtual DbSet<RequesterType> RequesterTypes { get; set; }
        public virtual DbSet<Restriction> Restrictions { get; set; }
        public virtual DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public virtual DbSet<Sex> Sexes { get; set; }
        public virtual DbSet<SideEffect> SideEffects { get; set; }
        public virtual DbSet<Specialty> Specialties { get; set; }
        public virtual DbSet<TempRouteType> TempRouteTypes { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<TestCategory> TestCategories { get; set; }
        public virtual DbSet<TestedBodyPart> TestedBodyParts { get; set; }
        public virtual DbSet<UserFacility> UserFacilities { get; set; }
        public virtual DbSet<UserProgram> UserPrograms { get; set; }
        public virtual DbSet<UserSecurityQuestion> UserSecurityQuestions { get; set; }
        public virtual DbSet<UserTable> UserTables { get; set; }
        public virtual DbSet<VisitType> VisitTypes { get; set; }
        public virtual DbSet<RevenueCode> RevenueCodes { get; set; }
        public virtual DbSet<ChargeDefinition> ChargeDefinitions { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Solution adapted from:
                //https://stackoverflow.com/questions/69519196/using-appsettings-json-configuration-in-ef-context
                // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                //TODO: If we want to connect to the deployed db, need to change the login in appsettings
                //string connection = builder.AddJsonFile("appsettings.json").Build()["Data:HIT-Deployed:ConnectionString"];
                string connection = builder.AddJsonFile("appsettings.json").Build()["Data:HIT:ConnectionString"];
                optionsBuilder.UseSqlServer(connection);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientRequest>().HasNoKey();

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AbnormalCondition>(entity =>
            {
                entity.HasKey(e => e.AbnormalConditionId).HasName("pk_AbnormalCondition");

                entity.ToTable("AbnormalCondition");

                entity.Property(e => e.AbnormalConditionId)
                    .HasColumnName("AbnormalConditionID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AbnormalConditionName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.AbnormalConditionDescription)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AddressStateID).HasColumnName("AddressStateID");
                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.AddressState)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.AddressStateID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Address_AddressState");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Address_CountryID");
            });

            modelBuilder.Entity<AddressState>(entity =>
            {
                entity.ToTable("AddressState");

                entity.HasKey(e => e.StateID);

                entity.Property(e => e.StateID).HasColumnName("StateID");

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StateAbbreviation)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<AdmitOrder>(entity =>
            {
                entity.ToTable("AdmitOrder");

                entity.Property(e => e.AdmitOrderId).HasColumnName("AdmitOrderID");

                entity.Property(e => e.OrderInfoId).HasColumnName("OrderInfoID");


                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AdmittingDiagnosis)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VisitTypeId).HasColumnName("VisitTypeID");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.AdmitOrders)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AdmitOrder_Department");

                entity.HasOne(d => d.OrderInfo)
                    .WithMany(p => p.AdmitOrders)
                    .HasForeignKey(d => d.OrderInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AdmitOrder_OrderInfoId");

                entity.HasOne(d => d.VisitType)
                    .WithMany(p => p.AdmitOrders)
                    .HasForeignKey(d => d.VisitTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AdmitOrder_VisitType");
            });

            modelBuilder.Entity<AdmitType>(entity =>
            {
                entity.ToTable("AdmitType");

                entity.Property(e => e.AdmitTypeId).HasColumnName("AdmitTypeID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Explaination).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WiPopCode)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<AdvancedDirective>(entity =>
            {
                entity.Property(e => e.AdvancedDirectiveId).HasColumnName("AdvancedDirectiveID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AlertType>(entity =>
            {
                entity.HasKey(e => e.AlertId);

                entity.ToTable("AlertType");

                entity.Property(e => e.AlertId).HasColumnName("AlertID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Allergen>(entity =>
            {
                entity.ToTable("Allergen");

                entity.Property(e => e.AllergenId).HasColumnName("AllergenID");

                entity.Property(e => e.AllergenName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AnestheticType>(entity =>
            {
                entity.ToTable("AnestheticType");

                entity.Property(e => e.AnestheticTypeId).HasColumnName("AnestheticTypeID");

                entity.Property(e => e.AnestheticType1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AnestheticType");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AspNetPermission>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).HasMaxLength(50).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(500);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetRolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<Birth>(entity =>
            {
                entity.HasKey(e => e.BirthId).HasName("PK__Birth__07804E10523BB145");

                entity.ToTable("Birth");

                entity.Property(e => e.BirthId).HasColumnName("BirthID");
                entity.Property(e => e.AddressId).HasColumnName("AddressID");
                entity.Property(e => e.BirthFatherId).HasColumnName("BirthFatherID");
                entity.Property(e => e.BirthFatherId).HasColumnName("BirthFatherID");
                entity.Property(e => e.BirthPlaceTypeId).HasColumnName("BirthPlaceTypeID");
                entity.Property(e => e.CertifierOfBirthId).HasColumnName("CertifierOfBirthID");
                entity.Property(e => e.CertifierSignature)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Comments)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.DeliveringAttendantId).HasColumnName("DeliveringAttendantID");
                entity.Property(e => e.DeliveryPaymentSourceId).HasColumnName("DeliveryPaymentSourceID");
                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
                entity.Property(e => e.FacilityTransferredFromName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.IsWctmcbirth).HasColumnName("IsWCTMCBirth");
                entity.Property(e => e.MotherMrn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("MotherMRN");

                entity.HasOne(d => d.Address).WithMany(p => p.Births)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Birth_AddressID");

                entity.HasOne(d => d.BirthFather).WithMany(p => p.Births)
                    .HasForeignKey(d => d.BirthFatherId)
                    .HasConstraintName("FK_Birth_BirthFatherID");
               
                entity.HasOne(d => d.FatherPerson)
                    .WithMany(p => p.BirthsAsFather)
                    .HasForeignKey(d => d.FatherPersonId)
                    .HasConstraintName("FK_Birth_FatherPersonID");
  
                entity.HasOne(d => d.MotherPerson)
                    .WithMany(p => p.BirthsAsMother)
                    .HasForeignKey(d => d.MotherPersonId)
                    .HasConstraintName("FK_Birth_MotherPersonID");
                
                entity.HasOne(d => d.BirthPlaceType).WithMany(p => p.Births)
                    .HasForeignKey(d => d.BirthPlaceTypeId)
                    .HasConstraintName("FK_Birth_BirthPlaceTypeID");

                entity.HasOne(d => d.CertifierOfBirth).WithMany(p => p.BirthCertifierOfBirths)
                    .HasForeignKey(d => d.CertifierOfBirthId)
                    .HasConstraintName("FK_Birth_CertifierOfBirthID");

                entity.HasOne(d => d.DeliveringAttendant).WithMany(p => p.BirthDeliveringAttendants)
                    .HasForeignKey(d => d.DeliveringAttendantId)
                    .HasConstraintName("FK_Birth_DeliveringAttendantID");

                entity.HasOne(d => d.DeliveryPaymentSource).WithMany(p => p.Births)
                    .HasForeignKey(d => d.DeliveryPaymentSourceId)
                    .HasConstraintName("FK_Birth_DeliveringPaymentSourceID");

                entity.HasOne(d => d.Facility).WithMany(p => p.Births)
                    .HasForeignKey(d => d.FacilityId)
                    .HasConstraintName("FK_Birth_FacilityID");

                entity.HasOne(d => d.MotherMrnNavigation).WithMany(p => p.Births)
                    .HasForeignKey(d => d.MotherMrn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Birth_MotherMRN");
            });

        modelBuilder.Entity<BirthFather>(entity =>
        {
            entity.HasKey(e => e.BirthFatherId).HasName("PK__BirthFat__2B0C6F1788FA6B4F");

            entity.ToTable("BirthFather");

            entity.HasIndex(e => new { e.FirstName, e.LastName }, "IX_FatherName");

            entity.Property(e => e.BirthFatherId).HasColumnName("BirthFatherID");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.EducationId).HasColumnName("EducationID");
            entity.Property(e => e.EthnicityId).HasColumnName("EthnicityID");
            entity.Property(e => e.FatherBirthplace)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ssn)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SSN");
            entity.Property(e => e.Suffix)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Education).WithMany(p => p.BirthFathers)
                .HasForeignKey(d => d.EducationId)
                .HasConstraintName("FK_BirthFather_EducationID");

            entity.HasOne(d => d.Ethnicity).WithMany(p => p.BirthFathers)
                .HasForeignKey(d => d.EthnicityId)
                .HasConstraintName("FK_BirthFather_EthnicityID");

            entity.HasMany(d => d.Races).WithMany(p => p.BirthFathers)
                .UsingEntity<Dictionary<string, object>>(
                    "BirthFatherRace",
                    r => r.HasOne<Race>().WithMany()
                        .HasForeignKey("RaceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BirthFatherRace_RaceID"),
                    l => l.HasOne<BirthFather>().WithMany()
                        .HasForeignKey("BirthFatherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BirthFatherRace_BirthFatherID"),
                    j =>
                    {
                        j.HasKey("BirthFatherId", "RaceId").HasName("PK__BirthFat__7B53D27AECF52B28");
                        j.ToTable("BirthFatherRace");
                        j.IndexerProperty<int>("BirthFatherId").HasColumnName("BirthFatherID");
                        j.IndexerProperty<byte>("RaceId").HasColumnName("RaceID");
                    });
        });

            modelBuilder.Entity<BirthPlaceType>(entity =>
            {
                entity.ToTable("BirthPlaceType");

                entity.Property(e => e.BirthPlaceTypeId)
                    .HasColumnName("BirthPlaceTypeID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.BirthPlaceTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BirthPlaceDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BloodPressureRouteType>(entity =>
            {
                entity.ToTable("BloodPressureRouteType");

                entity.Property(e => e.BloodPressureRouteTypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BloodPressureRouteTypeID");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Bmimethod>(entity =>
            {
                entity.ToTable("BMIMethod");

                entity.Property(e => e.BmimethodId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BMIMethodID");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BodyPart>(entity =>
            {
                entity.HasKey(e => e.PartId)
                    .HasName("PK__BodyPart__7C3F0D3057EE8F29");

                entity.ToTable("BodyPart");

                entity.Property(e => e.PartId).HasColumnName("PartID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BodySystemAssessment>(entity =>
            {
                entity.ToTable("BodySystemAssessment");

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhysicianAssessmentId).HasColumnName("PhysicianAssessmentID");

                entity.HasOne(d => d.BodySystemType)
                    .WithMany(p => p.BodySystemAssessments)
                    .HasForeignKey(d => d.BodySystemTypeId)
                    .HasConstraintName("fk_BodySystemType");

                entity.HasOne(d => d.PhysicianAssessment)
                    .WithMany(p => p.BodySystemAssessments)
                    .HasForeignKey(d => d.PhysicianAssessmentId)
                    .HasConstraintName("fk_PhysicianAssessment");
            });

            modelBuilder.Entity<BodySystemType>(entity =>
            {
                entity.ToTable("BodySystemType");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NormalLimitsDescription).IsUnicode(false);

                entity.HasOne(d => d.ExamType)
                    .WithMany(p => p.BodySystemTypes)
                    .HasForeignKey(d => d.ExamTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_BodySystemType_ExamTypeCode");
            });

            modelBuilder.Entity<CareSystemAssessment>(entity =>
            {
                entity.HasKey(e => new { e.CareSystemAssessmentId, e.Pcaid });

                entity.ToTable("CareSystemAssessment");

                entity.Property(e => e.CareSystemAssessmentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CareSystemAssessmentID");

                entity.Property(e => e.Pcaid).HasColumnName("PCAID");

                entity.Property(e => e.CareSystemParameterId).HasColumnName("CareSystemParameterID");

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.IsWithinNormalLimits)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CareSystemParameter)
                    .WithMany(p => p.CareSystemAssessments)
                    .HasForeignKey(d => d.CareSystemParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull) // if CareSystemParameter is deleted, its id in CareSystemAssessments is set to null
                    .HasConstraintName("fk_CareSystemAssessment_CareSystemParameterID");

                entity.HasOne(d => d.Pca)
                    .WithMany(p => p.CareSystemAssessments)
                    .HasForeignKey(d => d.Pcaid)
                    .OnDelete(DeleteBehavior.Cascade) // if PCA is deleted, the associated CareSystemAssessment is deleted
                    .HasConstraintName("fk_CareSystemAssessment_PCAID");
            });

            modelBuilder.Entity<CareSystemParameter>(entity =>
            {
                entity.ToTable("CareSystemParameter");

                entity.Property(e => e.CareSystemParameterId).HasColumnName("CareSystemParameterID");

                entity.Property(e => e.CareSystemTypeId).HasColumnName("CareSystemTypeID");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NormalLimitsDescription).IsUnicode(false);

                entity.HasOne(d => d.CareSystemType)
                    .WithMany(p => p.CareSystemParameters)
                    .HasForeignKey(d => d.CareSystemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CareSystemParameter_CareSystemTypeID");
            });

            modelBuilder.Entity<CareSystemType>(entity =>
            {
                entity.ToTable("CareSystemType");

                entity.Property(e => e.CareSystemTypeId).HasColumnName("CareSystemTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CharacteristicOfLabor>(entity =>
            {
                entity.HasKey(e => e.CharacteristicId).HasName("pk_CharacteristicOfLabor");

                entity.ToTable("CharacteristicOfLabor");

                entity.Property(e => e.CharacteristicId)
                    .HasColumnName("CharacteristicID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e. CharacteristicName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CharacteristicDescription)
                    .HasMaxLength(400)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ChargeDefinition>(entity =>
            {
                entity.HasKey(e => e.ChargeID).HasName("pk_ChargeDefinition");

                entity.ToTable("ChargeDefinition");

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LongDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ChargeAmount)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(e => e.DateActivated)
                    .HasColumnType("datetime");

                entity.Property(e => e.DateDeactivated)
                    .HasColumnType("datetime");

                entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");

                entity.Property(e => e.RevenueCodeID).HasColumnName("RevenueCode");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.ChargeDefinitions)
                    .HasForeignKey(d => d.DepartmentID)
                    .HasConstraintName("fk_ChargeDefinition_DepartmentID");

                entity.HasOne(d => d.RevenueCode)
                    .WithMany(p => p.ChargeDefinitions)
                    .HasForeignKey(d => d.RevenueCodeID)
                    .HasConstraintName("fk_ChargeDefinition_RevenueCode");
            });

            modelBuilder.Entity<ClinicalReminder>(entity =>
            {
                entity.Property(e => e.ClinicalReminderId).HasColumnName("ClinicalReminderID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CongenitalAnomaly>(entity =>
            {
                entity.HasKey(e => e.CongenitalAnomalyId).HasName("pk_CongenitalAnomaly");

                entity.ToTable("CongenitalAnomaly");

                entity.Property(e => e.CongenitalAnomalyId)
                    .HasColumnName("CongenitalAnomalyID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e. CongenitalAnomalyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CongenitalAnomalyDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.ContactId).HasName("pk_Contact");

                entity.ToTable("Contact");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");
                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.ShippingAddressId).HasColumnName("ShippingAddressID");

                entity.HasOne(d => d.ShippingAddress).WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.ShippingAddressId)
                    .HasConstraintName("fk_Contact_ShippingAddressID");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasMany(e => e.Addresses)
                    .WithOne(a => a.Country);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Discharge>(entity =>
            {
                entity.ToTable("Discharge");

                entity.Property(e => e.DischargeId).HasColumnName("DischargeID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.WiPopCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Disclosure>(entity =>
            {
                entity.ToTable("Disclosure");

                entity.Property(e => e.DisclosureId).HasColumnName("DisclosureID");
                entity.Property(e => e.Comments)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.RequestId).HasColumnName("RequestID");
                entity.Property(e => e.RequesterId).HasColumnName("RequesterID");

                entity.HasOne(d => d.Request).WithMany(p => p.Disclosures)
                    .HasForeignKey(d => d.RequestId)
                    .HasConstraintName("FK_Disclosure_RequestID");

                entity.HasOne(d => d.Requester).WithMany(p => p.Disclosures)
                    .HasForeignKey(d => d.RequesterId)
                    .HasConstraintName("FK_Disclosure_RequesterID");

                entity.HasMany(d => d.DisclosureFees).WithOne(df => df.Disclosure)
                    .HasForeignKey(df => df.DisclosureId)
                    .HasConstraintName("FK_Disclosure_DisclosureID");

            });

            modelBuilder.Entity<DisclosureFee>(entity =>
            {
                entity.ToTable("DisclosureFee");

                entity.HasKey(df => new { df.DisclosureId, df.DisclosureFeeTypeId });

                entity.Property(df => df.DisclosureId).HasColumnName("DisclosureID");
                entity.Property(df => df.DisclosureFeeTypeId).HasColumnName("DisclosureFeeTypeID");
                entity.Property(df => df.ItemCount).IsRequired();
                entity.Property(df => df.PerItemFee)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.HasOne(df => df.Disclosure)
                    .WithMany(d => d.DisclosureFees)
                    .HasForeignKey(df => df.DisclosureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisclosureFee_DisclosureID");

                entity.HasOne(df => df.DisclosureFeeType)
                    .WithMany(dft => dft.DisclosureFees)
                    .HasForeignKey(df => df.DisclosureFeeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisclosureFee_DisclosureFeeTypeID");
            });

            modelBuilder.Entity<DisclosureFeeType>(entity =>
            {
                entity.ToTable("DisclosureFeeType");

                entity.Property(e => e.DisclosureFeeTypeId).HasColumnName("DisclosureFeeTypeID");
                entity.Property(e => e.Comments)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.FeeAmount).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.FeeDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.HasMany(dft => dft.DisclosureFees).WithOne(df => df.DisclosureFeeType)
                .HasForeignKey(df => df.DisclosureFeeTypeId)
                .HasConstraintName("FK_DisclosureFee_DisclosureFeeTypeID");
            });

            modelBuilder.Entity<DisclosurePayment>(entity =>
            {
                entity.HasKey(e => e.DisclosurePaymentId).HasName("PK_DisclosuurePayment");

                entity.ToTable("DisclosurePayment");

                entity.Property(e => e.DisclosurePaymentId).HasColumnName("DisclosurePaymentID");
                entity.Property(e => e.DisclosureId).HasColumnName("DisclosureID");
                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.HasOne(d => d.Disclosure).WithMany(p => p.DisclosurePayments)
                    .HasForeignKey(d => d.DisclosureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisclosurePayment_DisclosureID");

                entity.HasOne(d => d.PaymentType).WithMany(p => p.DisclosurePayments)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisclosurePayment_PaymentTypeID");
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                // PK
                entity.HasKey(dt => dt.DocumentTypeID);
                entity.Property(dt => dt.DocumentTypeID).ValueGeneratedOnAdd();

                // Table name if you want to override conventions:
                entity.ToTable("DocumentType");

                // Columns
                entity.Property(dt => dt.DocumentTypeName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(dt => dt.DocumentTypeLevel)
                    .HasMaxLength(50);

            });

            modelBuilder.Entity<Document>(entity =>
            {
                // PK
                entity.HasKey(d => d.DocumentID);
                entity.Property(d => d.DocumentID).ValueGeneratedOnAdd();

                // Table
                entity.ToTable("Document");

                // Scalars
                entity.Property(d => d.Mrn)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(d => d.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(d => d.FileType)
                    .IsRequired()
                    .HasMaxLength(10);

                // varbinary(max) mapping
                entity.Property(d => d.DocumentContent)
                    .HasColumnType("varbinary(max)");

                // Timestamps
                entity.Property(d => d.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(d => d.LastModified)
                    .HasDefaultValueSql("GETDATE()");

                // establish relationship with DocumentType
                entity.HasOne(d => d.DocumentType)
                    .WithMany(dt => dt.Documents)
                    .HasForeignKey(d => d.DocumentTypeID)
                    .HasConstraintName("fk_Document_DocumentTypeID");

                // establish relationship with Patient
                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.Mrn)
                    .OnDelete(DeleteBehavior.Cascade)   // if Patient deleted, delete the Document
                    .HasConstraintName("fk_Document_MRN");

                // establish relationship with Encounter;
                // take no action when Encounter is deleted, so there must be code in the DeleteEncounter to handle any associated Documents
                entity.HasOne(d => d.Encounter)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.EncounterId)
                    .HasConstraintName("fk_Document_EncounterID");
            });


            modelBuilder.Entity<DocumentRequested>(entity =>
            {
                entity.ToTable("DocumentRequested");

                entity.Property(e => e.DocumentRequestedId).HasColumnName("DocumentRequestedID");
                entity.Property(e => e.DocumentRequested1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DocumentRequested");
                entity.Property(e => e.DocumentRequestedDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EducationLevel>(entity =>
            {
                entity.ToTable("EducationLevel");

                entity.HasKey(e => e.EducationId);

                entity.Property(e => e.EducationId)
                    .HasColumnName("EducationID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.EducationLevelName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EducationLevelDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<Emar>(entity =>
            {
                entity.HasKey(e => e.MedicationAdministrationId)
                    .HasName("PK__EMAR__417866C5DFA79EB5");

                entity.ToTable("EMAR");

                entity.HasIndex(e => new { e.AdministrationStatusId, e.AssignedAdministrator, e.AdministeredBy }, "FK");

                entity.Property(e => e.MedicationAdministrationId).HasColumnName("MedicationAdministrationID");

                entity.Property(e => e.AdministeredAttempt).HasColumnType("datetime");

                entity.Property(e => e.AdministrationCost).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.AdministrationStatusId).HasColumnName("AdministrationStatusID");

                entity.Property(e => e.ApprovedMedicineId).HasColumnName("ApprovedMedicineID");

                entity.HasOne(d => d.AdministeredByNavigation)
                    .WithMany(p => p.EmarAdministeredByNavigations)
                    .HasForeignKey(d => d.AdministeredBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EMAR__Administer__0ABD916C");

                entity.HasOne(d => d.AssignedAdministratorNavigation)
                    .WithMany(p => p.EmarAssignedAdministratorNavigations)
                    .HasForeignKey(d => d.AssignedAdministrator)
                    .HasConstraintName("FK__EMAR__AssignedAd__09C96D33");
            });

            modelBuilder.Entity<Employment>(entity =>
            {
                entity.ToTable("Employment");

                entity.Property(e => e.EmploymentId).HasColumnName("EmploymentID");

                entity.Property(e => e.AddressId)
                    .HasColumnName("AddressID");

                entity.Property(e => e.EmployerName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Occupation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity
                    .HasOne(e => e.Address);
            });

            modelBuilder.Entity<Encounter>(entity =>
            {
                entity.ToTable("Encounter");

                entity.Property(e => e.EncounterId).HasColumnName("EncounterID");

                entity.Property(e => e.AdmitDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AdmitTypeId).HasColumnName("AdmitTypeID");

                entity.Property(e => e.AdmittingDiagnosis).IsUnicode(false);

                entity.Property(e => e.ChiefComplaint)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DischargeAuthoringProviderId).HasColumnName("DischargeAuthoringProviderID");

                entity.Property(e => e.DischargeAuthoringProviderSignature).IsUnicode(false);

                entity.Property(e => e.DischargeAuthoringProviderSignedDate).HasColumnType("datetime");

                entity.Property(e => e.DischargeCoSigningProviderId).HasColumnName("DischargeCoSigningProviderID");

                entity.Property(e => e.DischargeCoSigningProviderSignature).IsUnicode(false);

                entity.Property(e => e.DischargeCoSigningProviderSignedDate).HasColumnType("datetime");

                entity.Property(e => e.DischargeComment).IsUnicode(false);

                entity.Property(e => e.DischargeDateTime).HasColumnType("datetime");

                entity.Property(e => e.DischargeDiagnosis).IsUnicode(false);

                entity.Property(e => e.DischargeDietInstructions).IsUnicode(false);

                entity.Property(e => e.DischargeDisposition).HasColumnName("DischargeDisposition");

                entity.Property(e => e.DischargeDispositionNote).IsUnicode(false);

                entity.Property(e => e.DischargeHospitalCourse).IsUnicode(false);

                entity.Property(e => e.EditedBy).HasColumnName("EditedBy");

                entity.Property(e => e.EncounterRestrictionId).HasColumnName("EncounterRestrictionID");

                entity.Property(e => e.EncounterTypeId).HasColumnName("EncounterTypeID");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.FacilityRegistryOptInOut).HasColumnName("FacilityRegistryOptInOut");

                entity.Property(e => e.HistoryOfPresentIllness).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MedicationsOnDischarge).IsUnicode(false);

                entity.Property(e => e.Mrn)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MRN")
                    .IsFixedLength(true);

                entity.Property(e => e.PatientCurrentAge).HasColumnName("PatientCurrentAge");

                entity.Property(e => e.PlaceOfServiceId).HasColumnName("PlaceOfServiceID");

                entity.Property(e => e.PointOfOriginId).HasColumnName("PointOfOriginID");

                entity.Property(e => e.RoomNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SignificantFindings).IsUnicode(false);

                entity.Property(e => e.WrittenDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AdmitType)
                    .WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.AdmitTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Encounter_AdmitTypeID");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("fk_Encounter_DepartmentID");

                entity.HasOne(p => p.DischargeAuthoringProvider)
                    .WithMany(d => d.DischargeAuthoringProviders)
                    .HasForeignKey(e => e.DischargeAuthoringProviderId)
                    .HasConstraintName("fk_Encounter_DischargeAuthoringProviderId");

                entity.HasOne(p => p.DischargeCoSigningProvider)
                    .WithMany(d => d.DischargeCoSigningProviders)
                    .HasForeignKey(e => e.DischargeCoSigningProviderId)
                    .HasConstraintName("fk_Encounter_DischargeCoSigningProviderId");

                entity.HasOne(d => d.DischargeDispositionNavigation)
                    .WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.DischargeDisposition)
                    .HasConstraintName("fk_Encounter_DischargeDisposition");

                entity.HasOne(d => d.EncounterType)
                    .WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.EncounterTypeId)
                    .HasConstraintName("fk_Encounter_EncounterTypeID");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Encounter_FacilityID");

                entity.HasOne(d => d.MrnNavigation)
                    .WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.Mrn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Encounter_MRN");


                entity.HasOne(d => d.PlaceOfService)
                    .WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.PlaceOfServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Encounter_PlaceOfServiceID");

                entity.HasOne(d => d.PointOfOrigin)
                    .WithMany(p => p.Encounters)
                    .HasForeignKey(d => d.PointOfOriginId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Encounter_PointOfOriginID");
            });

            modelBuilder.Entity<EncounterAlert>(entity =>
            {
                entity.HasKey(e => new { e.EncounterId, e.PatientAlertId });

                entity.Property(e => e.EncounterId).HasColumnName("EncounterID");

                entity.Property(e => e.PatientAlertId).HasColumnName("PatientAlertID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Encounter)
                    .WithMany(p => p.EncounterAlerts)
                    .HasForeignKey(d => d.EncounterId)
                    .HasConstraintName("fk_EncounterAlerts_EncounterID");

                entity.HasOne(d => d.PatientAlert)
                    .WithMany(p => p.EncounterAlerts)
                    .HasForeignKey(d => d.PatientAlertId)
                    .HasConstraintName("fk_EncounterAlerts_PatientAlertID");
            });

            modelBuilder.Entity<EncounterHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Encounter_History");

                entity.Property(e => e.AdmitDate).HasColumnName("Admit Date");

                entity.Property(e => e.ChiefComplaint)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Chief Complaint");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Department Name");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.DischargeDate)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Discharge Date");

                entity.Property(e => e.EncounterId).HasColumnName("Encounter ID");

                entity.Property(e => e.Explaination).IsUnicode(false);

                entity.Property(e => e.FacilityName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Facility Name");

                entity.Property(e => e.Mrn)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MRN")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EncounterPhysician>(entity =>
            {
                entity.HasKey(e => e.EncounterPhysiciansId);

                entity.Property(e => e.EncounterPhysiciansId).HasColumnName("EncounterPhysiciansID");

                entity.Property(e => e.EncounterId).HasColumnName("EncounterID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhysicianId).HasColumnName("PhysicianID");

                entity.Property(e => e.PhysicianRoleId).HasColumnName("PhysicianRoleID");

                entity.HasOne(d => d.Physician)
                    .WithMany(p => p.EncounterPhysiciansNavigation)
                    .HasForeignKey(d => d.PhysicianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_EncounterPhysicians_PhysicianID");

                entity.HasOne(d => d.PhysicianRole)
                    .WithMany(p => p.EncounterPhysicians)
                    .HasForeignKey(d => d.PhysicianRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_EncounterPhysicians_PhysicianRoleID");

                entity.HasOne(d => d.Encounter)
                    .WithMany(p => p.EncounterPhysicians)
                    .HasForeignKey(d => d.EncounterId)
                    .HasConstraintName("fk_EncounterPhysicians_EncounterID");
            });

            modelBuilder.Entity<EncounterType>(entity =>
            {
                entity.ToTable("EncounterType");

                entity.Property(e => e.EncounterTypeId).HasColumnName("EncounterTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ethnicity>(entity =>
            {
                entity.ToTable("Ethnicity");

                entity.Property(e => e.EthnicityId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("EthnicityID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WiPopCode);
            });


            modelBuilder.Entity<ExamType>(entity =>
            {
                entity.HasKey(e => e.ExamTypeCode)
                    .HasName("pk_ExamType");

                entity.ToTable("ExamType");

                entity.Property(e => e.ExamType1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ExamType");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.ToTable("Facility");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Facilities)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Facility_AddressID");
            });

            modelBuilder.Entity<FallRisk>(entity =>
            {
                entity.Property(e => e.FallRiskId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FallRiskID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FetalPresentationAtBirth>(entity =>
            {
                entity.HasKey(e => e.FetalPresentationAtBirthId).HasName("pk_FetalPresentationAtBirth");

                entity.ToTable("FetalPresentationAtBirth");

                entity.Property(e => e.FetalPresentationAtBirthId)
                    .HasColumnName("FetalPresentationAtBirthID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e. FetalPresentationName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FetalPresentationDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FinalRouteAndMethodOfDelivery>(entity =>
            {
                entity.HasKey(e => e.FinalRouteAndMethodId).HasName("pk_FinalRouteAndMethodOfDelivery");

                entity.ToTable("FinalRouteAndMethodOfDelivery");

                entity.Property(e => e.FinalRouteAndMethodId)
                    .HasColumnName("FinalRouteAndMethodID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e. FinalRouteAndMethodName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FinalRouteAndMethodDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.GenderId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("GenderID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GenderPronoun>(entity =>
            {
                entity.HasKey(gp => gp.GenderPronounId);

                entity.ToTable("GenderPronoun");

                entity.Property(e => e.GenderPronounId)
                        .ValueGeneratedOnAdd()
                    .HasColumnName("GenderPronounID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GenderPronouns)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InsuranceProvider>(entity =>
            {
                entity.HasKey(e => e.InsuranceProviderId).HasName("pk_InsuranceProvider");

                entity.ToTable("InsuranceProvider");

                entity.Property(e => e.InsuranceProviderId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("InsuranceProviderId");
                entity.Property(e => e.ProviderName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ProviderName");
            });

            modelBuilder.Entity<ItemStatus>(entity =>
            {
                entity.HasKey(e => e.ItemStatusId).HasName("pk_ItemStatus");

                entity.ToTable("ItemStatus");

                entity.Property(e => e.ItemStatusId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("ItemStatusID");
                entity.Property(e => e.ItemStatus1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ItemStatus");
                entity.Property(e => e.ItemStatusDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<LaborAndDelivery>(entity =>
            {
                entity.HasKey(e => e.LaborAndDeliveryId).HasName("pk_LaborAndDelivery");

                entity.ToTable("LaborAndDelivery");

                entity.Property(e => e.LaborAndDeliveryId).HasColumnName("LaborAndDeliveryID");
                entity.Property(e => e.Comments)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.FetalPresentationAtBirthId).HasColumnName("FetalPresentationAtBirthID");
                entity.Property(e => e.FinalRouteAndMethodId).HasColumnName("FinalRouteAndMethodID");
                entity.Property(e => e.NewbornId).HasColumnName("NewbornID");

                entity.HasOne(d => d.FetalPresentationAtBirth).WithMany(p => p.LaborAndDeliveries)
                    .HasForeignKey(d => d.FetalPresentationAtBirthId)
                    .HasConstraintName("fk_LaborAndDelivery_FetalPresentationAtBrithID");

                entity.HasOne(d => d.FinalRouteAndMethod).WithMany(p => p.LaborAndDeliveries)
                    .HasForeignKey(d => d.FinalRouteAndMethodId)
                    .HasConstraintName("fk_LaborAndDelivery_FinalRouteAndMethodID");

                entity.HasOne(d => d.Newborn).WithMany(p => p.LaborAndDeliveries)
                    .HasForeignKey(d => d.NewbornId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_LaborAndDelivery_NewbornID");

                entity.HasMany(d => d.Characteristics).WithMany(p => p.LaborAndDeliveries)
                    .UsingEntity<Dictionary<string, object>>(
                        "LaborAndDeliveryCharacteristic",
                        r => r.HasOne<CharacteristicOfLabor>().WithMany()
                            .HasForeignKey("CharacteristicId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_LaborAndDeliveryCharacteristics_CharacteristicID"),
                        l => l.HasOne<LaborAndDelivery>().WithMany()
                            .HasForeignKey("LaborAndDeliveryId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_LaborAndDeliveryCharacteristics_LaborAndDeliveryID"),
                        j =>
                        {
                            j.HasKey("LaborAndDeliveryId", "CharacteristicId").HasName("pk_LaborAndDeliveryCharacteristics");
                            j.ToTable("LaborAndDeliveryCharacteristics");
                            j.IndexerProperty<int>("LaborAndDeliveryId").HasColumnName("LaborAndDeliveryID");
                            j.IndexerProperty<byte>("CharacteristicId").HasColumnName("CharacteristicID");
                        });

                entity.HasMany(d => d.MaternalMorbidities).WithMany(p => p.LaborAndDeliveries)
                    .UsingEntity<Dictionary<string, object>>(
                        "LaborAndDeliveryMaternalMorbidity",
                        r => r.HasOne<MaternalMorbidity>().WithMany()
                            .HasForeignKey("MaternalMorbidityId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_LaborAndDeliveryMaternalMorbidities_MaternalMorbidityID"),
                        l => l.HasOne<LaborAndDelivery>().WithMany()
                            .HasForeignKey("LaborAndDeliveryId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_LaborAndDeliveryMaternalMorbidities_LaborAndDeliveryID"),
                        j =>
                        {
                            j.HasKey("LaborAndDeliveryId", "MaternalMorbidityId").HasName("pk_LaborAndDeliveryMaternalMorbidities");
                            j.ToTable("LaborAndDeliveryMaternalMorbidities");
                            j.IndexerProperty<int>("LaborAndDeliveryId").HasColumnName("LaborAndDeliveryID");
                            j.IndexerProperty<byte>("MaternalMorbidityId").HasColumnName("MaternalMorbidityID");
                        });

                entity.HasMany(d => d.OnsetOfLabors).WithMany(p => p.LaborAndDeliveries)
                    .UsingEntity<Dictionary<string, object>>(
                        "LaborAndDeliveryOnsetOfLabor",
                        r => r.HasOne<OnsetOfLabor>().WithMany()
                            .HasForeignKey("OnsetOfLaborId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_LaborAndDeliveryOnsetOfLabor_OnsetOfLaborID"),
                        l => l.HasOne<LaborAndDelivery>().WithMany()
                            .HasForeignKey("LaborAndDeliveryId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_LaborAndDeliveryOnsetOfLabor_LaborAndDeliveryID"),
                        j =>
                        {
                            j.HasKey("LaborAndDeliveryId", "OnsetOfLaborId").HasName("pk_LaborAndDeliveryOnsetOfLabor");
                            j.ToTable("LaborAndDeliveryOnsetOfLabor");
                            j.IndexerProperty<int>("LaborAndDeliveryId").HasColumnName("LaborAndDeliveryID");
                            j.IndexerProperty<byte>("OnsetOfLaborId").HasColumnName("OnsetOfLaborID");
                        });
            });
            modelBuilder.Entity<Newborn>(entity =>
            {
                entity.HasKey(e => e.NewbornId).HasName("pk_Newborn");

                entity.ToTable("Newborn");

                entity.Property(e => e.NewbornId).HasColumnName("NewbornID");
                entity.Property(e => e.BirthId).HasColumnName("BirthID");
                entity.Property(e => e.BirthWeightInLbs).HasColumnType("decimal(4, 2)");
                entity.Property(e => e.Comments)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.DateAndTimeOfBirth).HasColumnType("datetime");
                entity.Property(e => e.NameOfFacilityTransferredTo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.NewbornMrn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("NewbornMRN");
                
                entity.Property(e => e.PersonId).HasColumnName("PersonID");
                entity.Property(e => e.SexId).HasColumnName("SexID");
                entity.Property(e => e.SsnrequestedForChild).HasColumnName("SSNRequestedForChild");

                entity.HasOne(d => d.Birth).WithMany(p => p.Newborns)
                    .HasForeignKey(d => d.BirthId)
                    .HasConstraintName("fk_Newborn_BirthID");

                entity.HasOne(d => d.NewbornMrnNavigation).WithMany(p => p.Newborns)
                    .HasForeignKey(d => d.NewbornMrn)
                    .HasConstraintName("fk_Newborn_NewbornMRN");

                entity.HasOne(d => d.Sex).WithMany(p => p.Newborns)
                    .HasForeignKey(d => d.SexId)
                    .HasConstraintName("fk_Newborn_SexID");
                entity.HasOne(d => d.Person)
                    .WithMany() 
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Newborn_PersonID");
                entity.HasMany(d => d.AbnormalConditions).WithMany(p => p.Newborns)
                    .UsingEntity<Dictionary<string, object>>(
                        "NewbornAbnormalCondition",
                        r => r.HasOne<AbnormalCondition>().WithMany()
                            .HasForeignKey("AbnormalConditionId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_NewbornAbnormalConditions_AbnormalConditionID"),
                        l => l.HasOne<Newborn>().WithMany()
                            .HasForeignKey("NewbornId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_NewbornAbnormalConditions_NewbornID"),
                        j =>
                        {
                            j.HasKey("NewbornId", "AbnormalConditionId").HasName("pk_NewbornAbnormalConditions");
                            j.ToTable("NewbornAbnormalConditions");
                            j.IndexerProperty<int>("NewbornId").HasColumnName("NewbornID");
                            j.IndexerProperty<byte>("AbnormalConditionId").HasColumnName("AbnormalConditionID");
                        });

                entity.HasMany(d => d.CongenitalAnomalies).WithMany(p => p.Newborns)
                    .UsingEntity<Dictionary<string, object>>(
                        "NewbornCongenitalAnomaly",
                        r => r.HasOne<CongenitalAnomaly>().WithMany()
                            .HasForeignKey("CongenitalAnomalyId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_NewbornCongenitalAnomalies_CongenitalAnomalyID"),
                        l => l.HasOne<Newborn>().WithMany()
                            .HasForeignKey("NewbornId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("fk_NewbornCongenitalAnomalies_NewbornID"),
                        j =>
                        {
                            j.HasKey("NewbornId", "CongenitalAnomalyId").HasName("pk_NewbornCongenitalAnomalies");
                            j.ToTable("NewbornCongenitalAnomalies");
                            j.IndexerProperty<int>("NewbornId").HasColumnName("NewbornID");
                            j.IndexerProperty<byte>("CongenitalAnomalyId").HasColumnName("CongenitalAnomalyID");
                        });
            });

            modelBuilder.Entity<PatientInsurance>(entity =>
            {
                entity.ToTable("PatientInsurance");

                entity.Property(e => e.PatientInsuranceId).HasColumnName("PatientInsuranceID");

                entity.Property(e => e.PatientInsuranceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PatientInsuranceId");

                entity.Property(e => e.MRN).HasColumnName("MRN");

                entity.Property(e => e.InsuranceOrder).HasColumnName("InsuranceOrder");

                entity.Property(e => e.Guarantor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Subscriber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubscriberRelationshipID)
                    .HasColumnName("SubscriberRelationshipID");

                entity.Property(e => e.SubscriberNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GroupNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PlanName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PlanNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EffectiveDate).HasColumnType("EffectiveDate");

                entity.Property(e => e.Notes)
                    .IsUnicode(false);

                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.PatientInsurances)
                    .HasForeignKey(d => d.MRN)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_PatientInsurance_MRN");

                entity.HasOne(e => e.Relationship)
                    .WithMany(p => p.PatientInsurances)
                    .HasForeignKey(d => d.SubscriberRelationshipID)
                    .HasConstraintName("fk_PatientInsurance_SubscriberRelationshipID");

                entity.HasOne(pi => pi.InsuranceProvider)
                    .WithMany(ip => ip.PatientInsurances)
                    .HasForeignKey(pi => pi.InsuranceProviderId)
                    .HasConstraintName("fk_PatientInsurance_InsuranceProvider");
            });

            //Adding Medication List model builders

            modelBuilder.Entity<Medication>(entity =>
            {

                entity.HasKey(p => p.MedicationId);

                entity.ToTable("Medication");
                entity.Property(p => p.MedicationId).HasColumnName("MedicationID");

                entity.Property(p => p.Ndc).HasColumnName("NDC")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(p => p.BrandNameId).HasColumnName("BrandNameID");

                entity.Property(p => p.GenericNameId).HasColumnName("GenericNameID");

                entity.Property(p => p.ActiveStrength)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(p => p.ActiveIngredientUnits)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(p => p.DosageFormId).HasColumnName("DosageFormID");

                entity.Property(p => p.DeliveryRouteId).HasColumnName("DeliveryRouteID");

                entity.Property(p => p.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(p => p.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(m => m.MedicationBrandName)
                    .WithMany(b => b.Medications)
                    .HasForeignKey(m => m.BrandNameId)
                    .HasConstraintName("fk_MedicationBrandName_BrandNameID");

                entity.HasOne(m => m.MedicationDeliveryRoute)
                    .WithMany(d => d.Medications)
                    .HasForeignKey(m => m.DeliveryRouteId)
                    .HasConstraintName("fk_MedicationDeliveryRoute_DeliveryRouteID");

                entity.HasOne(m => m.MedicationDosageForm)
                    .WithMany(d => d.Medications)
                    .HasForeignKey(m => m.DosageFormId)
                    .HasConstraintName("fk_MedicationDosageForm_DosageFormID");

                entity.HasOne(m => m.MedicationGenericName)
                    .WithMany(g => g.Medications)
                    .HasForeignKey(m => m.GenericNameId)
                    .HasConstraintName("fk_MedicationGenericName_GenericNameID");

            });

            modelBuilder.Entity<MedicationBrandName>(entity =>
            {
                entity.HasKey(p => p.MedicationBrandId);

                entity.ToTable("MedicationBrandName");
                entity.Property(b => b.MedicationBrandId).HasColumnName("MedicationBrandID");

                entity.Property(b => b.BrandName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(b => b.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(b => b.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

            });

            modelBuilder.Entity<MedicationDeliveryRoute>(entity =>
            {
                entity.HasKey(d => d.DeliveryRouteId);

                entity.ToTable("MedicationDeliveryRoute");
                entity.Property(b => b.DeliveryRouteId).HasColumnName("DeliveryRouteID");

                entity.Property(b => b.DeliveryRouteName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(b => b.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(b => b.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

            });

            modelBuilder.Entity<MedicationDosageForm>(entity =>
            {
                entity.HasKey(d => d.DosageFormId);

                entity.ToTable("MedicationDosageForm");
                entity.Property(b => b.DosageFormId).HasColumnName("DosageFormID");

                entity.Property(b => b.DosageForm)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(b => b.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(b => b.DateModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

            });

            modelBuilder.Entity<MedicationFrequency>(entity =>
            {
                entity.HasKey(f => f.MedicationFrequencyId);

                entity.ToTable("MedicationFrequency");
                entity.Property(f => f.MedicationFrequencyId).HasColumnName("MedicationFrequencyID");

                entity.Property(F => F.FrequencyDescription)
                    .HasMaxLength(50).IsRequired()
                    .IsUnicode(false);

                entity.Property(F => F.FrequencyCode)
                    .HasMaxLength(5).IsRequired()
                    .IsUnicode(false);

                entity.Property(f => f.LastUpdate)
                    .HasColumnType("datetime");

            });
            //PatientMedicationList just putting it by the other medication stuff for ease
            modelBuilder.Entity<PatientMedicationList>(entity =>
            {
                entity.HasKey(f => f.PatientMedicationListID);

                entity.ToTable("PatientMedicationList");
                entity.Property(f => f.PatientMedicationListID).HasColumnName("PatientMedicationListID");

                entity.Property(F => F.Mrn)
                    .HasMaxLength(6).IsRequired();

                entity.Property(F => F.MedicationID).HasColumnName("MedicationID");

                entity.Property(F => F.FrequencyID).HasColumnName("FrequencyID");

                entity.Property(e => e.OtherFrequencyDescription)
                    .HasMaxLength(100);

                entity.Property(e => e.Indication)
                    .HasMaxLength(100);

                entity.Property(F => F.OrderingProviderID).HasColumnName("OrderingProviderID");

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasColumnType("datetime");
                // .HasDefaultValueSql("(getdate())")

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Comments)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("N/A");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("N/A").IsRequired();

                entity.Property(e => e.Mrn)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Mrn")
                    .IsFixedLength(true);

                entity.HasOne(d => d.MedicationFrequencies)
                    .WithMany(p => p.PatientMedicationLists)
                    .HasForeignKey(d => d.FrequencyID)
                    .HasConstraintName("fk_PatientMedicationList_FrequencyID");

                entity.HasOne(d => d.Medications)
                    .WithMany(p => p.PatientMedicationLists)
                    .HasForeignKey(d => d.MedicationID)
                    .HasConstraintName("fk_PatientMedicationList_MedicationID");

                entity.HasOne(d => d.Physicians)
                    .WithMany(p => p.PatientMedicationLists)
                    .HasForeignKey(d => d.OrderingProviderID)
                    .HasConstraintName("fk_PatientMedicationList_OrderingProviderID");

                entity.HasOne(d => d.Patients)
                    .WithMany(p => p.PatientMedicationLists)
                    .HasForeignKey(d => d.Mrn)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_PatientMedicationList_Mrn");

            });


            modelBuilder.Entity<MedicationGenericName>(entity =>
            {
                entity.HasKey(d => d.MedicationGenericId);

                entity.ToTable("MedicationGenericName");
                entity.Property(b => b.MedicationGenericId).HasColumnName("MedicationGenericID");

                entity.Property(b => b.GenericName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(b => b.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(b => b.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

            });



            // End of Medication model builders

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LegalStatus>(entity =>
            {
                entity.HasKey(ls => ls.LegalStatusId);

                entity.ToTable("LegalStatus");

                entity.Property(e => e.LegalStatusId)
                        .ValueGeneratedOnAdd()
                    .HasColumnName("LegalStatusID");

                entity.Property(e => e.RequiresLegalGuardian)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LegalStatusName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MaritalStatus>(entity =>
            {
                entity.ToTable("MaritalStatus");

                entity.Property(e => e.MaritalStatusId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MaritalStatusID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MaternalMorbidity>(entity =>
            {
                entity.HasKey(e => e.MaternalMorbidityId).HasName("pk_MaternalMorbidity");

                entity.ToTable("MaternalMorbidity");

                entity.Property(e => e.MaternalMorbidityId)
                    .HasColumnName("MaternalMorbidityID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e. MaternalMorbidityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaternalMorbidityDescription)
                    .HasMaxLength(400)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NoteType>(entity =>
            {
                entity.ToTable("NoteType");

                entity.Property(e => e.NoteTypeId).HasColumnName("NoteTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NoteType1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NoteType");
            });

            modelBuilder.Entity<O2deliveryType>(entity =>
            {
                entity.ToTable("O2DeliveryType");

                entity.Property(e => e.O2deliveryTypeId).HasColumnName("O2DeliveryTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.O2deliveryTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("O2DeliveryTypeName");
            });

            modelBuilder.Entity<OnsetOfLabor>(entity =>
            {
                entity.HasKey(e => e.OnsetOfLaborId).HasName("pk_OnsetOfLabor");

                entity.ToTable("OnsetOfLabor");

                entity.Property(e => e.OnsetOfLaborId)
                    .HasColumnName("OnsetOfLaborID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e. OnsetOfLaborName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OnsetOfLaborDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderInfo>(entity =>
            {
                entity.ToTable("OrderInfo");

                entity.Property(e => e.OrderInfoId).HasColumnName("OrderInfoID");

                entity.Property(e => e.OrderingProviderID).HasColumnName("OrderingProvider");

                entity.Property(e => e.AuthenticatingProviderID).HasColumnName("AuthenticatingProviderID");

                entity.Property(e => e.ProviderSignedDate).HasColumnType("datetime");

                entity.Property(e => e.OrderCompletedByID).HasColumnName("OrderCompletedByID");

                entity.Property(e => e.EncounterId).HasColumnName("EncounterID");

                entity.Property(e => e.CoAuthorApproved)
                    .IsRequired()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsOrderComplete)
                    .IsRequired()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsVerbalOrder)
                    .IsRequired()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsFasting)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Notes)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.AuthorESignature)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AuthenticatingProviderESignature)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProviderESignature)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderCompletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrderTypeId).HasColumnName("OrderTypeID");

                entity.Property(e => e.PriorityId).HasColumnName("PriorityID");

                entity.Property(e => e.AuthenticatingProviderSignedDate).HasColumnType("datetime");

                entity.Property(e => e.AdmittingDiagnosis).HasColumnName("AdmittingDiagnosis");

                entity.Property(e => e.OrderedItemChargeID).HasColumnName("OrderedItemChargeID");

                entity.Property(e => e.HoursFasting)
                    .HasColumnName("HoursFasting")
                    .HasColumnType("tinyint");

                entity.Property(e => e.TestReason).HasMaxLength(2000);

                entity.Property(e => e.AuthorId).HasColumnName("AuthorId");

                entity.Property(e => e.AuthorSignedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Encounter)
                    .WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.EncounterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__OrderInfo__Encou__62AFA012");

                entity.HasOne(d => d.OrderType)
                    .WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.OrderTypeId)
                    .HasConstraintName("FK__OrderInfo__Order__63A3C44B");

                entity.HasOne(d => d.AuthenticatingProvider)
                    .WithMany(p => p.OrderInfoCoProviders)
                    .HasForeignKey(d => d.AuthenticatingProviderID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderInfo_AuthenticatingProviderId");

                entity.HasOne(d => d.CompletedByProvider)
                    .WithMany(p => p.OrderCompletedByProvider)
                    .HasForeignKey(d => d.OrderCompletedByID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderInfo_OrderCompletedById");

                entity.HasOne(d => d.OrderingProvider)
                    .WithMany(p => p.OrderInfoOrderingProviderNavigations)
                    .HasForeignKey(d => d.OrderingProviderID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderInfo__Order__6497E884");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.PriorityId)
                    .HasConstraintName("FK__OrderInfo__Prior__658C0CBD");

                entity.HasOne(d => d.ChargeDefinition)
                    .WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.OrderedItemChargeID)
                    .HasConstraintName("FK__OrderInfo_OrderedItemChargeID");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.OrderInfoAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderInfo_AuthorId");

            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.ToTable("OrderType");

                entity.Property(e => e.OrderTypeId).HasColumnName("OrderTypeID");

                entity.Property(e => e.OrderName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PainParameter>(entity =>
            {
                entity.ToTable("PainParameter");

                entity.Property(e => e.PainParameterId).HasColumnName("PainParameterID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.PainScaleTypeId).HasColumnName("PainScaleTypeID");

                entity.Property(e => e.ParameterName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.PainScaleType)
                    .WithMany(p => p.PainParameters)
                    .HasForeignKey(d => d.PainScaleTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PainParameter_PainScaleTypeID");
            });

            modelBuilder.Entity<PainRating>(entity =>
            {
                entity.ToTable("PainRating");

                entity.Property(e => e.PainRatingId).HasColumnName("PainRatingID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.PainParameterId).HasColumnName("PainParameterID");

                entity.HasOne(d => d.PainParameter)
                    .WithMany(p => p.PainRatings)
                    .HasForeignKey(d => d.PainParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PainRating_PainParameterID");
            });

            modelBuilder.Entity<PainRatingImage>(entity =>
            {
                entity.HasKey(e => e.PainRatingId)
                    .HasName("pk_PainRatingImage");

                entity.ToTable("PainRatingImage");

                entity.Property(e => e.PainRatingId)
                    .ValueGeneratedNever()
                    .HasColumnName("PainRatingID");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.HasOne(d => d.PainRating)
                    .WithOne(p => p.PainRatingImage)
                    .HasForeignKey<PainRatingImage>(d => d.PainRatingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PainRatingImage_PainRatingID");
            });

            modelBuilder.Entity<PainScaleType>(entity =>
            {
                entity.ToTable("PainScaleType");

                entity.Property(e => e.PainScaleTypeId).HasColumnName("PainScaleTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PainScaleTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UseDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Mrn);

                entity.ToTable("Patient");

                entity.Property(e => e.Mrn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MRN")
                    .IsFixedLength(true);

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");
                entity.Property(e => e.EmploymentId).HasColumnName("EmploymentID");
                entity.Property(e => e.EducationId).HasColumnName("EducationID");

                entity.Property(e => e.EthnicityId).HasColumnName("EthnicityID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GenderId).HasColumnName("GenderID");
                entity.Property(e => e.GenderPronounId).HasColumnName("GenderPronounID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaidenName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
                entity.Property(e => e.MaritalStatusId).HasColumnName("MaritalStatusID");
                entity.Property(e => e.LegalStatusId).HasColumnName("LegalStatusID");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MothersMaidenName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReligionId).HasColumnName("ReligionID");

                entity.Property(e => e.SexId).HasColumnName("SexID");

                entity.Property(e => e.Ssn)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("SSN")
                    .IsFixedLength(true);

                entity.HasOne(e => e.Person)    // dependent => principal
                    .WithOne()  // principal has no navigation back to dependent
                    .HasForeignKey<Patient>(e => e.PersonId);

                entity.HasIndex(e => e.PersonId)
                    .IsUnique();    // enforce one to one at DB level

                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.EducationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_EducationID");

                entity.HasOne(d => d.Employment)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.EmploymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_EmploymentID");

                entity.HasOne(d => d.Ethnicity)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.EthnicityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_EthnicityID");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_GenderID");

                entity.HasOne(d => d.GenderPronoun)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.GenderPronounId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_GenderPronounID");

                entity.Property(e => e.IsCurrentMilitaryServiceMember)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsVeteran)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DeceasedLiving)
                    .HasDefaultValue(false);

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_FacilityID");

                entity.HasOne(d => d.MaritalStatus)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.MaritalStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_MaritalStatusID");

                entity.HasOne(d => d.LegalStatus)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.LegalStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_LegalStatusID");

                entity.HasOne(d => d.Religion)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.ReligionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_ReligionID");

                entity.HasOne(d => d.Sex)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.SexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Patient_SexID");
            });

            modelBuilder.Entity<PatientAdvancedDirective>(entity =>
            {
                entity.Property(e => e.PatientAdvancedDirectiveId).HasColumnName("PatientAdvancedDirectiveID");

                entity.Property(e => e.AdvancedDirectiveId).HasColumnName("AdvancedDirectiveID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PatientAlertId).HasColumnName("PatientAlertID");

                entity.HasOne(d => d.AdvancedDirective)
                    .WithMany(p => p.PatientAdvancedDirectives)
                    .HasForeignKey(d => d.AdvancedDirectiveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientAdvancedDirectives_AdvancedDirectiveID");

                entity.HasOne(d => d.PatientAlert)
                    .WithMany(p => p.PatientAdvancedDirectives)
                    .HasForeignKey(d => d.PatientAlertId)
                    .HasConstraintName("fk_PatientAdvancedDirectives_PatientAlertID");
            });

            modelBuilder.Entity<PatientAlert>(entity =>
            {
                entity.Property(e => e.PatientAlertId).HasColumnName("PatientAlertID");

                entity.Property(e => e.AlertTypeId).HasColumnName("AlertTypeID");

                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Mrn)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MRN")
                    .IsFixedLength(true);

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AlertType)
                    .WithMany(p => p.PatientAlerts)
                    .HasForeignKey(d => d.AlertTypeId)
                    .HasConstraintName("fk_PatientAlerts_AlertTypeID");

                entity.HasOne(d => d.MrnNavigation)
                    .WithMany(p => p.PatientAlerts)
                    .HasForeignKey(d => d.Mrn)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_PatientAlerts_MRN");
            });

            modelBuilder.Entity<PatientAlias>(entity =>
            {
                entity.HasKey(e => e.PatientAliasID);

                entity.ToTable("PatientAlias");

                entity.Property(e => e.PatientAliasID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PatientAliasID");

                entity.Property(e => e.PatientMRN)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("PatientMRN")
                    .IsFixedLength(true);

                entity.Property(e => e.AliasFirstName)
                    .HasMaxLength(50)
                    .IsRequired(false)
                    .IsUnicode(false);

                entity.Property(e => e.AliasLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AliasMiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AliasPriority).HasColumnName("AliasPriority");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientAliases)
                    .HasForeignKey(d => d.PatientMRN)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_PatientAlias_PatientMRN");

            });

            modelBuilder.Entity<PatientAllergy>(entity =>
            {
                entity.ToTable("PatientAllergy");

                entity.Property(e => e.PatientAllergyId).HasColumnName("PatientAllergyID");

                entity.Property(e => e.AllergenId).HasColumnName("AllergenID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PatientAlertId).HasColumnName("PatientAlertID");

                entity.Property(e => e.ReactionId).HasColumnName("ReactionID");

                entity.Property(e => e.GenericMedicationId).HasColumnName("GenericMedicationID");

                entity.HasOne(d => d.Allergen)
                    .WithMany(p => p.PatientAllergies)
                    .HasForeignKey(d => d.AllergenId)
                    .HasConstraintName("fk_PatientAllergy_AllergenID");

                entity.HasOne(d => d.PatientAlert)
                    .WithMany(p => p.PatientAllergies)
                    .HasForeignKey(d => d.PatientAlertId)
                    .HasConstraintName("fk_PatientAllergy_PatientAlertID");

                entity.HasOne(d => d.Reaction)
                    .WithMany(p => p.PatientAllergies)
                    .HasForeignKey(d => d.ReactionId)
                    .HasConstraintName("fk_PatientAllergy_ReactionID");

                entity.HasOne(d => d.MedicationGenericName)
                    .WithMany(p => p.PatientAllergies)
                    .HasForeignKey(d => d.GenericMedicationId)
                    .HasConstraintName("fk_PatientAllergy_GenericMedicationID");
            });

            modelBuilder.Entity<PatientClinicalReminder>(entity =>
            {
                entity.Property(e => e.PatientClinicalReminderId).HasColumnName("PatientClinicalReminderID");

                entity.Property(e => e.ClinicalReminderId).HasColumnName("ClinicalReminderID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PatientAlertId).HasColumnName("PatientAlertID");

                entity.HasOne(d => d.ClinicalReminder)
                    .WithMany(p => p.PatientClinicalReminders)
                    .HasForeignKey(d => d.ClinicalReminderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientClinicalReminders_ClinicalReminderID");

                entity.HasOne(d => d.PatientAlert)
                    .WithMany(p => p.PatientClinicalReminders)
                    .HasForeignKey(d => d.PatientAlertId)
                    .HasConstraintName("fk_PatientClinicalReminders_PatientAlertID");
            });

            modelBuilder.Entity<PatientContactDetail>(entity =>
            {
                entity.HasKey(e => e.PatientContactId);

                entity.Property(e => e.PatientContactId).HasColumnName("PatientContactID");

                entity.Property(e => e.CellPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MailingAddressId).HasColumnName("MailingAddressID");

                entity.Property(e => e.Mrn)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MRN")
                    .IsFixedLength(true);

                entity.Property(e => e.ResidenceAddressId).HasColumnName("ResidenceAddressID");

                entity.Property(e => e.WorkPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.MailingAddress)
                    .WithMany(p => p.PatientContactDetailMailingAddresses)
                    .HasForeignKey(d => d.MailingAddressId)
                    .HasConstraintName("fk_PatientContactDetails_MailingAddressID");

                entity.HasOne(d => d.MrnNavigation)
                    .WithMany(p => p.PatientContactDetails)
                    .HasForeignKey(d => d.Mrn)
                    .HasConstraintName("fk_PatientContactDetails_MRN");

                entity.HasOne(d => d.ResidenceAddress)
                    .WithMany(p => p.PatientContactDetailResidenceAddresses)
                    .HasForeignKey(d => d.ResidenceAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientContactDetails_ResidenceAddressID");
            });

            modelBuilder.Entity<PatientContactTime>(entity =>
            {
                entity.HasKey(e => e.PatientContactTimeId);

                entity.ToTable("PatientContactTimes");

                entity.Property(e => e.PatientContactTimeId).HasColumnName("PatientContactTimeID");

                entity.Property(e => e.PatientContactId).HasColumnName("PatientContactID");

                entity.Property(e => e.ContactTimeId).HasColumnName("ContactTimeID");

                entity.Property(e => e.LastModified).HasColumnName("LastModified");

                entity.HasOne(e => e.PatientContactDetail)
                    .WithMany(d => d.PatientContactTimes)
                    .HasForeignKey(e => e.PatientContactId);

                entity.HasOne(e => e.PreferredContactTime)
                    .WithMany(t => t.PatientContactTimes)
                    .HasForeignKey(e => e.ContactTimeId);
            });

            modelBuilder.Entity<PatientEmergencyContact>(entity =>
            {
                entity.HasKey(e => e.EmergencyContactId);

                entity.ToTable("PatientEmergencyContact");

                entity.Property(e => e.EmergencyContactId).HasColumnName("EmergencyContactID");

                entity.Property(e => e.ContactAddressId).HasColumnName("ContactAddressID");

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ContactEmail)
                    .HasColumnName("Email");

                entity.Property(e => e.ContactRelationshipId).HasColumnName("ContactRelationshipID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Mrn)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MRN")
                    .IsFixedLength(true);

                entity.HasOne(d => d.ContactAddress)
                    .WithMany(p => p.PatientEmergencyContacts)
                    .HasForeignKey(d => d.ContactAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientEmergencyContact_ContactAddressID");

                entity.HasOne(d => d.ContactRelationship)
                    .WithMany(p => p.PatientEmergencyContacts)
                    .HasForeignKey(d => d.ContactRelationshipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientEmergencyContact_ContactRelationshipID");

                entity.HasOne(d => d.MrnNavigation)
                    .WithMany(p => p.PatientEmergencyContacts)
                    .HasForeignKey(d => d.Mrn)
                    .HasConstraintName("fk_PatientEmergencyContact_MRN");
            });

            modelBuilder.Entity<PatientFallRisk>(entity =>
            {
                entity.Property(e => e.PatientFallRiskId).HasColumnName("PatientFallRiskID");

                entity.Property(e => e.FallRiskId).HasColumnName("FallRiskID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PatientAlertId).HasColumnName("PatientAlertID");

                entity.HasOne(d => d.FallRisk)
                    .WithMany(p => p.PatientFallRisks)
                    .HasForeignKey(d => d.FallRiskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientFallRisks_FallRiskID");

                entity.HasOne(d => d.PatientAlert)
                    .WithMany(p => p.PatientFallRisks)
                    .HasForeignKey(d => d.PatientAlertId)
                    .HasConstraintName("fk_PatientFallRisks_PatientAlertID");
            });

            modelBuilder.Entity<PatientGeneral>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Patient_General");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("First Name");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastFourSsn)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("Last Four SSN");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Last Name");

                entity.Property(e => e.MiddleInitial)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Middle Initial");

                entity.Property(e => e.Mrn)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MRN")
                    .IsFixedLength(true);

                entity.Property(e => e.PreferredPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Preferred Phone");

                entity.Property(e => e.PrimaryInsuranceProvider)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Primary Insurance Provider");

                entity.Property(e => e.ResidentAddress)
                    .IsRequired()
                    .HasMaxLength(368)
                    .IsUnicode(false)
                    .HasColumnName("Resident Address");
            });

            modelBuilder.Entity<PatientLanguage>(entity =>
            {
                entity.HasKey(e => new { e.LanguageId, e.Mrn });

                entity.ToTable("PatientLanguage");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.Mrn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MRN")
                    .IsFixedLength(true);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.PatientLanguages)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientLanguage_LanguageID");

                entity.HasOne(d => d.MrnNavigation)
                    .WithMany(p => p.PatientLanguages)
                    .HasForeignKey(d => d.Mrn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientLanguage_MRN");
            });

            modelBuilder.Entity<PatientModeOfContact>(entity =>
            {
                entity.HasKey(e => e.PatientModeOfContactId);

                entity.ToTable("PatientModeOfContacts");

                entity.Property(e => e.PatientModeOfContactId).HasColumnName("PatientModeOfContactID");

                entity.Property(e => e.PatientContactId).HasColumnName("PatientContactID");

                entity.Property(e => e.ModeOfContactId).HasColumnName("ModeOfContactID");

                entity.Property(e => e.LastModified).HasColumnName("LastModified");

                entity.HasOne(e => e.PatientContactDetail)
                    .WithMany(d => d.PatientModeOfContacts)
                    .HasForeignKey(e => e.PatientContactId);

                entity.HasOne(e => e.PreferredModeOfContact)
                    .WithMany(c => c.PatientModeOfContacts)
                    .HasForeignKey(e => e.ModeOfContactId);
            });

            modelBuilder.Entity<PatientRace>(entity =>
            {
                entity.HasKey(e => new { e.RaceId, e.Mrn });

                entity.ToTable("PatientRace");

                entity.Property(e => e.RaceId).HasColumnName("RaceID");

                entity.Property(e => e.Mrn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MRN")
                    .IsFixedLength(true);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.MrnNavigation)
                    .WithMany(p => p.PatientRaces)
                    .HasForeignKey(d => d.Mrn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientRace_MRN");

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.PatientRaces)
                    .HasForeignKey(d => d.RaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientRace_RaceID");
            });

            modelBuilder.Entity<PatientRepresentative>(entity =>
            {
                entity.HasKey(e => e.RepresentativeId);

                entity.ToTable("PatientRepresentative");

                entity.Property(e => e.RepresentativeId).HasColumnName("RepresentativeID");
                entity.Property(e => e.Comments)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.RepresentativeEmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.RepresentativeFirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RepresentativeLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RepresentativePhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PatientRestriction>(entity =>
            {
                entity.HasKey(e => e.RestrictionId);

                entity.Property(e => e.RestrictionId).HasColumnName("RestrictionID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PatientAlertId).HasColumnName("PatientAlertID");

                entity.Property(e => e.RestrictionTypeId).HasColumnName("RestrictionTypeID");

                entity.HasOne(d => d.PatientAlert)
                    .WithMany(p => p.PatientRestrictions)
                    .HasForeignKey(d => d.PatientAlertId)
                    .HasConstraintName("fk_PatientRestrictions_PatientAlertID");

                entity.HasOne(d => d.RestrictionType)
                    .WithMany(p => p.PatientRestrictions)
                    .HasForeignKey(d => d.RestrictionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PatientRestrictions_RestrictionID");
            });

            modelBuilder.Entity<PaymentPlan>(entity =>
            {
                entity.ToTable("PaymentPlan");

                entity.Property(e => e.PaymentPlanId).HasColumnName("PaymentPlanID");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WiPopCode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<PaymentSource>(entity =>
            {
                entity.ToTable("PaymentSource");

                entity.Property(e => e.PaymentSourceId).HasColumnName("PaymentSourceID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WiPopCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.HasKey(e => e.PaymentTypeId).HasName("pk_PaymentType");

                entity.ToTable("PaymentType");

                entity.Property(e => e.PaymentTypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PaymentTypeID");
                entity.Property(e => e.PaymentType1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PaymentType");
            });

            modelBuilder.Entity<Pcacomment>(entity =>
            {
                entity.ToTable("PCAComment");

                entity.Property(e => e.PcacommentId).HasColumnName("PCACommentID");

                entity.Property(e => e.DateCommentAdded).HasColumnType("datetime");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Pcacomment1)
                    .IsUnicode(false)
                    .HasColumnName("PCAComment");

                entity.Property(e => e.PcacommentTypeId).HasColumnName("PCACommentTypeID");

                entity.Property(e => e.Pcaid).HasColumnName("PCAID");

                entity.HasOne(d => d.PcacommentType)
                    .WithMany(p => p.Pcacomments)
                    .HasForeignKey(d => d.PcacommentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PCAComment_PCACommentTypeID");

                entity.HasOne(d => d.Pca)
                    .WithMany(p => p.Pcacomments)
                    .HasForeignKey(d => d.Pcaid)
                    .OnDelete(DeleteBehavior.Cascade)   // if PCA deleted, delete PCAComment
                    .HasConstraintName("fk_PCAComment_PCAID");
            });

            modelBuilder.Entity<PcacommentType>(entity =>
            {
                entity.ToTable("PCACommentType");

                entity.Property(e => e.PcacommentTypeId).HasColumnName("PCACommentTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PcacommentTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PCACommentTypeName");
            });

            modelBuilder.Entity<PcapainAssessment>(entity =>
            {
                entity.HasKey(e => e.PainAssessmentId)
                    .HasName("pk_PCAPainAssessment");

                entity.ToTable("PCAPainAssessment");

                entity.Property(e => e.PainAssessmentId).HasColumnName("PainAssessmentID");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.PainParameterId).HasColumnName("PainParameterID");

                entity.Property(e => e.PainRatingId).HasColumnName("PainRatingID");

                entity.Property(e => e.Pcaid).HasColumnName("PCAID");

                entity.HasOne(d => d.PainParameter)
                    .WithMany(p => p.PcapainAssessments)
                    .HasForeignKey(d => d.PainParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PCAPainAssessment_PainParameterID");

                entity.HasOne(d => d.PainRating)
                    .WithMany(p => p.PcapainAssessments)
                    .HasForeignKey(d => d.PainRatingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PCAPainAssessment_PainRatingID");

                entity.HasOne(d => d.Pca)
                    .WithMany(p => p.PcapainAssessments)
                    .HasForeignKey(d => d.Pcaid)
                    .OnDelete(DeleteBehavior.Cascade)  // if PCA is deleted, the associated PCAPainAssessment is deleted
                    .HasConstraintName("fk_PCAPainAssessment_PCAID");
            });

            modelBuilder.Entity<Pcarecord>(entity =>
            {
                entity.HasKey(e => e.Pcaid);

                entity.ToTable("PCARecord");

                entity.Property(e => e.Pcaid).HasColumnName("PCAID");

                entity.Property(e => e.BloodPressureRouteTypeId).HasColumnName("BloodPressureRouteTypeID");

                entity.Property(e => e.BmimethodId).HasColumnName("BMIMethodID");

                entity.Property(e => e.BodyMassIndex).HasColumnType("decimal(7, 3)");

                entity.Property(e => e.DateVitalsAdded).HasColumnType("datetime");

                entity.Property(e => e.EncounterId).HasColumnName("EncounterID");

                entity.Property(e => e.HeadCircumference).HasColumnType("decimal(7, 3)");

                entity.Property(e => e.HeadCircumferenceUnits)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Height).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.HeightUnits)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.O2deliveryTypeId).HasColumnName("O2DeliveryTypeID");

                entity.Property(e => e.OxygenFlow)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PainScaleTypeId).HasColumnName("PainScaleTypeID");

                entity.Property(e => e.PercentOxygenDelivered).HasColumnType("decimal(5, 4)");

                entity.Property(e => e.PulseRouteTypeId).HasColumnName("PulseRouteTypeID");

                entity.Property(e => e.TempRouteTypeId).HasColumnName("TempRouteTypeID");

                entity.Property(e => e.Temperature).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.WeightUnits)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BloodPressureRouteType)
                    .WithMany(p => p.Pcarecords)
                    .HasForeignKey(d => d.BloodPressureRouteTypeId)
                    .HasConstraintName("fk_PCARecord_BloodPressureRouteTypeID");

                entity.HasOne(d => d.Bmimethod)
                    .WithMany(p => p.Pcarecords)
                    .HasForeignKey(d => d.BmimethodId)
                    .HasConstraintName("fk_PCARecord_BMIMethodID");

                entity.HasOne(d => d.Encounter)
                    .WithMany(p => p.Pcarecords)
                    .HasForeignKey(d => d.EncounterId)
                    .HasConstraintName("fk_PCARecord_EncounterID");

                entity.HasOne(d => d.O2deliveryType)
                    .WithMany(p => p.Pcarecords)
                    .HasForeignKey(d => d.O2deliveryTypeId)
                    .HasConstraintName("fk_PCARecord_O2DeliveryTypeID");

                entity.HasOne(d => d.PainScaleType)
                    .WithMany(p => p.Pcarecords)
                    .HasForeignKey(d => d.PainScaleTypeId)
                    .HasConstraintName("fk_PCARecord_PainScaleTypeID");

                entity.HasOne(d => d.PulseRouteType)
                    .WithMany(p => p.Pcarecords)
                    .HasForeignKey(d => d.PulseRouteTypeId)
                    .HasConstraintName("fk_PCARecord_PulseRouteTypeID");

                entity.HasOne(d => d.TempRouteType)
                    .WithMany(p => p.Pcarecords)
                    .HasForeignKey(d => d.TempRouteTypeId)
                    .HasConstraintName("fk_PCARecord_TempRouteTypeID");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.PersonId);
                entity.ToTable("Person");
                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");
                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");
                entity.Property(e => e.EmploymentId).HasColumnName("EmploymentID");
                entity.Property(e => e.EducationId).HasColumnName("EducationID");
                entity.Property(e => e.EthnicityId).HasColumnName("EthnicityID");
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.GenderId).HasColumnName("GenderID");
                entity.Property(e => e.GenderPronounId).HasColumnName("GenderPronounID");
                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime2(7)")
                    .IsRequired()
                    .HasDefaultValueSql("SYSUTCDATETIME()");
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MaidenName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MaritalStatusId).HasColumnName("MaritalStatusID");
                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MothersMaidenName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Suffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ReligionId).HasColumnName("ReligionID");
                entity.Property(e => e.SexId).HasColumnName("SexID");
                entity.Property(e => e.Ssn)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("SSN")
                    .IsFixedLength(true);
                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.EducationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_EducationID");
                entity.HasOne(d => d.Employment)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.EmploymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_EmploymentID");
                entity.HasOne(d => d.Ethnicity)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.EthnicityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_EthnicityID");
                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_GenderID");
                entity.HasOne(d => d.GenderPronoun)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.GenderPronounId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_GenderPronounID");
                entity.Property(e => e.IsCurrentMilitaryServiceMember)
                    .HasDefaultValue(false)
                    .IsRequired();
                entity.Property(e => e.IsVeteran)
                    .HasDefaultValue(false)
                    .IsRequired();
                entity.Property(e => e.DeceasedLiving)
                    .HasDefaultValue(false)
                    .IsRequired();
                entity.Property(e => e.InterpreterNeeded)
                    .HasDefaultValue(false)
                    .IsRequired();
                entity.HasOne(d => d.MaritalStatus)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.MaritalStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_MaritalStatusID");
                entity.HasOne(d => d.Religion)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.ReligionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_ReligionID");
                entity.HasOne(d => d.Sex)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.SexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_SexID");
            });

            modelBuilder.Entity<PersonContactDetail>(entity =>
            {
                entity.ToTable("PersonContactDetail", "dbo");
                // Primary key
                entity.HasKey(e => e.PersonContactDetailId)
                    .HasName("PK_PersonContactDetail");
                // Column mappings and types
                entity.Property(e => e.PersonContactDetailId)
                    .HasColumnName("PersonContactDetailID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint")
                    .IsRequired();
                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .HasColumnType("int")
                    .IsRequired();
                entity.Property(e => e.CellPhone)
                    .HasColumnName("CellPhone")
                    .HasColumnType("varchar(10)")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.HomePhone )
                    .HasColumnName("HomePhone ")
                    .HasColumnType("varchar(10)")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.WorkPhone )
                    .HasColumnName("WorkPhone ")
                    .HasColumnType("varchar(10)")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.EmailAddress )
                    .HasColumnName("EmailAddress ")
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.MailingAddressId)
                    .HasColumnName("MailingAddressID")
                    .HasColumnType("int")
                    .IsRequired(false);
                entity.Property(e => e.ResidenceAddressId)
                    .HasColumnName("ResidenceAddressID")
                    .HasColumnType("int")
                    .IsRequired(false);
                entity.Property(e => e.LastModified)
                    .HasColumnName("LastModified")
                    .HasColumnType("datetime2(7)")
                    .IsRequired();

                // enforce one-to-one: unique index on PersonId, no foreign key on PersonContactDetail within Person when done this way
                entity.HasIndex(e => e.PersonId)
                    .IsUnique()
                    .HasDatabaseName("UQ_PersonContactDetail_PersonId");

                // Relationships

                // Person (one-to-one)
                entity.HasOne(d => d.Person)
                    .WithOne(p => p.PersonContactDetail)
                    .HasForeignKey<PersonContactDetail>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                entity.HasOne(pr => pr.MailingAddress)
                    .WithMany(r => r.MailingAddresses)      // Address has ICollection<PersonContactDetail> MailingAddresses
                    .HasForeignKey(pr => pr.MailingAddressId)
                    .HasConstraintName("FK_MailingAddress_Address")
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                entity.HasOne(pr => pr.ResidenceAddress)
                    .WithMany(r => r.ResidenceAddresses)      // Address has ICollection<PersonContactDetail> ResidenceAddresses
                    .HasForeignKey(pr => pr.ResidenceAddressId)
                    .HasConstraintName("FK_ResidenceAddress_Address")
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            });

            modelBuilder.Entity<PersonContactTime>(entity =>
            {
                entity.ToTable("PersonContactTime", "dbo");
            // Primary key
                entity.HasKey(e => e.PersonContactTimeId)
                    .HasName("PK_PersonContactTime");
            // Column mappings and types
                entity.Property(e => e.PersonContactTimeId)
                    .HasColumnName("PersonContactTimeID")
                .ValueGeneratedOnAdd()
                .HasColumnType("bigint")
            .IsRequired();
            entity.Property(e => e.PersonContactDetailId)
                    .HasColumnName("PersonContactDetailID")
                    .HasColumnType("bigint")
                    .IsRequired();
            entity.Property(e => e.ContactTimeId)
                    .HasColumnName("ContactTimeID")
                    .HasColumnType("int")
                    .IsRequired();
            entity.Property(e => e.LastModified)
                    .HasColumnName("LastModified")
                    .HasColumnType("datetime2(7)")
                    .IsRequired();
            entity.HasOne(pr => pr.PersonContactDetail)
                    .WithMany(p => p.PersonContactTimes)      // PersonContactDetail has ICollection<PersonContactTime> PersonContactTimes
                    .HasForeignKey(pr => pr.PersonContactDetailId)
                    .HasConstraintName("FK_PersonContactTime_PersonContactDetail")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            entity.HasOne(pr => pr.PreferredContactTime )
                    .WithMany(r => r.PersonContactTimes)      // PreferredContactTime has ICollection<PersonContactTime> PersonContactTimes
                    .HasForeignKey(pr => pr.ContactTimeId)
                    .HasConstraintName("FK_PersonContactTime_PreferredContactTime")
                .IsRequired();
            });

            modelBuilder.Entity<PersonLanguage>(entity =>
            {
                entity.ToTable("PersonLanguage", "dbo");
            // Primary key
                entity.HasKey(e => e.PersonLanguageId)
                    .HasName("PK_PersonLanguage");
            // Column mappings and types
                entity.Property(e => e.PersonLanguageId)
                    .HasColumnName("PersonLanguageID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("smallint")
                    .IsRequired();
            entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .HasColumnType("int")
                    .IsRequired();
            entity.Property(e => e.LanguageId)
                    .HasColumnName("LanguageID")
                    .HasColumnType("smallint")
                    .IsRequired();
            entity.Property(e => e.LastModified)
                    .HasColumnName("LastModified")
                    .HasColumnType("datetime2(7)")
                    .IsRequired();
            entity.HasOne(pr => pr.Person)
                    .WithMany(p => p.PersonLanguages)      // Person has ICollection<PersonLanguage> PersonLanguages
                    .HasForeignKey(pr => pr.PersonId)
                    .HasConstraintName("FK_PersonLanguage_Person")
                    .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pr => pr.Language)
                    .WithMany(r => r.PersonLanguages)      // Language has ICollection<PersonLanguage> PersonLanguages
                    .HasForeignKey(pr => pr.LanguageId)
                    .HasConstraintName("FK_PersonLanguage_Language")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PersonModeOfContact>(entity =>
            {
                entity.ToTable("PersonModeOfContact", "dbo");
            // Primary key
                entity.HasKey(e => e.PersonModeOfContactId)
                    .HasName("PK_PersonModeOfContact");
            // Column mappings and types
                entity.Property(e => e.PersonModeOfContactId)
                    .HasColumnName("PersonModeOfContactID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint")
                    .IsRequired();
                entity.Property(e => e.PersonContactDetailId)
                    .HasColumnName("PersonContactDetailID")
                    .HasColumnType("bigint")
                    .IsRequired();
                entity.Property(e => e.ModeOfContactId)
                    .HasColumnName("ModeOfContactID")
                    .HasColumnType("int")
                    .IsRequired();
                entity.Property(e => e.LastModified)
                    .HasColumnName("LastModified")
                    .HasColumnType("datetime2(7)")
                    .IsRequired();
                entity.HasOne(pr => pr.PersonContactDetail)
                    .WithMany(p => p.PersonModeOfContacts)      // PersonContactDetail has ICollection<PersonModeOfContact> PersonModeOfContacts
                    .HasForeignKey(pr => pr.PersonContactDetailId)
                    .HasConstraintName("FK_PersonModeOfContact_PersonContactDetail")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                entity.HasOne(pr => pr.PreferredModeOfContact )
                    .WithMany(r => r.PersonModeOfContacts)      // PreferredModeOfContact has ICollection<PersonModeOfContact> PersonModeOfContacts
                    .HasForeignKey(pr => pr.ModeOfContactId)
                    .HasConstraintName("FK_PersonModeOfContact_PreferredModeOfContact")
                    .IsRequired();
            });

            modelBuilder.Entity<PersonRace>(entity =>
            {
                entity.ToTable("PersonRace", "dbo");
            // Primary key
                entity.HasKey(e => e.PersonRaceId)
                    .HasName("PK_PersonRace");
            // Column mappings and types
                entity.Property(e => e.PersonRaceId)
                    .HasColumnName("PersonRaceID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("tinyint")
                    .IsRequired();
            entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .HasColumnType("int")
                    .IsRequired();
            entity.Property(e => e.RaceId)
                    .HasColumnName("RaceID")
                    .HasColumnType("tinyint")
                    .IsRequired();
            entity.Property(e => e.LastModified)
                    .HasColumnName("LastModified")
                    .HasColumnType("datetime2(7)")
                    .IsRequired();
            entity.HasOne(pr => pr.Person)
                    .WithMany(p => p.PersonRaces)      // Person has ICollection<PersonRace> PersonRaces
                    .HasForeignKey(pr => pr.PersonId)
                    .HasConstraintName("FK_PersonRace_Person")
                    .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pr => pr.Race)
                    .WithMany(r => r.PersonRaces)      // Race has ICollection<PersonRace> PersonRaces
                    .HasForeignKey(pr => pr.RaceId)
                    .HasConstraintName("FK_PersonRace_Race")
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Physician>(entity =>
            {
                entity.ToTable("Physician");

                entity.Property(e => e.PhysicianId).HasColumnName("PhysicianID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Credentials)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.License)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProviderTypeId).HasColumnName("ProviderTypeID");

                entity.Property(e => e.ProviderStatusId).HasColumnName("ProviderStatusID");

                entity.Property(e => e.SpecialtyId).HasColumnName("SpecialtyID");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Physicians)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Physician_AddressID");

                entity.HasOne(d => d.ProviderType)
                    .WithMany(p => p.Physicians)
                    .HasForeignKey(d => d.ProviderTypeId)
                    .HasConstraintName("FK_Physician_ProviderTypeID");

                entity.HasOne(d => d.ProviderStatus)
                    .WithMany(p => p.Physicians)
                    .HasForeignKey(d => d.ProviderStatusId)
                    .HasConstraintName("FK_Physician_ProviderStatusID");

                entity.HasOne(d => d.Specialty)
                    .WithMany(p => p.Physicians)
                    .HasForeignKey(d => d.SpecialtyId)
                    .HasConstraintName("FK_Physician_SpecialtyID");
            });

            modelBuilder.Entity<PhysicianAssessment>(entity =>
            {
                entity.ToTable("PhysicianAssessment");

                entity.Property(e => e.PhysicianAssessmentId).HasColumnName("PhysicianAssessmentID");

                entity.Property(e => e.Assessment).IsUnicode(false);

                entity.Property(e => e.ChiefComplaint).IsUnicode(false);

                entity.Property(e => e.EncounterId).HasColumnName("EncounterID");

                entity.Property(e => e.FamilyHistory).IsUnicode(false);

                entity.Property(e => e.HistoryOfPresentIllness).IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhysicianAssessmentDate).HasColumnType("date");

                entity.Property(e => e.PhysicianAssessmentTypeId).HasColumnName("PhysicianAssessmentTypeID");

                entity.Property(e => e.Plan).IsUnicode(false);

                entity.Property(e => e.SignificantDiagnosticTests).IsUnicode(false);

                entity.Property(e => e.SocialHistory).IsUnicode(false);

                entity.Property(e => e.WrittenDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AuthoringProviderNavigation)
                    .WithMany(p => p.PhysicianAssessmentAuthoringProviderNavigations)
                    .HasForeignKey(d => d.AuthoringProvider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PAAuthoringProvider");

                entity.HasOne(d => d.CoSignatureNavigation)
                    .WithMany(p => p.PhysicianAssessmentCoSignatureNavigations)
                    .HasForeignKey(d => d.CoSignature)
                    .HasConstraintName("fk_PACoSignature");

                entity.HasOne(d => d.Encounter)
                    .WithMany(p => p.PhysicianAssessments)
                    .HasForeignKey(d => d.EncounterId)
                    .HasConstraintName("fk_PAEncounterID");

                entity.HasOne(d => d.PhysicianAssessmentType)
                    .WithMany(p => p.PhysicianAssessments)
                    .HasForeignKey(d => d.PhysicianAssessmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PhysicianAssessmentTypeID");

                entity.HasOne(d => d.ReferringProviderNavigation)
                    .WithMany(p => p.PhysicianAssessmentReferringProviderNavigations)
                    .HasForeignKey(d => d.ReferringProvider)
                    .HasConstraintName("fk_PAReferringProvider");
            });

            modelBuilder.Entity<PhysicianAssessmentAllergy>(entity =>
            {
                entity.HasKey(e => e.PhysicianAssessmentAllergyId)
                    .HasName("PK_PhysicianAssessmentAllergies");

                entity.Property(e => e.PhysicianAssessmentAllergyId)
                    .HasColumnName("PhysicianAssessmentAllergyID");

                entity.Property(e => e.PhysicianAssessmentId)
                    .HasColumnName("PhysicianAssessmentID");

                entity.HasOne(d => d.PhysicianAssessment)
                    .WithMany(p => p.PhysicianAssessmentAllergies)
                    .HasForeignKey(d => d.PhysicianAssessmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PhysicianAssessmentAllergies_PhysicianAssessment");

            });



            modelBuilder.Entity<PhysicianAssessmentType>(entity =>
            {
                entity.ToTable("PhysicianAssessmentType");

                entity.Property(e => e.PhysicianAssessmentTypeId).HasColumnName("PhysicianAssessmentTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PhysicianRole>(entity =>
            {
                entity.ToTable("PhysicianRole");

                entity.Property(e => e.PhysicianRoleId).HasColumnName("PhysicianRoleID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PlaceOfServiceOutPatient>(entity =>
            {
                entity.HasKey(e => e.PlaceOfServiceId);

                entity.ToTable("PlaceOfServiceOutPatient");

                entity.Property(e => e.PlaceOfServiceId).HasColumnName("PlaceOfServiceID");

                entity.Property(e => e.Description)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PointOfOrigin>(entity =>
            {
                entity.ToTable("PointOfOrigin");

                entity.Property(e => e.PointOfOriginId).HasColumnName("PointOfOriginID");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Explaination).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WiPopCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PreferredContactTime>(entity =>
            {
                entity.HasKey(e => e.ContactTimeId);

                entity.ToTable("PreferredContactTime");

                entity.Property(e => e.ContactTimeId).HasColumnName("ContactTimeID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PreferredModeOfContact>(entity =>
            {
                entity.HasKey(e => e.ModeOfContactId);

                entity.ToTable("PreferredModeOfContact");

                entity.Property(e => e.ModeOfContactId).HasColumnName("ModeOfContactID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PregnancyInfection>(entity =>
            {
                entity.HasKey(e => e.InfectionId).HasName("pk_PregnancyInfection");

                entity.ToTable("PregnancyInfection");

                entity.Property(e => e.InfectionId)
                    .HasColumnName("InfectionID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e. InfectionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InfectionDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PregnancyRiskFactor>(entity =>
            {
                entity.HasKey(e => e.RiskFactorId).HasName("pk_PregnancyRiskFactors");

                entity.ToTable("PregnancyRiskFactor");

                entity.Property(e => e.RiskFactorId)
                    .HasColumnName("RiskFactorID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e. RiskFactorName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RiskFactorDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<Prenatal>(entity =>
            {
                entity.ToTable("Prenatal");

                entity.Property(e => e.PrenatalId).HasColumnName("PrenatalID");
                entity.Property(e => e.BirthId).HasColumnName("BirthID");
                entity.Property(e => e.DescInfertilityTreatment)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.DescPrenatalCare)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.IsWicrecipient).HasColumnName("IsWICRecipient");
                entity.Property(e => e.MothersHeightInInches).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.PrepregnancyWeightInLbs).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.WeightAtDeliveryInLbs).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Birth).WithMany(p => p.Prenatals)
                    .HasForeignKey(d => d.BirthId)
                    .HasConstraintName("FK_Prenatal_BirthID");

                entity.HasMany(d => d.Infections).WithMany(p => p.Prenatals)
                    .UsingEntity<Dictionary<string, object>>(
                        "PrenatalInfection",
                        r => r.HasOne<PregnancyInfection>().WithMany()
                            .HasForeignKey("InfectionId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_PrenatalInfections_InfectionID"),
                        l => l.HasOne<Prenatal>().WithMany()
                            .HasForeignKey("PrenatalId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_PrenatalInfections_PrenatalID"),
                        j =>
                        {
                            j.HasKey("PrenatalId", "InfectionId");
                            j.ToTable("PrenatalInfections");
                            j.IndexerProperty<int>("PrenatalId").HasColumnName("PrenatalID");
                            j.IndexerProperty<byte>("InfectionId").HasColumnName("InfectionID");
                        });

                entity.HasMany(d => d.RiskFactors).WithMany(p => p.Prenatals)
                    .UsingEntity<Dictionary<string, object>>(
                        "PrenatalRiskFactor",
                        r => r.HasOne<PregnancyRiskFactor>().WithMany()
                            .HasForeignKey("RiskFactorId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_PrenatalRiskFactors_RiskFactorID"),
                        l => l.HasOne<Prenatal>().WithMany()
                            .HasForeignKey("PrenatalId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_PrenatalRiskFactors_PrenatalID"),
                        j =>
                        {
                            j.HasKey("PrenatalId", "RiskFactorId");
                            j.ToTable("PrenatalRiskFactors");
                            j.IndexerProperty<int>("PrenatalId").HasColumnName("PrenatalID");
                            j.IndexerProperty<byte>("RiskFactorId").HasColumnName("RiskFactorID");
                        });
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.ToTable("Priority");

                entity.Property(e => e.PriorityId).HasColumnName("PriorityID");

                entity.Property(e => e.PriorityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProcedureReport>(entity =>
            {
                entity.ToTable("ProcedureReport");

                entity.Property(e => e.ProcedureReportId).HasColumnName("ProcedureReportID");

                entity.Property(e => e.Complications).IsUnicode(false);

                entity.Property(e => e.DescriptionOfProcedure).IsUnicode(false);

                entity.Property(e => e.Drains).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EncounterId).HasColumnName("EncounterID");

                entity.Property(e => e.EstiamtedBloodLoss).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OperativeIndications).IsUnicode(false);

                entity.Property(e => e.PostoperativeDiagnosis).IsUnicode(false);

                entity.Property(e => e.PreoperativeDiagonsis).IsUnicode(false);

                entity.Property(e => e.ProcedureDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(CONVERT([date],getdate()))");

                entity.Property(e => e.ProcedurePerformed).IsUnicode(false);

                entity.Property(e => e.WrittenDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AuthoringProviderNavigation)
                    .WithMany(p => p.ProcedureReportAuthoringProviderNavigations)
                    .HasForeignKey(d => d.AuthoringProvider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AuthoringProvider");

                entity.HasOne(d => d.CoSignatureNavigation)
                    .WithMany(p => p.ProcedureReportCoSignatureNavigations)
                    .HasForeignKey(d => d.CoSignature)
                    .HasConstraintName("fk_PRCoSignature");

                entity.HasOne(d => d.Encounter)
                    .WithMany(p => p.ProcedureReports)
                    .HasForeignKey(d => d.EncounterId)
                    .HasConstraintName("fk_PREncounterID");
            });

            modelBuilder.Entity<ProcedureReportAnestheticType>(entity =>
            {
                entity.HasKey(e => new { e.ProcedureReportId, e.AnestheticTypeId })
                    .HasName("pk_ProcedureReportAnestheticType");

                entity.ToTable("ProcedureReportAnestheticType");

                entity.Property(e => e.ProcedureReportId).HasColumnName("ProcedureReportID");

                entity.Property(e => e.AnestheticTypeId).HasColumnName("AnestheticTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AnestheticType)
                    .WithMany(p => p.ProcedureReportAnestheticTypes)
                    .HasForeignKey(d => d.AnestheticTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AnestheticTypeID");

                entity.HasOne(d => d.ProcedureReport)
                    .WithMany(p => p.ProcedureReportAnestheticTypes)
                    .HasForeignKey(d => d.ProcedureReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PRATProcedureReportID");
            });

            modelBuilder.Entity<ProcedureReportPhysician>(entity =>
            {
                entity.HasKey(e => new { e.ProcedureReportId, e.PhysicianId })
                    .HasName("pk_ProcedureReportPhysician");

                entity.ToTable("ProcedureReportPhysician");

                entity.Property(e => e.ProcedureReportId).HasColumnName("ProcedureReportID");

                entity.Property(e => e.PhysicianId).HasColumnName("PhysicianID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhysicianRoleId).HasColumnName("PhysicianRoleID");

                entity.HasOne(d => d.Physician)
                    .WithMany(p => p.ProcedureReportPhysicians)
                    .HasForeignKey(d => d.PhysicianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PRPPhysicianID");

                entity.HasOne(d => d.ProcedureReport)
                    .WithMany(p => p.ProcedureReportPhysicians)
                    .HasForeignKey(d => d.ProcedureReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PRPProcedureReportID");
            });

            modelBuilder.Entity<Program>(entity =>
            {
                entity.ToTable("Program");

                entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProgramFacility>(entity =>
            {
                entity.HasKey(e => e.ProgramFacilitiesId)
                    .HasName("pk_ProgramFacilities");

                entity.HasIndex(e => new { e.ProgramId, e.FacilityId }, "uk_ProgramFacilities")
                    .IsUnique();

                entity.Property(e => e.ProgramFacilitiesId).HasColumnName("ProgramFacilitiesID");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.ProgramFacilities)
                    .HasForeignKey(d => d.FacilityId)
                    .HasConstraintName("fk_ProgramFacilities_FacilityID");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.ProgramFacilities)
                    .HasForeignKey(d => d.ProgramId)
                    .HasConstraintName("fk_ProgramFacilities_ProgramID");
            });

            modelBuilder.Entity<ProgressNote>(entity =>
            {
                entity.ToTable("ProgressNote");

                entity.Property(e => e.ProgressNoteId).HasColumnName("ProgressNoteID");

                entity.Property(e => e.CoPhysicianId).HasColumnName("CoPhysicianID");

                entity.Property(e => e.EncounterId).HasColumnName("EncounterID");

                entity.Property(e => e.LastUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Note).IsUnicode(false);

                entity.Property(e => e.NoteTypeId).HasColumnName("NoteTypeID");

                entity.Property(e => e.PhysicianId).HasColumnName("PhysicianID");

                entity.Property(e => e.WrittenDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuthoringProviderSignature).HasColumnName("AuthoringProviderSignature");

                entity.Property(e => e.AuthoringProviderSignedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CoSigningProviderSignature).HasColumnName("CoSigningProviderSignature");

                entity.Property(e => e.CoSigningProviderSignedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CoPhysician)
                    .WithMany(p => p.ProgressNoteCoPhysicians)
                    .HasForeignKey(d => d.CoPhysicianId)
                    .HasConstraintName("fk_CoPhysicanID");

                entity.HasOne(d => d.Encounter)
                    .WithMany(p => p.ProgressNotes)
                    .HasForeignKey(d => d.EncounterId)
                    .HasConstraintName("fk_PNEncounterID");

                entity.HasOne(d => d.NoteType)
                    .WithMany(p => p.ProgressNotes)
                    .HasForeignKey(d => d.NoteTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_NoteTypeID");

                entity.HasOne(d => d.Physician)
                    .WithMany(p => p.ProgressNotePhysicians)
                    .HasForeignKey(d => d.PhysicianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PNPhysicanID");
            });

            modelBuilder.Entity<ProviderType>(entity =>
            {
                entity.ToTable("ProviderType");

                entity.Property(e => e.ProviderTypeId)
                    .HasColumnName("ProviderTypeID");

                entity.Property(e => e.Description)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProviderStatus>(entity =>
            {
                entity.ToTable("ProviderStatus");

                entity.Property(e => e.ProviderStatusId)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PulseRouteType>(entity =>
            {
                entity.ToTable("PulseRouteType");

                entity.Property(e => e.PulseRouteTypeId).HasColumnName("PulseRouteTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PulseRouteTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Race>(entity =>
            {
                entity.ToTable("Race");

                entity.Property(e => e.RaceId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RaceID");

                entity.Property(e => e.Category)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);


                entity.Property(e => e.WhiacCode);
            });

            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.ToTable("Reaction");

                entity.Property(e => e.ReactionId).HasColumnName("ReactionID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.ToTable("Relationship");

                entity.Property(e => e.RelationshipId).HasColumnName("RelationshipID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Religion>(entity =>
            {
                entity.ToTable("Religion");

                entity.Property(e => e.ReligionId).HasColumnName("ReligionID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(e => e.RequestId).HasName("pk_Request");

                entity.ToTable("Request");

                entity.Property(e => e.RequestId).HasColumnName("RequestID");
                entity.Property(e => e.Comments)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Defendant)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
                entity.Property(e => e.IsTpodisclosure).HasColumnName("IsTPODisclosure");
                entity.Property(e => e.PatientDob).HasColumnName("PatientDOB");
                entity.Property(e => e.PatientEmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.PatientFirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PatientLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PatientMiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PatientMrn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("PatientMRN");
                entity.Property(e => e.PatientPhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.PatientRepresentativeId).HasColumnName("PatientRepresentativeID");
                entity.Property(e => e.Plaintiff)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.RequestPriorityId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("RequestPriorityID");
                entity.Property(e => e.RequestPurposeId).HasColumnName("RequestPurposeID");
                entity.Property(e => e.RequestReleaseFormatId).HasColumnName("RequestReleaseFormatID");
                entity.Property(e => e.RequestStatusId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("RequestStatusID");
                entity.Property(e => e.RequestStatusReasonId).HasColumnName("RequestStatusReasonID");
                entity.Property(e => e.RequesterId).HasColumnName("RequesterID");
                entity.Property(e => e.CaseNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompletedByNavigation).WithMany(p => p.RequestCompletedByNavigations)
                    .HasForeignKey(d => d.CompletedBy)
                    .HasConstraintName("fk_Request_CompletedBy");

                entity.HasOne(d => d.EnteredByNavigation).WithMany(p => p.RequestEnteredByNavigations)
                    .HasForeignKey(d => d.EnteredBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Request_EnteredBy");

                entity.HasOne(d => d.Facility).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.FacilityId)
                    .HasConstraintName("fk_Request_FacilityID");

                entity.HasOne(d => d.PatientMrnNavigation).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.PatientMrn)
                    .HasConstraintName("fk_Request_PatientMRN");

                entity.HasOne(d => d.PatientRepresentative).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.PatientRepresentativeId)
                    .HasConstraintName("fk_Request_PatientRepresentativeID");

                entity.HasOne(d => d.RequestPriority).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.RequestPriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Request_RequestPriorityID");

                entity.HasOne(d => d.RequestPurpose).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.RequestPurposeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Request_RequestPurposeID");

                entity.HasOne(d => d.RequestReleaseFormat).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.RequestReleaseFormatId)
                    .HasConstraintName("fk_Request_RequestReleaseFormatID");

                entity.HasOne(d => d.RequestStatus).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.RequestStatusId)
                    .HasConstraintName("fk_Request_RequestStatusID");

                entity.HasOne(d => d.RequestStatusReason).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.RequestStatusReasonId)
                    .HasConstraintName("fk_Request_RequestStatusReasonID");

                entity.HasOne(d => d.Requester).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.RequesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Request_RequesterID");
            });

            modelBuilder.Entity<RequestPriority>(entity =>
            {
                entity.HasKey(e => e.PriorityId);

                entity.ToTable("RequestPriority");

                entity.Property(e => e.PriorityId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("PriorityID");
                entity.Property(e => e.PriorityDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.RequestPriority1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RequestPriority");
            });

            modelBuilder.Entity<RequestPurpose>(entity =>
            {
                entity.HasKey(e => e.PurposeId).HasName("pk_RequestPurpose");

                entity.ToTable("RequestPurpose");

                entity.Property(e => e.PurposeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PurposeID");
                entity.Property(e => e.PurposeDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.RequestPurpose1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RequestPurpose");
            });

            modelBuilder.Entity<RequestReleaseFormat>(entity =>
            {
                entity.HasKey(e => e.ReleaseFormatId).HasName("pk_RequestReleaseFormat");

                entity.ToTable("RequestReleaseFormat");

                entity.Property(e => e.ReleaseFormatId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ReleaseFormatID");
                entity.Property(e => e.ReleaseFormatDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.RequestReleaseFormat1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RequestReleaseFormat");
            });

            modelBuilder.Entity<RequestStatus>(entity =>
            {
                entity.ToTable("RequestStatus");

                entity.Property(e => e.RequestStatusId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("RequestStatusID");
                entity.Property(e => e.RequestStatus1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RequestStatus");
                entity.Property(e => e.StatusDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RequestStatusReason>(entity =>
            {
                entity.HasKey(e => e.RequestStatusReasonId).HasName("pk_RequestStatusReason");

                entity.ToTable("RequestStatusReason");

                entity.Property(e => e.RequestStatusReasonId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RequestStatusReasonID");
                entity.Property(e => e.RequestStatusReason1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RequestStatusReason");
                entity.Property(e => e.StatusReasonDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RequestedItem>(entity =>
            {
                entity.HasKey(e => e.RequestedItemId).HasName("PK_RequestItem");

                entity.ToTable("RequestedItem");

                entity.Property(e => e.RequestedItemId).HasColumnName("RequestedItemID");
                entity.Property(e => e.RequestId).HasColumnName("RequestID");
                entity.Property(e => e.Comments)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.DocumentRequestedId).HasColumnName("DocumentRequestedID");
                entity.Property(e => e.ItemStatusId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("ItemStatusID");

                entity.HasOne(d => d.Request).WithMany(p => p.RequestedItems)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_RequestedItem_RequestID");

                entity.HasOne(d => d.DocumentRequested).WithMany(p => p.RequestedItems)
                    .HasForeignKey(d => d.DocumentRequestedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestedItem_DocumentRequestedID");

                entity.HasOne(d => d.ItemStatus).WithMany(p => p.RequestedItems)
                    .HasForeignKey(d => d.ItemStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestedItem_ItemStatusID");
            });

            modelBuilder.Entity<Requester>(entity =>
            {
                entity.ToTable("Requester");

                entity.Property(e => e.RequesterId).HasColumnName("RequesterID");
                entity.Property(e => e.AddressId).HasColumnName("AddressID");
                entity.Property(e => e.Comments)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.RequesterStatusId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("RequesterStatusID");
                entity.Property(e => e.RequesterTypeId).HasColumnName("RequesterTypeID");

                entity.HasOne(d => d.Address).WithMany(p => p.Requesters)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Requester_AddressID");

                entity.HasOne(d => d.RequesterStatus).WithMany(p => p.Requesters)
                    .HasForeignKey(d => d.RequesterStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Requester_RequesterStatusID");

                entity.HasOne(d => d.RequesterType).WithMany(p => p.Requesters)
                    .HasForeignKey(d => d.RequesterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Requester_RequesterTypeID");

                entity.HasMany(d => d.Contacts).WithMany(p => p.Requesters)
                    .UsingEntity<Dictionary<string, object>>(
                        "RequesterContact",
                        r => r.HasOne<Contact>().WithMany()
                            .HasForeignKey("ContactId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_RequesterContacts_ContactID"),
                        l => l.HasOne<Requester>().WithMany()
                            .HasForeignKey("RequesterId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_RequesterContacts_RequesterID"),
                        j =>
                        {
                            j.HasKey("RequesterId", "ContactId");
                            j.ToTable("RequesterContacts");
                            j.IndexerProperty<int>("RequesterId").HasColumnName("RequesterID");
                            j.IndexerProperty<int>("ContactId").HasColumnName("ContactID");
                        });
            });

            modelBuilder.Entity<RequesterStatus>(entity =>
            {
                entity.HasKey(e => e.RequesterStatusId).HasName("pk_RequesterStatus");

                entity.ToTable("RequesterStatus");

                entity.Property(e => e.RequesterStatusId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("RequesterStatusID");
                entity.Property(e => e.RequesterStatus1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RequesterStatus");
                entity.Property(e => e.RequesterStatusDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RequesterType>(entity =>
            {
                entity.ToTable("RequesterType");

                entity.Property(e => e.RequesterTypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RequesterTypeID");
                entity.Property(e => e.RequesterType1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RequesterType");
                entity.Property(e => e.RequesterTypeDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Restriction>(entity =>
            {
                entity.Property(e => e.RestrictionId).HasColumnName("RestrictionID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RevenueCode>(entity =>
            {
                entity.HasKey(e => e.RevenueCodeID);

                entity.ToTable("RevenueCode");

                entity.Property(e => e.RevenueCodeID)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("RevenueCode");
                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LongDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SecurityQuestion>(entity =>
            {
                entity.ToTable("SecurityQuestion");

                entity.Property(e => e.SecurityQuestionId).HasColumnName("SecurityQuestionID");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sex>(entity =>
            {
                entity.ToTable("Sex");

                entity.Property(e => e.SexId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SexID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SideEffect>(entity =>
            {
                entity.ToTable("SideEffect");

                entity.Property(e => e.SideEffectId).HasColumnName("SideEffectID");

                entity.Property(e => e.SideEffectDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.ToTable("Specialty");

                entity.Property(e => e.SpecialtyId).HasColumnName("SpecialtyID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TempRouteType>(entity =>
            {
                entity.ToTable("TempRouteType");

                entity.Property(e => e.TempRouteTypeId).HasColumnName("TempRouteTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TempRouteTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("Test");

                entity.HasIndex(e => e.TestCategoryId, "FK");

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.Property(e => e.Description)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.TestCategoryId).HasColumnName("TestCategoryID");

                entity.Property(e => e.TestName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.TestCategory)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.TestCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Test__TestCatego__6D2D2E85");
            });

            modelBuilder.Entity<TestCategory>(entity =>
            {
                entity.ToTable("TestCategory");

                entity.Property(e => e.TestCategoryId).HasColumnName("TestCategoryID");

                entity.Property(e => e.CategoryDescription)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.TestCategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TestedBodyPart>(entity =>
            {
                entity.HasKey(e => new { e.TestId, e.PartId })
                    .HasName("PK__TestedBo__9B00C1D35B010DBE");

                entity.HasIndex(e => new { e.TestId, e.PartId }, "PK/FK");

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.Property(e => e.PartId).HasColumnName("PartID");

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.TestedBodyParts)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestedBod__PartI__70FDBF69");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestedBodyParts)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestedBod__TestI__70099B30");
            });

            modelBuilder.Entity<UserFacility>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FacilityId });

                entity.ToTable("UserFacility");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.UserFacilities)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserFacility_FacilityID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFacilities)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserFacility_UserID");
            });

            modelBuilder.Entity<UserProgram>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ProgramId })
                    .HasName("pk_UserPrograms");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.UserPrograms)
                    .HasForeignKey(d => d.ProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserPrograms_ProgramID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPrograms)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserPrograms_UserID");
            });

            modelBuilder.Entity<UserSecurityQuestion>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.SecurityQuestionId })
                    .HasName("pk_UserSecurityQuestion");

                entity.ToTable("UserSecurityQuestion");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.SecurityQuestionId).HasColumnName("SecurityQuestionID");

                entity.Property(e => e.AnswerHash)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.SecurityQuestion)
                    .WithMany(p => p.UserSecurityQuestions)
                    .HasForeignKey(d => d.SecurityQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserSecurityQuestion_QuestionID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSecurityQuestions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserSecurityQuestion_UserID");
            });

            modelBuilder.Entity<UserTable>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_User");

                entity.ToTable("UserTable");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.AspNetUsersId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("AspNetUsersID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('12/31/9999')");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProgramEnrolledIn)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AspNetUsers)
                    .WithMany(p => p.UserTables)
                    .HasForeignKey(d => d.AspNetUsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserTable_AspNetUsersID");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.InverseInstructor)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("fk_InstructorID");
            });

            modelBuilder.Entity<VisitType>(entity =>
            {
                entity.ToTable("VisitType");

                entity.Property(e => e.VisitTypeId).HasColumnName("VisitTypeID");

                entity.Property(e => e.LastModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });


            modelBuilder.HasSequence<int>("MRN_ID");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

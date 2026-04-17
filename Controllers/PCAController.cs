using IS_Proj_HIT.Services;
using IS_Proj_HIT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.Entities.Enum;
using Microsoft.AspNetCore.Authorization;

namespace IS_Proj_HIT.Controllers
{    
    [Authorize]
    [PermissionAuthorize("PCAAdd","PCADelete","PCAEdit","PCAView")]
    public class PCAController : BaseController
    {
        private readonly IWCTCHealthSystemRepository _repository;

        public PCAController(IWCTCHealthSystemRepository repository) => _repository = repository;

        /// <summary>
        ///     Show a detailed view of a single PCA
        /// </summary>
        /// <param name="assessmentId">PCA Id for Db Lookup</param>
        [Authorize]
        [PermissionAuthorize("PCAView")]
        public IActionResult ViewAssessment(int assessmentId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var assessment = _repository.PcaRecords
                .Include(pca => pca.Encounter)
                .Include(pca => pca.Pcacomments)
                .ThenInclude(com => com.PcacommentType)
                .Include(pca => pca.CareSystemAssessments)
                .ThenInclude(ca => ca.CareSystemParameter)
                .ThenInclude(cp => cp.CareSystemType)
                .Include(pca => pca.PcapainAssessments)
                .ThenInclude(pa => pa.PainParameter)
                .Include(pca => pca.PcapainAssessments)
                .ThenInclude(pa => pa.PainRating)
                .Include(pca => pca.PainScaleType)
                .Include(pca => pca.TempRouteType)
                .Include(pca => pca.Bmimethod)
                .Include(pca => pca.BloodPressureRouteType)
                .Include(pca => pca.PulseRouteType)
                .Include(pca => pca.O2deliveryType)
                .FirstOrDefault(pca => pca.Pcaid == assessmentId);
            if (assessment is null)
                return RedirectToAction("CheckedIn", "Encounter",
                    new { filter = "CheckedIn" });

            ViewBag.Patient = _repository.Patients.Include(p => p.PatientAlerts)
                .FirstOrDefault(p => p.Mrn == assessment.Encounter.Mrn);
            ViewBag.Encounter = _repository.Encounters
                    .FirstOrDefault(e => e.Mrn == assessment.Encounter.Mrn);

            return View(assessment);
        }

        /// <summary>
        /// Display the form to enter a PCA
        /// </summary>
        /// <param name="encounterId">Id of unique encounter</param>
        /// <param name="patientMrn">Unique Identifier of patient</param>
        [Authorize]
        [PermissionAuthorize("PCAAdd")]
        public IActionResult CreateAssessment(long encounterId, string mrn)
        {
            var encounter = _repository.Encounters.FirstOrDefault(e => e.EncounterId == encounterId);
            var patient = _repository.Patients.Include(p => p.PatientAlerts).FirstOrDefault(p => p.Mrn == mrn);

            if (encounter is null || patient is null)
                return RedirectToAction("ViewEncounter", "Encounter",
                    new { encounterId });

            ViewBag.Encounter = encounter;
            ViewBag.Patient = patient;

            var newFormPca = PrepareAssessmentFormPageModel();

            return View(newFormPca);
        }

        /// <summary>
        /// Post form of Create PCA. Check for errors. Trigger SaveAssessment if none.
        /// </summary>
        /// <param name="formPca">view model for PCA</param>
        /// 
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("PCAAdd")]
        public IActionResult CreateAssessment(AssessmentFormPageModel formPca)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Encounter = _repository.Encounters
                    .FirstOrDefault(e => e.EncounterId == formPca.PcaRecord.EncounterId);
                ViewBag.Patient = _repository.Patients
                    .Include(p => p.PatientAlerts)
                    .FirstOrDefault(p => p.Mrn == formPca.PatientMrn);

                formPca = PrepareAssessmentFormPageModel(formPca);
                return View(formPca);
            }

            return SaveAssessment(formPca);
        }

        /// <summary>
        /// Display the form to update an existing PCA
        /// </summary>
        /// <param name = "assessmentId" > PCA Id for Db Lookup</param>
        ///  <param name = "patientMrn" > Unique Identifier of patient</param>
        ///   <param name = "encounterId" > Unique Identifier of patient</param>
        [Authorize]
        [PermissionAuthorize("PCAEdit")]
        public IActionResult UpdateAssessment(int assessmentId, string patientMRN, long encounterId)
        {
                // Fetch User Session permissions using the helper method
            var permissions = PermissionAuthorizeAttribute.GetPermissionsForUser(HttpContext);
                // Assign permissions to ViewData and then pass to the cshtml
            ViewData["UserPermissions"] = permissions;

            var assessment = _repository.PcaRecords
                .Include(pca => pca.Encounter)
                .Include(pca => pca.CareSystemAssessments)
                .ThenInclude(ca => ca.CareSystemParameter)
                .ThenInclude(cp => cp.CareSystemType)
                .Include(pca => pca.PcapainAssessments)
                .ThenInclude(pa => pa.PainParameter)
                .Include(pca => pca.PcapainAssessments)
                .ThenInclude(pa => pa.PainRating)
                .Include(pca => pca.PainScaleType)
                .Include(pca => pca.TempRouteType)
                .Include(pca => pca.Bmimethod)
                .Include(pca => pca.BloodPressureRouteType)
                .Include(pca => pca.PulseRouteType)
                .Include(pca => pca.O2deliveryType)
                .FirstOrDefault(pca => pca.Pcaid == assessmentId);

            var vitalID = _repository.PcaComments.Where(pca => pca.Pcaid == assessmentId && pca.PcacommentTypeId == 11).Select(c => c.Pcacomment1).FirstOrDefault();

            string vitalsNotes = Convert.ToString(vitalID);

            var painID = _repository.PcaComments.Where(pa => pa.Pcaid == assessmentId && pa.PcacommentTypeId == 12).Select(c => c.Pcacomment1).FirstOrDefault();

            string painsNotes = Convert.ToString(painID);


            var patient = _repository.Patients.Include(p => p.PatientAlerts).FirstOrDefault(p => p.Mrn == patientMRN);
            var encounter = _repository.Encounters.FirstOrDefault(e => e.EncounterId == encounterId);
            var patientAlerts = _repository.PatientAlerts.Where(b => b.Mrn == patientMRN).Count();

            if (encounter is null || patient is null)
                return RedirectToAction("ViewEncounter", "Encounter",
                    new { encounterId = encounterId, isPatientEncounter = false });


            AddTooltips();
            AddUnits();
            AddRoutes();
            var painScales = _repository.PainScaleTypes
                .Include(ps => ps.PainParameters)
                .ThenInclude(pp => pp.PainRatings)
                .ToList();
            var painRatings = new Dictionary<int, int?>();


            // Create a pain assessment record for each pain scale/parameter/rating
            foreach (var ps in painScales)
            {
                foreach (var pp in ps.PainParameters)
                {
                    foreach (var pr in pp.PainRatings)
                    {
                        // If a PainAssessment exists for this parameter/rating, mark the value not null.
                        var existingPainAssessment = assessment.PcapainAssessments.FirstOrDefault(pa => pa.PainParameterId == pr.PainParameterId &&
                          pa.PainRatingId == pr.PainRatingId);
                        if (existingPainAssessment != null)
                        {
                            painRatings.Add(pr.PainRatingId, 1);
                        }
                        else
                        {
                            painRatings.Add(pr.PainRatingId, null);
                        }
                    }
                }
            }


            var secondarySystems = _repository.CareSystemAssessmentTypes
                .Include(cs => cs.CareSystemParameters)
                .ToList();


            var sysAssessments = new Dictionary<int, CareSystemAssessment>();

            var csaID = assessment.CareSystemAssessments.FirstOrDefault();


            foreach (var ss in secondarySystems)
            {
                foreach (var csp in ss.CareSystemParameters)
                {

                    var existingCareAssessments = assessment.CareSystemAssessments.FirstOrDefault(ca => ca.CareSystemParameterId == csp.CareSystemParameterId &&
                      csp.CareSystemTypeId == ss.CareSystemTypeId);

                    if (existingCareAssessments != null)
                    {
                        sysAssessments.Add(csp.CareSystemParameterId, existingCareAssessments);
                    }
                    else
                    {
                        sysAssessments.Add(csp.CareSystemParameterId, new CareSystemAssessment
                        {
                            CareSystemParameterId = csp.CareSystemParameterId,
                            IsWithinNormalLimits = null
                        });
                    }

                }
            }


            ViewBag.Encounter = encounter;
            ViewBag.Patient = patient;
            ViewBag.PcaRecord = assessment;
            var updatePCA = new AssessmentFormPageModel
            {
                PcaRecord = assessment,
                PainScales = painScales,
                PainRatings = painRatings,
                SecondarySystemTypes = secondarySystems,
                Assessments = sysAssessments,
                VitalNote = vitalsNotes,
                PainNote = painsNotes
            };

            return View(updatePCA);
        }

        /// <summary>
        /// Post form for updating existing PCA. Check for errors. Trigger SaveAssessment if none.
        /// </summary>
        /// <param name="formPca">view model for PCA</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [PermissionAuthorize("PCAEdit")]
        public IActionResult UpdateAssessment(AssessmentFormPageModel formPca)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Encounter = _repository.Encounters.FirstOrDefault(e => e.EncounterId == formPca.PcaRecord.EncounterId);
                ViewBag.Patient = _repository.Patients.FirstOrDefault(p => p.Mrn == formPca.PatientMrn);
                ViewBag.PatientAlertsCount = _repository.PatientAlerts.Count(b => b.Mrn == formPca.PatientMrn);
                return View(formPca);
            }

            return SaveAssessment(formPca);

        }

        /// <summary>
        /// Save the information added or updated in the PCA form. 
        /// Convert necessary units. Update date modified as needed.
        /// </summary>
        /// <param name="formPca">view model for PCA</param>
        /// <returns></returns>
        private IActionResult SaveAssessment(AssessmentFormPageModel formPca)
        {
            var pca = formPca.PcaRecord;


            //Convert temp to F, if entered in other unit
            if (pca.Temperature != null)
            {
                Enum.TryParse<TempUnit>(formPca.TempUnit, out var tempUnit);
                pca.Temperature = ConversionService.ConvertTemp(tempUnit, TempUnit.Fahrenheit, pca.Temperature);
            }

            // This is new PCA 
            if (pca.Pcaid is 0)
            {
                using (var tran = new TransactionScope())
                {
                    pca.DateVitalsAdded = DateTime.Now;
                    pca.LastModified = DateTime.Now;
                    _repository.AddPcaRecord(pca);

                    if (!string.IsNullOrWhiteSpace(formPca.VitalNote))
                    {
                        var vNote = _repository.PcaCommentTypes
                            .FirstOrDefault(t => t.PcacommentTypeName == "Vitals Notes");

                        _repository.AddPcaComment(new Pcacomment
                        {
                            Pcaid = pca.Pcaid,
                            PcacommentTypeId = vNote?.PcacommentTypeId ?? (int)PcaCommentTypes.Vital,
                            Pcacomment1 = formPca.VitalNote,
                            DateCommentAdded = DateTime.Now,
                            LastModified = DateTime.Now
                        });
                    }

                    if (!string.IsNullOrWhiteSpace(formPca.PainNote))
                    {
                        var vNote = _repository.PcaCommentTypes
                            .FirstOrDefault(t => t.PcacommentTypeName == "Pain Assessment Notes");

                        _repository.AddPcaComment(new Pcacomment
                        {
                            Pcaid = pca.Pcaid,
                            PcacommentTypeId = vNote?.PcacommentTypeId ?? (int)PcaCommentTypes.Pain,
                            Pcacomment1 = formPca.PainNote,
                            DateCommentAdded = DateTime.Now,
                            LastModified = DateTime.Now
                        });
                    }

                    var painScale = _repository.PainScaleTypes
                        .Include(ps => ps.PainParameters)
                        .ThenInclude(pp => pp.PainRatings)
                        .FirstOrDefault(ps => ps.PainScaleTypeId == pca.PainScaleTypeId);
                    if (painScale != null)
                    {
                        foreach (var (paramId, ratingId) in formPca.PainRatings)
                        {
                            if (painScale.PainParameters.Any(pp => pp.PainParameterId == paramId) && ratingId.HasValue)
                            {
                                _repository.AddPainAssessment(new PcapainAssessment
                                {
                                    Pcaid = pca.Pcaid,
                                    PainParameterId = paramId,
                                    PainRatingId = (int)ratingId,
                                    LastModified = DateTime.Now
                                });
                            }
                        }
                    }
                    foreach (var (systemParamId, assessment) in formPca.Assessments.Where(a =>
                        a.Value.IsWithinNormalLimits != null))
                    {
                        assessment.Pcaid = pca.Pcaid;
                        assessment.CareSystemParameterId = (short)systemParamId;
                        assessment.LastModified = DateTime.Now;
                        _repository.AddSystemAssessment(assessment);
                    }

                    tran.Complete();
                }
            } //UPDATE/////////////////////////////////
            else
            {
                using (var tran = new TransactionScope())
                {

                    var existingRecord = _repository.PcaRecords
                        .Include(pca => pca.PcapainAssessments)
                        .Include(pca => pca.Pcacomments)
                        .Include(pca => pca.CareSystemAssessments)
                        .FirstOrDefault(pcr => pcr.Pcaid == pca.Pcaid);

                    existingRecord.Temperature = pca.Temperature;
                    existingRecord.HeadCircumference = pca.HeadCircumference;
                    existingRecord.HeadCircumferenceUnits = pca.HeadCircumferenceUnits;
                    existingRecord.Height = pca.Height;
                    existingRecord.HeightUnits = pca.HeightUnits;
                    existingRecord.LastModified = DateTime.Now;
                    existingRecord.O2deliveryTypeId = pca.O2deliveryTypeId;
                    existingRecord.OxygenFlow = pca.OxygenFlow;
                    existingRecord.PainLevelGoal = pca.PainLevelGoal;
                    existingRecord.PercentOxygenDelivered = pca.PercentOxygenDelivered;
                    existingRecord.Pulse = pca.Pulse;
                    existingRecord.PulseOximetry = pca.PulseOximetry;
                    existingRecord.PulseRouteTypeId = pca.PulseRouteTypeId;
                    existingRecord.Respiration = pca.Respiration;
                    existingRecord.SystolicBloodPressure = pca.SystolicBloodPressure;
                    existingRecord.DiastolicBloodPressure = pca.DiastolicBloodPressure;
                    existingRecord.TempRouteTypeId = pca.TempRouteTypeId;
                    existingRecord.Weight = pca.Weight;
                    existingRecord.WeightUnits = pca.WeightUnits;




                    var vitalComment = existingRecord.Pcacomments.FirstOrDefault(c => c.PcacommentTypeId == (int)PcaCommentTypes.Vital);
                    if (vitalComment == null)
                    {
                        if (!string.IsNullOrWhiteSpace(formPca.VitalNote))
                        {
                            var vNote = _repository.PcaCommentTypes
                                .FirstOrDefault(t => t.PcacommentTypeName == "Vitals Notes");

                            _repository.AddPcaComment(new Pcacomment
                            {
                                Pcaid = pca.Pcaid,
                                PcacommentTypeId = vNote?.PcacommentTypeId ?? (int)PcaCommentTypes.Vital,
                                Pcacomment1 = formPca.VitalNote,
                                DateCommentAdded = DateTime.Now,
                                LastModified = DateTime.Now
                            });
                        }
                    }
                    else if (vitalComment != null)
                    {
                        vitalComment.Pcacomment1 = formPca.VitalNote;
                        vitalComment.LastModified = DateTime.Now;
                    }


                    var painComment = existingRecord.Pcacomments.FirstOrDefault(c => c.PcacommentTypeId == (int)PcaCommentTypes.Pain);
                    if (painComment == null)
                    {
                        if (!string.IsNullOrWhiteSpace(formPca.PainNote))
                        {
                            var vNote = _repository.PcaCommentTypes
                                .FirstOrDefault(t => t.PcacommentTypeName == "Pain Assessment Notes");

                            _repository.AddPcaComment(new Pcacomment
                            {
                                Pcaid = pca.Pcaid,
                                PcacommentTypeId = vNote?.PcacommentTypeId ?? (int)PcaCommentTypes.Pain,
                                Pcacomment1 = formPca.PainNote,
                                DateCommentAdded = DateTime.Now,
                                LastModified = DateTime.Now
                            });
                        }
                    }
                    else if (painComment != null)
                    {
                        painComment.Pcacomment1 = formPca.PainNote;
                        painComment.LastModified = DateTime.Now;
                    }

                    var painScale = _repository.PainScaleTypes
                        .Include(ps => ps.PainParameters)
                        .ThenInclude(pp => pp.PainRatings)
                        .FirstOrDefault(ps => ps.PainScaleTypeId == pca.PainScaleTypeId);

                    var painAssesmentWasNull = existingRecord.PainScaleTypeId == null;

                    if (painScale != null)
                    {
                        if (painAssesmentWasNull)
                        {
                            foreach (var (paramId, ratingId) in formPca.PainRatings)
                            {
                                if (painScale.PainParameters.Any(pp => pp.PainParameterId == paramId) && ratingId.HasValue)
                                {
                                    existingRecord.PainScaleTypeId = pca.PainScaleTypeId;

                                    _repository.AddPainAssessment(new PcapainAssessment
                                    {
                                        Pcaid = pca.Pcaid,
                                        PainParameterId = paramId,
                                        PainRatingId = (int)ratingId,
                                        LastModified = DateTime.Now
                                    });

                                }
                            }
                        }
                        else
                        {
                            var painAssesments = _repository.PainAssessments.Where(p => p.Pcaid == pca.Pcaid).ToList();
                            var firstcheck = (pca.PainScaleTypeId == (int?)PcaPainScaleType.WongBaker || pca.PainScaleTypeId == (int?)PcaPainScaleType.Numerical) && painAssesments.Count() > 1;
                            var secondcheck = (pca.PainScaleTypeId == (int?)PcaPainScaleType.CRIES || pca.PainScaleTypeId == (int?)PcaPainScaleType.Nonverbal) && painAssesments.Count() == 1;
                            if (firstcheck || secondcheck)
                            {
                                foreach (var painAssesment in painAssesments)
                                {
                                    _repository.DeletePainAssessment(painAssesment);
                                }

                                foreach (var (paramId, ratingId) in formPca.PainRatings)
                                {
                                    if (painScale.PainParameters.Any(pp => pp.PainParameterId == paramId) && ratingId.HasValue)
                                    {
                                        existingRecord.PainScaleTypeId = pca.PainScaleTypeId;

                                        _repository.AddPainAssessment(new PcapainAssessment
                                        {
                                            Pcaid = pca.Pcaid,
                                            PainParameterId = paramId,
                                            PainRatingId = (int)ratingId,
                                            LastModified = DateTime.Now
                                        });

                                    }
                                }

                            }
                            else
                            {
                                foreach (var (paramId, ratingId) in formPca.PainRatings)
                                {
                                    if (painScale.PainParameters.Any(pp => pp.PainParameterId == paramId) && ratingId.HasValue)
                                    {
                                        existingRecord.PainScaleTypeId = pca.PainScaleTypeId;
                                        var painAssesment = _repository.PainAssessments.Where(p => p.Pcaid == pca.Pcaid).FirstOrDefault();
                                        painAssesment.PainParameterId = paramId;
                                        painAssesment.PainRatingId = (int)ratingId;
                                        painAssesment.LastModified = DateTime.Now;

                                        _repository.EditPainAssessment(painAssesment);
                                    }
                                }
                            }

                        }

                    }


                    //loop through caresystemassessments
                    foreach (var formCsa in formPca.Assessments)
                    {
                        var existCsa = existingRecord.CareSystemAssessments
                            .FirstOrDefault(a => a.CareSystemParameterId == formCsa.Key);

                        if (existCsa is null) //the assessment to update does not currently exist
                        {
                            if (formCsa.Value.IsWithinNormalLimits != null)
                            {
                                formCsa.Value.Pcaid = pca.Pcaid;
                                formCsa.Value.CareSystemParameterId = (short)formCsa.Key;
                                formCsa.Value.LastModified = DateTime.Now;

                                existingRecord.CareSystemAssessments.Add(formCsa.Value);
                            }
                        }
                        else //the assessment DOES exist and needs to be updated properly
                        {
                            existCsa.Comment = formCsa.Value.Comment;
                            existCsa.IsWithinNormalLimits = formCsa.Value.IsWithinNormalLimits;
                        }
                    }

                    _repository.EditPcaRecord(existingRecord);

                    tran.Complete();
                }
            }

            // Return to assessment view
            return RedirectToAction("ViewAssessment",
                new { assessmentId = formPca.PcaRecord.Pcaid });
        }


        /// <summary>
        /// Preprocessing to the PCA view model to load tooltips, units, and routes dropdowns.
        /// Also load pain scale tab and secondary systems/systems review tab.
        /// </summary>
        /// <param name="formPca"></param>
        /// <returns></returns>
        private AssessmentFormPageModel PrepareAssessmentFormPageModel(AssessmentFormPageModel formPca = null)
        {
            if (formPca is null)
                formPca = new AssessmentFormPageModel();

            AddTooltips();
            AddUnits();
            AddRoutes();

            formPca.PainScales = _repository.PainScaleTypes
                .Include(ps => ps.PainParameters)
                .ThenInclude(pp => pp.PainRatings)
                .ToList();
            formPca.PainScales.ForEach(ps =>
                ps.PainParameters.ToList().ForEach(pp =>
                    pp.PainRatings.ToList().ForEach(pr =>
                    {
                        if (!formPca.PainRatings.ContainsKey(pr.PainRatingId))
                            formPca.PainRatings.Add(pr.PainRatingId, null);
                    })));
            // Checks for less than 100 to avoid junk/testing types
            formPca.SecondarySystemTypes = _repository.CareSystemAssessmentTypes
                .Where(cs => cs.CareSystemTypeId < 100)
                .Include(cs => cs.CareSystemParameters)
                .ToList();
            formPca.SecondarySystemTypes.ForEach(s =>
                s.CareSystemParameters.ToList().ForEach(sp =>
                {
                    if (!formPca.Assessments.ContainsKey(sp.CareSystemParameterId))
                        formPca.Assessments.Add(sp.CareSystemParameterId,
                            new CareSystemAssessment
                            {
                                CareSystemParameterId = sp.CareSystemParameterId,
                                IsWithinNormalLimits = null
                            });
                }));

            return formPca;
        }


        /// <summary>
        /// Populate the informational messages 
        /// </summary>
        private void AddTooltips()
        {
            ViewBag.TempWnl = "WNL: 97°F or higher AND  no higher than 101°F";
            ViewBag.BpWnl =
                "WNL: <br />Less than 120 / 80;<br />" +
                "ELEVATED: <br />120-129 / less than 80 <br />" +
                "STAGE 1 HIGH BP: <br />130-139 / 80-89 <br />" +
                "STAGE 1 HIGH BP: <br />140 and above<br /> / 90 and above <br />" +
                "HYPERTENSION CRISIS: <br />Higher than 180<br /> / higher than 120";
            ViewBag.PulseWnl = "WNL: 60 BPM or higher AND  no higher than 100 BPM";
            ViewBag.RespWnl = "WNL: 15/min or higher AND  no higher than 30/min";
            ViewBag.PulseOxWnl = "WNL: 90% or higher";
            ViewBag.WeightWnl =
                "Document whether the patient has experienced any unusual/unexplained weight loss or weight gain";
            ViewBag.FacesExplaination =
                "•	Explain to the person that each face represents a person who has no pain (hurt), or some, or a lot of pain. <br />" +
                "•	Face 0 doesn’t hurt at all. <br />" +
                "•	Face 2 hurts just a little bit. <br />" +
                "•	Face 4 hurts a little bit more. <br />" +
                "•	Face 6 hurts even more. <br />" +
                "•	Face 8 hurts a whole lot. <br />" +
                "•	Face 10 hurts as much as you can imagine although you don’t have to be crying to have this worst pain. <br />" +
                "•	Ask the person to choose the face that best depicts the pain they are experiencing.";
        }


        /// <summary>
        /// Populate the units dropdowns
        /// </summary>
        private void AddUnits()
        {
            ViewBag.WeightUnits = new List<SelectListItem>
            {
                new SelectListItem("Kilograms", WeightUnit.Kilograms.ToString(), true),
                new SelectListItem("Grams", WeightUnit.Grams.ToString()),
                new SelectListItem("Pounds", WeightUnit.Pounds.ToString())
            };
            ViewBag.LengthUnits = new List<SelectListItem>
            {
                new SelectListItem("Inches", LengthUnit.Inches.ToString(), true),
                new SelectListItem("Feet", LengthUnit.Feet.ToString()),
                new SelectListItem("Centimeters", LengthUnit.Centimeters.ToString()),
                new SelectListItem("Meters", LengthUnit.Meters.ToString())
            };
            ViewBag.TempUnits = new List<SelectListItem>
            {
                new SelectListItem("Fahrenheit", TempUnit.Fahrenheit.ToString()),
                new SelectListItem("Celsius", TempUnit.Celsius.ToString())
            };
        }


        /// <summary>
        /// Populate the routes dropdowns
        /// </summary>
        //i == number is how the defaults for this are set
        private void AddRoutes()
        {
            ViewBag.TempRoutes = new List<SelectListItem>(
                _repository.TempRouteTypes.ToList()
                    .OrderBy(r => r.TempRouteTypeName)
                    .Select((r, i) =>
                        new SelectListItem(
                            r.TempRouteTypeName,
                            r.TempRouteTypeId.ToString(),                       
                            i == 2)));

            ViewBag.O2DeliveryRoutes = new List<SelectListItem>(
                _repository.O2DeliveryTypes.ToList()
                .OrderBy(r => r.O2deliveryTypeName)
                .Select((r, i) => new SelectListItem(
                    r.O2deliveryTypeName,
                    r.O2deliveryTypeId.ToString(),
                    i == 5)));

            ViewBag.BloodPressureRoutes = new List<SelectListItem>(
                _repository.BloodPressureRouteTypes.ToList()
                    .OrderBy(r => r.Name)
                    .Select((r, i) =>
                        new SelectListItem(
                            r.Name,
                            r.BloodPressureRouteTypeId.ToString(), 
                            i == 0)));
            ViewBag.PulseRoute = new List<SelectListItem>(
                _repository.PulseRouteTypes.ToList()
                .OrderBy(r => r.PulseRouteTypeName)
                    .Select((r, i) =>
                        new SelectListItem(
                            r.PulseRouteTypeName,
                            r.PulseRouteTypeId.ToString(), 
                            i == 0)));
            ViewBag.BodyMassIndexRoutes = new List<SelectListItem>(
                _repository.BmiMethods.ToList()
                    .OrderBy(r => r.Name)
                    .Select((r, i) =>
                        new SelectListItem(
                            r.Name,
                            r.BmimethodId.ToString(), i==1)));
        }

        /// <summary>
        ///     Delete Patient Assessment
        /// </summary>
        /// <param name="assessmentId"></param>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [Authorize]
        [PermissionAuthorize("PCADelete")]
        public IActionResult DeleteAssessment(int assessmentId, long encounterId)
        {
            bool exists = _repository.PcaRecords.Any(p => p.Pcaid == assessmentId);
            if (exists)
            {
                using var tran = new TransactionScope();
                
                var assessment = _repository.PcaRecords
                .Include(pca => pca.Encounter)
                .Include(pca => pca.Pcacomments)
                .ThenInclude(com => com.PcacommentType)
                .Include(pca => pca.CareSystemAssessments)
                .ThenInclude(ca => ca.CareSystemParameter)
                .ThenInclude(cp => cp.CareSystemType)
                .Include(pca => pca.PcapainAssessments)
                .ThenInclude(pa => pa.PainParameter)
                .Include(pca => pca.PcapainAssessments)
                .ThenInclude(pa => pa.PainRating)
                .Include(pca => pca.PainScaleType)
                .Include(pca => pca.TempRouteType)
                .Include(pca => pca.Bmimethod)
                .Include(pca => pca.BloodPressureRouteType)
                .Include(pca => pca.PulseRouteType)
                .Include(pca => pca.O2deliveryType)
                .FirstOrDefault(pca => pca.Pcaid == assessmentId);

                try
                {
                    _repository.DeletePcaRecord(assessment);
                    // Complete the transaction
                    tran.Complete();
                    TempData["Success"] = "The selected record was deleted.";
                }
                catch(Exception ex)
                {
                    // Roll back transaction in case of errors
                    tran.Dispose();
                    TempData["Error"] = $"An exception occurred:  {ex.Message}";
                }
                
            }
            else
            {
                TempData["Error"] = "The record does not exist";
            }

            return RedirectToRoute(new
            {
                controller = "Encounter",
                action = "PcaIndex",
                encounterId = encounterId
            });

        }
    }

}
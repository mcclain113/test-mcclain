using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Enum;

namespace IS_Proj_HIT.ViewModels
{
    public class OrdersViewModel
    {
        public IEnumerable<OrderInfo> OrderInfos { get; set; }
        public IEnumerable<VisitType> VisitType { get; set; }
        public IEnumerable<Physician> Physicians {get; set;}
        public IEnumerable<OrderType> OrderTypes {get; set;}
        
        // Search field added
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public OrderInfo OrderInfo {get; set;}
        public OrderType OrderType {get; set;}
        public Encounter Encounter {get; set;}
        public Patient Patient { get; set; }

        public bool? IsFastingVm {get; set;}
        public string CompletionNotes {get; set;}

        public Physician AdmittingPhysician {get; set;}
        public Physician AttendingPhysician {get; set;}
        public Physician EmergencyPhysician {get; set;}
        public Physician PrimaryCarePhysician {get; set;}
        public Physician ReferringPhysician {get; set;}

        public OrdersViewModel(){}

        public OrdersViewModel(OrderInfo orderInfo)
        {
            this.OrderInfo = orderInfo;
        }
        public OrdersViewModel(Encounter encounter)
        {
            this.Encounter = encounter;

            if (encounter.EncounterPhysicians != null)
            {
                foreach (EncounterPhysician ep in Encounter.EncounterPhysicians)
                {
                    if (ep.PhysicianRoleId == (int)PhysicianRoleType.AttendingPhysician)
                    {
                        AttendingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.EmergencyPhysician)
                    {
                        EmergencyPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.AdmittingPhysician)
                    {
                        AdmittingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.ReferringPhysician)
                    {
                        ReferringPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.PrimaryCare)
                    {
                        PrimaryCarePhysician = ep.Physician;
                    }
                }
            }
        }

        public OrdersViewModel(Encounter encounter, Patient patient)
        {
            this.Encounter = encounter;
            this.Patient = patient;

            if (encounter.EncounterPhysicians != null)
            {
                foreach (EncounterPhysician ep in Encounter.EncounterPhysicians)
                {
                    if (ep.PhysicianRoleId == (int)PhysicianRoleType.AttendingPhysician)
                    {
                        AttendingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.EmergencyPhysician)
                    {
                        EmergencyPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.AdmittingPhysician)
                    {
                        AdmittingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.ReferringPhysician)
                    {
                        ReferringPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.PrimaryCare)
                    {
                        PrimaryCarePhysician = ep.Physician;
                    }
                }
            }
        }

        public OrdersViewModel(Encounter encounter, Patient patient, OrderInfo orderInfo)
        {
            this.Encounter = encounter;
            this.Patient = patient;
            this.OrderInfo = orderInfo;

            if (encounter.EncounterPhysicians != null)
            {
                foreach (EncounterPhysician ep in Encounter.EncounterPhysicians)
                {
                    if (ep.PhysicianRoleId == (int)PhysicianRoleType.AttendingPhysician)
                    {
                        AttendingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.EmergencyPhysician)
                    {
                        EmergencyPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.AdmittingPhysician)
                    {
                        AdmittingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.ReferringPhysician)
                    {
                        ReferringPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.PrimaryCare)
                    {
                        PrimaryCarePhysician = ep.Physician;
                    }
                }
            }
        }
    }
}
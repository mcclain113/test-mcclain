using IS_Proj_HIT.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IS_Proj_HIT.ViewModels.BirthRegistry
{
    public class LaborAndDeliveryViewModel
    {
        public int? BirthId { get; set; }    
        public int? LaborAndDeliveryId { get; set; }
        public int? NewbornId { get; set; }

        public int? FetalPresentationAtBirthId { get; set; }
        public int? FinalRouteAndMethodId { get; set; }
        public bool? TrialOfLaborBeforeCesarean { get; set; }

        [MaxLength(200)]
        public string Comments { get; set; }

        public List<SelectListItem> CharacteristicsOfLabor { get; set; } = new();
        public List<SelectListItem> FetalPresentationAtBirths { get; set; } = new();
        public List<SelectListItem> FinalRouteAndMethodOfDeliveries { get; set; } = new();
        public List<SelectListItem> MaternalMorbidities { get; set; } = new();
        public List<SelectListItem> OnsetOfLabors { get; set; } = new();

        public List<int> SelectedCharacteristicIds { get; set; } = new();
        public List<int> SelectedMaternalMorbidityIds { get; set; } = new();
        public List<int> SelectedOnsetOfLaborIds { get; set; } = new();

        public bool NoCharacteristics { get; set; }
        public bool NoMaternalMorbidities { get; set; }

        public bool NoneOfTheAboveCharacteristics => !SelectedCharacteristicIds.Any();
        public bool NoneOfTheAboveMorbidity => !SelectedMaternalMorbidityIds.Any();

        public LaborAndDeliveryViewModel() { }

        public LaborAndDeliveryViewModel(LaborAndDelivery laborAndDelivery) : this()
        {
            if (laborAndDelivery == null) return;

            LaborAndDeliveryId = laborAndDelivery.LaborAndDeliveryId;
            NewbornId = laborAndDelivery.NewbornId;
            Comments = laborAndDelivery.Comments;

            FetalPresentationAtBirthId = laborAndDelivery.FetalPresentationAtBirthId.HasValue
                ? (int?)laborAndDelivery.FetalPresentationAtBirthId.Value
                : null;

            FinalRouteAndMethodId = laborAndDelivery.FinalRouteAndMethodId.HasValue
                ? (int?)laborAndDelivery.FinalRouteAndMethodId.Value
                : null;

            TrialOfLaborBeforeCesarean = laborAndDelivery.TrialOfLaborBeforeCesarean;

            if (laborAndDelivery.Characteristics != null)
            {
                SelectedCharacteristicIds = laborAndDelivery.Characteristics
                    .Select(c => (int)c.CharacteristicId).ToList();
            }
            if (laborAndDelivery.MaternalMorbidities != null)
            {
                SelectedMaternalMorbidityIds = laborAndDelivery.MaternalMorbidities
                    .Select(m => (int)m.MaternalMorbidityId).ToList();
            }
            if (laborAndDelivery.OnsetOfLabors != null)
            {
                SelectedOnsetOfLaborIds = laborAndDelivery.OnsetOfLabors
                    .Select(o => (int)o.OnsetOfLaborId).ToList();
            }

            NoCharacteristics = !SelectedCharacteristicIds.Any();
            NoMaternalMorbidities = !SelectedMaternalMorbidityIds.Any();
        }

        public LaborAndDelivery ToEntity(int newbornId, LaborAndDelivery existingEntity = null)
        {
            var entity = existingEntity ?? new LaborAndDelivery
            {
                NewbornId = newbornId
            };

            entity.NewbornId = newbornId;

            entity.FetalPresentationAtBirthId = FetalPresentationAtBirthId.HasValue
                ? (byte?)System.Convert.ToByte(FetalPresentationAtBirthId.Value)
                : null;

            entity.FinalRouteAndMethodId = FinalRouteAndMethodId.HasValue
                ? (byte?)System.Convert.ToByte(FinalRouteAndMethodId.Value)
                : null;

            entity.TrialOfLaborBeforeCesarean = TrialOfLaborBeforeCesarean;

            if (!string.IsNullOrWhiteSpace(Comments))
                entity.Comments = Comments.Length > 200 ? Comments.Substring(0, 200) : Comments;
            else
                entity.Comments = null;

            return entity;
        }
    }
}

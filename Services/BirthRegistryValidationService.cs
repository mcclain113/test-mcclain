using System.Linq;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.Services
{
        public class BirthRegistryValidationService : IBirthRegistryValidationService
    {
        public bool IsRegistryComplete(Birth birth)
        {
            if (birth == null) return false;
            if (string.IsNullOrEmpty(birth.MotherMrn)) return false;
            if (string.IsNullOrEmpty(birth.CertifierSignature)) return false;
            if (string.IsNullOrEmpty(birth.RegistrarSignature)) return false;
            if (birth.Newborns == null || !birth.Newborns.Any()) return false;
            if (birth.Prenatals == null || !birth.Prenatals.Any()) return false;

            foreach (var newborn in birth.Newborns)
            {
                if (newborn.LaborAndDeliveries == null || !newborn.LaborAndDeliveries.Any()) 
                    return false;
            }

            return true;
        }
    }
}
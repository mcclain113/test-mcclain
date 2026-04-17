using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace IS_Proj_HIT.Entities.Helpers
{
    public static class UtilityHelper
    {
        // This class is meant to contain helper methods which can be applied to any controller
        // method for any object.


        // This method is used to check all of the fields/properties of an entity for values.
        //  For example:  Entity A has a foreign key to Entity B, but Entity B is nullable.  When
        //  Entity A is created this method can be called to check if the user populated any of the
        //  fields for Entity B.  If none are populated, it returns 'false'.  If any ARE populated, it 
        //  returns 'true'.
        //      Many entities have their own connections to other entities through virtual properties and
        //      collections.  These are tested for and skipped.
        //      Some entities have properties which have default values.  These are tested for and skipped, but
        //      must be identified in the calling method.
        //          For an example of a calling method:
        //              see PhysicianStructuredDataController, CreatePhysician() HttpPost method
        public static bool IsAnyPropertyNotNullOrNonZeroExcludingDefaults(object obj, ILogger logger, params (string PropertyName, object DefaultValue)[] defaultProperties)
        {
            var defaultPropertiesDict = defaultProperties.ToDictionary(p => p.PropertyName, p => p.DefaultValue);

            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj);

                // Skip virtual properties and collections except string
                if (property.GetMethod.IsVirtual && property.PropertyType != typeof(string))
                {
                    logger.LogInformation($"Skipping virtual property: {property.Name}");
                    continue;
                }

                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                {
                    logger.LogInformation($"Skipping collection property: {property.Name}");
                    continue;
                }
                
                // Check for default value
                if (defaultPropertiesDict.TryGetValue(property.Name, out var defaultValue))
                {
                    if (value != null && !object.Equals(Convert.ChangeType(value, defaultValue.GetType()), defaultValue))
                    {
                        logger.LogInformation($"Property {property.Name} has a non-default value: {value}");
                        return true;
                    }
                }
                else
                {
                    // Handle nullable properties, integers that are zero, and special cases
                    if (value != null && 
                        (!(value is string str && string.IsNullOrEmpty(str))) && 
                        (!(value is int intValue && intValue == 0)) &&
                        (!(value is short shortValue && shortValue == 0)) &&
                        (!(value is DateTime dateTimeValue && dateTimeValue == DateTime.MinValue)) &&
                        (!(value is bool boolValue && boolValue == false))) 
                    {
                        logger.LogInformation($"Property {property.Name} is not null or empty and not zero: {value}");
                        return true;
                    }
                }
            }
            logger.LogInformation("No relevant properties have values.");
            return false;
        }


    }
}

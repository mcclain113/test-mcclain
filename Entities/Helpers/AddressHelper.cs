using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.Entities.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
public static class AddressHelper
{
    public static SelectList GetStatesWithWisconsinFirst(IWCTCHealthSystemRepository repository)
    {
        var states = new SelectList(repository.AddressStates, "StateID", "StateName");
        var statesList = states.ToList();
        
        // Set Wisconsin at the top of the list
        var state50 = statesList.FirstOrDefault(s => s.Value == "50");
        if (state50 != null)
        {
            statesList.Remove(state50);
            statesList.Insert(0, state50);
        }

        return new SelectList(statesList, "Value", "Text");
    }

    public static SelectList GetCountries(IWCTCHealthSystemRepository repository)
    {
        return new SelectList(repository.Countries, "CountryId", "Name");
    }

    // A variation of the above but as a List of SelectListItems
    // These variants are used in the DisclosureController GetRequestDetailsViewModel method and
    // are needed because the associated view model builds lists
    public static List<SelectListItem> GetCountryItems(IWCTCHealthSystemRepository repository) 
    { 
        return repository.Countries.Select(country => new SelectListItem 
        { 
            Value = country.CountryId.ToString(), 
            Text = country.Name 
        }).ToList();
    }

    public static List<SelectListItem> GetStateItemsWithWisconsinFirst(IWCTCHealthSystemRepository repository) 
    { 
        var statesList = repository.AddressStates.Select(state => new SelectListItem 
        { 
            Value = state.StateID.ToString(), 
            Text = state.StateName 
        }).ToList(); 
        
        // Set Wisconsin at the top of the list 
        var state50 = statesList.FirstOrDefault(s => s.Value == "50"); 
        if (state50 != null) 
        { 
            statesList.Remove(state50); 
            statesList.Insert(0, state50); 
        } 
        return statesList; 
    }
}

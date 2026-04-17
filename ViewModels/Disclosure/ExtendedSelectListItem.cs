using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IS_Proj_HIT.ViewModels.Disclosure
{
    public class ExtendedSelectListItem : SelectListItem
    {
        public IDictionary<string, string> DataAttributes { get; set; } = new Dictionary<string, string>();
    }   
}

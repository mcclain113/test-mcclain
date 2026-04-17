using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.Disclosure
{
    public class RequestListViewModel 
    {
        public List<Request> Requests { get; set; }
        public Requester Requester { get; set; }
    }
}

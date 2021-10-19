using Model;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels.VehicleMake
{
    public class ListVehicleModelViewModel
    {
        public List<VehicleModelViewModel> VehicleModels { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}

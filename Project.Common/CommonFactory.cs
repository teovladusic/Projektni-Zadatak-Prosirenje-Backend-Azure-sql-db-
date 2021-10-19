using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class CommonFactory
    {
        public static IPagingParams CreatePagingParams(int pageNumber, int pageSize)
        {
            return new PagingParams(pageNumber, pageSize);
        }

        public static ISortParams CreateSortParams(string orderBy)
        {
            if (orderBy == "Order By")
            {
                orderBy = null;
            }
            return new SortParams(orderBy);
        }

        public static IVehicleMakeFilterParams CreateVehicleMakeFilterParams(string searchQuery)
        {
            return new VehicleMakeFilterParams(searchQuery);
        }

        public static IVehicleModelFilterParams CreateVehicleModelFilterParams(string searchQuery, string makeName)
        {
            if (makeName == "Make Name")
            {
                makeName = null;
            }
            return new VehicleModelFilterParams(searchQuery, makeName);
        }

        public static IPagedList<T> CreatePagedList<T>(List<T> items, int totalCount, int currentPage,
            int pageSize)
        {
            // just converts List to PagedList (it doesn't paginate items).
            return new PagedList<T>(items, totalCount, currentPage, pageSize);
        }
    }
}

using AutoMapper;
using Common;
using DAL;
using DAL.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Repository.Common;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Repository.Tests
{
    public class VehicleMakesRepositoryTests
    {
        [Fact]
        public async Task GetAll()
        {
           /* var vehicleMakeParams = new VehicleMakeParams();
            var sortParams = new SortParams(vehicleMakeParams.OrderBy);
            var pagingParams = new PagingParams(vehicleMakeParams.PageNumber, vehicleMakeParams.PageSize);
            var vehicleMakeFilterParams = new VehicleMakeFilterParams(sortParams, pagingParams, vehicleMakeParams.SearchQuery);

            var listToReturn = new List<VehicleMake> { new VehicleMake() };
            var viewModelList = new List<VehicleMakeViewModel> { new VehicleMakeViewModel() };
            var iViewModelList = viewModelList.Cast<IVehicleMakeViewModel>().ToList();
            var pagedListToReturn = new PagedList<VehicleMake>(
                listToReturn,
                pagingParams.CurrentPage,
                pagingParams.PageSize,
                listToReturn.Count);

            var repositoryStub = new Mock<IVehicleMakesRepository>();

            repositoryStub.Setup(repo => repo.GetAll(It.IsAny<VehicleMakeFilterParams>()))
                .ReturnsAsync(pagedListToReturn).Verifiable();

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object);

            var mapperStub = new Mock<IMapper>();

            mapperStub.Setup(mapper => mapper.Map<List<VehicleMakeViewModel>>(pagedListToReturn))
                .Returns(viewModelList);

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object, mapperStub.Object,
                loggerStub.Object);

            var result = await service.GetVehicleMakes(vehicleMakeFilterParams);

            var pagedMakes = new PagedList<IVehicleMakeViewModel>(iViewModelList, pagedListToReturn.TotalCount,
                pagingParams.CurrentPage, pagingParams.PageSize);

            repositoryStub.Verify();
            result.Should().BeEquivalentTo(
                pagedMakes,
                options => options.ComparingByMembers<IVehicleMakeViewModel>());*/
        }
    }
}

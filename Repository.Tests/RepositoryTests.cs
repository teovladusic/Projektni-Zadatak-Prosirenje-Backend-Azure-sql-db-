using DAL;
using DAL.Models;
using Moq;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Common;
using Model;

namespace Repository.Tests
{
    public class RepositoryTests
    {

        //currently method GetAll is not used and cannot
        //be tested because test relays on other methods calling this one.
        [Fact]
        public void GetAll_ReturnsAllItems()
        {
        }

        [Fact]
        public async Task GetById_WithExistingItem_ReturnsItem()
        {
            int id = 1;
            //there is no parameterles repository consturctor, so it gives the error
            var repositoryStub = new Mock<IRepository<VehicleMake>>();

            var vehicleMakeToReturn = new VehicleMake();

            repositoryStub.Setup(repo => repo.GetById(id))
                .ReturnsAsync(vehicleMakeToReturn).Verifiable();

            var unitOfWorkStub = new Mock<IUnitOfWork>();

            var mapperStub = new Mock<IMapper>();

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var sortHelperStub = new Mock<ISortHelper<VehicleMake>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object, loggerStub.Object,
                sortHelperStub.Object, mapperStub.Object);
            
            var actual = await service.GetVehicleMake(id);

            repositoryStub.Verify();
            actual.Should().BeEquivalentTo(
                vehicleMakeToReturn,
                options => options.ComparingByMembers<VehicleMake>());
        }

       /* [Fact]
        public async Task GetById_WithUnexistingItem_ReturnsNull()
        {
            int id = 1;
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var vehicleMakeToReturn = (VehicleMake)null;

            repositoryStub.Setup(repo => repo.GetById(id))
                .ReturnsAsync(vehicleMakeToReturn);

            var unitOfWorkStub = new Mock<IUnitOfWork>();

            var mapperStub = new Mock<IMapper>();

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object,
                mapperStub.Object, loggerStub.Object);

            var actual = await service.GetVehicleMake(id);

            repositoryStub.Verify();
            actual.Should().BeEquivalentTo(
                vehicleMakeToReturn,
                options => options.ComparingByMembers<VehicleMake>());
        }

        [Fact]
        public async Task Insert_WithValidEntity_ReturnsInsertedItem()
        {
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var vehicleMakeToInsert = new VehicleMake();

            repositoryStub.Setup(repo => repo.Insert(vehicleMakeToInsert))
                .Returns(vehicleMakeToInsert);

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object).Verifiable();

            var mapperStub = new Mock<IMapper>();

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object,
                mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleMake(vehicleMakeToInsert);

            result.Should().BeEquivalentTo(
                vehicleMakeToInsert,
                options => options.ComparingByMembers<VehicleMake>());
        }

        [Fact]
        public async Task Delete_WithValidEntity_DeletesItemAndReturns1()
        {
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var vehicleMakeToDelete = new VehicleMake();

            repositoryStub.Setup(repo => repo.Delete(vehicleMakeToDelete));

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object).Verifiable();

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var mapperStub = new Mock<IMapper>();

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object,
                mapperStub.Object, loggerStub.Object);

            var result = await service.DeleteVehicleMake(vehicleMakeToDelete);

            result.Should().Be(1);
        }

        [Fact]
        public async Task Update_WithValidEntity_UpdatesItemAndReturns1()
        {
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var vehicleMakeToUpdate = new VehicleMake();

            repositoryStub.Setup(repo => repo.Update(vehicleMakeToUpdate));

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object).Verifiable();

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var mapperStub = new Mock<IMapper>();

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object,
                mapperStub.Object, loggerStub.Object);

            var result = await service.UpdateVehicleMake(vehicleMakeToUpdate);

            result.Should().Be(1);
        }*/
    }
}

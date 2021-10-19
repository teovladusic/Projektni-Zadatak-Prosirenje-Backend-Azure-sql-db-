using AutoMapper;
using Common;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Model.VehicleModels;
using Service;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ViewModels.VehicleMake;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleModelsController : Controller
    {
        private readonly IVehicleModelsService _vehicleModelsService;
        private readonly ILogger<VehicleModelsController> _logger;
        private readonly IMapper _mapper;

        public VehicleModelsController(IVehicleModelsService vehicleModelsService, ILogger<VehicleModelsController> logger,
            IMapper mapper)
        {
            _vehicleModelsService = vehicleModelsService;
            _logger = logger;
            _mapper = mapper;
        }


        //GET /VehicleModels
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] VehicleModelParams vehicleModelParams)
        {
            var sortParams = CommonFactory.CreateSortParams(vehicleModelParams.OrderBy);
            var pagingParams = CommonFactory.CreatePagingParams(vehicleModelParams.PageNumber, vehicleModelParams.PageSize);
            var vehicleModelFilterParams = CommonFactory.CreateVehicleModelFilterParams(vehicleModelParams.SearchQuery,
                vehicleModelParams.MakeName);

            var pagedDomainModels = await _vehicleModelsService.GetVehicleModels(sortParams, pagingParams, vehicleModelFilterParams);

            var viewModels = _mapper.Map<List<VehicleModelViewModel>>(pagedDomainModels);

            var pagedViewModels = CommonFactory.CreatePagedList(viewModels, pagedDomainModels.TotalCount, pagedDomainModels.CurrentPage,
                pagedDomainModels.PageSize);

            var listViewModels = new ListVehicleModelViewModel
            {
                VehicleModels = viewModels,
                CurrentPage = pagedDomainModels.CurrentPage,
                HasNext = pagedDomainModels.HasNext,
                HasPrevious = pagedDomainModels.HasPrevious,
                PageSize = pagedDomainModels.PageSize,
                TotalCount = pagedDomainModels.TotalCount,
                TotalPages = pagedDomainModels.TotalPages
            };

            return Ok(listViewModels);
        }

        //GET /VehicleModels/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var domainModel = await _vehicleModelsService.GetVehicleModel((int)id);

            if (domainModel is null)
            {
                return NotFound();
            }

            var vehicleModelViewModel = _mapper.Map<VehicleModelViewModel>(domainModel);

            return Ok(vehicleModelViewModel);
        }

        //POST /VehicleModels/Create
        [HttpPost("Create/")]
        public async Task<IActionResult> Create([Bind("Name,Abrv,VehicleMakeId")] CreateVehicleModelViewModel createVehicleModelViewModel)
        {
            var createVehicleModelDomainModel = _mapper.Map<CreateVehicleModelDomainModel>(createVehicleModelViewModel);

            if (!createVehicleModelDomainModel.IsValid())
            {
                return BadRequest();
            }

            try
            {
                var response = await _vehicleModelsService.InsertVehicleModel(createVehicleModelDomainModel);
                if (response is null)
                {
                    return BadRequest();
                }

                var createdModelViewModel = _mapper.Map<VehicleModelViewModel>(response);

                return Created(createdModelViewModel.Id.ToString(), createdModelViewModel);
            }
            catch (Exception _)
            {
                return BadRequest("Invalid parameters. Check make ID!");
            }
        }

        //POST /VehicleModels/Delete/{id}
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vehicleModelDomainModel = await _vehicleModelsService.GetVehicleModel((int)id);

            if (vehicleModelDomainModel is null)
            {
                return NotFound();
            }

            await _vehicleModelsService.DeleteVehicleModel((int)id);

            return Ok();
        }

        [HttpPost("Edit/")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Abrv,VehicleMakeId")] EditVehicleModelViewModel editVehicleModelViewModel)
        {
            var editedDomainModel = _mapper.Map<VehicleModelDomainModel>(editVehicleModelViewModel);

            if (!editedDomainModel.IsValid())
            {
                return BadRequest();
            }

            var domainModelToEdit = await _vehicleModelsService.GetVehicleModel(editVehicleModelViewModel.Id);

            if (domainModelToEdit is null)
            {
                return NotFound();
            }

            try
            {
                await _vehicleModelsService.UpdateVehicleModel(editedDomainModel);

            }
            catch (Exception _)
            {
                return NotFound("An error occurred. Check Id or make Id");
            }

            return Ok();
        }
    }
}

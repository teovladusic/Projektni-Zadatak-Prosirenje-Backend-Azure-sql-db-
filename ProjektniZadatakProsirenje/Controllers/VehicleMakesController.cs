using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.Extensions.Logging;
using Model;
using AutoMapper;
using Project.Model;
using Model.VehicleMakes;
using WebAPI.ViewModels.VehicleMake;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleMakesController : Controller
    {
        private readonly IVehicleMakesService _vehicleMakesService;
        private readonly ILogger<VehicleMakesController> _logger;
        private readonly IMapper _mapper;

        public VehicleMakesController(IVehicleMakesService vehicleMakesService, ILogger<VehicleMakesController> logger,
            IMapper mapper)
        {
            _vehicleMakesService = vehicleMakesService;
            _logger = logger;
            _mapper = mapper;
        }


        //GET /VehicleMakes
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] VehicleMakeParams vehicleMakeParams)
        {
            _logger.LogInformation(vehicleMakeParams.SearchQuery);
            _logger.LogInformation(vehicleMakeParams.PageNumber.ToString());
            _logger.LogInformation(vehicleMakeParams.PageSize.ToString());
            _logger.LogInformation(vehicleMakeParams.OrderBy);
            var pagingParams = CommonFactory.CreatePagingParams(vehicleMakeParams.PageNumber, vehicleMakeParams.PageSize);
            var sortParams = CommonFactory.CreateSortParams(vehicleMakeParams.OrderBy);
            var vehicleMakeFilterParams = CommonFactory.CreateVehicleMakeFilterParams(vehicleMakeParams.SearchQuery);

            var pagedMakes = await _vehicleMakesService.GetVehicleMakes(sortParams, pagingParams, vehicleMakeFilterParams);

            var makeViewModels = _mapper.Map<List<VehicleMakeViewModel>>(pagedMakes);

            var listViewModel = new ListVehicleMakeViewModel
            {
                VehicleMakes = makeViewModels,
                CurrentPage = pagedMakes.CurrentPage,
                HasNext = pagedMakes.HasNext,
                HasPrevious = pagedMakes.HasPrevious,
                PageSize = pagedMakes.PageSize,
                TotalCount = pagedMakes.TotalCount,
                TotalPages = pagedMakes.TotalPages
            };

            return Ok(listViewModel);
        }

        //GET /VehicleMakes/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vehicleMake = await _vehicleMakesService.GetVehicleMake((int)id);

            if (vehicleMake is null)
            {
                return NotFound();
            }

            var vehicleMakeViewModel = _mapper.Map<VehicleMakeViewModel>(vehicleMake);

            return Ok(vehicleMakeViewModel);
        }

        //POST /VehicleMakes/Create
        [HttpPost("Create/")]
        public async Task<IActionResult> Create([Bind("Name,Abrv")] CreateVehicleMakeViewModel createVehicleMakeViewModel)
        {
            var createVehicleMakeDomainModel = _mapper.Map<CreateVehicleMakeDomainModel>(createVehicleMakeViewModel);

            if (!createVehicleMakeDomainModel.IsValid())
            {
                return BadRequest();
            }

            var insertedVehicleMake = await _vehicleMakesService.InsertVehicleMake(createVehicleMakeDomainModel);

            if (insertedVehicleMake is null)
            {
                return BadRequest();
            }

            var vehicleMakeViewModel = _mapper.Map<VehicleMakeViewModel>(insertedVehicleMake);

            var created = Created(vehicleMakeViewModel.Id.ToString(), vehicleMakeViewModel);
            _logger.LogInformation(created.Value.ToString());

            return created;
        }

        //POST /VehicleMakes/Delete/{id}
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vehicleMake = await _vehicleMakesService.GetVehicleMake((int)id);

            if (vehicleMake == null)
            {
                return NotFound();
            }

            await _vehicleMakesService.DeleteVehicleMake(vehicleMake);

            return Ok();
        }

        [HttpPost("Edit/")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Abrv")] VehicleMakeViewModel vehicleMakeViewModel)
        {
            var editedDomainModel = _mapper.Map<VehicleMakeDomainModel>(vehicleMakeViewModel);
            if (!editedDomainModel.IsValid())
            {
                return BadRequest();
            }

            var existingVehicleMakeDomainModel =
                await _vehicleMakesService.GetVehicleMake(vehicleMakeViewModel.Id);

            if (existingVehicleMakeDomainModel is null)
            {
                return NotFound();
            }

            await _vehicleMakesService.UpdateVehicleMake(editedDomainModel);
            return Ok();
        }
    }
}
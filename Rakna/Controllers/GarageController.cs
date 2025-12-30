using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rakna.DAL;
using Rakna.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Rakna.BAL.DTO.GarageDto;
using Rakna.BAL.DTO;
using Rakna.BAL.DTO.GarageStaffDto;
using Microsoft.AspNetCore.Authorization;

namespace Rakna.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "Garage")]
    public class GarageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GarageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// This endpoint is returning all cities that contains garages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Cities")]
        public async Task<ActionResult> CitiesContiansGarage()
        {
            var garages = _unitOfWork.Garage.AsNoTracking().GroupBy(g => g.city).Select(p => new CitiesDto
            {
                City = p.Key,
                NumberOfGarage = p.Count()
            }).ToList();
            return Ok(garages);
        }

        /// <summary>
        /// This endpoint is returning all garages in specific city
        /// </summary>
        /// <param name="City"></param>
        /// <returns></returns>
        [HttpGet("GaragesInSpacificCity")]
        public async Task<ActionResult> GetGaragesInSpacificCity(string City)
        {
            var garages = await _unitOfWork.Garage.AsNoTracking().Where(c => c.city == City).Select(g => new GetGarageDto
            {
                longitude = g.Longitude,
                latitude = g.Latitude,
                street = g.street,
                city = g.city,
                HourPrice = g.HourPrice,
                AvailableSpaces = g.AvailableParkingSlots,
                TotalSpaces = g.TotalParkingSlots,
                GarageName = g.garageName,
                GarageId = g.GarageId
            }).ToListAsync();

            if (garages.Count == 0)
            {
                return BadRequest("No Garages found in this area");
            }

            return Ok(garages);
        }
    }
}
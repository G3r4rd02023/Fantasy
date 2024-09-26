using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.DTO;
using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Fantasy.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : GenericController<Country>
    {
        private readonly ICountriesUnitOfWork _countriesUnitOfWork;

        public CountriesController(IGenericUnitOfWork<Country> unit, ICountriesUnitOfWork countriesUnitOfWork) : base(unit)
        {
            _countriesUnitOfWork = countriesUnitOfWork;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _countriesUnitOfWork.GetComboAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _countriesUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _countriesUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpGet("paginated")]
        public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
        {
            var response = await _countriesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalRecordsPaginated")]
        public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _countriesUnitOfWork.GetTotalRecordsAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}
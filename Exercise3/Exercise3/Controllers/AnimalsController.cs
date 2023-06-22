using Exercise3.Models;
using Exercise3.Models.DTOs;
using Exercise3.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Exercise3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalsRepository _animalsRepository;
        public AnimalsController(IAnimalsRepository animalsRepository)
        {

            _animalsRepository = animalsRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAnimals(string orderBy = "name")
        {
            var list = await _animalsRepository.GetAnimalsAsync(orderBy);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimal(AnimalPOST animalPOST)
        {
            if (animalPOST == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            if (await _animalsRepository.DoesAnimalExist(animalPOST.ID))
            {
                return Conflict();
            }
            _animalsRepository.AddAnimal(animalPOST);
            return Created("api/animals", animalPOST);
        }

        [HttpPut("{animalID}")]
        public async Task<IActionResult> PutAnimal(int animalID, AnimalPUT animalPUT)
        {
            if (animalPUT == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!(await _animalsRepository.DoesAnimalExist(animalID)))
            {
                return NotFound();
            }
            var animalNew = await _animalsRepository.PutAnimalAsync(animalID, animalPUT);
            return Ok(animalNew);
        }

        [HttpDelete("{animalID}")]
        public async Task<IActionResult> RemAnimal(int animalID)
        {
            if (!(await _animalsRepository.DoesAnimalExist(animalID)))
            {
                return NotFound();
            }

            await _animalsRepository.RemAnimal(animalID);
            return Ok();
        }
    }
}
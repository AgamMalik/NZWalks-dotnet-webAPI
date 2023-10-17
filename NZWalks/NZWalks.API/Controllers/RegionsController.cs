using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        //we are now accessing dbcontext which we injected as dependency injection in program.cs..we are using it here through constructor injection.
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        //get all regions
        [HttpGet]   
        public IActionResult GetAll()
        {
            //get data from database - domain models
            var regionsDomain = dbContext.Regions.ToList();


            //map domain models to DTOs
            var regionsDto = new List<RegionDto>();


            //now loop over all the regions and convert them to regiondtos
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }



            //return DTOs
            return Ok(regionsDto);
        }




        //get single region by id
        [HttpGet]
        [Route("id:Guid")]
        public IActionResult GetById([FromRoute] Guid id) 
        {
            // var region = dbContext.Regions.Find(id);
            //get region domain model from database
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null) 
            {
                return NotFound();
            }

            // map/convert the region domain model to regiondto
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            // return dto back to client
            return Ok(regionDto);
        }


        // POST to create new region
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {
            // map or convert dto to domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //use domain model to create region using dbcontext
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            // map domain model back to dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new {id = regionDomainModel.Id}, regionDto);
        }


        // PUT: update region
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // check if region exists
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // map dto to domain model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

            // convert domain model to dto to send back as response
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);

        }


        // delete
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // delete region
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

            return Ok();
        }

    }
}

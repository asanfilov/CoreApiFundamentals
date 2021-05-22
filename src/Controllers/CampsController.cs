﻿using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using CoreCodeCamp.Data;
using CoreCodeCamp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;

        public CampsController(ICampRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
        {
            try
            {
                var camps = await repository.GetAllCampsAsync( includeTalks );
                return mapper.Map<CampModel[]>( camps );
            }
            catch (System.Exception ex)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }

        [HttpGet( "{moniker}" )]
        public async Task<ActionResult<CampModel>> Get(string moniker)
        {
            try
            {
                var camp = await repository.GetCampAsync( moniker );
                if (camp == null)
                {
                    return NotFound();
                }
                return mapper.Map<CampModel>( camp );
            }
            catch (Exception ex)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }

        [HttpGet( "search" )]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                var camps = await repository.GetAllCampsByEventDate( theDate, includeTalks );
                if (!camps.Any()) return NotFound();

                return mapper.Map<CampModel[]>( camps );
            }
            catch (Exception ex)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }

        public async Task<ActionResult<CampModel>> Create(CampModel campModel)
        {
            try
            {
                var camp = mapper.Map<Camp>( campModel );
                repository.Add( camp );
                if (await repository.SaveChangesAsync())
                {
                    return Created( $"/api/camps/{camp.Moniker}", mapper.Map<CampModel>( camp ) );
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
            return BadRequest();
        }
    }
}

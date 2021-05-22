﻿using System.Threading.Tasks;

using AutoMapper;

using CoreCodeCamp.Data;
using CoreCodeCamp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CoreCodeCamp.Controllers
{
    [Route( "api/camps/{moniker}/[controller]" )]
    [ApiController]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public TalksController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {
            try
            {
                var talks = await repository.GetTalksByMonikerAsync( moniker );
                return mapper.Map<TalkModel[]>( talks );
            }
            catch (System.Exception ex)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }

        [HttpGet( "{id:int}" )]
        public async Task<ActionResult<TalkModel>> GetById(string moniker, int id)
        {
            try
            {
                var talk = await repository.GetTalkByMonikerAsync( moniker, id );
                return mapper.Map<TalkModel>( talk );
            }
            catch (System.Exception ex)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }
    }
}

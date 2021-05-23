using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel talkModel)
        {
            try
            {
                var camp = await repository.GetCampAsync( moniker );
                if (camp == null) return BadRequest( "Camp does not exist" );

                if (talkModel.Speaker == null) return BadRequest( "Speaker ID is required" );
                var speaker = await repository.GetSpeakerAsync( talkModel.Speaker.SpeakerId );
                if (speaker == null) return BadRequest( "Speaker could not be found" );

                var talk = mapper.Map<Talk>( talkModel );
                talk.Camp = camp;
                talk.Speaker = speaker;

                repository.Add( talk );

                if (await repository.SaveChangesAsync())
                {
                    string url = linkGenerator.GetPathByAction( HttpContext, "GET",
                                                               values: new { moniker, talkId = talk.TalkId } );
                    //TODO is there a better way to build the url that [HttpGet( "{id:int}" )] expects?
                    url = url.Replace( "?talkId=", "/" );
                    return Created( url, mapper.Map<TalkModel>( talk ) );
                }
                else
                {
                    return BadRequest( "Failed to save new Talk" );
                }

                return mapper.Map<TalkModel>( talk );
            }
            catch (System.Exception ex)
            {
                return this.StatusCode( StatusCodes.Status500InternalServerError, "Database failure" );
            }
        }
    }
}

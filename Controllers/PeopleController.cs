using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ELK.Playground.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class PeopleController(IElasticClient client) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Person person, CancellationToken cancellationToken)
        {
            await client.IndexDocumentAsync(person, cancellationToken);
            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await client.SearchAsync<Person>(s =>
                s.Query(q =>
                    q.Match(m =>
                        m.Field("id")
                            .Query(id.ToString())
                        )), cancellationToken);

            return Ok(result.Documents);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByName(string key, CancellationToken cancellationToken)
        {
            var result = await client.SearchAsync<Person>(s =>
                s.Query(q =>
                    q.Match(m =>
                        m.Field("name")
                            .Query(key)
                        )), cancellationToken);

            return Ok(result.Documents);
        }
    }
}

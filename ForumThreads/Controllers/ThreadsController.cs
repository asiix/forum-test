using ForumThreads.Model;
using ForumThreads.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForumThreads.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly ThreadsService _threadsService;
        private readonly CommentsService _commentsService;

        public ThreadsController(ThreadsService threadsService, CommentsService commentsService)
        {
            _threadsService = threadsService;
            _commentsService = commentsService;
        }

        // GET: api/<ThreadsController>
        [HttpGet]
        public async Task<List<ForumThreads.Model.Thread>> Get() =>
        await _threadsService.GetAsync();

        // GET api/<ThreadsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ForumThreads.Model.Thread>> Get(string id)
        {
            var thread = await _threadsService.GetAsync(id);

            if (thread is null)
            {
                return NotFound();
            }

            return thread;
        }

        // POST api/<ThreadsController>
        [HttpPost]
        public async Task<IActionResult> Post(ForumThreads.Model.Thread thread)
        {
            await _threadsService.CreateAsync(thread);
            return CreatedAtAction(nameof(Get), new { id = thread._id }, thread);
        }

        // PUT api/<ThreadsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ForumThreads.Model.Thread updatedThread)
        {
            var thread = await _threadsService.GetAsync(id);

            if (thread is null)
            {
                return NotFound();
            }

            updatedThread._id = thread._id;

            await _threadsService.UpdateAsync(id, updatedThread);

            return NoContent();
        }

        // DELETE api/<ThreadsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var thread = await _threadsService.GetAsync(id);

            if (thread is null)
            {
                return NotFound();
            }

            await _threadsService.RemoveAsync(id);

            return NoContent();
        }
    }
}

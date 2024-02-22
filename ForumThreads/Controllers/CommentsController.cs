using ForumThreads.Model;
using ForumThreads.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForumThreads.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ThreadsService _threadsService;
        private readonly CommentsService _commentsService;

        public CommentsController(ThreadsService threadsService, CommentsService commentsService)
        {
            _threadsService = threadsService;
            _commentsService = commentsService;
        }

        // GET api/Threads/1/1
        [HttpGet("getbyid/{threadId}/{commentId}")]
        public async Task<ActionResult<Comment>> Get(string threadId, string commentId)
        {
            var comment = await _commentsService.GetCommentAsync(threadId, commentId);

            if (comment == null)
                return NotFound();

            return comment;
        }

        // POST api/Threads/1
        [HttpPost("create/{threadId}")]
        public async Task<IActionResult> PostComment(string threadId, Comment comment, string? originalCommentId = null)
        {
            await _commentsService.CreateCommentAsync(threadId, comment, originalCommentId);
            return Ok("Comment added succesfully");
        }

        // PUT api/Threads/5
        [HttpPut("modify/{threadId}/{commentId}")]
        public async Task<IActionResult> UpdateComment(string threadId, string commentId, Comment updatedComment)
        {
            await _commentsService.ModifyCommentAsync(threadId, commentId, updatedComment);
            return Ok("Comment Updated succesfully");
        }

        // DELETE api/Threads/5
        [HttpPut("delete/{threadId}/{commentId}")]
        public async Task<IActionResult> DeleteComment(string threadId, string commentId)
        {
            await _commentsService.DeleteCommentAsync(threadId, commentId);
            return Ok("Comment Deleted succesfully");
        }
    }
}

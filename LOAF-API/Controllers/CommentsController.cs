﻿using LOAF_API.Data.Contexts;
using LOAF_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace LOAF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly LOAFDbContext _LOAFDbContext;

        public CommentsController(LOAFDbContext loafDbContext)
        {
            _LOAFDbContext = loafDbContext;
        }

        // GET: api/Comments/Post/5
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPost(int postId)
        {
            var comments = await _LOAFDbContext.Comments
                .Where(c => c.PostId == postId)
                .ToListAsync();

            return Ok(comments);
        }

        // POST api/Comments/Post/5
        [HttpPost("post/{postId}")]
        public async Task<IActionResult> AddCommentToPost(int postId, [FromBody] Comment comment)
        {
            comment.PostId = postId;
            comment.Date = System.DateTime.Now.ToString();
            // increment the number of replies for the post
            var post = await _LOAFDbContext.Posts.FindAsync(postId);
            post.Replies++;
            await _LOAFDbContext.Comments.AddAsync(comment);
            await _LOAFDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommentsByPost), new { postId = postId }, comment);
        }
    }
}

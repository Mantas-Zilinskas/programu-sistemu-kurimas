using EmocineSveikataServer.Data;
using EmocineSveikataServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EmocineSveikataServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TestController> _logger;
        private readonly DataContext _context;

        public TestController(ILogger<TestController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("discussion")]
        public DiscussionModel GetDiscussion()
        {
            DiscussionModel disc = new DiscussionModel {
                Title = "I hate my job",
                Content = "I hate my job, how do I stop?"
            };
            disc.Likes = 6;

            _context.Discussions.Add(disc);
            _context.SaveChanges();

            return _context.Discussions.Include(d => d.Comments).FirstOrDefault();
        }

        [HttpGet("comment")]
        public CommentModel GetComment()
        {

            DiscussionModel disc = new DiscussionModel
            {
                Title = "I hate my job",
                Content = "I hate my job, how do I stop?"
            };
            disc.Likes = 6;

            _context.Discussions.Add(disc);
            _context.SaveChanges();

            disc = _context.Discussions.Find(1);

            CommentModel com = new CommentModel
            {
                Content = "Same, dude"
            };
            disc.Comments.Add(com);
            _context.SaveChanges();

            var com1 = _context.Comments.FirstOrDefault();
            com = new CommentModel
            {
                Content = "Same, dude"
            };
            com1.Replies.Add(com);
            _context.SaveChanges();

            return _context.Comments.Include(d => d.Replies).FirstOrDefault();
        }
    }
}

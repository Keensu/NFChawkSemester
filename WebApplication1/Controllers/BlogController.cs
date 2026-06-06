using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models;
using NFChawk.Models.Entities;
using NFChawk.Models.Interfaces;
using NFChawk.Models.ViewModels;

namespace NFChawk.Controllers
{
    public class BlogController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public BlogController(IEmailService emailService, ApplicationDbContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        // GET: Blog
        public async Task<IActionResult> Index(string? searchField)
        {
            var postsQuery = _context.BlogPosts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchField))
            {
                string searchLower = searchField.ToLower();

                postsQuery = postsQuery.Where(p =>
                    p.Title.ToLower().Contains(searchLower) ||
                    p.Content.ToLower().Contains(searchLower) ||
                    p.CategoryDisplay.ToLower().Contains(searchLower)
                );
                ViewData["CurrentFilter"] = searchField;
            }

            var posts = await postsQuery.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View(posts);
        }

        // GET: Blog/Single
        public async Task<IActionResult> Single(int id)
        {
            var post = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var viewModel = new BlogSingleViewModel
            {
                Post = post,
                CommentForm = new CommentFormViewModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendComment(BlogSingleViewModel model, int postId)
        {
            try
            {
                if (string.IsNullOrEmpty(model.CommentForm.Name) || string.IsNullOrEmpty(model.CommentForm.Email))
                {
                    TempData["Error"] = "Please fill in all required fields.";
                    return RedirectToAction("Single", new { id = postId });
                }

                string subject = $"New comment on post ID {postId} from {model.CommentForm.Name}";
                string body = $"<p><strong>Name:</strong> {model.CommentForm.Name}</p>" +
                              $"<p><strong>Email:</strong> {model.CommentForm.Email}</p>" +
                              $"<p><strong>Message:</strong> {model.CommentForm.Message}</p>";

                await _emailService.SendEmailAsync("work3do1send@gmail.com", subject, body);

                TempData["Success"] = "Thank you! Your comment has been sent.";
            }
            catch
            {
                TempData["Error"] = "An error occurred while sending your comment. Please try again later.";
            }

            return RedirectToAction("Single", new { id = postId });
        }
    }
}
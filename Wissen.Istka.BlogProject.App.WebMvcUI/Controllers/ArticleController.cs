using Microsoft.AspNetCore.Mvc;
using Wissen.Istka.BlogProject.App.Entity.Entities;
using Wissen.Istka.BlogProject.App.Entity.Services;
using Wissen.Istka.BlogProject.App.Entity.ViewModels;

namespace Wissen.Istka.BlogProject.App.WebMvcUI.Controllers
{
	public class ArticleController : Controller
	{
		private readonly IArticleService _articleService;
		private readonly ICommentService _commentService;
		private readonly IAccountService _accountService;

		public ArticleController(IArticleService articleService, ICommentService commentService, IAccountService accountService)
		{
			_articleService = articleService;
			_commentService = commentService;
			_accountService = accountService;
		}
		public async Task<IActionResult> Index(int? id, string? search)
		{
			var list = await _articleService.GetAll();

			if(id != null)		//id - CategoryId
			{
				list = list.Where(a => a.CategoryId == id).ToList();		
			}
			if(search != null)
			{
				list = list.Where(a => a.Title.ToLower().Contains(search.ToLower().Trim())).ToList();
			}

			return View(list);
		}
		public async Task<IActionResult> Details(int id)
		{
			ViewBag.Comments = await _commentService.GetAllByArticleId(id);
			var model = await _articleService.Get(id);

			return View(model);
		}
		public async Task<IActionResult> CreateComment(string message, int id)
		{
			var user = await _accountService.Find(User.Identity.Name);
			CommentViewModel model = new()
			{
				ArticleId = id,
				Content = message,
				UserId = user.Id 
			};
			await _commentService.Add(model);
			return RedirectToAction("Index");
		}
        [HttpGet]
        public IActionResult CreateArticle()
        {
            return View();
        }
        public async Task<IActionResult> CreateArticle(Article article,int id)
		{
			var user = await _accountService.Find(User.Identity.Name);
			ArticleViewModel model = new()
			{
				CategoryId = id,
				PictureUrl = article.PictureUrl,
				Summary = article.Summary,
				Title = article.Title,
				UserId = user.Id,
				CreatedDate = DateTime.Now,
				Content = article.Content,
			};
			await _articleService.Add(model);
			return RedirectToAction("Index");
		}
	}
}

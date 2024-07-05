using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wissen.Istka.BlogProject.App.DataAccess.Identity;
using Wissen.Istka.BlogProject.App.Entity.Services;
using Wissen.Istka.BlogProject.App.Entity.ViewModels;

namespace Wissen.Istka.BlogProject.App.Service.Services
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IMapper _mapper;

		public AccountService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IMapper mapper)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_mapper = mapper;
		}

		public async Task<string> CreateUserAsync(RegisterViewModel model)
		{
			string message = string.Empty;
			AppUser user = new AppUser()
			{
				Name = model.FirstName,
				Surname = model.LastName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				UserName = model.UserName
			};
			var identityResult = await _userManager.CreateAsync(user, model.ConfirmPassword);

			if (identityResult.Succeeded)
			{
				message = "OK";
			}
			else
			{
				foreach (var error in identityResult.Errors)
				{
					message = error.Description;
				}
			}
			return message;
		}

        public async Task<UserViewModel> Find(string username)
		{
            var user = await _userManager.FindByNameAsync(username);
			return _mapper.Map<UserViewModel>(user);
        }

		public async Task<string> FindByNameAsync(LoginViewModel model)
		{
			string message = string.Empty;
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
				message = "kullanıcı bulunamadı!";
				return message;
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false); 
			if (signInResult.Succeeded)
			{
				message = "OK";
			}
			return message;
        }
	}
}

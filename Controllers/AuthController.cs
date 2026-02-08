using HomeChefss.Data;
using HomeChefss.DTO.Auth;
using HomeChefss.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using RegisterChefDto = HomeChefss.DTO.Chef.RegisterChefDto;

namespace HomeChefss.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly AppDbContext _context;
		private readonly IConfiguration _config;

		public AuthController(
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager, 
			AppDbContext context, 
			IConfiguration config)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
			_config = config;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterUserDto dto)
		{
			if (await _userManager.FindByEmailAsync(dto.Email) != null)
				return BadRequest("Email already registered");


			var user = new ApplicationUser
			{
				UserName = dto.Email,
				Email = dto.Email,
				FullName = dto.Fullname,
			};

			var result = await _userManager.CreateAsync(user, dto.Password);
			
			if (!result.Succeeded) 
				return BadRequest(result.Errors);

			await _userManager.AddToRoleAsync(user, "User");

			return Ok("User registered successfully");
		}

		//[HttpPost("register-chef")]
		//public async Task<IActionResult> RegisterChef(RegisterChefDto dto)
		//{
		//	if (await _userManager.FindByEmailAsync(dto.Email) != null)
		//		return BadRequest("Email already registered");

		//	var user = new ApplicationUser
		//	{
		//		UserName = dto.Email,
		//		Email = dto.Email,
		//		FullName = dto.Fullname,
		//	};

		//	var result = await _userManager.CreateAsync(user, dto.Password);

		//	if (!result.Succeeded)
		//		return BadRequest(result.Errors);

		//	await _userManager.AddToRoleAsync(user, "Chef");

		//	var chef = new Chef
		//	{
		//		UserId = user.Id,
		//		KitchenName = dto.KitchenName,
		//		City = dto.City,
		//		Area = dto.Area,
		//		IsVerified = false,
		//	};

		//	_context.Chefs.Add(chef);
		//	await _context.SaveChangesAsync();

		//	return Ok("Chef registered successfully");
		//}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);

			if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
				return Unauthorized("Invalid credentials");

			var roles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, dto.Email),
			};

			foreach (var role in roles)
				claims.Add(new Claim(ClaimTypes.Role, role));

			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims = claims,
				expires: DateTime.UtcNow.AddDays(7),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);

			return Ok(new
			{
				token = new JwtSecurityTokenHandler().WriteToken(token)
			});
		}

		
		[Authorize]
		[HttpPost("apply-chef")]
		public async Task<IActionResult> ApplyChef(ApplyChefDto dto)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userId))
				return Unauthorized("User ID not found in token");

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				return Unauthorized("User not found");

			if (await _context.Chefs.AnyAsync(c => c.UserId == user.Id))
				return BadRequest("Chef profile already exists");

			////  SAFETY CHECK: prevent duplicate Chef row
			//var existingChef = await _context.Chefs
			//	.AnyAsync(c => c.UserId == user.Id);

			//if (existingChef)
			//	return BadRequest("Chef profile already exists");

			//await _userManager.AddToRoleAsync(user, "Chef");

			if (!await _userManager.IsInRoleAsync(user, "User"))
				return BadRequest("Only users can apply to become chefs");

			var chef = new Chef
			{
				UserId = user.Id,
				//User = user,
				KitchenName = dto.KitchenName,
				City = dto.City,
				Area = dto.Area,
				GovernmentIdNumber = dto.GovernmentIdNumber,
				CulinaryCertificateUrl = dto.CulinaryCertificateUrl,
				YearsOfExperience = dto.YearsOfExperience,

				VerificationStatus = VerificationStatus.Pending,
				ChefLevel = 0,
				IsVerified = false,
			};

			
				_context.Chefs.Add(chef);
				await _context.SaveChangesAsync();


			//if (!await _userManager.IsInRoleAsync(user, "ChefPending"))
			//	await _userManager.AddToRoleAsync(user, "ChefPending");

			var roleResult = await _userManager.AddToRoleAsync(user, "ChefPending");

			if (!roleResult.Succeeded)
			{
				return BadRequest(roleResult.Errors);
			} 

			return Ok("Chef application submitted and pendind admin review");
		}

	}
}

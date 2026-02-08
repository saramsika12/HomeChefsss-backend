using Microsoft.EntityFrameworkCore;
using HomeChefss.Data;
using HomeChefss.Models;
using Microsoft.AspNetCore.Identity;
using HomeChefss.Repositories.Interface;
using HomeChefss.Repositories;
using HomeChefss.Services.Interfaces;
using HomeChefss.Services;
using HomeChefss.Middlewares;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using HomeChefss.Services.Background;
using HomeChefss.Services.Gateway;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler =
			System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
	});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter: Bearer {your JWT token}"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});


builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(
		  builder.Configuration.GetConnectionString("DefaultConnection"))
	  .EnableSensitiveDataLogging()
	  .EnableDetailedErrors()
	  );

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var jwtKey = builder.Configuration["Jwt:Key"]
	?? throw new InvalidOperationException("JWT Key is not configured.");

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options => 
{ 
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,

		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(jwtKey)
		)
	};
});


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();

builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IFoodItemServices, FoodItemServices>();
builder.Services.AddScoped<IChefRepository, ChefRepository>();
builder.Services.AddScoped<IChefServices, ChefServices>();
builder.Services.AddScoped<IAdminServices, AdminServices>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<IDeliveryFareService, DeliveryFareService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();

builder.Services.AddScoped<EsewaGateway>();
builder.Services.AddScoped<KhaltiGateway>();
builder.Services.AddScoped<DummyCardGateway>();

builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<INotificationService, NotificationService>();

//builder.Services.AddHostedService<ChefTrialMonitorServices>();
//builder.Services.AddHostedService<FoodExpiryCheckerService>();





var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

	if (!await roleManager.RoleExistsAsync("Admin"))
		await roleManager.CreateAsync(new IdentityRole("Admin"));

	var adminEmail = "admin@homechef.com";
	var adminUser = await userManager.FindByEmailAsync(adminEmail);

	if (adminUser == null)
	{
		adminUser = new ApplicationUser
		{
			UserName = adminEmail,
			Email = adminEmail,
			FullName = "System Admin"
		};

		await userManager.CreateAsync(adminUser, "Admin@123");
		await userManager.AddToRoleAsync(adminUser, "Admin");
	}

	string[] roles = { "User", "Chef", "ChefPending", "DeliveryPartner", "Admin" };

	foreach (var role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
			await roleManager.CreateAsync(new IdentityRole(role));
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

app.Run();

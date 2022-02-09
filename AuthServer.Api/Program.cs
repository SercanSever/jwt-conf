using AuthServer.Core.Configuration;
using AuthServer.Core.Models;
using AuthServer.Core.Repository;
using AuthServer.Core.Service;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data;
using AuthServer.Data.Repository;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IServiceGeneric<,>), typeof(GenericService<,>));
builder.Services.AddDbContext<AppDbContext>(opt =>
{
   opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
   {
      sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
   });
});
builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
   opt.User.RequireUniqueEmail = true;
   opt.Password.RequireDigit = false;
   opt.Password.RequiredLength = 6;
   opt.Password.RequireLowercase = false;
   opt.Password.RequireNonAlphanumeric = false;
   opt.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();









var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

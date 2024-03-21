using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETFinalProject.Data;
using NETFinalProject.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<FinanceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<FinanceContext>();


//builder.Services.AddIdentity<User, IdentityRole>(options =>
//{
//    // Configure identity options here
//})
//    .AddEntityFrameworkStores<FinanceContext>()
//    .AddDefaultTokenProviders();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication(); // Enables authentication

app.UseAuthorization();

app.MapControllers();

app.Run();

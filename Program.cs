using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CPRM.Data;
using CPRM.Areas.Identity.Data;
using CPRM.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("cprmContext") ?? throw new InvalidOperationException("Connection string 'CPRMContextConnection' not found.");

builder.Services.AddDbContext<CPRMContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<CPRMDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<CPRMUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<CPRMContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<FacePlusPlusService>();
builder.Services.AddScoped<IEmailSender, EmailService>();
builder.Services.AddScoped<TawkToService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

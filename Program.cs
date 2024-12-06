using Microsoft.EntityFrameworkCore;
using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Bat session
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPersonalTaskRepository, PersonalTaskRepository>();
builder.Services.AddScoped<IPersonalNoteRepository, PersonalNoteRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupTaskRepository, GroupTaskRepository>();
builder.Services.AddScoped<IGroupNoteRepository, GroupNoteRepository>();
builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
builder.Services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Su dung Session
app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

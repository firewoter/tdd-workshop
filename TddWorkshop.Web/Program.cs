using FluentValidation.AspNetCore;
using MediatR;
using TddWorkshop.Domain.InstantCredit;
using TddWorkshop.Web.Pipeline;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();

if (isDevelopment)
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            b => b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });
}

builder.Services.AddSingleton<ExceptionMiddleware>();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICriminalRecordChecker, CriminalRecordChecker>();
builder.Services.AddMediatR(typeof(CalculateCreditHandler));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<CalculateCreditRequest>());

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapFallbackToFile("index.html");
app.Run();
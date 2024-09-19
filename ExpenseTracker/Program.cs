using ExpenseTracker.Data;
using ExpenseTracker.Repositories;
using ExpenseTracker.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ExpenseDbContext>(options =>
            //options.UseSqlServer(connectionString));
            options.UseInMemoryDatabase(databaseName: "DefaultConnection"));

builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<ExpenseTypeService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers()
    // for infinite loop return of CreatedAtAction(...)
    .AddJsonOptions(x => 
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//To build the SqlServer database, run the following commands in the terminal:
//dotnet ef migrations add InitialCreate
//dotnet ef database update
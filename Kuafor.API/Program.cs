using Kuafor.DataAccess; // DbContext için
using Kuafor.Core; // Modeller için
using Microsoft.EntityFrameworkCore; // EF Core için
using Kuafor.Business; // RandevuService'i tanýmasý için

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. ApplicationDbContext'i projenin servislerine (DI) ekle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString) // EF Core'a SQL Server kullanacaðýmýzý söyle
);
// Ýþ mantýðý servislerimizi buraya kaydediyoruz
builder.Services.AddScoped<RandevuService>();

builder.Services.AddControllers();
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

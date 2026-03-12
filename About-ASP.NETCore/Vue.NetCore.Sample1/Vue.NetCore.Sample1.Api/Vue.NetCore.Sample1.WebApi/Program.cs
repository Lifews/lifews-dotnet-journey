var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowVueDev",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173") // 允许 Vue 开发服务器的源
                .AllowAnyHeader() // 允许所有请求头
                .AllowAnyMethod(); // 允许所有 HTTP 方法 (GET, POST, PUT...)
        }
    );
});

// Add services to the container.

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

// 启用 CORS 中间件（注意顺序：应在 UseAuthorization 之前）
app.UseCors("AllowVueDev");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

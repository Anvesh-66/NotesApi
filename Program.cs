using NotesApi.Services;
using NotesApi.Middleware;
using NotesApi.Model;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<INoteService, NoteService>();
builder.Services.Configure<NoteSettings>(builder.Configuration.GetSection("NoteSettings"));


var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

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

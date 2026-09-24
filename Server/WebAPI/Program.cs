using FileRepositories;
using RepositoryContract;
using WebApi.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<IPostVoteRepository, PostVoteFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();
builder.Services.AddScoped<ICommentVoteRepository, CommentVoteFileRepository>();
builder.Services.AddScoped<ISubforumRepository, SubforumFileRepository>();
builder.Services.AddScoped<IUserRepository, UserFileRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapControllers();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
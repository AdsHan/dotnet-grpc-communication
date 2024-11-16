using GrpcCommunication.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration();
builder.Services.AddGrpcConfiguration();
builder.Services.AddDependencyConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseApiConfiguration();
app.UseGrpcConfiguration();
app.UseSwaggerConfiguration();

app.Run();
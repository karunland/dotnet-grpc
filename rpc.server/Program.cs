using System.IO.Compression;
using rpc.server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(x =>
{
    x.EnableDetailedErrors = true;
    x.MaxReceiveMessageSize = 10 * 1024 * 1024; // 10MB
    x.MaxSendMessageSize = 10 * 1024 * 1024; // 10MB
    x.ResponseCompressionLevel = CompressionLevel.Optimal;
});

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<FileServiceImpl>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
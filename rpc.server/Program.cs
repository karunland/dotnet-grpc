using rpc.server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(x =>
{
    x.EnableDetailedErrors = true;
});

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<FileServiceImpl>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made");


app.Run();
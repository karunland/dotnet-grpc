using Microsoft.Extensions.DependencyInjection;

namespace rpc.sdk;

public static class RpcClientExtensions
{
    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        services.AddSingleton<RpcClient>();
        services.AddGrpcClient<Greeter.GreeterClient>(x =>
        {
            x.Address = new Uri("http://localhost:5000");
        });
        
        services.AddGrpcClient<FileService.FileServiceClient>(x =>
        {
            x.Address = new Uri("http://localhost:5000");
        });
        return services;
    }
} 
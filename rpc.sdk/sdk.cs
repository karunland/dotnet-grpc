using Microsoft.Extensions.DependencyInjection;

namespace rpc.sdk;

public class RpcClient(Greeter.GreeterClient greeterClient, FileService.FileServiceClient fileServiceClient)
{
    public async Task<string> SayHelloAsync(string name)
    {
        var response = await greeterClient.SayHelloAsync(new HelloRequest { Name = name });
        return response.Message;
    }
    
    public async Task<string> UploadFileAsync(string bytes, string fileName, string fileType)
    {
        var response = fileServiceClient.UploadFile(new UploadFileRequest { Bytes = bytes, FileName = fileName, FileType = fileType });
        return response.Message;
    }

    public async Task<DownloadFileResponse> DownloadFileAsync(string fileName)
    {
        var response = fileServiceClient.DownloadFile(new DownloadFileRequest { FileName = fileName });
        return response;
    }
}

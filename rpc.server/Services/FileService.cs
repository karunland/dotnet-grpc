using Grpc.Core;
using rpc.sdk;

namespace rpc.server.Services;

public class FileServiceImpl : FileService.FileServiceBase
{
    public override Task<UploadFileResponse> UploadFile(UploadFileRequest request, ServerCallContext context)
    {
        try
        {
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            Directory.CreateDirectory(uploadDir);
            
            var filePath = Path.GetFullPath(Path.Combine(uploadDir, request.FileName));

            if (!filePath.StartsWith(uploadDir))
            {
                return Task.FromResult(new UploadFileResponse { Message = "Invalid file path" });
            }

            byte[] fileBytes = Convert.FromBase64String(request.Bytes);
            
            File.WriteAllBytes(filePath, fileBytes);
            
            return Task.FromResult(new UploadFileResponse { Message = "File uploaded successfully" });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new UploadFileResponse { Message = $"Error uploading file: {ex.Message}" });
        }
    }

    public override Task<DownloadFileResponse> DownloadFile(DownloadFileRequest request, ServerCallContext context)
    {
        try
        {
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var filePath = Path.GetFullPath(Path.Combine(uploadDir, request.FileName));

            // Security check
            if (!filePath.StartsWith(uploadDir))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid file path"));
            }

            if (!File.Exists(filePath))
            {
                throw new RpcException(new Status(StatusCode.NotFound, "File not found"));
            }

            // Read the file
            byte[] fileBytes = File.ReadAllBytes(filePath);
            string base64Content = Convert.ToBase64String(fileBytes);

            // Get file type from extension
            string fileType = Path.GetExtension(request.FileName).TrimStart('.').ToLower();

            return Task.FromResult(new DownloadFileResponse 
            { 
                Bytes = base64Content,
                FileName = request.FileName,
                FileType = fileType
            });
        }
        catch (RpcException)
        {
            throw; // Rethrow RPC exceptions as is
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, $"Error downloading file: {ex.Message}"));
        }
    }
}
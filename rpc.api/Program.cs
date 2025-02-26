using Microsoft.AspNetCore.Mvc;
using rpc.sdk;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterService();
builder.Services.AddAntiforgery(options => 
{
    options.HeaderName = "X-XSRF-TOKEN";
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.UseSwagger();
app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

app.MapGet("/SayHelloJohn", async (RpcClient rpc) =>
    {
        var result = await rpc.SayHelloAsync("John");
        return result;
    });

app.MapPost("/upload", async ([FromForm] FileUpload request, RpcClient rpc) =>
    {
        if (request.File == null)
        {
            return Results.BadRequest("No file provided");
        }

        try
        {
            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);
            var fileBytes = Convert.ToBase64String(ms.ToArray());

            var response = await rpc.UploadFileAsync(fileBytes, request.File.FileName, request.FileType.ToString().ToLower());

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error uploading file: {ex.Message}");
        }
    })
    .DisableAntiforgery();

app.MapGet("/download/{fileName}", async (string fileName, RpcClient rpc) =>
    {
        try
        {
            var response = await rpc.DownloadFileAsync(fileName);

            var fileBytes = Convert.FromBase64String(response.Bytes);
            var contentType = GetContentType(response.FileType);
            
            return Results.File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error downloading file: {ex.Message}");
        }
    });

app.Run();

string GetContentType(string fileType)
{
    return fileType.ToLower() switch
    {
        "txt" => "text/plain",
        "pdf" => "application/pdf",
        "doc" => "application/msword",
        "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "xls" => "application/vnd.ms-excel",
        "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "png" => "image/png",
        "jpg" => "image/jpeg",
        "jpeg" => "image/jpeg",
        "gif" => "image/gif",
        "csv" => "text/csv",
        _ => "application/octet-stream"
    };
}

public class FileUpload
{
    public FileType FileType { get; set; }
    public required IFormFile File { get; set; }
}

public enum FileType
{
    Image,
    Video,
    Audio,
    Document
}
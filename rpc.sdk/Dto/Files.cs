
namespace rpc.sdk.Dto;

public class DownloadFileRequestDto
{
    public string FileName { get; set; }
}

public class DownloadFileResponseDto
{
    public string Bytes { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
}

public class UploadFileRequestDto
{
    public string Bytes { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
}

public class UploadFileResponseDto
{
    public string Message { get; set; }
}

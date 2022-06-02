

namespace Application.Common.FileStorage;

public class FileUploadRequest
{
    //默认值不能为空
    public string Name { get; set; } = default!;
    //Image Extension 
    public string Extension { get; set; } = default!;
    //Image Data
    public string Data { get; set; } = default!;
}

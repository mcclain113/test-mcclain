using Microsoft.AspNetCore.Http;    // for IFormFile
public class CreateDocumentDto
{
    public int FileIndex { get; set; }       // for correlating hidden file-input
    public IFormFile File { get; set; }
    public byte DocumentTypeID { get; set; }

#nullable enable
    public string? DocumentDescription { get; set; }
    public string? Notes { get; set; }
#nullable disable
    
}
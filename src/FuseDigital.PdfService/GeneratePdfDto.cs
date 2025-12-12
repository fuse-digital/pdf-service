using System.ComponentModel.DataAnnotations;

public class GeneratePdfDto
{
    [Required]
    public required string FileName { get; set; }
    
    [Required]
    public required string ContentHtml { get; set; }
}
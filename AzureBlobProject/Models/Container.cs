
using System.ComponentModel.DataAnnotations;

namespace AzureBlobProject.Models;
public class Container
{
    [Required]
    public string Name {  get; set; }   
}

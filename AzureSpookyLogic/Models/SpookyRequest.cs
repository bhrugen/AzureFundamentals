
using System.ComponentModel.DataAnnotations;

namespace AzureSpookyLogic.Models;
public class SpookyRequest
{
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
}

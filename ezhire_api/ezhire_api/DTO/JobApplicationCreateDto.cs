using System.ComponentModel.DataAnnotations;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public class JobApplicationCreateDto
{
    [Required] public DateTime DateApplied { get; set; }

    [Required]
    [EnumDataType(typeof(ApplicationStatus))]
    public ApplicationStatus Status { get; set; }


    [Required] public int PostingId { get; set; }

    [Required] public int ApplicantId { get; set; }
}
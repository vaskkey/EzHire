using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

public enum RecruiterType
{
    IN_HOUSE,
    EXTERNAL
}

public class Recruiter : User
{
    [Required]
    [Column("type")]
    [EnumDataType(typeof(RecruiterType))]
    public RecruiterType Type { get; set; }
}
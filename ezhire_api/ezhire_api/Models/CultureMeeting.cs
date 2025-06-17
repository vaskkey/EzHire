using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

public class CultureMeeting : RecruitmentStage
{
    [MinLength(1)] [Column("values")] public ICollection<string> Values { get; set; }
}
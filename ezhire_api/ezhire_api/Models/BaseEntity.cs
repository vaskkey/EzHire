using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

/**
 * Defines DB maintenance columns for easier management
 */
public class BaseEntity
{
    [Column("created_at")] public DateTime CreatedAt { get; set; }

    [Column("updated_at")] public DateTime UpdatedAt { get; set; }
}
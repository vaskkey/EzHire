using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

/**
 * Defines DB maintenance columns for easier management
 */
public abstract class BaseEntity
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; }

    [Column("updated_at")] public DateTime UpdatedAt { get; set; }
}
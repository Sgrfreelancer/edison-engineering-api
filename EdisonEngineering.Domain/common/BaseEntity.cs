namespace EdisonEngineering.Domain.Common;

public abstract class BaseEntity
{
    // =====================================================
    // PRIMARY KEY
    // =====================================================

    public int Id { get; set; }

    // =====================================================
    // AUDIT FIELDS
    // =====================================================

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    // =====================================================
    // AUDIT USERS
    // =====================================================

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? DeletedBy { get; set; }

    // =====================================================
    // SOFT DELETE
    // =====================================================

    public bool IsDeleted { get; set; }
}
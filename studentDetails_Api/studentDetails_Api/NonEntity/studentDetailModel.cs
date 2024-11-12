using System.ComponentModel.DataAnnotations;

public partial class studentDetailModel
{
    [Key]
    public long studentID { get; set; }

    [Required]
    [StringLength(50)] // Maximum length for first name
    public string firstName { get; set; } = null!;

    [Required]
    [StringLength(50)] // Maximum length for last name
    public string lastName { get; set; } = null!;

    [Required]
    public DateOnly dateOfBirth { get; set; }

    [Required]
    [StringLength(10)] // Gender can be a small value like 'Male', 'Female'
    public string gender { get; set; } = null!;

    [Required]
    [EmailAddress] // Validates email format
    public string email { get; set; } = null!;

    [Required]
    public long mobileNumber { get; set; }

    public DateTime? createdOn { get; set; }

    public long? createdBy { get; set; }

    public DateTime? modifiedOn { get; set; }

    public long? modifiedBy { get; set; }

    [Required]
    [StringLength(255)] // Adjust based on expected password length
    public string studentPassword { get; set; } = null!;

    [Required]
    public int studentstatus { get; set; }
}

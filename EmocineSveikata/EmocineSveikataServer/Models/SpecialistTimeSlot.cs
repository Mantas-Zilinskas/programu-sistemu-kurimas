using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmocineSveikataServer.Models
{
    public class SpecialistTimeSlot
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        public bool IsBooked { get; set; } = false;
        
        public int? BookedByUserId { get; set; }
        
        [ForeignKey("BookedByUserId")]
        public User? BookedByUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

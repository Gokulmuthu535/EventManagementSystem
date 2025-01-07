using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class Booking
    {
        
            public int BookingID { get; set; }

            [Required]
            [StringLength(100)]
            public string CustomerName { get; set; }

            [Required]
            [StringLength(15)]
            public string ContactNumber { get; set; }

            [EmailAddress]
            [StringLength(100)]
            public string Email { get; set; }

            [Required]
            public int NumberOfTickets { get; set; }

            [Required]
            [Range(0, double.MaxValue, ErrorMessage = "Total amount must be a positive number")]
            public decimal TotalAmount { get; set; }

            public DateTime BookingDate { get; set; } = DateTime.Now;

            [Required]
            public int EventID { get; set; }

            [Required]
            public int UserID { get; set; }


        public bool IsConfirmed { get; set; }
    }
}

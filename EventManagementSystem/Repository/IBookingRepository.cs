using EventManagementSystem.Models;

namespace EventManagementSystem.Repository
{
    public interface IBookingRepository
    {
        Task AddBooking(Booking newBooking);
        Task<List<Booking>> GetBookingDetailsByUserId(int userId);
        Task<List<Booking>> GetAllBookings();
        Task UpdateBookingConfirmation(int bookingId, bool isConfirmed);
    }

    
}

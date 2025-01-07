using EventManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EventManagementSystem.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly string _connectionString;

        public BookingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task AddBooking(Booking newBooking)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("CreateBooking", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerName", newBooking.CustomerName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ContactNumber", newBooking.ContactNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", newBooking.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@NumberOfTickets", newBooking.NumberOfTickets);
                    command.Parameters.AddWithValue("@TotalAmount", newBooking.TotalAmount);
                    command.Parameters.AddWithValue("@EventID", newBooking.EventID);
                    command.Parameters.AddWithValue("@UserID", newBooking.UserID);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Booking>> GetBookingDetailsByUserId(int userId)
        {
            var bookings = new List<Booking>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetBookingDetailsByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var booking = new Booking
                            {
                                BookingID = reader.GetInt32(reader.GetOrdinal("BookingID")),
                                CustomerName = reader["CustomerName"] as string,
                                ContactNumber = reader["ContactNumber"] as string,
                                Email = reader["Email"] as string,
                                NumberOfTickets = reader.GetInt32(reader.GetOrdinal("NumberOfTickets")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                EventID = reader.GetInt32(reader.GetOrdinal("EventID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
                                IsConfirmed = reader.GetBoolean(reader.GetOrdinal("IsConfirmed"))

                            };

                            bookings.Add(booking);
                        }
                    }
                }
            }

            return bookings;
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            var bookings = new List<Booking>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetAllBookings", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var booking = new Booking
                            {
                                BookingID = reader.GetInt32(reader.GetOrdinal("BookingID")),
                                CustomerName = reader["CustomerName"] as string,
                                ContactNumber = reader["ContactNumber"] as string,
                                Email = reader["Email"] as string,
                                NumberOfTickets = reader.GetInt32(reader.GetOrdinal("NumberOfTickets")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                EventID = reader.GetInt32(reader.GetOrdinal("EventID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
                                IsConfirmed = reader.GetBoolean(reader.GetOrdinal("IsConfirmed"))
                            };

                            bookings.Add(booking);
                        }
                    }
                }
            }

            return bookings;
        }   

        public async Task UpdateBookingConfirmation(int bookingId, bool isConfirmed)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("UpdateBookingConfirmation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookingID", bookingId);
                    command.Parameters.AddWithValue("@IsConfirmed", isConfirmed);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }


    }
}

using EventManagementSystem.Models;
using EventManagementSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly IEventRepository _eventRepository;

        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;

        public BookingController(IEventRepository eventRepository, IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;

        }

        private async Task<User> GetLoggedInUserDetails()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return null; // Return null if the session doesn't have a username
            }

            return await _userRepository.GetUserByUsernameAsync(username);
        }


        [HttpGet]
        public async Task<ActionResult> AddBooking(string eventName)
        {
            var userDetail = await GetLoggedInUserDetails();
            if (userDetail == null)
            {
                // Redirect to login if user details are not found
                return RedirectToAction("SignIn", "Home");
            }

            var eventDetail = await _eventRepository.GetEventByName(eventName);
            ViewBag.EventID = eventDetail.EventID;
            ViewBag.UserID = userDetail.UserId;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddBooking(Booking newBooking)
        {
            if (ModelState.IsValid)
            {
                await _bookingRepository.AddBooking(newBooking);
                TempData["SuccessMessage"] = "Booking added successfully!";
                return RedirectToAction("UserHomepage", "User");
            }
            return View(newBooking);
        }

        public async Task<IActionResult> UserBookings()
        {
            var userDetail = await GetLoggedInUserDetails();
            if (userDetail == null)
            {
                // Redirect to login if user details are not found
                return RedirectToAction("SignIn", "Home");
            }
            var bookings = await _bookingRepository.GetBookingDetailsByUserId(userDetail.UserId);

            return View(bookings); // Pass bookings to the view
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAllBookings();
            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int bookingId)
        {
            await _bookingRepository.UpdateBookingConfirmation(bookingId, true);
            TempData["SuccessMessage"] = "Booking confirmed successfully!";
            return RedirectToAction("GetAllBookings");
        }


    }
}

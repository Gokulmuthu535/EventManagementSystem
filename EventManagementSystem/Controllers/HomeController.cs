using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Azure.Core;
using EventManagementSystem.Models;
using EventManagementSystem.Repositories;
using EventManagementSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventrepository;

        public HomeController(IUserRepository userRepository, IEventRepository eventrepository)
        {
            _userRepository = userRepository;
            _eventrepository = eventrepository;
        }

        // Home Page
        public async Task<IActionResult> Home()
        {
            var events = await _eventrepository.GetAllEvents();
            return View(events);
        }



        // About Us
        public IActionResult AboutUs()
        {
            return View();
        }

        // Sign In
        [HttpGet]
        public IActionResult SignIn()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request); // Show validation messages.
            }

            var user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null || ComputeSha256Hash(request.Password) != user.Password)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(request); // Stay on the same page.
            }

            TempData["WelcomeMessage"] = $"Welcome, {user.Username}!";
            HttpContext.Session.SetString("Username", user.Username);

            if (user.Username == "Admin")
            {
                return RedirectToAction("AdminHomePage", "Event");
            }
            return RedirectToAction("UserHomePage", "User");
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }

        // Sign Up
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            if (!ModelState.IsValid)
                return View(user); // Return the view with validation errors.

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View(user);
            }

            if (!Regex.IsMatch(user.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                ModelState.AddModelError(string.Empty, "Password must be at least 8 characters long, include at least one uppercase letter, one lowercase letter, and one number.");
                return View(user);
            }

            var passwordHash = ComputeSha256Hash(user.Password);

            try
            {
                await _userRepository.AddUserAsync(user, passwordHash);
                TempData["SuccessMessage"] = "Registration successful! You can now login.";
                return RedirectToAction("SignIn", "Home");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }


        // Contact Us
        public IActionResult ContactUs()
        {
            return View();
        }
        // Logout
        [HttpPost]
        public IActionResult Signout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Optionally, clear cookies if authentication cookies are used
            HttpContext.Response.Cookies.Delete("AuthToken");

            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("SignIn", "Home");
        }

        // About Us
        public IActionResult PhotoGallery()
        {
            return View();



        }

    }
}

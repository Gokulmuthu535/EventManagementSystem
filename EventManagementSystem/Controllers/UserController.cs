using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Models;
using EventManagementSystem.Repositories;
using System.Security.Cryptography;
using System.Text;
using EventManagementSystem.Repository;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        public UserController(IUserRepository userRepository, IEventRepository eventRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        // Home Page
        [Route("homepage")]
        public async Task<IActionResult> UserHomepage()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            var events = await _eventRepository.GetAllEvents();
            return View(events);
        }

        //Users
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return View(users);
        }

    }
}

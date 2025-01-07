using EventManagementSystem.Models;
using EventManagementSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class EventController : Controller
{
    private readonly IEventRepository _eventRepository;

    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;

    public EventController(IEventRepository eventRepository, IBookingRepository bookingRepository, IUserRepository userRepository)
    {
        _eventRepository = eventRepository;
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;

    }

    // Home Page
    public IActionResult AdminHomepage()
    {
        ViewBag.SuccessMessage = TempData["SuccessMessage"];
        return View();
    }
    public async Task<ActionResult> ListEvents()
    {
        var events = await _eventRepository.GetAllEvents();
        return View(events);
    }

    public async Task<ActionResult> GetEvent(int id)
    {
        var eventDetail = await _eventRepository.GetEventById(id);
        if (eventDetail == null) return NotFound();

        return View(eventDetail);
    }

    public async Task<ActionResult> UpdateEvent(int id)
    {
        var eventToUpdate = await _eventRepository.GetEventById(id);
        if (eventToUpdate == null) return NotFound();

        return View(eventToUpdate);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateEvent(Event updatedEvent)
    {
        if (ModelState.IsValid)
        {
            await _eventRepository.UpdateEvent(updatedEvent);
            TempData["SuccessMessage"] = "Event updated successfully!";
            return RedirectToAction("ListEvents");
        }
        return View(updatedEvent);
    }

    [HttpGet]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        var eventToDelete = await _eventRepository.GetEventById(id);
        if (eventToDelete == null) return NotFound();

        return View(eventToDelete);
    }


    [HttpPost]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var eventToDelete = await _eventRepository.GetEventById(id);
        if (eventToDelete == null)
            return NotFound();

        await _eventRepository.DeleteEvent(id);  // Delete the event from the repository
        TempData["SuccessMessage"] = "Event deleted successfully!";
        return RedirectToAction("ListEvents");  // Redirect to the event list
    }

    [HttpGet]
    public ActionResult AddEvent()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> AddEvent(Event newEvent)
    {
        if (ModelState.IsValid)
        {
            await _eventRepository.AddEvent(newEvent);
            TempData["SuccessMessage"] = "Event added successfully!";
            return RedirectToAction("ListEvents");
        }
        return View(newEvent);
    }
      

    

}

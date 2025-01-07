using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Models;

namespace EventManagementSystem.Repository
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllEvents();
        Task<Event> GetEventById(int eventId);
        Task AddEvent(Event newEvent);
        Task UpdateEvent(Event updatedEvent);
        Task DeleteEvent(int eventId);
        Task<Event> GetEventByName(string eventName);
    }
}

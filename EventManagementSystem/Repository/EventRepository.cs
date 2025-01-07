using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using EventManagementSystem.Models;

namespace EventManagementSystem.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly string _connectionString;

        public EventRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Event>> GetAllEvents()
        {
            var events = new List<Event>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetEvent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var eventItem = new Event
                            {
                                EventID = Convert.ToInt32(reader["event_id"]),
                                Name = reader["name"]?.ToString(),
                                Date = reader["date"] != DBNull.Value ? Convert.ToDateTime(reader["date"]) : (DateTime?)null,
                                Location = reader["location"]?.ToString(),
                                Description = reader["description"]?.ToString(),
                                ImageURL = reader["image_url"]?.ToString(),
                                EventType = reader["event_type"]?.ToString()
                            };
                            events.Add(eventItem);
                        }
                    }
                }
            }

            return events;
        }

        public async Task<Event> GetEventById(int eventId)
        {
            Event eventItem = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetEvent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventID", eventId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            eventItem = new Event
                            {
                                EventID = Convert.ToInt32(reader["event_id"]),
                                Name = reader["name"]?.ToString(),
                                Date = reader["date"] != DBNull.Value ? Convert.ToDateTime(reader["date"]) : (DateTime?)null,
                                Location = reader["location"]?.ToString(),
                                Description = reader["description"]?.ToString(),
                                ImageURL = reader["image_url"]?.ToString(),
                                EventType = reader["event_type"]?.ToString()
                            };
                        }
                    }
                }
            }

            return eventItem;
        }

        public async Task AddEvent(Event newEvent)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("AddEvent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", newEvent.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Date", newEvent.Date ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Location", newEvent.Location ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Description", newEvent.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ImageURL", newEvent.ImageURL ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EventType", newEvent.EventType ?? (object)DBNull.Value);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateEvent(Event updatedEvent)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("UpdateEvent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventID", updatedEvent.EventID);
                    command.Parameters.AddWithValue("@Name", updatedEvent.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Date", updatedEvent.Date ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Location", updatedEvent.Location ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Description", updatedEvent.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ImageURL", updatedEvent.ImageURL ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EventType", updatedEvent.EventType ?? (object)DBNull.Value);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteEvent(int eventId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("DeleteEvent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventID", eventId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Event> GetEventByName(string eventName)
        {
            Event eventItem = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetEventByName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventName", eventName);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            eventItem = new Event
                            {
                                EventID = Convert.ToInt32(reader["event_id"]),
                                Name = reader["name"]?.ToString(),
                                Date = reader["date"] != DBNull.Value ? Convert.ToDateTime(reader["date"]) : (DateTime?)null,
                                Location = reader["location"]?.ToString(),
                                Description = reader["description"]?.ToString(),
                                ImageURL = reader["image_url"]?.ToString(),
                                EventType = reader["event_type"]?.ToString()
                            };
                        }
                    }
                }
            }

            return eventItem;
        }

    }
}

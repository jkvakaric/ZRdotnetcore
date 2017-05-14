using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZRdotnetcore.Data;
using ZRdotnetcore.Models;
using ZRdotnetcore.Repos.Interfaces;

namespace ZRdotnetcore.Repos
{
    public class ReadingsRepo : IReadingsRepo
    {
        private readonly YoctoDbContext _context;
        public ReadingsRepo(YoctoDbContext context)
        {
            _context = context;
        }

        public void AddActiveReading(ActiveReading reading)
        {
            _context.ActiveReadings.Add(reading);
            _context.SaveChanges();
        }

        public bool CheckReadingNameAlreadyExists(string name, string userId)
        {
            bool existsar = _context.ActiveReadings.Any(ar => userId.Equals(ar.Owner.UserId) && name.Equals(ar.Name));
            bool existsr = _context.Readings.Any(r => userId.Equals(r.Owner.UserId) && name.Equals(r.Name));
            return existsar || existsr;
        }

        public ActiveReading GetActiveReading(string readingId)
        {
            ActiveReading reading = _context.ActiveReadings
                .Include(ar => ar.ReadingType)
                .Include(ar => ar.Device)
                    .ThenInclude(d => d.User)
                .SingleOrDefault(ar => readingId.Equals(ar.Id));
            return reading;
        }

        public void DeleteActive(ActiveReading reading)
        {
            _context.ActiveReadings.Remove(reading);
            _context.SaveChanges();
        }

        public ActiveReading GetActiveReadingWithReadings(string readingId)
        {
            ActiveReading reading = _context.ActiveReadings
                .Include(ar => ar.Readings)
                .Include(ar => ar.Device)
                    .ThenInclude(d => d.User)
                .SingleOrDefault(ar => readingId.Equals(ar.Id));
            return reading;
        }

        public List<Reading> GetAllFromUser(string userId)
        {
            return _context.Readings
                .Include(r => r.Device)
                .Include(r => r.ReadingType)
                .Where(r => userId.Equals(r.Owner.UserId))
                .ToList();
        }

        public List<ReadingType> GetReadingTypesAll()
        {
            var list = _context.ReadingTypes.ToList();
            return list;
        }

        public ReadingType GetReadingType(string readingTypeId)
        {
            ReadingType type = _context.ReadingTypes.Find(readingTypeId);
            return type;
        }

        public void Delete(Reading reading)
        {
            _context.Readings.Remove(reading);
            _context.SaveChanges();
        }

        public Reading GetReading(string readingId)
        {
            Reading reading = _context.Readings
                .Include(r => r.ReadingType)
                .Include(r => r.Owner)
                .SingleOrDefault(r => readingId.Equals(r.Id));
            return reading;
        }
    }
}
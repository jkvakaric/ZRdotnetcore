using System.Collections.Generic;
using ZRdotnetcore.Models;

namespace ZRdotnetcore.Repos.Interfaces
{
    public interface IReadingsRepo
    {
        void AddActiveReading(ActiveReading reading);
        bool CheckReadingNameAlreadyExists(string name, string userId);
        ActiveReading GetActiveReading(string readingId);
        void DeleteActive(ActiveReading reading);
        ActiveReading GetActiveReadingWithReadings(string readingId);
        List<Reading> GetAllFromUser(string userId);
        List<ReadingType> GetReadingTypesAll();
        ReadingType GetReadingType(string readingTypeId);
        void Delete(Reading reading);
        Reading GetReading(string readingId);
        List<Reading> GetReadingsByName(string readingName);
        void DeleteByName(string readingName);
    }
}
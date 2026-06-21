using user.Data;

namespace user.Extensions
{
    public static class PersonExtensions
    {
        public static bool IsRemoved(this AppDbContext db, int personId)
        {
            return db.Person_Removed
                .Any(x => x.PersonId == personId);
        }
    }
}
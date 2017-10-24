using DoutrinaAgil.Data.Datacontext;

namespace DoutrinaAgil.Data.Repository
{
    public class BaseRepository
    {
        public static DoutrinaAgilEntities GetDbContext()
        {
            var context = new DoutrinaAgilEntities();
            context.Configuration.ProxyCreationEnabled = false;
            context.Configuration.LazyLoadingEnabled = true;

            return context;
        }
    }
}

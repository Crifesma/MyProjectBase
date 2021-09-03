using GAE.AIQ.API.Data;

namespace API.Data
{
    public interface IDataBaseFactory
    {
        ApplicationDbContext GetContext();
    }
}

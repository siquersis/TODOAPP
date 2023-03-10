using API.Data.Requests;

namespace API.Data.Interfaces
{
    public interface ITodoAppRepository
    {
        void Add(TodoAppRequest todo);
        void GetBy(string todo);
    }
}

using API.Data.Interfaces;
using API.Data.Requests;
using API.Domain;

namespace API.Data.Repositories
{
    public class TodoAppRepository : ITodoAppRepository
    {
        public TodoAppRepository()
        {
        }

        public List<TodoAppRequest> Todos { get; set; }

        public void Add(TodoAppRequest todo)
        {
            Todos ??= new List<TodoAppRequest>();
            Todos.Add(todo);
        }

        public void GetBy(string todo)
            => Todos.First(x => x.Descricao == todo);
    }
}

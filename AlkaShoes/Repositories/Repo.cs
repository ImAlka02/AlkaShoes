using AlkaShoes.Models.Entities;

namespace AlkaShoes.Repositories
{
    public class Repo<T> where T : class
    {
        public AlkashoesContext Context { get; }
        public Repo(AlkashoesContext context)
        {
            Context = context;
        }
        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public virtual T? Get(object id)
        {
            return Context.Find<T>(id);
        }

        public virtual void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }
    }
}

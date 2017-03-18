using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notes.Web.Models;
using System.Linq.Expressions;

namespace Notes.Web.Data.Repositories
{
    public class NotebookRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<Notebook> _dbSet;

        public NotebookRepository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Notebooks;
        }

        public async Task<ICollection<Notebook>> List(ApplicationUser user, string order = "UpdatedAt", string direction = "DESC")
        {
            var query = _dbSet.Where(n => n.ApplicationUser == user).Include(n => n.Notes)
                .OrderQuery(order, direction);

            return await query.ToListAsync();
        }
    }

    public static class NotebookDbSetExtensions
    {
        // http://stackoverflow.com/questions/21743393/cannot-convert-linq-iorderedenumerablet-to-linq-iqueryablet
        public static IQueryable<Notebook> OrderQuery(this IQueryable<Notebook> notebooks, string order, string direction)
        {
            switch(order)
            {
                case "UpdatedAt":
                    return SwitchOrder(notebooks, direction, n => n.UpdatedAt);
                case "Name":
                    return SwitchOrder(notebooks, direction, n => n.Name);
                case "Description":
                    return SwitchOrder(notebooks, direction, n => n.Description);
                case "CreatedAt":
                    return SwitchOrder(notebooks, direction, n => n.CreatedAt);
                default:
                    return notebooks.OrderByDescending(n => n.UpdatedAt);
            }
        }

        private static IQueryable<Notebook> SwitchOrder<TKey>(IQueryable<Notebook> notebooks, string direction, Expression<Func<Notebook, TKey>> property)
        {
            if (direction == "DESC")
                return notebooks.OrderByDescending(property);
            else
                return notebooks.OrderBy(property);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notes.Web.Data
{
    public class RepoHelpers
    {
        public static IQueryable<TEntity> SwitchOrder<TEntity, TProperty>(IQueryable<TEntity> entity, string direction,
            Expression<Func<TEntity, TProperty>> property)
        {
            if (direction == "DESC")
                return entity.OrderByDescending(property);
            else
                return entity.OrderBy(property);
        }
    }

    public class ListOptions
    {
        public ListOptions()
        { }

        public ListOptions(string order, string direction, int take, int skip)
        {
            Order = order;
            OrderDirection = direction;
            TakeCount = take;
            SkipCount = skip;
        }

        public string Order { get; set; }
        public string OrderDirection { get; set; }
        public int TakeCount { get; set; }
        public int SkipCount { get; set; }
    }
}

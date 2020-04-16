using CFFA_API.Models;
using CFFA_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static CFFA_API.Logic.Helpers.LinqPaginationExtension;

namespace CFFA_API.Repository.Implementations
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(DbContext context) : base(context) { }
        public ApplicationDbContext AppContext { get { return Context as ApplicationDbContext; } }

        public void Add(string TagName)
        {
            Context.Set<Tag>().Add(new Tag { Name = TagName });
            Context.SaveChanges();
        }

        public IQueryable<Tag> GetLikeTags(string like, int pageIndex, int pageSize)
        {
            return Context.Set<Tag>().Where(p => p.Name.Contains(like)).Page(pageIndex, pageSize)
                                        .AsNoTracking();
        }
    }
}

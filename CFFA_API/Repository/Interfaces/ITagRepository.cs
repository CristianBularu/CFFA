using CFFA_API.Models;
using System.Linq;

namespace CFFA_API.Repository.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        IQueryable<Tag> GetLikeTags(string like, int pageIndex, int pageSize);
        void Add(string TagName);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models
{
    public class Sketch
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        //public string Title { get; set; }
        public string Extension { get; set; }
        //public int PageCount { get; set; }
        //public float PageHeight { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models.ViewModels
{
    public class UnderPostViewModel
    {
        public int UpVotes { get; set; }

        public int DownVotes { get; set; }

        public int Comments { get; set; }

        public bool? Favorite { get; set; }

        public bool? ViewerVote { get; set; }

        public long Id { get; set; }
    }
}

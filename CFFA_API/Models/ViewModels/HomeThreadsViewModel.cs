using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models.ViewModels
{
    public class HomeThreadsViewModel
    {

        public List<PostOnlyViewModel> Popular { get; set; }
        public List<PostOnlyViewModel> Fresh { get; set; }
        public List<PostOnlyViewModel> Feed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API.Models
{
    public enum CustomTokenState
    {
        Valid,
        Invalid,
        Expired,
        SelfDestruct,
        NotCreated
    }

    public enum TokenType
    {
        Confirmation,
        Reset,
        Refresh
    }

    public class EnumStates
    {
    }
}

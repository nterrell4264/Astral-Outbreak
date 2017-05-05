using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    //Does the thing a boss needs to do
    public interface Boss
    {
        float Health { get; set; }
        float MaxHealth { get; set; }

    }
}

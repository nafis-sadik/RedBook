using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Clients
{
    public static class ClientStaticData
    {
        public static string BackendUrl { get { return "http://localhost:5062/"; } private set { } }
    }
}

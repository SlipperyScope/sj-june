using Remaster.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Utilities
{
    public interface IPrintable
    {
        public List<PrintBlock> PrintBlocks { get; }
    }
}

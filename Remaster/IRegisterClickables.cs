using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster
{
    /// <summary>
    /// Registers clickables
    /// </summary>
    public interface IRegisterClickables
    {
        /// <summary>
        /// Register a clickable
        /// </summary>
        /// <param name="clickable">Clickable to register</param>
        public void Register(Clickable clickable);
    }
}

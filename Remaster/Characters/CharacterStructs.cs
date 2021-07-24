using Remaster.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Characters
{
    public struct CharacterResponse : IPrintable
    {
        public List<PrintBlock> Blocks;
        
        /// <summary>
        /// True if the character is willing to exchange the item
        /// </summary>
        public Boolean Willing;

        /// <summary>
        /// Creates a character response from printblock list
        /// </summary>
        public CharacterResponse(Boolean desiresItem, List<PrintBlock> blocks)
        {
            Willing = desiresItem;
            Blocks = blocks ?? new List<PrintBlock>();
        }

        /// <summary>
        /// Creates a character response from one or more printblocks
        /// </summary>
        public CharacterResponse(Boolean desiresItem, PrintBlock block, params PrintBlock[] args)
        {
            Willing = desiresItem;
            Blocks = args.Prepend(block).ToList();
        }

        /// <summary>
        /// Adds a new print block
        /// </summary>
        public void Add(PrintBlock block) => Blocks.Add(block);

        #region IPrintable
        public List<PrintBlock> PrintBlocks => Blocks;
        #endregion
    }
}

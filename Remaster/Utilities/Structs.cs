﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Utilities
{
    /// <summary>
    /// Block of printable data
    /// </summary>
    public struct PrintBlock
    {
        public PrintToken Token;
        public String Text;

        /// <summary>
        /// Creates a block of printable data
        /// </summary>
        /// <param name="text">
        /// <para>Text: String of text
        /// <br />Pause: Length of pause
        /// <br />Image: Path to texture resource
        /// <br />Clear: N/A
        /// </para></param>
        /// <param name="token">Block type</param>
        public PrintBlock(String text, PrintToken token = PrintToken.Text)
        {
            Token = token;
            Text = text;
        }

        public PrintBlock(PrintToken token, String text = "")
        {
            Token = token;
            Text = text;
        }

        public static implicit operator PrintBlock(String s) => new PrintBlock(s);
        public static implicit operator PrintBlock((String Text, PrintToken Token) block) => new PrintBlock(block.Text, block.Token);
        public static implicit operator PrintBlock((PrintToken Token, String Text) block) => new PrintBlock(block.Text, block.Token);
    }
}

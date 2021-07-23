using Godot;
using Remaster.Items;
using Remaster.Utilities;
using System.Collections.Generic;
using System;
using rItem = Remaster.Items.Item;

namespace Remaster.HUD
{
    public class SubConsole : Clickable
    {
        private const String TextBoxPath = "Textbox";
        private TextboxPrinter TextBox;

        /// <summary>
        /// Ready
        /// </summary>
        protected override void OnReady()
        {
            TextBox = GetNode<TextboxPrinter>(TextBoxPath);
        }

        public void Print(object obj) => TextBox.GoBrrr(obj switch
        {
            rItem item => item.Description(nameof(SubConsole)).PrintBlocks,
            IPrintable printable => printable.PrintBlocks,
            _ => new List<PrintBlock> { obj.ToString() }
        });

        public override List<PrintBlock> PrintBlocks => new List<PrintBlock>()
        {
            new PrintBlock("Computer console:\n"),
            new PrintBlock(PrintToken.Pause, "0.2"),
            new PrintBlock("The latest and greatest!")
        };
    }
}


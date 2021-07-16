using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Remaster.Items;

namespace Remaster.Utilities
{
    /// <summary>
    /// Prints text to a textbox
    /// </summary>
    public class TextboxPrinter : RichTextLabel
    {
        readonly char[] WordSperators = { ' ', '\n' };

        /// <summary>
        /// Method of printing
        /// <para/>
        /// <p>  All:       Prints entire text, ignoring pauses
        /// <br/>Word:      Prints one word at a time
        /// <br/>Character: Prints one character at a time
        /// </p>
        /// </summary>
        [Export]
        public PrintMethod Method { get; set; } = PrintMethod.Character;

        /// <summary>
        /// Length of pause after a print unit is printed
        /// </summary>
        [Export]
        public Single PrintSpeed { get; set; }

        private Single PrintSpeedOffset = 0;

        private Timer PrintTimer;
        private Timer PauseTimer;

        private List<PrintBlock> Meal;

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            AddChild(PrintTimer = new Timer());
            PrintTimer.OneShot = true;
            PrintTimer.WaitTime = PrintSpeed;
            PrintTimer.Connect("timeout", this, nameof(PrintNext));

            AddChild(PauseTimer = new Timer());
            PauseTimer.OneShot = true;
            PauseTimer.Connect("timeout", this, nameof(PrintNext));

            Text = String.Empty;
        }

        /// <summary>
        /// Start printing printblocks
        /// </summary>
        public void GoBrrr(List<PrintBlock> blocks)
        {
            if (blocks?.Count > 0)
            {
                Meal = blocks.Select(b => b).ToList();
                Text += '\n';
                PrintTimer.Stop();
                PauseTimer.Stop();
                PrintNext();
            }
        }

        /// <summary>
        /// Start printing printblocks
        /// </summary>
        public void GoBrrr(PrintBlock block, params PrintBlock[] args) => GoBrrr(args.Prepend(block).ToList());

        /// <summary>
        /// Start printing printable
        /// </summary>
        /// <param name="printable">Printable to print</param>
        public void GoBrrr(IPrintable printable) => GoBrrr(printable.PrintBlocks);

        /// <summary>
        /// Prints the next thing, taking into account method and type
        /// </summary>
        private void PrintNext()
        {
            if (Meal?.Count > 0)
            {
                var snacc = Meal[0];
                switch (snacc.Token)
                {
                    case PrintToken.Text when snacc.Text.Length == 0:
                        Meal.RemoveAt(0);
                        PrintNext();
                        break;

                    case PrintToken.Text when Method == PrintMethod.All:
                        Text += snacc.Text;
                        Meal.RemoveAt(0);
                        PrintNext();
                        break;

                    case PrintToken.Text when Method == PrintMethod.Section:
                        Text += snacc.Text;
                        Meal.RemoveAt(0);
                        PrintTimer.Start();
                        break;

                    case PrintToken.Text when Method == PrintMethod.Word:
                        var separator = snacc.Text.IndexOfAny(WordSperators);
                        if (separator == -1) // No more seperators
                        {
                            Text += snacc.Text;
                            Meal.RemoveAt(0);
                            PrintTimer.Start();
                        }
                        else if (separator == 0) // First item is seperator
                        {
                            Text += snacc.Text[0];
                            snacc.Text = snacc.Text.Remove(0, 1);
                            Meal[0] = snacc;
                            PrintNext();
                        }
                        else
                        {
                            Text += snacc.Text.Substring(0, separator);
                            snacc.Text = snacc.Text.Remove(0, separator);
                            Meal[0] = snacc;
                            PrintTimer.Start();
                        }
                        break;

                    case PrintToken.Text when Method == PrintMethod.Character:
                        Text += snacc.Text[0];
                        snacc.Text = snacc.Text.Remove(0, 1);
                        Meal[0] = snacc;
                        PrintTimer.Start();
                        break;

                    case PrintToken.Pause when Method != PrintMethod.All:
                        PauseTimer.WaitTime = snacc.Text.ToFloat();
                        Meal.RemoveAt(0);
                        PauseTimer.Start();
                        break;

                    case PrintToken.Pause:
                        Meal.RemoveAt(0);
                        PrintNext();
                        break;

                    case PrintToken.Image when Method != PrintMethod.All:
                            PrintTimer.Start();
                        break;

                    case PrintToken.Image:
                            Meal.RemoveAt(0);
                            PrintNext();
                        break;

                    case PrintToken.Clear:
                        // Clear image?
                        Text = "\n";
                        Meal.RemoveAt(0);
                        PrintNext();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}

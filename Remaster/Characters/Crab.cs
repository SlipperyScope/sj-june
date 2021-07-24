using Remaster.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remaster.Items;
using rItem = Remaster.Items.Item;

namespace Remaster.Characters
{
    public class Crab : Clickable, IHostItems
    {
        private List<rItem> Inventory = new List<rItem>();
        private List<String> History = new List<String>();
        private Int32 EquippedIndex = 0;
        private rItem EquippedItem => Inventory[EquippedIndex];

        protected override void OnEnterTree()
        {
            Inventory.Add(new NoneItem());
        }

        #region IHostItems
        public rItem RequestItem()
        {
            throw new NotImplementedException();
        }
        public Boolean GiveItem(rItem item)
        {
            if (item is Pipe && Inventory.All(item => item is Pipe is false))
            {
                History.Add($"{nameof(GiveItem)}{nameof(Pipe)}");
                Inventory.Add(item);
                return true;
            }

            return false;
        }

        public rItem Trade(rItem item)
        {
            var toReturn = EquippedItem;
            Inventory[EquippedIndex] = item;
            return toReturn;
        }

        public ItemDescription PostGive()
        {
            if (History.Last() == $"{nameof(GiveItem)}{nameof(Pipe)}")
            {
                return new ItemDescription("Thanks for the pipe");
            }

            return new ItemDescription("I didn't just receive an item");
        }

        public CharacterResponse Response(rItem item) => item switch
        {
            Pipe pipe when Inventory.Any(item => item is Pipe) => Pipe_SecondPipe(),
            Pipe pipe when History.Contains(nameof(Seaweed_Default)) => Pipe_AfterSeaweed(),
            Pipe pipe => Pipe_BeforeSeaweed(),
            Seaweed seaweed => Seaweed_Default(),
            NoneItem none when Inventory.Count == 0 => Nothing_Default(),
            NoneItem none => Nothing_WithItem(),
            _ => DefaultResponse(item)
        };
        #endregion

        #region Responses
        private CharacterResponse DefaultResponse(rItem item) => new CharacterResponse
        (
            false,
            $"I don't know how to feel about {item.Name}"
        );

        // None item
        private CharacterResponse Nothing_Default()
        {
            History.Add(nameof(Nothing_Default));
            return new CharacterResponse(false, "Please don't suck me");
        }

        private CharacterResponse Nothing_WithItem()
        {
            History.Add(nameof(Nothing_WithItem));
            return new CharacterResponse(false, "You can't have it.");
        }

        // Seaweed
        private CharacterResponse Seaweed_Default()
        {
            History.Add(nameof(Seaweed_Default));

            if (Globals.Random.Next(2) == 0)
            {
                return new CharacterResponse(false, $"I don't like salad.", (PrintToken.Pause, 0.5f), $"\nToo floppy.");
            }
            else
            {
                return new CharacterResponse(false, $"No thanks, it looks soggy");
            }
        }

        // Pipe
        private CharacterResponse Pipe_SecondPipe()
        {
            History.Add(nameof(Pipe_SecondPipe));
            return new CharacterResponse(false, "I've already got one of those");
        }

        private CharacterResponse Pipe_BeforeSeaweed()
        {
            History.Add(nameof(Pipe_BeforeSeaweed));
            return new CharacterResponse(true, $"This will be handy for beating off",
                                                (PrintToken.Pause, 0.1f),
                                                ".",
                                                (PrintToken.Pause, 0.1f),
                                                ".",
                                                (PrintToken.Pause, 0.1f),
                                                ". ",
                                                (PrintToken.Pause, 0.75f),
                                                $"barnicles");
        }

        private CharacterResponse Pipe_AfterSeaweed() 
        {
            History.Add(nameof(Pipe_AfterSeaweed));
            return new CharacterResponse(true, "Now that's what I call stiff");
        }
        #endregion
    }
}
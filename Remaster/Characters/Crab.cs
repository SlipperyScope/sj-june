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
        #region IHostItems
        public rItem RequestItem()
        {
            throw new NotImplementedException();
        }

        public ItemDescription Response(rItem item) => item switch
        {
            Pipe pipe => PipeResponce,
            Seaweed seaweed => SeaweedResponse,
            _ => DefaultResponce(item)
        };

        private ItemDescription DefaultResponce(rItem item) => new ItemDescription
        (
            $"I don't know how to feel about {item.Name}"
        );

        private ItemDescription SeaweedResponse
        {   
            get
            {
                History.Add(nameof(Seaweed));
                return new ItemDescription
                        (
                            $"I don't like salad.",
                            (PrintToken.Pause, 0.5f),
                            $"\nToo floppy."
                        );
            }
        }
        private ItemDescription PipeResponce => History.Contains(nameof(Seaweed)) is true
            ? new ItemDescription
            (
                $"Now that's nice and stiff",
                (PrintToken.Pause, 0.5f),
                $"\nJust how I like it"
            )
            : new ItemDescription
            (
                $"This will be handy for beating off",
                (PrintToken.Pause, 0.1f),
                ".",
                (PrintToken.Pause, 0.1f),
                ".",
                (PrintToken.Pause, 0.1f),
                ". ",
                (PrintToken.Pause, 0.75f),
                $"barnicles"
            );
        #endregion
    }
}
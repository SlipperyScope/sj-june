using System;
using rItem = Remaster.Items.Item;

namespace Remaster.HUD
{
    /// <summary>
    /// Facilitates transitioning items between an item arm and an item window
    /// </summary>
    public class ItemTransitionBay
    {
        /// <summary>
        /// Item from an item bay
        /// </summary>
        public rItem BayItem { get; private set; }

        /// <summary>
        /// Bay from which the item came
        /// </summary>
        public ItemWindow LastBay { get; private set; }

        /// <summary>
        /// Arm from which the item came
        /// </summary>
        public SubArm LastArm { get; private set; }

        /// <summary>
        /// Item from an item arm
        /// </summary>
        public rItem ArmItem { get; private set; }

        /// <summary>
        /// Deposite an item into the transition bay from an item arm
        /// </summary>
        /// <param name="arm">Arm to exchange with</param>
        /// <param name="item">Item to exchange</param>
        public void Deposit(SubArm arm, rItem item)
        {
            LastArm = arm;
            ArmItem = item;

            if (BayItem != null)
            {
                PushItems();
            }
        }

        /// <summary>
        /// Deposit an item into the transition bay from an item window
        /// </summary>
        /// <param name="window">Window to exchange with</param>
        /// <param name="item">Item to exchange</param>
        public void Deposit(ItemWindow window, rItem item)
        {
            LastBay = window;
            BayItem = item;

            if (ArmItem != null)
            {
                PushItems();
            }
        }

        /// <summary>
        /// True if the bay is empty
        /// </summary>
        public Boolean Empty => BayItem is null && ArmItem is null;

        /// <summary>
        /// Index of last item bay
        /// </summary>
        public Int32 BayIndex => LastBay?.Index ?? -1;

        /// <summary>
        /// Push items through
        /// </summary>
        private void PushItems()
        {
            LastBay.Intake(ArmItem);
            LastArm.Output(BayItem);
            BayItem = null;
            LastBay = null;
            LastArm = null;
            ArmItem = null;
        }
    }
}

using STVRogue.GameLogic;
using System;

namespace STVRogue
{
    public class Command
    {
        private string action;
        private Player player;
        private Node node;

        public Command (Node node, Player player) {
            WritePossibleActionsToScreen();
            action = Console.ReadLine();
            this.player = player;
            this.node = node;
        }

        public void executeCommand() {
            if (action == "attack")
                Attack();
            else if (action == "item")
                UseItem();
            else if (action == "flee") {
                Flee();
            }
            else {
                Console.WriteLine("Action not recognized.");
            }
        }

        private void Attack() {
            foreach (Pack p in node.packs)
                foreach (Monster m in p.members) {
                    player.Attack(m);
                }
        }

        private void UseItem() {
            WritePossibleItemUsage();
            string item = Console.ReadLine();
            if (item == "hp potion")
                useHPPotion();
            else if (item == "crystal")
                useCrystal();
        }

        private void useCrystal() {
            foreach (Item i in player.bag) {
                if (i.isCrystal()) {
                    player.use(i);
                    return;
                }
            }
        }

        private void useHPPotion() {
            foreach (Item i in player.bag) {
                if (i.isHealingPotion()) {
                    player.use(i);
                    return;
                }
            }
        }

        private void Flee() {
            player.location = node.neighbors[0];
        }

        private void WritePossibleActionsToScreen() {
            Console.WriteLine("Perform an action by typing: ");
            Console.WriteLine("1. attack");
            Console.WriteLine("2. item");
            Console.WriteLine("3. flee");
        }
        private static void WritePossibleItemUsage() {
            Console.WriteLine("Use an item by writing: ");
            Console.WriteLine("a. hp potion");
            Console.WriteLine("b. magic crystal");
        }
    }
}

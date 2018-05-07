using System;
namespace STVRogue
{
    public class Command
    {
        private string action;
        public Command(string action) {
            this.action = action;     
        }

        public void executeCommand() {
            if (action == "attack") {
                Attack();
            }
            else if (action == "item") {
                WritePossibleItemUsage();
                string item = Console.ReadLine();
                if (item == "hp potion") {
                    foreach (Item i in player.bag) {
                        if (i.isHealingPotion()) {
                            player.use(i);
                            return;
                        }
                    }
                }
                else if (item == "crystal") {
                    foreach (Item i in player.bag) {
                        if (i.isCrystal()) {
                            player.use(i);
                            return;
                        }
                    }
                }
            }
            else if (action == "flee") {
                foreach (Node n in neighbors) {
                    if (!n.packs.Any())
                        player.location = n;
                }
                player.location = neighbors[0];
            }
            else {
                Console.WriteLine("Action not recognized.");
            }
        }

        private static void Attack() {
            foreach (Pack p in packs)
                foreach (Monster m in p.members) {
                    player.Attack(m);
                }
        }
    }
}

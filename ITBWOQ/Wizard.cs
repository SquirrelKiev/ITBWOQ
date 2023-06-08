using System;
using System.Runtime.CompilerServices;

namespace ITBWOQ
{
    public enum Element
    {
        Earth,
        Water,
        Fire,
        Air
    }

    public struct WizardState
    {
        public readonly string id = UniqueObject.New();

        public string name = "Golzondas";
        public int wisdom = 0; // attack damage
        public int health = 0;
        public int maxHealth = 0;
        public int dexterity = 0; // who attacks first
        public Element selectedElement;

        private const int minPossibleWisdom = 5;
        private const int maxPossibleWisdom = 20;
        
        private const int minPossibleHealth = 100;
        private const int maxPossibleHealth = 120;

        private const int minPossibleDexterity = 0;
        private const int maxPossibleDexterity = 100;

        public WizardState()
        {
            var elements = Enum.GetValues<Element>();

            selectedElement = elements[State.random.Next(0, elements.Length)];
        }

        public WizardState Randomise()
        {
            wisdom = State.random.Next(minPossibleWisdom, maxPossibleWisdom);
            maxHealth = State.random.Next(minPossibleHealth, maxPossibleHealth);
            dexterity = State.random.Next(minPossibleDexterity, maxPossibleDexterity);

            return this;
        }

        public WizardState ResetHealth()
        {
            health = maxHealth;

            return this;
        }

        public WizardState Attack(int damage, Element attackerElement)
        {
            var us = this;

            var fullDamage = (int)MathF.Round(damage * GetDamageMultiplier(attackerElement, selectedElement));

            us.health -= fullDamage;

            return us;
        }

        private static float GetDamageMultiplier(Element attackerElement, Element defenderElement)
        {
            return attackerElement switch
            {
                Element.Fire => defenderElement == Element.Air ? 1.5f : 1f,
                Element.Air => defenderElement == Element.Earth ? 1.5f : 1f, // ???
                Element.Water => defenderElement == Element.Fire ? 1.5f : 1f,
                Element.Earth => defenderElement == Element.Water ? 1.5f : 1f, // ???
                _ => 1f,
            };
        }
    }
}

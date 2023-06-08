using System;
using System.Collections.Generic;
using System.Linq;

namespace InsideTheBoxWiseOnesQuest
{
    public class State
    {
        public static readonly Random random = new();
        public readonly string id = UniqueObject.New();

        public enum CurrentState
        {
            CharacterCustomization,
            Battle,
            GameOver
        }
        
        // expansion later down the line
        public enum CurrentControlsScreen
        {
            MainMenu
        }
        
        public enum Fighter
        {
            None,
            Player,
            Opponent,
        }

        public CurrentState currentState;
        // probably should be in separate UI State class
        public CurrentControlsScreen currentControlsScreen;

        public Fighter CurrentTurn => fighters.First.Value;
        public Fighter Winner { get; private set; }

        // would use queue but that doesnt have Remove()
        public LinkedList<Fighter> fighters { get;private set; }
        public Dictionary<Fighter, WizardState> Wizards { get; } = new Dictionary<Fighter, WizardState>();

        public State()
        {
            Reset();
        }

        public void Reset()
        {
            currentState = CurrentState.CharacterCustomization;
            currentControlsScreen = CurrentControlsScreen.MainMenu;

            foreach (var fighter in Enum.GetValues<Fighter>())
            {
                if (fighter == Fighter.None)
                    continue;

                Wizards[fighter] = new WizardState();
            }
        }

        public void BeginBattle()
        {
            foreach(var fighter in Enum.GetValues<Fighter>())
            {
                if (fighter == Fighter.None)
                    continue;

                Wizards[fighter] = Wizards[fighter].Randomise().ResetHealth();
            }

            fighters = new(Wizards.OrderByDescending(kvp => kvp.Value.dexterity).Select(kvp => kvp.Key));

            Winner = Fighter.None;

            currentState = CurrentState.Battle;

            ProcessTurn();
        }

        public void Fight(Fighter attacking, Fighter defending)
        {
            Wizards[defending] = Wizards[defending].Attack(Wizards[attacking].wisdom, Wizards[attacking].selectedElement);

            NextTurn();
        }

        private void NextTurn()
        {
            var currentFighter = CurrentTurn;
            fighters.RemoveFirst();
            fighters.AddLast(currentFighter);

            var fightersToRemove = new HashSet<Fighter>();

            foreach(var fighter in fighters)
            {
                if (Wizards[fighter].health <= 0)
                {
                    fightersToRemove.Add(fighter);
                }
            }

            foreach(var fighterToRemove in fightersToRemove)
            {
                fighters.Remove(fighterToRemove);
            }

            if(fighters.Count <= 1)
            {
                currentState = CurrentState.GameOver;

                Winner = currentFighter;
            }
            else
            {
                ProcessTurn();
            }
        }

        private void ProcessTurn()
        {
            if (CurrentTurn != Fighter.Player)
            {
                AiTurn();
            }
        }

        private void AiTurn()
        {
            Fight(Fighter.Opponent, Fighter.Player);
        }
    }
}

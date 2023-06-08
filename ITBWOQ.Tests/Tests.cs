using System.Diagnostics;

namespace ITBWOQ.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private void StartWizardBattle(State state, WizardState playerWizard, WizardState opponentWizard)
        {
            state.Wizards.Clear();

            state.Wizards[State.Fighter.Player] = playerWizard;
            state.Wizards[State.Fighter.Opponent] = opponentWizard;

            state.BeginBattle(randomizeStats: false);
        }

        private static void LogBattleState(State state)
        {
            foreach(var wizard in state.Wizards)
            {
                Debug.WriteLine($"\n{wizard.Key}");
                Debug.WriteLine($"Health: {wizard.Value.health}");
                Debug.WriteLine($"Dexterity: {wizard.Value.dexterity}");
                Debug.WriteLine($"Wisdom: {wizard.Value.wisdom}");
                Debug.WriteLine($"Element: {wizard.Value.selectedElement}");
            }

            Debug.WriteLine($"");
            Debug.WriteLine($"Game Screen: {state.Screen}");
            Debug.WriteLine($"Current Turn: {state.CurrentTurn}");
            Debug.WriteLine($"Current Winner: {state.Winner}");
        }

        [Test]
        public void ElementalDamageApplies()
        {
            var state = new State();

            StartWizardBattle(state,
                new WizardState
                {
                    wisdom = 20,
                    maxHealth = 100,
                    dexterity = 100,
                    selectedElement = Element.Water
                },
                new WizardState
                {
                    wisdom = 20,
                    maxHealth = 100,
                    dexterity = 0,
                    selectedElement = Element.Fire
                });

            state.Fight(State.Fighter.Player, State.Fighter.Opponent);

            var player = state.Wizards[State.Fighter.Player];
            var opponent = state.Wizards[State.Fighter.Opponent];

            Assert.That(opponent.health, Is.EqualTo(opponent.maxHealth - (player.wisdom * 1.5f)));

            LogBattleState(state);
        }

        [Test]
        public void PlayerWinByDexterity()
        {
            var state = new State();

            StartWizardBattle(state,
                new WizardState
                {
                    wisdom = 20,
                    maxHealth = 100,
                    dexterity = 100,
                    selectedElement = Element.Air
                },
                new WizardState
                {
                    wisdom = 20,
                    maxHealth = 100,
                    dexterity = 0,
                    selectedElement = Element.Air
                });

            while(state.Screen != State.CurrentScreen.GameOver)
            {
                state.Fight(State.Fighter.Player, State.Fighter.Opponent);
            }

            Assert.That(state.Winner, Is.EqualTo(State.Fighter.Player));

            LogBattleState(state);
        }

        [Test]
        public void OpponentWinByDexterity()
        {
            var state = new State();

            StartWizardBattle(state,
                new WizardState
                {
                    wisdom = 20,
                    maxHealth = 100,
                    dexterity = 0,
                    selectedElement = Element.Air
                },
                new WizardState
                {
                    wisdom = 20,
                    maxHealth = 100,
                    dexterity = 100,
                    selectedElement = Element.Air
                });

            while (state.Screen != State.CurrentScreen.GameOver)
            {
                state.Fight(State.Fighter.Player, State.Fighter.Opponent);
            }

            Assert.That(state.Winner, Is.EqualTo(State.Fighter.Opponent));

            LogBattleState(state);
        }

        [Test]
        public void PlayerWinByWisdom()
        {
            var state = new State();

            StartWizardBattle(state,
                new WizardState
                {
                    wisdom = 20,
                    maxHealth = 100,
                    dexterity = 100,
                    selectedElement = Element.Air
                },
                new WizardState
                {
                    wisdom = 10,
                    maxHealth = 100,
                    dexterity = 100,
                    selectedElement = Element.Air
                });

            while (state.Screen != State.CurrentScreen.GameOver)
            {
                state.Fight(State.Fighter.Player, State.Fighter.Opponent);
            }

            Assert.That(state.Winner, Is.EqualTo(State.Fighter.Player));

            LogBattleState(state);
        }

        [Test]
        public void OpponentWinByWisdom()
        {
            var state = new State();

            StartWizardBattle(state,
                new WizardState
                {
                    wisdom = 10,
                    maxHealth = 100,
                    dexterity = 100,
                    selectedElement = Element.Air
                },
                new WizardState
                {
                    wisdom = 20,
                    maxHealth = 100,
                    dexterity = 100,
                    selectedElement = Element.Air
                });

            while (state.Screen != State.CurrentScreen.GameOver)
            {
                state.Fight(State.Fighter.Player, State.Fighter.Opponent);
            }

            Assert.That(state.Winner, Is.EqualTo(State.Fighter.Opponent));

            LogBattleState(state);
        }
    }
}
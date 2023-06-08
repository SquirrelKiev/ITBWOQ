using ImGuiNET;
using System;
using System.Numerics;

namespace ITBWOQ
{
    // This should only be UI code to make the program unit testable
    public static class ImGuiUi
    {
        const int defaultWindowWidth = 200;

        public static void RenderWindow(State state)
        {
            switch (state.Screen)
            {
                case State.CurrentScreen.CharacterCustomization:
                    var playerWizard = state.Wizards[State.Fighter.Player];

                    bool shouldBeginBattle = RenderWizardCustomization(ref playerWizard);
                    state.Wizards[State.Fighter.Player] = playerWizard;

                    if(shouldBeginBattle)
                    {
                        state.BeginBattle();
                    }
                    break;

                case State.CurrentScreen.Battle:
                    // TODO: Allow more than two wizards - place info above wizard head omori style?
                    bool anchorRight = false;

                    foreach(var wizard in state.Wizards.Values)
                    {
                        RenderWizardInfo(wizard, anchorRight);
                        anchorRight = !anchorRight;
                    }

                    RenderWizardControls(state);
                    break;

                case State.CurrentScreen.GameOver:
                    RenderGameOverScreen(state);
                    break;
            }
        }

        private static bool RenderWizardCustomization(ref WizardState state)
        {
            BeginFullScreen("Wizard Customization");

            // Name
            ImGui.InputText($"Name##{state.id}", ref state.name, 20);

            bool nameValid = state.name.Length > 2;
            if (!nameValid)
            {
                ImGui.Text("Name is too short!");
            }

            if (ImGui.BeginCombo($"Element##{state.id}", state.selectedElement.ToString()))
            {
                foreach (var element in Enum.GetValues<Element>())
                {
                    bool isSelected = state.selectedElement == element;

                    if (ImGui.Selectable(element.ToString(), isSelected))
                        state.selectedElement = element;
                }

                ImGui.EndCombo();
            }


            bool valid = nameValid;

            if (!valid) ImGui.BeginDisabled();
            bool readyToContinue = ImGui.Button($"Fight!##{state.id}");
            if (!valid) ImGui.EndDisabled();

            ImGui.End();

            return readyToContinue;
        }

        private static void BeginFullScreen(string name)
        {
            var io = ImGui.GetIO();

            ImGui.SetNextWindowSize(io.DisplaySize);
            ImGui.SetNextWindowPos(Vector2.Zero);

            ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;

            ImGui.Begin(name, flags);
        }

        private static void RenderWizardInfo(WizardState state, bool anchorRight)
        {
            ImGui.SetNextWindowSize(new Vector2(defaultWindowWidth, -1));

            if (anchorRight)
                ImGui.SetNextWindowPos(new Vector2(ImGui.GetIO().DisplaySize.X - defaultWindowWidth, 0), ImGuiCond.Once);
            else
                ImGui.SetNextWindowPos(Vector2.Zero);

            ImGui.Begin($"{state.name} - {state.selectedElement} Wizard##{state.id}", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove);

            ImGui.ProgressBar((float)state.health / state.maxHealth, new Vector2(-1, 0), $"{state.health} / {state.maxHealth}");

            ImGui.Text($"W: {state.wisdom}");

            var dexterityText = $"D: {state.dexterity}";
            // should probably only calculate this once and reuse
            ImGui.SameLine(ImGui.GetWindowWidth() - ImGui.CalcTextSize(dexterityText).X - ImGui.GetStyle().ItemSpacing.X);

            ImGui.Text(dexterityText);

            ImGui.End();
        }

        #region Wizard Controls
        private static void RenderWizardControls(State state)
        {
            ImGui.SetNextWindowSize(new Vector2(defaultWindowWidth, -1));

            ImGui.Begin("Controls");

            switch (state.currentControlsScreen)
            {
                case State.CurrentControlsScreen.MainMenu:
                    RenderMainScreen(state);
                    break;
            }

            var windowSize = ImGui.GetWindowSize();
            var displaySize = ImGui.GetIO().DisplaySize;

            ImGui.SetWindowPos(new Vector2(displaySize.X - windowSize.X, displaySize.Y - windowSize.Y));

            ImGui.End();
        }

        private static void RenderMainScreen(State state)
        {
            if(ImGui.Button($"Fight!##{state.id}", new Vector2(-1, ImGui.GetTextLineHeight() * 2)))
            {
                state.Fight(State.Fighter.Player, State.Fighter.Opponent);
            }
        }
        #endregion

        private static void RenderGameOverScreen(State state)
        {
            BeginFullScreen("Game over");

            ImGui.Text($"{state.Wizards[state.Winner].name} wins!");

            if (ImGui.Button("Rematch"))
            {
                state.BeginBattle();
            }

            ImGui.SameLine();

            if (ImGui.Button("Restart"))
            {
                state.Reset();
            }

            ImGui.End();
        }
    }
}

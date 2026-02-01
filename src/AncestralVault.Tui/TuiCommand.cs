// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Tui.UI.Screens;
using Terminal.Gui.App;
using Terminal.Gui.Views;

namespace AncestralVault.Tui
{
    public class TuiCommand
    {
        private readonly Dictionary<Type, ScreenBase> screens = new();
        private Window? window;
        private ScreenBase? currentScreen;

        public async Task ExecuteAsync(TuiOptions options, CancellationToken stoppingToken)
        {
            await Task.CompletedTask;

            try
            {
                using (IApplication app = Application.Create())
                {
                    app.Init();

                    using (window = new Window())
                    {
                        window.Title = "Ancestral Vault (Esc to quit)";

                        RegisterScreen(new ScreenA());
                        RegisterScreen(new ScreenB());

                        NavigateTo(typeof(ScreenA));

                        app.Run(window);
                    }
                }
            }
            finally
            {
                // Workaround for Terminal.Gui alpha bug: restore cursor visibility
                Console.CursorVisible = true;
            }
        }

        private void RegisterScreen(ScreenBase screen)
        {
            screens[screen.GetType()] = screen;
            screen.NavigateRequested += NavigateTo;
        }

        private void NavigateTo(Type screenType)
        {
            if (window == null || !screens.TryGetValue(screenType, out var newScreen))
            {
                return;
            }

            if (currentScreen != null)
            {
                window.Remove(currentScreen);
            }

            currentScreen = newScreen;
            window.Add(currentScreen);
            currentScreen.SetFocus();
        }
    }
}

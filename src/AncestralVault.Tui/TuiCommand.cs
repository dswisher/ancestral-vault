// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Common.Utilities;
using AncestralVault.Tui.UI;
using AncestralVault.Tui.UI.Screens;
using AncestralVault.Tui.Views;
using Terminal.Gui.App;
using Terminal.Gui.Input;

namespace AncestralVault.Tui
{
    public class TuiCommand
    {
        private readonly IVaultSeeker seeker;
        private readonly ScreenNavigator navigator;

        public TuiCommand(
            IVaultSeeker seeker,
            ScreenNavigator navigator)
        {
            this.seeker = seeker;
            this.navigator = navigator;
        }


        public async Task ExecuteAsync(TuiOptions options, CancellationToken stoppingToken)
        {
            await Task.CompletedTask;

            // Set up the vault info
            seeker.Configure(options.VaultPath);

            try
            {
                using (IApplication app = Application.Create())
                {
                    app.Init();

                    // app.Keyboard.QuitKey = Key.Q.WithCtrl;
                    app.Keyboard.KeyBindings.Remove(Key.Esc);

                    using (var window = new MainWindow(navigator))
                    {
                        navigator.SetWindow(window);

                        // navigator.NavigateTo<ScreenA>();
                        navigator.PushView<HomeView>();

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
    }
}

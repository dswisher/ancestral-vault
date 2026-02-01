// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using AncestralVault.Tui.UI;
using AncestralVault.Tui.UI.Screens;
using Terminal.Gui.App;

namespace AncestralVault.Tui
{
    public class TuiCommand
    {
        private readonly ScreenNavigator navigator;

        public TuiCommand(ScreenNavigator navigator)
        {
            this.navigator = navigator;
        }

        public async Task ExecuteAsync(TuiOptions options, CancellationToken stoppingToken)
        {
            await Task.CompletedTask;

            try
            {
                using (IApplication app = Application.Create())
                {
                    app.Init();

                    using (var window = new MainWindow())
                    {
                        navigator.SetWindow(window);
                        navigator.NavigateTo<ScreenA>();

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

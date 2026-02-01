// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Terminal.Gui.Input;
using Terminal.Gui.Views;

namespace AncestralVault.Tui.UI
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            Title = "Ancestral Vault";
        }

        protected override bool OnKeyDown(Key key)
        {
            if (key.NoAlt.NoCtrl.NoShift == Key.Q)
            {
                RequestStop();
                return true;
            }

            // Consume ESC to prevent default quit behavior
            if (key.NoAlt.NoCtrl.NoShift == Key.Esc)
            {
                return true;
            }

            return base.OnKeyDown(key);
        }
    }
}

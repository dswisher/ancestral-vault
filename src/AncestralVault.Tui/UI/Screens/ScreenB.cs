// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace AncestralVault.Tui.UI.Screens
{
    public class ScreenB : View
    {
        private readonly ScreenNavigator navigator;

        public ScreenB(ScreenNavigator navigator)
        {
            this.navigator = navigator;

            Width = Dim.Fill();
            Height = Dim.Fill();
            CanFocus = true;

            var titleLabel = new Label
            {
                Text = "This is Screen B",
                X = Pos.Center(),
                Y = 2
            };

            var instructionLabel = new Label
            {
                Text = "Press A to go to screen A",
                X = Pos.Center(),
                Y = 3
            };

            Add(titleLabel, instructionLabel);
        }

        protected override bool OnKeyDown(Key key)
        {
            if (key.NoAlt.NoCtrl.NoShift == Key.A)
            {
                navigator.NavigateTo<ScreenA>();
                return true;
            }

            return base.OnKeyDown(key);
        }
    }
}

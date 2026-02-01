// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace AncestralVault.Tui.UI.Screens
{
    public class ScreenA : ScreenBase
    {
        public ScreenA()
        {
            Width = Dim.Fill();
            Height = Dim.Fill();
            CanFocus = true;

            var titleLabel = new Label
            {
                Text = "This is Screen A",
                X = Pos.Center(),
                Y = 2
            };

            var instructionLabel = new Label
            {
                Text = "Press B to go to screen B",
                X = Pos.Center(),
                Y = 3
            };

            Add(titleLabel, instructionLabel);
        }

        protected override bool OnKeyDown(Key key)
        {
            if (key.NoAlt.NoCtrl.NoShift == Key.B)
            {
                NavigateTo<ScreenB>();
                return true;
            }

            return base.OnKeyDown(key);
        }
    }
}

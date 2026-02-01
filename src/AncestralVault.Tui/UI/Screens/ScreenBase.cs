// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Terminal.Gui.ViewBase;

namespace AncestralVault.Tui.UI.Screens
{
    public abstract class ScreenBase : View
    {
        public event Action<Type>? NavigateRequested;

        protected void NavigateTo<T>()
        where T : ScreenBase
        {
            NavigateRequested?.Invoke(typeof(T));
        }
    }
}

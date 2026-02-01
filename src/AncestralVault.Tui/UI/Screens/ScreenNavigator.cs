// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace AncestralVault.Tui.UI.Screens
{
    public class ScreenNavigator
    {
        private readonly IServiceProvider serviceProvider;
        private Window? window;
        private View? currentScreen;

        public ScreenNavigator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void SetWindow(Window window)
        {
            this.window = window;
        }

        public void NavigateTo<T>()
            where T : View
        {
            NavigateTo(typeof(T));
        }

        public void NavigateTo(Type screenType)
        {
            if (window == null)
            {
                throw new InvalidOperationException("Window has not been set. Call SetWindow first.");
            }

            var newScreen = (View)serviceProvider.GetRequiredService(screenType);

            if (currentScreen != null)
            {
                window.Remove(currentScreen);
                currentScreen.Dispose();
            }

            currentScreen = newScreen;
            window.Add(currentScreen);
            currentScreen.SetFocus();
        }
    }
}

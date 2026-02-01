// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace AncestralVault.Tui.UI.Screens
{
    public class ScreenNavigator
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Stack<Type> viewStack = new();

        private Window? window;
        private View? currentView;


        public ScreenNavigator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }


        public void SetWindow(Window win)
        {
            window = win;
        }


        public void PushView<T>()
            where T : View
        {
            viewStack.Push(typeof(T));

            SetView(typeof(T));
        }


        public void PopView()
        {
            if (viewStack.Count == 0)
            {
                window?.RequestStop();
            }
            else
            {
                var newView = viewStack.Pop();

                SetView(newView);
            }
        }


        public void SetView<T>()
            where T : View
        {
            SetView(typeof(T));
        }


        private void SetView(Type screenType)
        {
            if (window == null)
            {
                throw new InvalidOperationException("Window has not been set. Call SetWindow first.");
            }

            var newView = (View)serviceProvider.GetRequiredService(screenType);

            if (currentView != null)
            {
                window.Remove(currentView);
                currentView.Dispose();
            }

            currentView = newView;
            window.Add(currentView);
            currentView.SetFocus();
        }
    }
}

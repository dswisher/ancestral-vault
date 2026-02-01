// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.ObjectModel;
using System.Linq;
using AncestralVault.Common.Database;
using AncestralVault.Tui.UI;
using AncestralVault.Tui.UI.Screens;
using Microsoft.Extensions.Logging;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace AncestralVault.Tui.Views
{
    public class HomeView : View
    {
        private readonly ScreenNavigator navigator;
        private readonly NavigationContext navContext;
        private readonly ObservableCollection<PersonaListItem> personas;
        private readonly ListView listView;

        public HomeView(
            ScreenNavigator navigator,
            NavigationContext navContext,
            IAncestralVaultDbContextFactory dbContextFactory,
            ILogger<HomeView> logger)
        {
            this.navigator = navigator;
            this.navContext = navContext;

            Width = Dim.Fill();
            Height = Dim.Fill();
            CanFocus = true;

            var titleLabel = new Label
            {
                Text = "Ancestral Vault",
                X = Pos.Center(),
                Y = 1
            };

            var instructionLabel = new Label
            {
                Text = "Select a persona and press Enter to view details",
                X = Pos.Center(),
                Y = 3
            };

            // Load personas from database
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var personaList = dbContext.CompositePersonas
                    .OrderBy(p => p.Name)
                    .Select(p => new PersonaListItem(p.CompositePersonaId, p.Name))
                    .ToList();
                personas = new ObservableCollection<PersonaListItem>(personaList);
            }

            listView = new ListView
            {
                X = 2,
                Y = 5,
                Width = Dim.Fill(2),
                Height = Dim.Fill(1),
                CanFocus = true
            };

            listView.SetSource(personas);

            listView.Accepting += (_, x) =>
            {
                // TODO - remove this debug code
                logger.LogInformation("ListView accepting");
                HandleListViewSelect();
                x.Handled = true;
            };

            listView.Accepted += (_, _) =>
            {
                // TODO - remove this debug code
                logger.LogInformation("ListView accepted");
            };

            listView.Activating += (_, _) =>
            {
                // TODO - remove this debug code
                logger.LogInformation("ListView accepted");
            };

            // Select the first item by default
            if (personas.Count > 0)
            {
                listView.SelectedItem = 0;
            }

            Add(titleLabel, instructionLabel, listView);

            // Focus the list view when the view is ready
            Initialized += (_, _) => listView.SetFocus();
        }


        private void HandleListViewSelect()
        {
            var selectedIndex = listView.SelectedItem;
            if (selectedIndex.HasValue && selectedIndex.Value >= 0 && selectedIndex.Value < personas.Count)
            {
                var selected = personas[selectedIndex.Value];
                navContext.SelectedCompositePersonaId = selected.Id;
                navigator.PushView<CompositePersonaView>();
            }
        }


        private record PersonaListItem(string Id, string Name)
        {
            public override string ToString() => Name;
        }
    }
}

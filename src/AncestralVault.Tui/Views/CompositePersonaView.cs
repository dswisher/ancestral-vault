// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using AncestralVault.Common.Database;
using AncestralVault.Tui.UI;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace AncestralVault.Tui.Views
{
    public class CompositePersonaView : View
    {
        public CompositePersonaView(
            NavigationContext navContext,
            IAncestralVaultDbContextFactory dbContextFactory)
        {
            Width = Dim.Fill();
            Height = Dim.Fill();
            CanFocus = true;

            var personaId = navContext.SelectedCompositePersonaId;

            string personaName = "Unknown";
            if (personaId != null)
            {
                using var dbContext = dbContextFactory.CreateDbContext();
                var persona = dbContext.CompositePersonas.Find(personaId);
                if (persona != null)
                {
                    personaName = persona.Name;
                }
            }

            var titleLabel = new Label
            {
                Text = personaName,
                X = Pos.Center(),
                Y = 1
            };

            var idLabel = new Label
            {
                Text = $"ID: {personaId ?? "N/A"}",
                X = Pos.Center(),
                Y = 3
            };

            var backLabel = new Label
            {
                Text = "Escape: back | Q: quit",
                X = Pos.Center(),
                Y = 5
            };

            Add(titleLabel, idLabel, backLabel);
        }
    }
}

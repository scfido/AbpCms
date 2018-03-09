using System;
using Abp.Application.Navigation;
using Abp.Localization;

namespace Cms.Todo.Web
{
    public class TodoNavigationProvider: NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "Todo.Index",
                        new FixedLocalizableString("Todo"),
                        url: "/todo",
                        icon: "list"
                        )
                );
        }

    }
}

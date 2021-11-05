using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditonPetrpku.Client.Tests.Helpers
{
    public static class TestContextExtensions
    {
        public static TestContext AddMudBlazorSupport(this TestContext context)
        {
            context.Services.AddSingleton<MudSnackbarProvider>();
            context.Services.AddSingleton<MudDialogProvider>();

            context.Services.AddSingleton<SnackbarService>();
            context.Services.AddSingleton<ISnackbar>(s => s.GetRequiredService<SnackbarService>());

            context.Services.AddSingleton<DialogService>();
            context.Services.AddSingleton<IDialogService>(s => s.GetRequiredService<DialogService>());

            return context;
        }
    }
}

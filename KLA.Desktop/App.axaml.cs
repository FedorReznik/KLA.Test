using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KLA.Desktop.Models;
using KLA.Desktop.Services;
using KLA.Desktop.ViewModels;
using KLA.Desktop.Views;
using KLA.Domain.Shared.Services;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace KLA.Desktop;

public partial class App : Application
{
    private readonly Container _container = new ();
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        RegisterTypes();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _container.GetInstance<IMainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void RegisterTypes()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        var settings = configuration.GetRequiredSection("Settings").Get<Settings>();
        
        _container.RegisterSingleton(() => settings!);
        _container.Register<IMainWindowViewModel, MainWindowViewModel>();
        _container.RegisterSingleton<ICurrencyParser, CurrencyParser>();
        _container.RegisterSingleton<ICurrencyRangeValidator, CurrencyRangeValidator>();
        _container.RegisterSingleton<ICurrencyToTextConverterServiceProxy, CurrencyToTextConverterServiceProxy>();
        
        _container.Verify();
    }
}
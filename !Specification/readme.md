# KLA Coding Task. Solution overview.

## 1. Solution structure.

- KLA.Domain: contains the services and classes implementing conversion from currency to words
- KLA.Domain.Shared: contains services used across client & server, for example: parsing and validation services
- KLA.WebAPI: contains server implementation using ASP.Net Core
- KLA.Desktop: contains cross-platform desktop implementation using Avalonia UI.
- KLA.UnitTests: contains unit tests for meaningful solution classes.

## 2. Decisions.

### 2.1 Conversion Algorithm.
Conversion algorithm is implemented based on scanning the currency amount value from left to right, please refer to: CurrencyToTextConverterService class. 
This gives us the possibility to use StringBuilder for accumulating the result string, thus minifying memory allocation and GC. 


### 2.2 Server implementation.
As the task states that solution should be based on .Net 6.0 it seems reasonable to implement server-side part as a REST service based on ASP.Net Core. 
We could also consider RPC like communication between client and server based on websockets, but this seems overcomplicated.
Also REST approach looks natural here: we have a resource which we can query e.g. currency/text.

What input type should currency/text endpoint expect?
From one point seems logical to expect decimal value, cause we need to validate the value on client also.
But if imaginary domain works with string currency literals separated by comma, 
then probably there would be more than one client and we need to be able to accept strings.
For this test the later approach was selected, which cause the need of server side validation and parsing. 


### 2.3 Desktop vs Web. Desktop Technology selection.
Desktop application option was selected for this implementation - to stay in the same eco-system for each component. 

As for desktop technology there are different options in the world of .Net.
If we will exclude very old technologies like Windows.Forms we will have the following list to consider:
- [Xamarin.Forms](https://dotnet.microsoft.com/en-us/apps/xamarin/xamarin-forms) - unfortunately MicroSoft is planning to deprecate it in favor of MAUI
- [MAUI](https://dotnet.microsoft.com/en-us/apps/maui) - cross platform UI framework from MicroSoft,
stable version was released quite recently and still a bit immature, as well as it more targets mobile device, than pure desktop
- [.Net Core WPF](https://github.com/dotnet/wpf) - mature framework for implementing desktop applications with recent support of .Net Core.
But it can only run on Windows OS.
- [Avalonia UI](https://avaloniaui.net/) - mature cross-platform framework based on .Net. More suitable for desktop development, than MAUI.  
It utilize the same principles as WPF: MVVM, DataTemplates, markup, binding, data template selection etc, thus for WPF developers it is rather easy to move on Avalonia UI.

Due to it's maturity and cross-platform capabilities, as well as similarity to WPF - Avalonia UI was selected as a framework to implement desktop GUI.

**Important**

To unleash Avalonia UI support in Visual Studio or Rider one will need to run the following command (For .Net 6.0 and earlier, replace install with --install): 

> dotnet new install Avalonia.Templates

For Visual Studio it is also valuable to install [Avalonia for Visual Studio extension](https://docs.avaloniaui.net/docs/next/get-started/set-up-an-editor#visual-studio)  

### 2.4 Unit tests in one project.
All unit tests are placed in one project.
Tthough in production code the probably should be split into different projects on per component basis,
for test task it seems reasonable to keep them in one project for simplicity

### 2.5 Absence of logging.
Decision not to add logging was made, to simplify implementation and skip creation of cohesive log structure for different processes.

### 2.6 Absence of resiliency strategy during communication with server.
For simplicity it was decided to skip creating resilience strategies such as Retry, Circuit Breaker, etc during communication with server: all faults are reported as exceptions, see CurrencyToTextConverterServiceProxy.
For production code this kind of strategies can be easily added using, for example, [Polly](https://github.com/App-vNext/Polly).

### 2.7 Absence of pretty UI.
For simplicity and considering test nature of solution it was decided not to focus on neat UI.  




 
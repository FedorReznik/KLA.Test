using System.ComponentModel;
using System.Windows.Input;

namespace KLA.Desktop.ViewModels;

public interface IMainWindowViewModel : INotifyDataErrorInfo
{
    string? CurrencyInput { get; set; }
    public string? CurrencyText { get; }
    public bool IsLoading { get; }
    public bool HasServerError { get; }
    public string? ServerError { get; }
    
    ICommand ConvertCommand { get; }
}
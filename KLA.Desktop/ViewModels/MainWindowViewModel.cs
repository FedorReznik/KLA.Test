using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KLA.Desktop.Exceptions;
using KLA.Desktop.Services;
using KLA.Domain.Shared.Services;
using ReactiveUI;

namespace KLA.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private readonly ICurrencyParser _currencyParser;
    private readonly ICurrencyRangeValidator _rangeValidator;
    private readonly ICurrencyToTextConverterServiceProxy _currencyToTextConverterServiceProxy;
    private readonly Dictionary<string, List<string>> _propertyToErrorListMap = new ();
    
    private string? _currencyInput;
    private string? _currencyText;
    private bool _isLoading;
    private string? _serverError;
    private bool _hasServerError;

    public MainWindowViewModel(
        ICurrencyParser currencyParser,
        ICurrencyRangeValidator rangeValidator,
        ICurrencyToTextConverterServiceProxy currencyToTextConverterServiceProxy)
    {
        _currencyParser = currencyParser ?? throw new ArgumentNullException(nameof(currencyParser));
        _rangeValidator = rangeValidator ?? throw new ArgumentNullException(nameof(rangeValidator));
        _currencyToTextConverterServiceProxy = currencyToTextConverterServiceProxy ?? throw new ArgumentNullException(nameof(currencyToTextConverterServiceProxy));

        ConvertCommand = ReactiveCommand.Create(
            async () => await ConvertCurrencyToText(),
            this.WhenAnyValue(x => x.CurrencyInput, ValidateCurrencyInput));
    }

    public string? CurrencyInput
    {
        get => _currencyInput;
        set => this.RaiseAndSetIfChanged(ref _currencyInput, value);
    }

    public string? CurrencyText
    {
        get => _currencyText;
        private set => this.RaiseAndSetIfChanged(ref _currencyText, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public bool HasServerError
    {
        get => _hasServerError;
        private set => this.RaiseAndSetIfChanged(ref _hasServerError, value);
    }

    public string? ServerError
    {
        get => _serverError;
        private set => this.RaiseAndSetIfChanged(ref _serverError, value);
    }

    public ICommand ConvertCommand { get; }
    
    public IEnumerable GetErrors(string? propertyName)
    {
        return _propertyToErrorListMap.TryGetValue(propertyName!, out var errorList) 
            ? errorList 
            : Enumerable.Empty<string>();
    }

    public bool HasErrors => _propertyToErrorListMap.Count > 0;
    
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    private async Task ConvertCurrencyToText()
    {
        if (!HasErrors)
        {
            HasServerError = false;
            ServerError = string.Empty;
            
            IsLoading = true;
            try
            {
                var text = await _currencyToTextConverterServiceProxy.Convert(CurrencyInput!);
                CurrencyText = text;
            }
            catch (ServerException e)
            {
                HasServerError = true;
                ServerError = e.Message;
            }
            
            IsLoading = false;
        }
    }
    
    private bool ValidateCurrencyInput(string? text)
    {
        _propertyToErrorListMap.Clear();

        if (string.IsNullOrWhiteSpace(text))
        {
            AddError(nameof(CurrencyInput), "Value cannot be empty.");
            return false;
        }

        var parsingResult = _currencyParser.Parse(text);
        if (!parsingResult.Parsed)
        {
            AddError(nameof(CurrencyInput), parsingResult.Message); 
            return false;
        }

        var validatingResult = _rangeValidator.Validate(parsingResult.ParsedValue!.Value);
        if (validatingResult.Status != CurrencyValidationStatus.Valid)
        {
            AddError(nameof(CurrencyInput), validatingResult.Message);
            return false;
        }

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CurrencyInput)));
        return true;
    }
    
    private void AddError(string propertyName, string error)
    {
        if (_propertyToErrorListMap.TryGetValue(propertyName, out var errorList))
        {
            if (!errorList.Contains(error))
            {
                errorList.Add(error);
            }
        }
        else
        {
            _propertyToErrorListMap.Add(propertyName, new List<string>{ error });
        }
        
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CurrencyInput)));
    }
}
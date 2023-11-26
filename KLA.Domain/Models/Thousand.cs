using System.Text;
using JetBrains.Annotations;

namespace KLA.Domain.Models;

public struct Thousand
{
    private const ushort OneHundredScale = 100;
    private const ushort MaxValue = 999;

    private byte? _hundreds = null;
    private Hundred? _hundred = null;
    
    public Thousand(ushort value, byte scale)
    {
        if(value > MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"{typeof(Thousand)} can only accept values < {MaxValue + 1}, but received {value}");
        
        Scale = scale;
        Actual = value;
    }

    [PublicAPI]
    public byte Hundreds
    {
        get
        {
            if (!_hundreds.HasValue)
            {
                Parse();
            }

            return _hundreds!.Value;
        }
    }
    
    [PublicAPI]
    public Hundred Hundred
    {
        get
        {
            if (!_hundred.HasValue)
            {
                Parse();
            }

            return _hundred!.Value;
        }
    }
    
    public ushort Actual { get; }
    
    [PublicAPI]
    public byte Scale { get; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        AppendToText(builder);

        return builder.ToString();
    }

    public void AppendToText(StringBuilder accumulator)
    {
        if (Actual == 0)
        {
            return;
        }

        var hundredsAdded = false;
        if (Hundreds != 0)
        {
            accumulator.Append(Vocabulary.OneToTwentyNumbersTexts[Hundreds - 1]);
            accumulator.Append(' ');
            accumulator.Append(Vocabulary.HundredText);
            hundredsAdded = true;
        }

        if (Hundred.Actual > 0)
        {
            if (hundredsAdded)
            {
                accumulator.Append(' ');
            }

            Hundred.AppendToText(accumulator);
        }
        
        if (Scale > 0)
        {
            accumulator.Append(' ');
            accumulator.Append(Vocabulary.ThousandsTexts[Scale - 1]);
        }
    }

    private void Parse()
    {
        _hundreds = (byte) (Actual / OneHundredScale);
        var belowHundred = (byte) (Actual - _hundreds.Value * OneHundredScale);
        _hundred = new Hundred(belowHundred);
    }
}
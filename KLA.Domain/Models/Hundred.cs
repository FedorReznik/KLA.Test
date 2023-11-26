using System.Text;
using JetBrains.Annotations;

namespace KLA.Domain.Models;

public struct Hundred
{
    private const byte MaxValue = 99;
    private const byte TenScale = 10;
    
    private byte? _tenths = null;
    private byte? _digits = null;

    public Hundred(byte value)
    {
        if(value > MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(Hundred)} can only accept values < {MaxValue + 1}, but received {value}");
        
        Actual = value;
    }

    [PublicAPI]
    public byte Tenths
    {
        get
        {
            if (!_tenths.HasValue)
            {
                Parse();
            }

            return _tenths!.Value;
        }
    }

    [PublicAPI]
    public byte Digits
    {
        get
        {
            if (!_digits.HasValue)
            {
                Parse();
            }

            return _digits!.Value;
        }
    }
    
    public byte Actual { get; }
    
    public override string ToString()
    {
        var builder = new StringBuilder();
        AppendToText(builder);

        return builder.ToString();
    }

    public void AppendToText(StringBuilder accumulator)
    {
        if(Actual == 0)
        {
            return;
        }

        if (Actual < 20)
        {
            accumulator.Append(Vocabulary.OneToTwentyNumbersTexts[Actual - 1]);
        }
        else
        {
            var tenthsAppended = false;
            if (Tenths > 0)
            {
                accumulator.Append(Vocabulary.TenthsTexts[Tenths - 1]);
                tenthsAppended = true;
            }

            if (Digits > 0)
            {
                if (tenthsAppended)
                {
                    accumulator.Append('-');
                }

                accumulator.Append(Vocabulary.OneToTwentyNumbersTexts[Digits - 1]);
            }
        }
    }
    
    private void Parse()
    {
        _tenths = (byte) (Actual / TenScale);
        _digits = (byte) (Actual - _tenths.Value * TenScale);
    }
}
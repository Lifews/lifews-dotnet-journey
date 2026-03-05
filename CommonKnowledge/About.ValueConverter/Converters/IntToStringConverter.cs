using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace About.ValueConverter;

public class IntToStringConverter : BaseValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double f)
        {
            var n = (int)f;
            if (culture.TwoLetterISOLanguageName == "en")
            {
                switch (n)
                {
                    case 1:
                        return "One";
                    case 2:
                        return "Two";
                }
            }
            else if (culture.TwoLetterISOLanguageName == "zh")
            {
                switch (n)
                {
                    case 1:
                        return "一";
                    case 2:
                        return "二";
                }
            }
        }
        return Binding.DoNothing;
    }
}

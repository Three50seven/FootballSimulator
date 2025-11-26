using System;
using System.Globalization;
using System.Text;

namespace Common.AspNetCore
{
    /// <summary>
    /// Class with helper functions to work with MomentJS
    /// Reference: http://stackoverflow.com/questions/30572317/how-can-i-convert-date-time-format-string-used-by-c-sharp-to-the-format-used-by 
    /// </summary>
    public static class MomentJSHelper
    {
        /// <summary>
        /// Translate a dotnet datetime format string to a MomentJS format string<br/>
        /// Restriction:
        /// Fractional Seconds Lowecase F and Uppercase F are difficult to translate to MomentJS, so closest 
        /// translation used
        /// </summary>
        /// <param name="fText">The dotnet datetime format string to convert</param>
        /// <param name="fTolerant">If true, some cases where there is not exact equivalent in MomentJs is 
        /// handled by generating a similar format instead of throwing a UnsupportedFormatException exception.</param>
        /// <param name="fCulture">The Culture to use. If none, the current Culture is used</param>
        /// <returns>A format string to be used in MomentJS</returns>
        /// <exception cref="NotSupportedException">Conversion fails because we have an element in the format string
        /// that has no equivalent in MomentJS</exception>
        /// <exception cref="FormatException">fText is no valid DateTime format string in dotnet</exception>
        /// <exception cref="ArgumentNullException">fText is null</exception>
        public static string GenerateMomentJSFormatString(string fText, bool fTolerant = true, CultureInfo fCulture = null)
        {
            fCulture ??= CultureInfo.CurrentCulture;

            ArgumentNullException.ThrowIfNull(fText);

            return fText.Length switch
            {
                0 => "",
                1 => GenerateMomentJSFormatStringFromStandardFormat(fText, fTolerant, fCulture),
                _ => GenerateMomentJSFormatStringFromUserFormat(fText, fTolerant, fCulture)
            };
        }

        /// <summary>
        /// State of StateMachine while scanning Format DateTime string.
        /// </summary>
        private enum State
        {
            None,
            LowerD1,
            LowerD2,
            LowerD3,
            LowerD4,
            LowerF1,
            LowerF2,
            LowerF3,
            LowerF4,
            LowerF5,
            LowerF6,
            LowerF7,
            CapitalF1,
            CapitalF2,
            CapitalF3,
            CapitalF4,
            CapitalF5,
            CapitalF6,
            CapitalF7,
            LowerG,
            LowerH1,
            LowerH2,
            CapitalH1,
            CapitalH2,
            CapitalK,
            LowerM1,
            LowerM2,
            CapitalM1,
            CapitalM2,
            CapitalM3,
            CapitalM4,
            LowerS1,
            LowerS2,
            LowerT1,
            LowerT2,
            LowerY1,
            LowerY2,
            LowerY3,
            LowerY4,
            LowerY5,
            LowerZ1,
            LowerZ2,
            LowerZ3,
            InSingleQuoteLiteral,
            InDoubleQuoteLiteral,
            EscapeSequence
        }

        private static string GenerateMomentJSFormatStringFromUserFormat(string fText, bool fTolerant, CultureInfo fCulture)
        {
            StringBuilder lResult = new();

            State lState = State.None;
            StringBuilder lTokenBuffer = new();

            var ChangeState = new Action<State>((State fNewState) =>
            {
                switch (lState)
                {
                    case State.LowerD1:
                        lResult.Append('D');
                        break;
                    case State.LowerD2:
                        lResult.Append("DD");
                        break;
                    case State.LowerD3:
                        lResult.Append("ddd");
                        break;
                    case State.LowerD4:
                        lResult.Append("dddd");
                        break;
                    case State.LowerF1:
                    case State.CapitalF1:
                        lResult.Append('S');
                        break;
                    case State.LowerF2:
                    case State.CapitalF2:
                        lResult.Append("SS");
                        break;
                    case State.LowerF3:
                    case State.CapitalF3:
                        lResult.Append("SSS");
                        break;
                    case State.LowerF4:
                    case State.CapitalF4:
                        lResult.Append("SSSS");
                        break;
                    case State.LowerF5:
                    case State.CapitalF5:
                        lResult.Append("SSSSS");
                        break;
                    case State.LowerF6:
                    case State.CapitalF6:
                        lResult.Append("SSSSSS");
                        break;
                    case State.LowerF7:
                    case State.CapitalF7:
                        lResult.Append("SSSSSSS");
                        break;
                    case State.LowerG:
                        throw new NotSupportedException("Era not supported in MomentJS");
                    case State.LowerH1:
                        lResult.Append('h');
                        break;
                    case State.LowerH2:
                        lResult.Append("hh");
                        break;
                    case State.CapitalH1:
                        lResult.Append('H');
                        break;
                    case State.CapitalH2:
                        lResult.Append("HH");
                        break;
                    case State.LowerM1:
                        lResult.Append('m');
                        break;
                    case State.LowerM2:
                        lResult.Append("mm");
                        break;
                    case State.CapitalM1:
                        lResult.Append('M');
                        break;
                    case State.CapitalM2:
                        lResult.Append("MM");
                        break;
                    case State.CapitalM3:
                        lResult.Append("MMM");
                        break;
                    case State.CapitalM4:
                        lResult.Append("MMMM");
                        break;
                    case State.LowerS1:
                        lResult.Append('s');
                        break;
                    case State.LowerS2:
                        lResult.Append("ss");
                        break;
                    case State.LowerT1:
                        if (fTolerant)
                            lResult.Append('A');
                        else
                            throw new NotSupportedException("Single Letter AM/PM not supported in MomentJS");
                        break;
                    case State.LowerT2:
                        lResult.Append('A');
                        break;
                    case State.LowerY1:
                        if (fTolerant)
                        {
                            lResult.Append("YY");
                        }
                        else
                        {
                            throw new NotSupportedException("Single Letter Year not supported in MomentJS");
                        }
                        break;
                    case State.LowerY2:
                        lResult.Append("YY");
                        break;
                    case State.LowerY3:
                        if (fTolerant)
                        {
                            lResult.Append("YYYY");
                        }
                        else
                        {
                            throw new NotSupportedException("Three Letter Year not supported in MomentJS");
                        }
                        break;
                    case State.LowerY4:
                        lResult.Append("YYYY");
                        break;
                    case State.LowerY5:
                        if (fTolerant)
                        {
                            lResult.Append('Y');
                        }
                        else
                        {
                            throw new NotSupportedException("Five or more Letter Year not supported in MomentJS");
                        }
                        break;
                    case State.LowerZ1:
                    case State.LowerZ2:
                        if (fTolerant)
                        {
                            lResult.Append("ZZ");
                        }
                        else
                        {
                            throw new NotSupportedException("Hours offset not supported in MomentJS");
                        }
                        break;
                    case State.LowerZ3:
                        lResult.Append('Z');
                        break;
                    case State.InSingleQuoteLiteral:
                    case State.InDoubleQuoteLiteral:
                    case State.EscapeSequence:
                        foreach (var lCharacter in lTokenBuffer.ToString())
                        {
                            lResult.Append("[" + lCharacter + "]");
                        }
                        break;
                }

                lTokenBuffer.Clear();
                lState = fNewState;
            }); // End ChangeState

            foreach (var character in fText)
            {
                if (lState == State.EscapeSequence)
                {
                    lTokenBuffer.Append(character);
                    ChangeState(State.None);
                }
                else if (lState == State.InDoubleQuoteLiteral)
                {
                    if (character == '\"')
                    {
                        ChangeState(State.None);
                    }
                    else
                    {
                        lTokenBuffer.Append(character);
                    }
                }
                else if (lState == State.InSingleQuoteLiteral)
                {
                    if (character == '\'')
                    {
                        ChangeState(State.None);
                    }
                    else
                    {
                        lTokenBuffer.Append(character);
                    }
                }
                else
                {
                    switch (character)
                    {
                        case 'd':
                            switch (lState)
                            {
                                case State.LowerD1:
                                    lState = State.LowerD2;
                                    break;
                                case State.LowerD2:
                                    lState = State.LowerD3;
                                    break;
                                case State.LowerD3:
                                    lState = State.LowerD4;
                                    break;
                                case State.LowerD4:
                                    break;
                                default:
                                    ChangeState(State.LowerD1);
                                    break;
                            }
                            break;
                        case 'f':
                            switch (lState)
                            {
                                case State.LowerF1:
                                    lState = State.LowerF2;
                                    break;
                                case State.LowerF2:
                                    lState = State.LowerF3;
                                    break;
                                case State.LowerF3:
                                    lState = State.LowerF4;
                                    break;
                                case State.LowerF4:
                                    lState = State.LowerF5;
                                    break;
                                case State.LowerF5:
                                    lState = State.LowerF6;
                                    break;
                                case State.LowerF6:
                                    lState = State.LowerF7;
                                    break;
                                case State.LowerF7:
                                    break;
                                default:
                                    ChangeState(State.LowerF1);
                                    break;
                            }
                            break;
                        case 'F':
                            switch (lState)
                            {
                                case State.CapitalF1:
                                    lState = State.CapitalF2;
                                    break;
                                case State.CapitalF2:
                                    lState = State.CapitalF3;
                                    break;
                                case State.CapitalF3:
                                    lState = State.CapitalF4;
                                    break;
                                case State.CapitalF4:
                                    lState = State.CapitalF5;
                                    break;
                                case State.CapitalF5:
                                    lState = State.CapitalF6;
                                    break;
                                case State.CapitalF6:
                                    lState = State.CapitalF7;
                                    break;
                                case State.CapitalF7:
                                    break;
                                default:
                                    ChangeState(State.CapitalF1);
                                    break;
                            }
                            break;
                        case 'g':
                            switch (lState)
                            {
                                case State.LowerG:
                                    break;
                                default:
                                    ChangeState(State.LowerG);
                                    break;
                            }
                            break;
                        case 'h':
                            switch (lState)
                            {
                                case State.LowerH1:
                                    lState = State.LowerH2;
                                    break;
                                case State.LowerH2:
                                    break;
                                default:
                                    ChangeState(State.LowerH1);
                                    break;
                            }
                            break;
                        case 'H':
                            switch (lState)
                            {
                                case State.CapitalH1:
                                    lState = State.CapitalH2;
                                    break;
                                case State.CapitalH2:
                                    break;
                                default:
                                    ChangeState(State.CapitalH1);
                                    break;
                            }
                            break;
                        case 'K':
                            ChangeState(State.None);
                            if (fTolerant)
                            {
                                lResult.Append('Z');
                            }
                            else
                            {
                                throw new NotSupportedException("TimeZoneInformation not supported in MomentJS");
                            }
                            break;
                        case 'm':
                            switch (lState)
                            {
                                case State.LowerM1:
                                    lState = State.LowerM2;
                                    break;
                                case State.LowerM2:
                                    break;
                                default:
                                    ChangeState(State.LowerM1);
                                    break;
                            }
                            break;
                        case 'M':
                            switch (lState)
                            {
                                case State.CapitalM1:
                                    lState = State.CapitalM2;
                                    break;
                                case State.CapitalM2:
                                    lState = State.CapitalM3;
                                    break;
                                case State.CapitalM3:
                                    lState = State.CapitalM4;
                                    break;
                                case State.CapitalM4:
                                    break;
                                default:
                                    ChangeState(State.CapitalM1);
                                    break;
                            }
                            break;
                        case 's':
                            switch (lState)
                            {
                                case State.LowerS1:
                                    lState = State.LowerS2;
                                    break;
                                case State.LowerS2:
                                    break;
                                default:
                                    ChangeState(State.LowerS1);
                                    break;
                            }
                            break;
                        case 't':
                            switch (lState)
                            {
                                case State.LowerT1:
                                    lState = State.LowerT2;
                                    break;
                                case State.LowerT2:
                                    break;
                                default:
                                    ChangeState(State.LowerT1);
                                    break;
                            }
                            break;
                        case 'y':
                            switch (lState)
                            {
                                case State.LowerY1:
                                    lState = State.LowerY2;
                                    break;
                                case State.LowerY2:
                                    lState = State.LowerY3;
                                    break;
                                case State.LowerY3:
                                    lState = State.LowerY4;
                                    break;
                                case State.LowerY4:
                                    lState = State.LowerY5;
                                    break;
                                case State.LowerY5:
                                    break;
                                default:
                                    ChangeState(State.LowerY1);
                                    break;
                            }
                            break;
                        case 'z':
                            switch (lState)
                            {
                                case State.LowerZ1:
                                    lState = State.LowerZ2;
                                    break;
                                case State.LowerZ2:
                                    lState = State.LowerZ3;
                                    break;
                                case State.LowerZ3:
                                    break;
                                default:
                                    ChangeState(State.LowerZ1);
                                    break;
                            }
                            break;
                        case ':':
                            ChangeState(State.None);
                            lResult.Append("[" + fCulture.DateTimeFormat.TimeSeparator + "]");
                            break;
                        case '/':
                            ChangeState(State.None);
                            lResult.Append("[" + fCulture.DateTimeFormat.DateSeparator + "]");
                            break;
                        case '\"':
                            ChangeState(State.InDoubleQuoteLiteral);
                            break;
                        case '\'':
                            ChangeState(State.InSingleQuoteLiteral);
                            break;
                        case '%':
                            ChangeState(State.None);
                            break;
                        case '\\':
                            ChangeState(State.EscapeSequence);
                            break;
                        default:
                            ChangeState(State.None);
                            lResult.Append("[" + character + "]");
                            break;
                    }
                }
            }

            if (lState == State.EscapeSequence || lState == State.InDoubleQuoteLiteral || lState == State.InSingleQuoteLiteral)
            {
                throw new FormatException("Invalid Format String");
            }

            ChangeState(State.None);


            return lResult.ToString();
        }

        private static string GenerateMomentJSFormatStringFromStandardFormat(string fText, bool fTolerant, CultureInfo fCulture)
        {
            string result = fText switch
            {
                "d" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.ShortDatePattern, fTolerant, fCulture),
                "D" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.LongDatePattern, fTolerant, fCulture),
                "f" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.LongDatePattern + " " + fCulture.DateTimeFormat.ShortTimePattern, fTolerant, fCulture),
                "F" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.FullDateTimePattern, fTolerant, fCulture),
                "g" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.ShortDatePattern + " " + fCulture.DateTimeFormat.ShortTimePattern, fTolerant, fCulture),
                "G" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.ShortDatePattern + " " + fCulture.DateTimeFormat.LongTimePattern, fTolerant, fCulture),
                "M" or "m" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.MonthDayPattern, fTolerant, fCulture),
                "O" or "o" => GenerateMomentJSFormatStringFromUserFormat("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK", fTolerant, fCulture),
                "R" or "r" => throw new NotSupportedException("RFC 1123 not supported  in MomentJS"),
                "s" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.SortableDateTimePattern, fTolerant, fCulture),
                "t" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.ShortTimePattern, fTolerant, fCulture),
                "T" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.LongTimePattern, fTolerant, fCulture),
                "u" => throw new NotSupportedException("Universal Sortable Format not supported in MomentJS"),
                "U" => throw new NotSupportedException("Universal Fulll Format not supported in MomentJS"),
                "Y" or "y" => GenerateMomentJSFormatStringFromUserFormat(fCulture.DateTimeFormat.YearMonthPattern, fTolerant, fCulture),
                _ => throw new FormatException("Unknown Standard DateTime Format"),
            };
            return result;
        }
    }
}

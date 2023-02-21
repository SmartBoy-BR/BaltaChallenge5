using System;
using System.Collections.Generic;

public class NumberToWordsConverter
{
    private Dictionary<int, string> _units = new Dictionary<int, string>
    {
        {0, "zero" },
        {1, "um" },
        {2, "dois" },
        {3, "três" },
        {4, "quatro" },
        {5, "cinco" },
        {6, "seis" },
        {7, "sete" },
        {8, "oito" },
        {9, "nove" }
    };

    private Dictionary<int, string> _teens = new Dictionary<int, string>
    {
        {10, "dez" },
        {11, "onze" },
        {12, "doze" },
        {13, "treze" },
        {14, "quatorze" },
        {15, "quinze" },
        {16, "dezesseis" },
        {17, "dezessete" },
        {18, "dezoito" },
        {19, "dezenove" }
    };

    private Dictionary<int, string> _tens = new Dictionary<int, string>
    {
        {2, "vinte" },
        {3, "trinta" },
        {4, "quarenta" },
        {5, "cinquenta" },
        {6, "sessenta" },
        {7, "setenta" },
        {8, "oitenta" },
        {9, "noventa" }
    };

    private Dictionary<int, string> _hundreds = new Dictionary<int, string>
    {
        {1, "cento" },
        {2, "duzentos" },
        {3, "trezentos" },
        {4, "quatrocentos" },
        {5, "quinhentos" },
        {6, "seiscentos" },
        {7, "setecentos" },
        {8, "oitocentos" },
        {9, "novecentos" }
    };

    private string[] _groups = { "", "mil", "milhões", "bilhões" };

    public string WriteNumberInFull(decimal value)
    {
        if (value < 0 || value >= 1_000_000_000_000)
        {
            throw new ArgumentException("O valor deve estar entre 0 e 999.999.999.999.");
        }

        if (value == 0)
        {
            return $"{_units[0]} reais";
        }

        long integerValue = (long)Math.Floor(value);
        int decimalValue = (int)((value - integerValue) * 100);

        string integerPartInFull = "";

        if (integerValue >= 1000)
        {
            int group = 0;

            while (integerValue > 0)
            {
                long currentGroup = integerValue % 1000;
                integerValue = (long)Math.Floor(integerValue / 1000.0);

                string groupInFull = GetGroupInFull((int)currentGroup);
                if (!string.IsNullOrWhiteSpace(groupInFull))
                {
                    if (group > 0)
                    {
                        if (currentGroup == 1) // Valor
                        {
                            if (group == 1) // Grupo dos milhares
                            {
                                if (integerPartInFull != "")
                                    integerPartInFull = $"{_groups[group]} {integerPartInFull}";
                                else
                                    integerPartInFull = $"{_groups[group]} reais";
                            }
                            else // Outros grupos
                            {
                                if (integerPartInFull != "")
                                    integerPartInFull = $"{groupInFull} {_groups[group].Replace("ões", "ão")} {integerPartInFull}";
                                else
                                    integerPartInFull = $"{groupInFull} {_groups[group].Replace("ões", "ão")} de reais";
                            }
                        }
                        else
                        {
                            if (integerPartInFull != "")
                            {
                                integerPartInFull = $"{groupInFull} {_groups[group]} {integerPartInFull}";
                            }
                            else
                            {
                                if (group == 1) // Grupo dos milhares
                                    integerPartInFull = $"{groupInFull} {_groups[group]} reais";
                                else
                                    integerPartInFull = $"{groupInFull} {_groups[group]} de reais";
                            }
                        }
                    }
                    else
                    {
                        if (currentGroup == 1) // Valor
                            integerPartInFull = $"{groupInFull} real";
                        else
                            integerPartInFull = $"{groupInFull} reais";
                    }
                }

                group++;
            }
        }
        else
        {
            if (integerValue == 1) integerPartInFull = $"{GetGroupInFull((int)integerValue)} real";
            else integerPartInFull = $"{GetGroupInFull((int)integerValue)} reais";
        }

        string result = integerPartInFull;

        if (decimalValue > 0)
        {
            if (decimalValue == 1) result = $"{result} e {GetDecimalInFull(decimalValue)} centavo";
            else result = $"{result} e {GetDecimalInFull(decimalValue)} centavos";
        }

        return result.Trim();
    }

    private string GetDecimalInFull(int value)
    {
        if (value >= 1_000_000_000)
        {
            throw new ArgumentException("O valor deve ser menor que 1 bilhão.");
        }

        string valueInFull = "";

        if (value < 10)
        {
            valueInFull = _units[value];
        }
        else if (value < 20)
        {
            valueInFull = _teens[value];
        }
        else
        {
            int units = value % 10;
            int tens = value / 10;

            valueInFull = $"{_tens[tens]}";
            if (units > 0)
            {
                valueInFull += $" e {_units[units]}";
            }
        }

        return valueInFull;
    }


    private string GetUnitsInFull(int value)
    {
        if (value >= 100)
        {
            throw new ArgumentException("O valor deve ser menor que 100.");
        }

        if (value == 0)
        {
            return "";
        }

        return (string)_units[value];
    }

    private string GetTensInFull(int value)
    {
        if (value >= 100)
        {
            throw new ArgumentException("O valor deve ser menor que 100.");
        }

        if (value < 10)
        {
            return GetUnitsInFull(value);
        }
        else if (value < 20)
        {
            return _teens[value];
        }
        else
        {
            int quotient = value / 10;
            int remainder = value % 10;

            string tensInFull = (string)_tens[quotient];

            if (remainder > 0)
            {
                tensInFull += $" e {GetUnitsInFull(remainder)}";
            }

            return tensInFull;
        }
    }

    private string GetHundredsInFull(int value)
    {
        if (value >= 1000)
        {
            throw new ArgumentException("O valor deve ser menor que 1000.");
        }

        if (value < 100)
        {
            return GetTensInFull(value);
        }
        else
        {
            int quotient = value / 100;
            int remainder = value % 100;

            string hundredsInFull = _hundreds[quotient];

            if (remainder > 0)
            {
                hundredsInFull += $" e {GetTensInFull(remainder)}";
            }

            return hundredsInFull;
        }
    }

    private string GetGroupInFull(int value)
    {
        if (value >= 1000)
        {
            throw new ArgumentException("O valor deve ser menor que 1000.");
        }

        if (value < 100)
        {
            return GetTensInFull(value);
        }
        else
        {
            return GetHundredsInFull(value);
        }
    }
}




using Microsoft.Win32;
using System.Collections.Generic;

namespace Intel8086
{
    internal class Program
    {
        static string[,] registers = new string[8, 2] {
            { "AL", "" },
            { "AH", "" },
            { "BL", "" },
            { "BH", "" },
            { "CL", "" },
            { "CH", "" },
            { "DL", "" },
            { "DH", "" }
        };
        static void Main(string[] args)
        {           
            RegInit(registers);
            Methods(registers);
        }

        static void RegInit(string[,] registers)
        {
            bool isReg = false;
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                do
                {
                    Console.Write($"Podaj zawartość rejestru {registers[i, 0]}: ");
                    registers[i, 1] = Console.ReadLine().ToUpper();

                    if (IsHex(registers[i, 1]) && IsTwoValues(registers[i, 1]))
                    {
                        isReg = true;
                    }
                    else if (IsTwoValues(registers[i, 1]) == false)
                    {
                        Console.WriteLine("\nZa dużo lub za mało wartości wprowadź dokładnie dwie wartości.\n");
                        isReg = false;
                    }
                    else if (IsHex(registers[i, 1]) == false)
                    {
                        Console.WriteLine("\nTo nie liczba heksadecymalna! Możesz wybrać tylko te wartości: '0123456789ABCDEF'.\n");
                        isReg = false;
                    }
                } while (!isReg);
            }
        }
        static void Methods(string[,] registers)
        {
            Console.WriteLine("\nAktualny stan rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nWybierz numer metody, którą chcesz zastosować:");
            Console.WriteLine("1 -> MOV");
            Console.WriteLine("2 -> XCHG");
            Console.WriteLine("3 -> INC");
            Console.WriteLine("4 -> DEC");
            Console.WriteLine("5 -> NOT");
            Console.WriteLine("6 -> OR");
            Console.WriteLine("7 -> XOR");
            Console.WriteLine("8 -> ADD");
            Console.WriteLine("9 -> SUB");
            Console.WriteLine("10 -> AND\n");

            string method = Console.ReadLine();
            int numberMethod;
            int number;
            if (int.TryParse(method, out number))
            {
                numberMethod = number;
            }
            else
            {
                numberMethod = 0;
                Console.WriteLine("Podana wartość nie była liczbą więc przypisano 0, które nie jest numerem żadnej metody.");
            }

            if (numberMethod == 1 || numberMethod == 2 || numberMethod == 6 || numberMethod == 7 ||
                numberMethod == 8 || numberMethod == 9 || numberMethod == 10)
            {
                switch (numberMethod)
                {
                    case 1:
                        MOV(registers);
                        break;
                    case 2:
                        XCHG(registers);
                        break;
                    case 6:
                        OR(registers);
                        break;
                    case 7:
                        XOR(registers);
                        break;
                    case 8:
                        ADD(registers);
                        break;
                    case 9:
                        SUB(registers);
                        break;
                    case 10:
                        AND(registers);
                        break;
                }

            }
            else if (numberMethod == 3 || numberMethod == 4 || numberMethod == 5)
            {
                switch (numberMethod)
                {
                    case 3:
                        INC(registers);
                        break;
                    case 4:
                        DEC(registers);
                        break;
                    case 5:
                        NOT(registers);
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nTo nie jest poprawny numer metody!");
                Methods(registers);
            }
            Console.ReadKey();
        }
        static bool IsTwoValues(string register)
        {
            if (register.Length != 2)
            {
                return false;
            }
            return true;
        }
        static bool IsHex(string register)
        {
            int i = 0;
            bool isHex = true;
            foreach (char reg in register)
            {
                if (reg >= 'A' && reg <= 'F' || reg >= '0' && reg <= '9')
                {
                    isHex = true;
                    i++;
                    if (i != 2)
                    {
                        isHex = false;
                    }
                }
                else
                {
                    isHex = false;
                }
            }
            return isHex;
        }
        static bool IsReg(string reg)
        {
            string[] regs = { "AL", "AH", "BL", "BH", "CL", "CH", "DL", "DH" };

            if (regs.Contains(reg))
            {
                return true;
            }
            return false;
        }
        static string FirstValue()
        {
            string first;
            do
            {
                Console.WriteLine("\nWybierz rejestr pierwszy: ");
                first = Console.ReadLine().ToUpper();
                if (IsReg(first))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nTo nie jest nazwa rejestru! Spróbuj ponownie.");
                }
            } while (!IsReg(first));
            return first;
        }
        static string SecondValue(string first)
        {
            bool isReg;
            string second;
            do
            {
                Console.WriteLine("\nWybierz rejestr drugi: ");
                second = Console.ReadLine().ToUpper();
                if (IsReg(second) && (first != second))
                {
                    isReg = true;
                }
                else if (first == second)
                {
                    Console.WriteLine("\nNie można wybrać dwóch takich samych rejestrów.");
                    isReg = false;
                }
                else
                {
                    Console.WriteLine("\nTo nie jest nazwa rejestru! Spróbuj ponownie.");
                    isReg = false;
                }
            } while (!isReg);
            return second;
        }
        static void MOV(string[,] registers)
        {
            Console.WriteLine("\nMetoda MOV:");
            string regValue = "";
            string first = FirstValue();
            string second = SecondValue(first);

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(second))
                {
                    regValue = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    registers[i, 1] = regValue;
                }
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość zmienionych rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void XCHG(string[,] registers)
        {
            Console.WriteLine("\nMetoda XCHG:");
            string regValue1 = "";
            string regValue2 = "";
            string first = FirstValue();
            string second = SecondValue(first);

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    regValue1 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(second))
                {
                    regValue2 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    registers[i, 1] = regValue2;
                }

                if (registers[i, 0].Contains(second))
                {
                    registers[i, 1] = regValue1;
                }
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość zmienionych rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void INC(string[,] registers)
        {
            Console.WriteLine("\nMetoda INC:");
            string regValue = "";
            int hex;
            string first = FirstValue();

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    regValue = registers[i, 1];
                    hex = Convert.ToInt32(regValue, 16);

                    if (hex != 255)
                    {
                        hex++;
                    }
                    else
                    {
                        Console.WriteLine("\nNie można wykonać metody gdy zawartość rejestru jest równa 255.");
                        Methods(registers);
                    }
                    if (hex <= 15)
                    {
                        regValue = "0" + $"{hex:X}".ToUpper();
                    }
                    else
                    {
                        regValue = $"{hex:X}".ToUpper();
                    }
                    registers[i, 1] = regValue;
                }
            }
            Console.WriteLine($"\nZawartość zmienionego rejestru: {regValue}\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void DEC(string[,] registers)
        {
            Console.WriteLine("\nMetoda DEC:");
            string regValue = "";
            int hex;
            string first = FirstValue();

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    regValue = registers[i, 1];
                    hex = Convert.ToInt32(regValue, 16);
                    if (hex != 0)
                    {
                        hex--;
                    }
                    else
                    {
                        Console.WriteLine("\nNie można wykonać metody gdy zawartość rejestru jest równa 0.");
                        Methods(registers);
                    }
                    if (hex <= 15)
                    {
                        regValue = "0" + $"{hex:X}".ToUpper();
                    }
                    else
                    {
                        regValue = $"{hex:X}".ToUpper();
                    }
                    registers[i, 1] = regValue;
                }
            }
            Console.WriteLine($"\nZawartość zmienionego rejestru: {regValue}\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void AND(string[,] registers)
        {
            Console.WriteLine("\nMetoda AND:");
            string regValue1 = "";
            string regValue2 = "";
            string first = FirstValue();
            string second = SecondValue(first);
            int hex1;
            int hex2;
            int value;

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    regValue1 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(second))
                {
                    regValue2 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    hex1 = Convert.ToInt32(regValue1, 16);
                    hex2 = Convert.ToInt32(regValue2, 16);

                    value = hex1 & hex2;
                    if (value <= 15)
                    {
                        regValue1 = "0" + $"{value:X}".ToUpper();
                    }
                    else
                    {
                        regValue1 = $"{value:X}".ToUpper();
                    }
                    registers[i, 1] = regValue1;
                }
            }
            Console.WriteLine($"\nZawartość zmienionego rejestru: {regValue1}\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void NOT(string[,] registers)
        {
            Console.WriteLine("\nMetoda NOT:");
            string regValue = "";
            int hex;
            string first = FirstValue();

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    regValue = registers[i, 1];
                    hex = Convert.ToInt32(regValue, 16);

                    hex = ~hex;

                    regValue = $"{hex:X}".ToUpper();
                    registers[i, 1] = regValue.Substring(6, 2);
                }
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void OR(string[,] registers)
        {
            Console.WriteLine("\nMetoda OR:");
            string regValue1 = "";
            string regValue2 = "";
            string first = FirstValue();
            string second = SecondValue(first);
            int hex1;
            int hex2;
            int value;

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    regValue1 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(second))
                {
                    regValue2 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    hex1 = Convert.ToInt32(regValue1, 16);
                    hex2 = Convert.ToInt32(regValue2, 16);

                    value = hex1 | hex2;
                    if (value <= 15)
                    {
                        regValue1 = "0" + $"{value:X}".ToUpper();
                    }
                    else
                    {
                        regValue1 = $"{value:X}".ToUpper();
                    }
                    registers[i, 1] = regValue1;
                }
            }
            Console.WriteLine($"\nZawartość zmienionego rejestru: {regValue1}\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void XOR(string[,] registers)
        {
            Console.WriteLine("\nMetoda XOR:");
            string regValue1 = "";
            string regValue2 = "";
            string first = FirstValue();
            string second = SecondValue(first);
            int hex1;
            int hex2;
            int value;

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    regValue1 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(second))
                {
                    regValue2 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    hex1 = Convert.ToInt32(regValue1, 16);
                    hex2 = Convert.ToInt32(regValue2, 16);

                    value = hex1 ^ hex2;
                    if (value <= 15)
                    {
                        regValue1 = "0" + $"{value:X}".ToUpper();
                    }
                    else
                    {
                        regValue1 = $"{value:X}".ToUpper();
                    }
                    registers[i, 1] = regValue1;
                }
            }
            Console.WriteLine($"\nZawartość zmienionego rejestru: {regValue1}\n");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void ADD(string[,] registers)
        {
            Console.WriteLine("\nMetoda ADD:");
            string regValue1 = "";
            string regValue2 = "";
            string first = FirstValue();
            string second = SecondValue(first);
            int hex1;
            int hex2;
            int value;

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    regValue1 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(second))
                {
                    regValue2 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    hex1 = Convert.ToInt32(regValue1, 16);
                    hex2 = Convert.ToInt32(regValue2, 16);

                    value = hex1 + hex2;
                    if (value <= 15)
                    {
                        regValue1 = "0" + $"{value:X}".ToUpper();
                    } else if(value > 255)
                    {
                        regValue1 = "FF";
                    }
                    else
                    {
                        regValue1 = $"{value:X}".ToUpper();
                    }
                    registers[i, 1] = regValue1;
                }
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość zmienionych rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void SUB(string[,] registers)
        {
            Console.WriteLine("\nMetoda SUB:");
            string regValue1 = "";
            string regValue2 = "";
            string first = FirstValue();
            string second = SecondValue(first);
            int hex1;
            int hex2;
            int value;

            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    regValue1 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(second))
                {
                    regValue2 = registers[i, 1];
                }
            }
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                if (registers[i, 0].Contains(first))
                {
                    hex1 = Convert.ToInt32(regValue1, 16);
                    hex2 = Convert.ToInt32(regValue2, 16);

                    value = hex1 - hex2;
                    if (value <= 15 && value > 0)
                    {
                        regValue1 = "0" + $"{value:X}".ToUpper();
                    } else if(value <= 0)
                    {
                        regValue1 = "00";
                    }
                    else
                    {
                        regValue1 = $"{value:X}".ToUpper();
                    }
                    registers[i, 1] = regValue1;
                }
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("\nZawartość zmienionych rejestrów:\n");
            showResults(registers);
            Console.WriteLine("--------------------------------------------------");
            askToContinue(registers);
        }
        static void showResults(string[,] registers)
        {
            for (int i = 0; i < registers.GetLength(0); i++)
            {
                Console.WriteLine($"Zawartość rejestru {registers[i, 0]}: {registers[i, 1]}");
            }
        }
        static void askToContinue(string[,] registers)
        {
            Console.WriteLine("\nCzy chcesz kontynuować?\nt - tak, n - nie");
            string continueProgram = Console.ReadLine();
            if (continueProgram.Equals("t"))
            {
                Console.Clear();
                Methods(registers);
            }
            else
            {
                Console.WriteLine("Dziękuję za skorzystanie z programu i zapraszam ponownie :)");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
/**
This code determines NMI verification using the Luhn algorithm.

The Luhn algorithm, also known as the modulus 10 or mod 10 algorithm, is a simple checksum formula used to validate a variety of identification numbers.

The steps in the checksum formula are as follows:
1. Double the value of alternate digits beginning with the rightmost digit (10th digit for NMIs, then 8th, 6th etc.). 
2. Add the individual digits comprising the products obtained in step 1 to each of the unaffected digits in the original number.
3. Find the next highest multiple of 10
4. The check digit is the value obtained in step 2 subtracted from the value obtained in step 3.
5. END

This script is more or less a like for like translation of the AEMO NMI Checksum Sample Java Code from page 78 of: https://www.aemo.com.au/Electricity/National-Electricity-Market-NEM/Retail-and-metering/-/media/EBA9363B984841079712B3AAD374A859.ashx
**/

namespace NMICheckSumCalculator
{
    public class NMICheckSumCalculator
    {
        public static void Main(string[] args)
        {
            //Set a test list of example NMIs obtained from AEMO NMI Procedure document
            List<string> tests = new List<string>() 
            {
                "2001985732", //8
                "QAAAVZZZZZ", //3
                "2001985733", //6
                "QCDWW00010", //2
                "3075621875", //8
                "SMVEW00085", //8
                "3075621876", //6
                "VAAA000065", //7
                "4316854005", //9
                "VAAA000066" //5
            };

            //Go
            foreach(string nmi in tests) { Console.WriteLine(nmi + @" / " + CalculateNmiCheckSum(nmi)); }
        }

        /// <summary>
        /// Calculate a NMI checksum bit from a 10 digit NMI
        /// </summary>
        /// <param name="nmi"></param>
        /// <returns></returns>
        public static int CalculateNmiCheckSum(string input)
        {
            string nmi = input.ToUpper(); //Uppercase input NMI
            if (Regex.Match(nmi, "^[A-Z0-9]{10}$").Success)
            {
                int value = 0;
                bool multiply = true;
                for (int count = nmi.Length; count > 0; count--)  //Read the NMI character by character starting with the right most character.
                {
                    int digit = nmi[count - 1];
                    if (multiply) { digit *= 2; } //Double the ASCII value.
                    multiply = !multiply;

                    while (digit > 0) //Find the next highest multiple of 10.
                    {
                        value += digit % 10;
                        digit /= 10;
                    }
                }
                return (10 - (value % 10)) % 10;
            }
            else { return -1; } //Invalid input return negative value
        }
    }
}
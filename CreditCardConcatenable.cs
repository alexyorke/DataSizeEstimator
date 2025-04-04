﻿namespace DataSizeEstimator;

class CreditCardConcatenable : IConcatenableType
{
    private readonly Random rndInstance;

    public CreditCardConcatenable(Random rndInstance)
    {
        this.rndInstance = rndInstance ?? Random.Shared;
    }
    public IConcatenableType Concat(IConcatenableType toConcatWith)
    {
        return toConcatWith.Concat(this);
    }

    public object GetValue()
    {
        return RandomCreditCardNumberGenerator.GenerateMasterCardNumber(rndInstance);
    }

    public Type GetUnderlyingType()
    {
        return typeof(string);
    }
}

public static class RandomCreditCardNumberGenerator
{
    /*This is a port of the port of of the Javascript credit card number generator now in C#
    * by Kev Hunter https://kevhunter.wordpress.com
    * See the license below. Obviously, this is not a Javascript credit card number
     generator. However, The following class is a port of a Javascript credit card
     number generator.
     @author robweber
     Javascript credit card number generator Copyright (C) 2006 Graham King
     graham@darkcoding.net
     This program is free software; you can redistribute it and/or modify it
     under the terms of the GNU General Public License as published by the
     Free Software Foundation; either version 2 of the License, or (at your
     option) any later version.
     This program is distributed in the hope that it will be useful, but
     WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General
     Public License for more details.

     You should have received a copy of the GNU General Public License along
     with this program; if not, write to the Free Software Foundation, Inc.,
     51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
     www.darkcoding.net
    */


    public static string[] AMEX_PREFIX_LIST = new[] { "34", "37" };


    public static string[] DINERS_PREFIX_LIST = new[]
                                                    {
                                                            "300",
                                                            "301", "302", "303", "36", "38"
                                                        };


    public static string[] DISCOVER_PREFIX_LIST = new[] { "6011" };


    public static string[] ENROUTE_PREFIX_LIST = new[]
                                                    {
                                                            "2014",
                                                            "2149"
                                                        };


    public static string[] JCB_15_PREFIX_LIST = new[]
                                                    {
                                                            "2100",
                                                            "1800"
                                                        };


    public static string[] JCB_16_PREFIX_LIST = new[]
                                                    {
                                                            "3088",
                                                            "3096", "3112", "3158", "3337", "3528"
                                                        };


    public static string[] MASTERCARD_PREFIX_LIST = new[]
                                                        {
                                                                "51",
                                                                "52", "53", "54", "55"
                                                            };


    public static string[] VISA_PREFIX_LIST = new[]
                                                {
                                                        "4539",
                                                        "4556", "4916", "4532", "4929", "40240071", "4485", "4716", "4"
                                                    };


    public static string[] VOYAGER_PREFIX_LIST = new[] { "8699" };

    private static string CreateFakeCreditCardNumber(string prefix, int length, Random rndInstance)
    {
        string ccnumber = prefix;

        while (ccnumber.Length < (length - 1))
        {
            double rnd = (rndInstance.NextDouble() * 1.0f - 0f);
            ccnumber += Math.Floor(rnd * 10);
        }


        // reverse number and convert to int
        var reversedCCnumberstring = ccnumber.ToCharArray().Reverse();

        var reversedCCnumberList = reversedCCnumberstring.Select(c => Convert.ToInt32(c.ToString()));

        // calculate sum

        int sum = 0;
        int pos = 0;
        int[] reversedCCnumber = reversedCCnumberList.ToArray();

        while (pos < length - 1)
        {
            int odd = reversedCCnumber[pos] * 2;

            if (odd > 9)
                odd -= 9;

            sum += odd;

            if (pos != (length - 2))
                sum += reversedCCnumber[pos + 1];

            pos += 2;
        }

        // calculate check digit
        int checkdigit =
            Convert.ToInt32((Math.Floor((decimal)sum / 10) + 1) * 10 - sum) % 10;

        ccnumber += checkdigit;

        return ccnumber;
    }


    public static IEnumerable<string> GetCreditCardNumbers(string[] prefixList, int length,
                                              int howMany, Random rndInstance)
    {
        var result = new Stack<string>();

        for (int i = 0; i < howMany; i++)
        {
            int randomPrefix = rndInstance.Next(0, prefixList.Length - 1);

            if (randomPrefix > 1)
            {
                randomPrefix--;
            }

            string ccnumber = prefixList[randomPrefix];

            result.Push(CreateFakeCreditCardNumber(ccnumber, length, rndInstance));
        }

        return result;
    }


    public static string GenerateMasterCardNumber(Random rndInstance)
    {
        return GetCreditCardNumbers(MASTERCARD_PREFIX_LIST, 16, 1, rndInstance).First();
    }
}
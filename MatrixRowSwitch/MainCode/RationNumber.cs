using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixRowSwitch.MainCode {
    static class RationNumber {
        static private int GetGCD(int a, int b) {
            while (b != 0) {
                int temp = a;
                a = b;
                b = temp % b;
            }
            return a;
        }
        static private void GetDM(string num, out int mul, out int den) {
            if (num.Contains('/')) {
                int dIndex = num.IndexOf('/');
                mul = Convert.ToInt32(num.Substring(0, dIndex));
                den = Convert.ToInt32(num.Substring(dIndex + 1));
            } else {
                mul = Convert.ToInt32(num);
                den = 1;
            }
        }
        static private string GetNumber(int mul, int den) {
            int gcd = GetGCD(mul, den);
            mul /= gcd;
            den /= gcd;
            if (mul % den == 0) {
                return (mul / den).ToString();
            } else {
                string num = $"{Math.Abs(mul)}/{Math.Abs(den)}";
                if (mul < 0 || den < 0) {
                    num = $"-{num}";
                }
                return num;
            }
        }
        static public string Add(string num1, string num2) {
            int mA, dA, mB, dB;
            int mul, den;
            GetDM(num1, out mA, out dA);
            GetDM(num2, out mB, out dB);
            if (dA == dB) {
                mul = mA + mB;
                den = dA;
            } else {
                mul = mA * dB + mB * dA;
                den = dA * dB;
            }
            return GetNumber(mul, den);
        }
        static public string Mulpitly(string num1, string num2) {
            int mA, dA, mB, dB;
            int mul, den;
            GetDM(num1, out mA, out dA);
            GetDM(num2, out mB, out dB);
            mul = mA * mB;
            den = dA * dB;
            return GetNumber(mul, den);
        }
        static public string Reciprocal(string num) {
            int mul;
            int den;
            GetDM(num, out mul, out den);
            return GetNumber(den, mul);
        }
    }
}

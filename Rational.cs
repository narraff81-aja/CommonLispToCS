using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
    public struct Rational {
        public long Numerator;
        public long Denumerator;

        public Rational(long numerator, long denumerator) {
            long mcd = MCD(numerator, denumerator);
            Numerator = numerator / mcd;
            Denumerator = denumerator / mcd;
        }        
        public static Rational operator +(Rational a, Rational b) {
            //(a/b) + (c/d) = (ad)/(bd) + (cb)/(bd) =  (ad + cb)/(bd)
            return new Rational(a.Numerator * b.Denumerator + b.Numerator * a.Denumerator, a.Denumerator * b.Denumerator);
        }
        public static Rational operator -(Rational a, Rational b) {
            //(a/b) + (c/d) = (ad)/(bd) - (cb)/(bd) =  (ad - cb)/(bd)
            return new Rational(a.Numerator * b.Denumerator - b.Numerator * a.Denumerator, a.Denumerator * b.Denumerator);
        }
        public static Rational operator *(Rational a, Rational b) {
            //(a/b) * (c/d) = (ac)/(bd)
            return new Rational(a.Numerator * b.Numerator, a.Denumerator * b.Denumerator);
        }
        public static Rational operator /(Rational a, Rational b) {
            //(a/b) / (c/d) = (ad)/(bc)
            return new Rational(a.Numerator * b.Denumerator, a.Denumerator * b.Numerator);
        }
        public static Rational operator +(Rational a, long b) {
            //(a/c) + b =  (a + bc)/(c)
            return new Rational(a.Numerator + b * a.Denumerator, a.Denumerator);
        }
        public static Rational operator +(long a,Rational b) {           
            return new Rational(a * b.Denumerator+ b.Numerator, b.Denumerator);
        }
        public static Rational operator -(Rational a, long b) {
            //(a/c) - b =  (a - bc)/(c)
            return new Rational(a.Numerator - b * a.Denumerator, a.Denumerator);
        }
        public static Rational operator -(long a, Rational b) {
            return new Rational(a * b.Denumerator - b.Numerator, b.Denumerator);
        }
        public static Rational operator *(Rational a, long b) {
            //(a/c) * (b) = (ab)/(c)
            return new Rational(a.Numerator * b, a.Denumerator);
        }
        public static Rational operator *(long a,Rational  b) {
            //(a) * (b/c) = (ab)/(c)
            return new Rational(a * b.Numerator, b.Denumerator);
        }
        public static Rational operator /(Rational a, long b) {
            //(a/c) / (b) = (a)/(bc)
            return new Rational(a.Numerator, a.Denumerator * b);
        }
        public static Rational operator /(long a,Rational  b) {
            //(a) / (b/c) = (ac)/(b)
            return new Rational(a * b.Denumerator, b.Numerator );
        }
        public static long MCD(long a, long b) {
            //metodo di Euclide
            long max = Math.Max(a, b);
            long min = Math.Min(a, b);
            long resto = -1;
            do {
                resto = max % min;
                Console.WriteLine($"{max} : {min} = {(long)Math.Floor((decimal)max/min)} e % {resto}");
                max = Math.Max(min, resto);
                min = Math.Min(min, resto);
            } while (resto != 0);
            return max;
        }
    }
}

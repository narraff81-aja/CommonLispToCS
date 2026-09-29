using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
    public class Macros {
        /* public static int next2(int index, int initial, Predicate<int> condition) {
             // TYPE (* integer (integer->boolean)) -> integer
             // First integer greater or equal to $initial$ such that
             // $condition$ holds.
             index = initial;
             if (condition(index)) {
                 return index;
             } else {
                 return next2(index, index + 1, condition);
             }
         }*/
        /// <summary>
        /// First integer greater or equal to $initial$ such that $condition$ holds.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="initial"></param>
        /// <param name="condition">Predicate( index)</param>
        /// <returns></returns>
        public static int next(int index, int initial, Predicate<int> condition) {
            // TYPE (* integer (integer->boolean)) -> integer            
            for (index = initial; !condition(index); index++) {
            }
            return index;
        }

        /* public static int final2(int index, int initial, Predicate<int> condition) {
             // TYPE (* integer (integer->boolean)) -> integer
             // Last integer greater or equal to $initial$ such that
             // $condition$ holds.
             index = initial;
             if (!condition(index)) {
                 return index - 1;
             } else {
                 return final2(index, index + 1, condition);
             }
         }*/
        /// <summary>
        /// Last integer greater or equal to $initial$ such that $condition$ holds.  
        /// </summary>
        /// <param name="index"></param>
        /// <param name="initial"></param>
        /// <param name="condition">Predicate( index)</param>
        /// <returns></returns>
        public static int final(int index, int initial, Predicate<int> condition) {
            // TYPE (* integer (integer->boolean)) -> integer                       
            for (index = initial; condition(index); index++) { // (!(!x)) == x
            }
            return index - 1;
        }
        /// <summary>
        /// Sum $expression$ for $index$ = $initial$ and successive integers, as long as $condition$ holds.
        /// </summary>
        /// <param name="expression">Func( index)</param>
        /// <param name="index"></param>
        /// <param name="initial"></param>
        /// <param name="condition">Predicate( index)</param>
        /// <returns></returns>
        public static long sum(Func<int, long> expression, int index, int initial, Predicate<int> condition) {
            // TYPE ((integer->real) * integer (integer->boolean)) -> real
            long temp = 0;
            index = initial;
            for (temp = 0, index = initial; condition(index); temp += expression(index), index++) {
            }
            return temp;
        }
        /*  public static double sum2(Func<int, double> expression, int index, int initial, Predicate<int> condition) {
              // TYPE ((integer->real) * integer (integer->boolean)) -> real
              // Sum $expression$ for $index$ = $initial$ and successive
              // integers, as long as $condition$ holds.
              double temp = 0;
              index = initial;
              if (!condition(index)) {
                  return temp;
              } else {
                  temp += expression(index);
                  index++;
                  return temp + sum2(expression, index, index + 1, condition);
              }
          }*/
        /// <summary>
        /// Bisection search for $x$ in [$lo$,$hi$] such that $end$ holds. $test$ determines when to go left.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="lo"></param>
        /// <param name="h"></param>
        /// <param name="hi"></param>
        /// <param name="x"></param>
        /// <param name="test">Predicate( x )</param>
        /// <param name="end">Func (l, h)</param>
        /// <returns></returns>
        public static double binary_search(double l, double lo, double h, double hi, double x, Predicate<double> test, Func<double, double, bool> end) {
            // TYPE (* real * real * (real->boolean) ((real real)->boolean)) -> real
            /*(let* ((left (gensym)))
            `(do* ((,x false (/ (+ ,h ,l) 2))
            (,left false ,test)
            (,l ,lo (if ,left ,l ,x))
            (,h ,hi (if ,left ,x ,h)))
            (,end ( ( ,h +,l)/ 2))))*/
            bool left;
            for (x = double.NaN, left = false, l = lo, h = hi;
                !end(l, h); //non sono sicuro !end(h, l)
                x = (h + l) / 2, left = test(x), l = left ? l : x, h = left ? x : h) {
            }
            return (h + l) / 2;
        }

        public static double invert_angular(Func<double, double> f, double y, double a, double b) {
            // TYPE (real->angle real real real) -> real
            // Use bisection to find inverse of angular function
            // $f$ at $y$ within interval [$a$,$b$].
            double varepsilon = 1 / 100000d;// Desired accuracy
            /*( 
            `(binary-search l ,a u ,b x pred pred2)            
            ))*/
            double l = 0, u = 0, x = 0;
            Predicate<double> pred = w => (((f(w) - y) % 360.0) < 180.0);// (< (mod (- (,f x) ,y) 360) (deg 180))
            Func<double, double, bool> pred2 = (m, n) => ((m - n) < varepsilon);// (< (- u l) ,varepsilon)
            //^ (m - n) o (n - m) ???
            return binary_search(l, a, u, b, x, pred, pred2);
        }
        /*  /// <summary>
          /// $list$ is of the form ((i1 l1)..(in ln)). Sum of $body$ for indices i1..in running simultaneously thru lists l1..ln.
          /// </summary>
          /// <param name="list"></param>
          /// <param name="body"></param>
          /// <returns></returns>
          public static double sigma(dynamic list, dynamic body) {
              // TYPE (list-of-pairs (list-of-reals->real)) -> real
              /*`(apply '+ (mapcar 
               *            (function (lambda ,(mapcar 'car list) ,body)) 
               *            ,@(mapcar 'cadr list)
               *           )
               * )* /
              return 0;
          }*/

        /* es (sigma ((x (list 1 2 3 4)) (y (list 5 6 7 8))) (* x y))
         * = 70 
         *  (sigma ((x (list 1 2 3 4)) (y (list 5 6 7 8)) (z (list 2 4 6 8)) ) (- (* x y) z))
         *  = 50
         */

        /*
            //int a = 5;
            //int b = Macros.next(a, 2, (x) => x > 7);
            //Console.WriteLine("next " + a + " " + b);
            //b = Macros.next(a, 2, (x) => x < 7);//con 12 invece di 2,non rischio di loop inf ma int.MinValue
            //Console.WriteLine("next " + a + " " + b);

            //b = Macros.final(a, 2, (x) => x > 7);//con 12 invece di 2,non rischio di loop inf ma int.MaxValue
            //Console.WriteLine("final " + a + " " + b);
            //b = Macros.final(a, 2, (x) => x < 7);
            //Console.WriteLine("final " + a + " " + b);

            //b = Macros.next2(a, 2, (x) => x > 7);
            //Console.WriteLine("next2 " + a + " " + b);
            //b = Macros.next2(a, 2, (x) => x < 7);//con 12 invece di 2, rischio di loop inf
            //Console.WriteLine("next2 " + a + " " + b);

            //b = Macros.final2(a, 2, (x) => x > 7);//con 12 invece di 2, rischio di loop inf
            //Console.WriteLine("final2 " + a + " " + b);
            //b = Macros.final2(a, 2, (x) => x < 7);
            //Console.WriteLine("final2 " + a + " " + b);
            //int a = 2;
            //int inz = 4;
            //double d = Macros.sum(x => x + .1, a, inz, y => y < 30);
            //Console.WriteLine("sum  " + d);
            //d = Macros.sum(x => x + .2, a, 2, y => y < 20);
            //Console.WriteLine("sum  " + d);
            //d = Macros.sum2(x => x + .1, a, inz, y => y < 30);
            //Console.WriteLine("sum2 " + d);
            //d = Macros.sum2(x => x + .2, a, 2, y => y < 20);
            //Console.WriteLine("sum2 " + d);//toppato! 2 volte

            //Func<int, long> expression = x => Math.Max(7, x);
            //Macros.sum(expression, 3, 2, x => x > 5);

            double b = Macros.binary_search(0, 2, 0, 37, 0, x => x < 5, (l, h) => (l - h) < 3);
            Console.WriteLine("binary_search " + b);
         
         
         */
    }
}

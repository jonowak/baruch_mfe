using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesCalculator
{
    class NumericalApproximation
    {
        public static double NewtonsMethod(double initGuess, Option o, double tolerance)
        {
            double xNew;
            int i = 0;
            xNew = initGuess;
            double xOld = initGuess - 1;
            while (Math.Abs(xNew - xOld) > tolerance)
            {
                Console.WriteLine("X{0}={1}", i, xNew);
                xOld = xNew;
                xNew = xNew - ((o.CalculateGreeksSimpson(xNew) - o.Price) / o.Vega);

                i++;

            }
            Console.WriteLine("X{0}={1}", i, xNew);
            return xNew;
        }

        public static double NewtonsMethodForOptionDelta(double initGuess, Option o, double tolerance, double delta)
        {
            double xNew;
            double g;
            int i = 0;
            xNew = initGuess;
            double xOld = initGuess - 1;
            while (Math.Abs(xNew - xOld) > tolerance)
            {
                Console.WriteLine("X{0}={1}", i, xNew);
                xOld = xNew;
                g = o.CalculateStrikeSimpson(xNew);
               // Console.WriteLine("Delta = {0} - {1} / {2} = {3}", o.Delta, delta, o.Gamma, ((o.Delta - delta) / o.Gamma));
                xNew = xOld - (( o.Delta - delta) / DeltaPrime(o, xNew));

                i++;

            }
            Console.WriteLine("X{0}={1}", i, xNew);
            return xNew;
        }

        private static double DeltaPrime(Option o, double k) 
        {
            return (1.0/(-k * o.ImpliedVolatility * Math.Sqrt(o.Maturity))) * Math.Exp(-o.DRate * o.Maturity);
        }

        public static double NewtonsMethodForBondYield(double initGuess, Bond o, double tolerance)
        {
            double xNew;
            int i = 0;
            xNew = initGuess;
            double xOld = initGuess - 1;
            while (Math.Abs(xNew - xOld) > tolerance)
            {
                Console.WriteLine("X{0}={1}", i, xNew);
                xOld = xNew;
                xNew = xNew -((o.GetYield(xNew) - o.BondPrice) / o.GetYieldPrime(xNew));

                i++;

            }
            Console.WriteLine("X{0}={1}", i, xNew);
            return xNew;
        }

       

        public static double SecantMethod(double x0, Option o, double tolerance)
        {
            double xNew;
            int i = 0;
            xNew = x0;
            double xOld = 0.499;
            double xOldest = 0.0;
            while (Math.Abs(xNew - xOld) > tolerance)
            {
                Console.WriteLine("X{0}={1}", i, xNew);
                xOldest = xOld;
                xOld = xNew;
                xNew = xOld - (o.CalculateGreeksSimpson(xOld) - o.Price) * ((xOld - xOldest) / ((o.CalculateGreeksSimpson(xOld) - o.Price) - (o.CalculateGreeksSimpson(xOldest) - o.Price)));

                i++;

            }
            Console.WriteLine("X{0}={1}", i, xNew);
            return xNew;
        }

        public static double BisectionMethod(double a, double b, Option o, double tolerance)
        {
            double xL;
            double xR;
            double xM = 0;
            int i = 0;

            xL = a;
            xR = b;

            while (xR - xL > tolerance)
            {
                Console.WriteLine("{0} from {1} to {2}", i, xL, xR);
                i++;
                xM = (xL + xR) / 2.0;
                if ((o.CalculateGreeksSimpson(xL) - o.Price) * (o.CalculateGreeksSimpson(xM) - o.Price) < 0)
                    xR = xM;
                else
                    xL = xM;
            }
            Console.WriteLine("{0} from {1} to {2}", i, xL, xR);
            return xM;
        }


        /*
        private static double NewtonsMethod(double initGuess, Option o, double tolerance)
        {
            double tolerance = 10e-12;
            double xNew;
            int i = 1;
            xNew = initGuess;
            double xOld = initGuess - 1;
            while (Math.Abs(xNew - xOld) > tolerance)
            {
                xOld = xNew;
                xNew = xNew - ((o.CalculateGreeks3_1(xNew) - o.Price) / o.Vega);
                Console.WriteLine("X{0}={1}", i, xNew);
                i++;

            }
            return xNew;
        }

        private static double SecantMethod(double x0, Option o, double tolerance)
        {
            double tolerance = 10e-12;
            double xNew;
            int i = 1;
            xNew = x0;
            double xOld = 0.499;
            double xOldest = 0.0;
            while (Math.Abs(xNew - xOld) > tolerance)
            {
                xOldest = xOld;
                xOld = xNew;
                xNew = xOld - (o.CalculateGreeks3_1(xOld) - o.Price) * ((xOld - xOldest) / ((o.CalculateGreeks3_1(xOld) - o.Price) - (o.CalculateGreeks3_1(xOldest) - o.Price)));
                Console.WriteLine("X{0}={1}", i, xNew);
                i++;

            }
            return xNew;
        }

        private static double BisectionMethod(double a, double b, Option o, double tolerance)
        {
            double tolerance = 10e-12;
            double xL;
            double xR;
            double xM = 0;
            int i = 1;

            xL = a;
            xR = b;

            while (xR - xL > tolerance)
            {
                Console.WriteLine("{0} from {1} to {2}", i, xL, xR);
                i++;
                xM = (xL + xR) / 2.0;
                if ((o.CalculateGreeks3_1(xL) - o.Price) * (o.CalculateGreeks3_1(xM) - o.Price) < 0)
                    xR = xM;
                else
                    xL = xM;
            }

            return xM;
        }

        */
    }
}

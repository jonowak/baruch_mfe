using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesCalculator
{
    class HW_5
    {
        public void Run() 
        {
            //double[] tolerance;
            double[] datesOfCF;
            double tol;
            double couponRate;
            int m; 
            double price;
            double initGuess;
            double yield;
            
            const int SEMIANNUAL = 2;
           /* const int ANNUAL = 1;
            const int QARTERLY = 4;
            const int MONTHLY = 12;*/
            Func<double, double> f;

            //Problem 1
            #region
            m = 6;
            couponRate = 0.04;
            price = 101;
            tol = 10e-6;
            initGuess= 0.1;
            datesOfCF = new double[]{0.5, 0.1, 1.5, 2.0, 2.5, 3.0};

            Bond b1 = new Bond(m, SEMIANNUAL, couponRate, datesOfCF, price);
            yield = NumericalApproximation.NewtonsMethodForBondYield(initGuess, b1, tol);

            b1.BondCalculator(yield);
            b1.Yield = yield;
            b1.Display("Problem 1. Three-year semiannual coupon bond with a 4% coupon rate and price $101.");
            #endregion

            //Book example page 149
            #region
            m = 6;
            couponRate = 0.08;
            f = Zero_Rate2;
            tol = 10e-6;
            initGuess = 0.1;
            price = 105;
            datesOfCF = new double[] {4.0/12, 10.0/12, 16.0/12, 22.0/12, 28.0/12 ,34.0/12};

            Bond b = new Bond(m, SEMIANNUAL, couponRate, datesOfCF, price);
            yield = NumericalApproximation.NewtonsMethodForBondYield(initGuess, b, tol);

            b.BondCalculator(yield);
            b.Yield = yield;
            b.Display("34-month semiannual coupon bond with a coupon rate of 8% and price $105");
            #endregion

            //Problem 2 
            #region
            m = 5;
            couponRate = 0.035;
            price = 0;
            tol = 10e-6;
            initGuess = 0.1;
            datesOfCF = new double[] {1.0/12, 7.0/12, 13.0/12, 19.0/12, 25.0/12 };

            Bond b2 = new Bond(m, SEMIANNUAL, couponRate, f, datesOfCF);
            //calculate disc factors and bond price
            b2.BondCalculator(f);
            yield = NumericalApproximation.NewtonsMethodForBondYield(initGuess, b2, tol);
            b2.Yield = yield;
            b2.GetConvexity(yield);
            b2.GetDuration(yield); 
            b2.Display("25-month semiannual copon bond with coupon rate 3.5%.");
            
            #endregion
            //Problem 3
            #region

            double sigma;
            double S;
            double q;
            double r;
            double t;
            bool call;
            double delta;

            delta = 0.50;
            call = true;
            t = 0.25; 
            r = 0.025;
            q = 0.01;
            S = 30.0;
            sigma = 0.30;
            initGuess = 29.00;
            tol = 10e-6;
            Option o3 = new Option(S, initGuess, t, 0, q, r, call);
            o3.ImpliedVolatility = sigma;

            double strike = NumericalApproximation.NewtonsMethodForOptionDelta(initGuess, o3, tol, delta);
            Console.WriteLine("Strike of a 50-delta call maturing in 3 months is {0}", strike.ToString());

            #endregion

            //problem 4
            //boot strapping
            /*
            double overnightRate = 0.05;
            initGuess = 0.05;

            Bond[] bonds = new Bond[4];
            double[] dates1 = new double[]{0.5};
            bonds[0] = new Bond(1, SEMIANNUAL, 0, dates1, 97.5);
            double[] dates2 = new double[] { 0.5, 1.0 };
            bonds[1] = new Bond(2, SEMIANNUAL, 5.0, dates2, 100);
            double[] dates3 = new double[] { 6/12, 12/12, 18/12, 24/12, 30/12, 36/12};
            bonds[1] = new Bond(6, SEMIANNUAL, 5.0, dates3, 102);
            double[] dates4 = new double[] { 6/12, 12/12, 18/12, 24/12, 30/12, 36/12, 42/12, 48/12, 54/12, 60/12};
            bonds[1] = new Bond(10, SEMIANNUAL, 6.0, dates4, 104);
             * */


            //ivy example
            //select * from option_price where securityId = 137410 and date = '10/3/2013' and callput = 'C' and strike = 22000 and expiration = '11/20/2013'
            /*
            Option c = new Option(24.54, 22.0, 0.128767, 0, 0, 0.00228279788347442, true);
            c.CalculateGreeksSimpson(0.4007059);
            Console.WriteLine("Delta = {0}", c.Delta);
            Console.WriteLine("Vega = {0}", c.Vega);
            Console.WriteLine("Gamma = {0}", c.Gamma);
            */
            

        }

        private static double Zero_Rate2(double t)
        {
            return 0.015 + (t / (100 + Math.Sqrt(1 + t * t)));
        }
    }
}

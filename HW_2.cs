using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesCalculator
{
    class HW_2
    {

        public void Run()
        {
            double[] tolerance;
            double[] datesOfCF;
            double couponRate;
            Func<double, double> f;
            const int SEMIANNUAL = 2;
            const int ANNUAL = 1;
            //const int QARTERLY = 4;
            //const int MONTHLY = 12;


            //Problem #6 (i),
            #region
            Console.WriteLine("Problem #6 (i)");
            Console.WriteLine("Find a price of a two year semiannual coupon bond with coupon rate 7%.");
            int m = 4;
            f = ZeroCurveRate_6;
            couponRate = 0.07;
            datesOfCF = new double[] { 0.5, 1.0, 1.5, 2.0 };
            Bond b6 = new Bond(m, SEMIANNUAL, couponRate, f, datesOfCF);
            b6.BondCalculator(f);
            b6.Display("Two year semiannual coupon bond with coupon rate 7%");

            //Console.WriteLine("Bond price = " + bondPrice.ToString());
            Console.WriteLine();

            //Problem #6 (ii),
            Console.WriteLine("Problem #6 (ii)");
            Console.WriteLine("Find a price of a semiannual coupon bond with coupon rate 7% and maturity in 19 months.");
            m = 4;
            double[] dates_6ii = new double[] { (1.0 / 12.0), (7.0 / 12.0), (13.0 / 12.0), (19.0 / 12.0) };
            Bond b6ii = new Bond(m, SEMIANNUAL, couponRate, f, dates_6ii);
            b6ii.BondCalculator(f);
            b6ii.Display("Semiannual coupon bond with coupon rate 7% and maturity in 19 months");

            //problem #7
            Console.WriteLine("Problem #7");
            Console.WriteLine("Find a value of a 19-month bond with a coupon rate 4% and face value $100, ");
            Console.WriteLine("if the bond is an annual coupon bond.");
            m = 2;
            f = ZeroCurveRate_7;
            couponRate = 0.04;
            double[] dates_7a = new double[] { (7.0 / 12.0), (19.0 / 12.0) };
            Bond b7Annual = new Bond(m, ANNUAL, couponRate, f, dates_7a);
            b7Annual.BondCalculator(f);
            b7Annual.Display("19-month bond with a coupon rate 4% and face value $100. The bond is an annual coupon bond.");

            Console.WriteLine("Find a value of a 19-month bond with a coupon rate 4% and face value $100, ");
            Console.WriteLine("if the bond is a semiannual coupon bond.");
            m = 4;
            f = ZeroCurveRate_7;
            couponRate = 0.04;
            Bond b7Semi = new Bond(m, SEMIANNUAL, couponRate, f, dates_6ii);
            b7Semi.BondCalculator(f);
            b7Semi.Display("19-month bond with a coupon rate 4% and face value $100. The bond is a semiannual coupon bond.");
            /*
            Console.WriteLine("Find a value of a 19-month bond with a coupon rate 4% and face value $100, ");
            Console.WriteLine("if the bond is a quarterly coupon bond.");
            m = 7;
            f = ZeroCurveRate_7;
            couponRate = 0.04;
            double[] dates_7q = new double[] { (1.0 / 12.0), (4.0 / 12.0), (7.0 / 12.0), (10.0 / 12.0), (13.0 / 12.0), (16.0 / 12.0), (19.0 / 12.0) };
            DisplayTimeIntervals(dates_7q);
            cashFlows = GetCashFlows(m, couponRate, 4);
            discFactors = DiscFactorCalculator(dates_7q, f);
            bondPrice = BondCalculator(discFactors, cashFlows);
            DisplayBondPrice(bondPrice);
            //Console.WriteLine("Bond price = " + bondPrice.ToString());
            Console.WriteLine();
             * */
            #endregion

            //Problem #8
            #region
            Console.WriteLine("Problem #8. Book example. Test. page 69");
            m = 4;
            double yield = 0.065;
            couponRate = 0.06;
            double[] dates_8 = new double[] { (double)(2.0/12.0), (double)(8.0 / 12.0), (double)(14.0 / 12.0), (double)(20.0 / 12.0) };
            Bond b8 = new Bond(m, SEMIANNUAL, yield, couponRate, dates_8);
              //double[] cf_8 = GetCashFlows(m, 0.06, 2);
            b8.BondCalculator(yield);


            Console.WriteLine("Problem #8");
            Console.WriteLine();
            m = 4;
            yield = 0.025;
            dates_8 = new double[] { (double)(1.0 / 12.0), (double)(7.0 / 12.0), (double)(13.0 / 12.0), (double)(19.0 / 12.0) };

            b8 = new Bond(m, SEMIANNUAL, yield, couponRate, dates_8);
            b8.BondCalculator(yield);
            
            #endregion

            //Problem #9
            #region
            Console.WriteLine();
            Console.WriteLine("Problem #9");
            Console.WriteLine("Given instantaneous curve rate r(t) compute 6-month, 1-year, 18-month discount factors "+ 
                "with 6 decimal degits of accurecy.");
            tolerance = new double[] { 1e-6, 1e-6, 1e-6, 1e-8 };
            f = InstantaneousRateCurve_9;
            m = 4;
            couponRate = 0.06;
            double x1 = 0.0;
            //6 months
            //double tolerance = 1e-12;
            double[] df_9 = new double[]{0.05, 1.0, 1.5, 2.0 };
            Bond b9 = new Bond(m, SEMIANNUAL, couponRate, f, df_9);
            b9.BondCalculator(f, tolerance, x1, m);

            b9.Display("");
            #endregion
            Console.WriteLine("Seniannul bond given instantaneous curve rate.");
        }

        public static double ZeroCurveRate_6(double t)
        {
            return 0.05 + 0.005 * Math.Sqrt(1 + t);
        }

        public static double ZeroCurveRate_7(double t)
        {
            return 0.02 + (t / (200.0 - t));
        }

        public static double InstantaneousRateCurve_9(double t)
        {
            return 0.05 / (1 + 2 * Math.Exp(-(1 + t) * (1 + t)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesCalculator
{

    public class NumericalIntegration
    {
        //CUM_DIST_NORMAL constants
        const double a1 = 0.319381530;
        const double a2 = -0.356563782;
        const double a3 = 1.781477937;
        const double a4 = -1.821255978;
        const double a5 = 1.330274429;
        const double a = 0.2316419;

        public static double CummulativeDistributionOfNormalVariable(double x)
        {
            double z = Math.Abs(x);
            double y = 1.0 / (1.0 + a * z);
            double iSum = 1.0 - (Math.Exp(-(x * x) / 2.0) * (a1 * y + a2 * y * y + a3 * y * y * y + a4 * y * y * y * y + a5 * y * y * y * y * y) / Math.Sqrt(2 * Math.PI));

            if (x > 0)
                return iSum;
            else
                return 1.0 - iSum;

        }
      

        public static double Integrate(Func<double, double> f, double x1, double x2, double n)
        {

            double h = (x2 - x1) / (double)n;
            double integrationSum;
            integrationSum = (f(x1) + f(x2)) / (double)6.0;

            for (int i = 1; i < n; i++)
                integrationSum += f(x1 + i * h) / (double)3.0;

            for (int i = 1; i <= (n); i++)
                integrationSum += (double)2.0 * f(x1 + ((double)i - (double)0.5) * h) / (double)3.0;

            integrationSum *= h;
            return integrationSum;

        }

        public static double Simpsons(Func<double, double> f, double t, double x1, double x2, double n)
        {
            double valPrev = Integrate(f, x1, x2, n);
            //Console.WriteLine(n.ToString() + " " + Math.Round(valPrev, 12).ToString());
            n *= 2;
            double valCurrent = Integrate(f, x1, x2, n);
            //Console.WriteLine(n.ToString() + " " + Math.Round(valCurrent, 12).ToString());

            while (Math.Abs(valCurrent - valPrev) > t)
            {
                valPrev = valCurrent;
                n *= 2;
                valCurrent = Integrate(f, x1, x2, n);
               // Console.WriteLine(n.ToString() + "  " + Math.Round(valCurrent, 12).ToString());
            }

            return valCurrent;
        }

        static double NDistFunc(double x)
        {
            double r = (double)Math.Exp(-(x * x) * 0.5);
            return r;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesCalculator
{
    class BootStrappingTest
    {
        const int FACE = 100;
        const double INIT_GUESS = 0.05;
        const double TOLERANCE = 10e-6;
        const int SEMIANNUAL = 2;

        public struct InterestRate
        {
            public double rate;
            public double t;
        }

        public void Run()
        {
            //bootStrapping
            //Book example 
            double tolerance = TOLERANCE;
           

            Bond[] bonds = new Bond[4];
            double[] dates1 = new double[] { 0.5 };
            bonds[0] = new Bond(1, SEMIANNUAL, 0, dates1, 99);
            double[] dates2 = new double[] { 0.5, 1.0 };
            bonds[1] = new Bond(2, SEMIANNUAL, 0.04, dates2, 102);
            double[] dates3 = new double[] { 0.5, 1.0, 1.5, 2.0};
            bonds[2] = new Bond(4, SEMIANNUAL, 0.04, dates3, 103.5);
            double[] datesMax = new double[] { 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0 };
            bonds[3] = new Bond(10, SEMIANNUAL, 0.04, datesMax, 109);


            //get first value of R
            InterestRate[] rates = new InterestRate[10];
            for (int i = 0; i < datesMax.Length; i++)
            {
                rates[i].t = datesMax[i];
                rates[i].rate = -1;//initilize the rate 
            }

            int l = datesMax.Length -1;

            rates[0].rate = BootStrapping(bonds[0]);
            rates[1].rate = BootStrapping2(bonds, rates[0].rate);
            rates[l].rate = NewtonsApproximationForZeroRate(bonds[2], rates, INIT_GUESS, tolerance);
        }

        public double BootStrapping(Bond b)
        {
            double r = Math.Log(b.BondPrice / FACE) / - b.DatesOfCF[0];
            return r;
        }

        private double BootStrapping2(Bond [] b, double r)
        {
            double rate = 0;
            rate =  b[1].CashFlows[0] * Math.Exp(-r*b[1].DatesOfCF[0]);
            rate =  Math.Log((b[1].BondPrice - rate)/(b[1].CashFlows[1]))/-1;
            return rate;
        }

        private static double FindZeroRate(Bond b, InterestRate[] r, double x, bool derivative)
        {
            double sum = 0;
            double sumPrime = 0;
            int i;
            int len = b.CashFlows.Length;
            for (i = 0; i < len; i++)
            {
                if (r[i].rate > -1)
                {
                    sum += b.CashFlows[i] * Math.Exp(-r[i].rate * r[i].t);
                    sumPrime += r[i].t * b.CashFlows[i] * Math.Exp(-r[i].rate * r[i].t);
                }
                else if (r[i].rate == -1)
                {
                    int l=i-1;//2
                    double knowRate = r[l].rate; //1
                    for (int k =i; k < len; k++)
                    {
                        sum += b.CashFlows[k] * Math.Exp(-b.DatesOfCF[k] * GetRate(r[l].t, b.DatesOfCF[len - 1], knowRate, x, r[k].t));
                        sumPrime += r[k].t * b.CashFlows[k] * Math.Exp(-b.DatesOfCF[k] * GetRate(r[l].t, b.DatesOfCF[len - 1], knowRate, x, r[k].t));
                        // ((r[l- 1].rate + xNew) / 2.0));
                    }
                    break;
                }
              
            }

            if (derivative)
                return sumPrime;
            else
                return sum;

        }

        private static double NewtonsApproximationForZeroRate(Bond b, InterestRate[] r, double initGuess, double tolerance) 
        {
            double xNew;
            double fOfX;
            double fOfXPrime;
            int i = 0;
            xNew = initGuess;
            double xOld = initGuess - 1;
            while (Math.Abs(xNew - xOld) > tolerance)
            {
                Console.WriteLine("X{0}={1}", i, xNew);
                xOld = xNew;
                fOfX = FindZeroRate(b, r, xNew, false);
                fOfXPrime = FindZeroRate(b, r, xNew, true); 
                xNew = xNew - ((fOfX - b.BondPrice) / - fOfXPrime);
                i++;

            }
            Console.WriteLine("X{0}={1}", i, xNew);
            return xNew;
        }

        private static double GetRate(double rT, double bT, double knownRate, double xRate, double t) 
        { 
            //rT is the t of the max known rate
            //bT is the max t (maturity) of the bond where at least rate at t-1 and t are unknown
            //bT < rT by at least 2 time intervals
            //return r(0,t)
            return ((t - (bT - rT)) / (bT - rT)) * (xRate) + ((bT - t) / (bT - rT)) * (knownRate);
        }
    }
}

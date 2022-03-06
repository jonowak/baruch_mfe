using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesCalculator
{
    class Bond
    {
        private double[] cashFlows;
        private double[] datesOfCF;
        private double[] discFactors;
        private double couponRate;
        private double bondPrice = 0;
        private Func<double, double> zeroRate;
        private double yield;
        private double duration;
        private double convexity;
        const int FACE_VALUE = 100;


        //setter and getters 
        #region
        public double Convexity
        {
            get { return convexity; }
            set { convexity = value; }
        }
        public double Duration
        {
            get { return duration; }
            set { duration = value; }
        } 
        public double Yield
        {
            get { return yield; }
            set { yield = value; }
        } 

        public Func<double, double> ZeroRate
        {
            get { return zeroRate; }
            set { zeroRate = value; }
        }

        public double BondPrice
        {
            get { return bondPrice; }
            set { bondPrice = value; }
        }


        public double CouponRate
        {
            get { return couponRate; }
            set { couponRate = value; }
        }

        public double[] DiscFactors
        {
            get { return discFactors; }
            set { discFactors = value; }
        }

        public double[] DatesOfCF
        {
            get { return datesOfCF; }
            set { datesOfCF = value; }
        }

        public double[] CashFlows
        {
            get { return cashFlows; }
            set { cashFlows = value; }
        }
        #endregion


        //constructors
        #region
        public Bond() 
        {
            bondPrice = 0;
            convexity = 0;
            duration = 0;

        }

        public Bond(int n, int nOPaymentsPerYear, double cR,  Func<double , double> f, double[] d) 
        { 
            datesOfCF = d;
            couponRate = cR;
            zeroRate = f;
            cashFlows = new double[n];
            discFactors = new double[n];
            cashFlows = GetCashFlows(n, nOPaymentsPerYear);
        }

        public Bond(int m, int nOPaymentsPerYear, double y, double cR, double[] d)
        {
            datesOfCF = d;
            couponRate = cR;
            cashFlows = new double[m];
            cashFlows = GetCashFlows(m, nOPaymentsPerYear);
            discFactors = new double[m];//no need when yield is provided
        }

        public Bond(int m, int nOPaymentsPerYear, double cR, double[] d, double p)
        {
            datesOfCF = d;
            couponRate = cR;
            cashFlows = new double[m];
            cashFlows = GetCashFlows(m, nOPaymentsPerYear);
            discFactors = new double[m];//no need when yield is provided
            bondPrice = p;
        }
        #endregion

        //functions
        #region

        public double GetYield(double x) {
            double sum = 0;
            for (int i = 0; i < DatesOfCF.Length; i++)
            {
                sum += (CashFlows[i] * Math.Exp(-x*DatesOfCF[i]));
            }
            return sum;
        }

        public double GetYieldPrime(double x)
        {
            double sum = 0;
            for (int i = 0; i < DatesOfCF.Length; i++)
            {
                sum += (DatesOfCF[i]*CashFlows[i] * Math.Exp(-x * DatesOfCF[i]));
            }
            return -sum;
        }

        public double GetPrice()
        {
            double price = 0;
            for (int i = 0; i < CashFlows.Length; i++)
                price += CashFlows[i] * DiscFactors[i];
            this.BondPrice = price;
            return price;
        }

        //passing zero curve 
        public void BondCalculator(Func<double, double> f)
        {
            this.DiscFactorCalculator(f);
            this.GetPrice();
        }

        //passing the yield
        public double BondCalculator(double y)
        {
            this. discFactors = DiscFactorCalculator(y);
            double price = 0.0;
            for (int i = 0; i < CashFlows.Length; i++)
            {
                price += CashFlows[i] * Math.Exp(-(y * DatesOfCF[i]));
            }
            this.bondPrice = price;
            this.GetDuration(yield);
            this.GetConvexity(yield);
            return price;
        }

        //passing instantenius rate
        public void BondCalculator(Func<double, double> f, double[] tol, double x1, double m){
            //calculate disc factors
            for (int i = 0; i <= m; i++)
            {
                double iSum = NumericalIntegration.Simpsons(f, tol[i], x1, this.DatesOfCF[i], m);
                this.DiscFactors[i] = Math.Exp(-iSum);
            }
            //calculate price
            this.GetPrice();
        }

        public double GetConvexity(double y)
        {
            double sum = 0.0;
            for (int i = 0; i < CashFlows.Length; i++)
            {
                sum += datesOfCF[i] * datesOfCF[i] * CashFlows[i] * Math.Exp(-(y * datesOfCF[i]));
            }
            this.Convexity = sum / bondPrice;
            return this.Convexity;
        }

        public double GetDuration(double y)
        {
            double sum = 0.0;
            for (int i = 0; i < CashFlows.Length; i++)
            {
                sum += DatesOfCF[i] * CashFlows[i] * Math.Exp(-(y * DatesOfCF[i]));
            }
            this.duration = sum / this.BondPrice;
            return this.Duration;
        }

        //passing the zero curve
        public double[] DiscFactorCalculator( Func<double, double> f)
        {
            double[] df = new double[this.datesOfCF.Length];

            for (int i = 0; i < datesOfCF.Length; i++)
            {
                df[i] = Math.Exp(-(f(datesOfCF[i]) * datesOfCF[i]));
                Console.WriteLine("Discount Factor {0}: {1}", (i + 1).ToString(), Math.Round(df[i], 12));
            }
            this.DiscFactors = df;
            return df;
        }

        //passing the yield
        public double[] DiscFactorCalculator(double yield)
        {
            double[] df = new double[this.datesOfCF.Length];

            for (int i = 0; i < datesOfCF.Length; i++)
            {
                df[i] = Math.Exp(-(datesOfCF[i] * yield));
                Console.WriteLine("Discount Factor {0}: {1}", (i + 1).ToString(), Math.Round(df[i], 12));
            }
            this.DiscFactors = df;
            return df;
        }
      /*  public double[] YieldDiscFactorCalculator(double y)
        {
            double[] df = new double[this.datesOfCF.Length];

            for (int i = 0; i < datesOfCF.Length; i++)
            {
                df[i] = Math.Exp(-(DatesOfCF[i] * y));
                Console.WriteLine("Discount Factor {0}: {1}", (i + 1).ToString(), Math.Round(df[i], 12));
            }
            this.DiscFactors = df;
            return df;
        }*/

        public double[] GetCashFlows(int m, int f)
        {//f like frequency
            double[] cf = new double[m];
            Console.Write("Cashflows: ");
            for (int i = 0; i < (m - 1); i++)
            {
                cf[i] = this.CouponRate / f * FACE_VALUE;
                Console.Write(cf[i].ToString() + ", ");
            }
            cf[m - 1] = 100 + (this.CouponRate / f * FACE_VALUE);
            Console.Write(cf[m - 1].ToString());
            Console.WriteLine();

            this.CashFlows = cf;

            return cf;
        }

    /*    public void DisplayTimeIntervals()
        {
            for (int i = 0; i < this.DatesOfCF.Length; i++)
            {
                Console.WriteLine("t{0} = {1}", (i + 1), DatesOfCF[i]);
            }
        }*/

        public void Display(string s)
        {
            Console.WriteLine(s);
            Console.WriteLine("Bond Price = {0}", Math.Round(this.BondPrice, 9));

            //display discount factors
            Console.Write("Disc factors: ");
            for (int i = 0; i< DiscFactors.Length; i++)
                Console.Write(DiscFactors[i] + ", ");
            Console.WriteLine();
            //display payment dates
            Console.Write("Cash flow dates: ");
            for (int i = 0; i < DatesOfCF.Length; i++)
                Console.Write(DatesOfCF[i] + ", ");
            Console.WriteLine();
            //display cash flows
            Console.Write("Cash flows: ");
            for (int i = 0; i < CashFlows.Length; i++)
                Console.Write(CashFlows[i] + ", ");
            Console.WriteLine();
            //display yield
            Console.WriteLine("Yiled = {0}", Yield.ToString());
           // Console.WriteLine("Convexity = {0}", );
            Console.WriteLine("Bond Duration = {0}", duration);
            Console.WriteLine("Bond Convexity = {0}", convexity);
            Console.WriteLine();
        }

  
        #endregion
    }
}

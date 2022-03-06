using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesCalculator
{
    class Option
    {
        //constants for Simpson's method
        const double tolerance = 1.0e-12;
        const int n = 4;
        const double PI = 3.14159265358979323846264338; 

        //option characteristics
        private double strike;
        private double spot;
        private double maturity;
        private bool call;
        private double nOfD1;
        private double nOfD2;
        private double delta;
        private double price;
        private double gamma;
        private double vega;
        private double dRate;
        private double iRate;
        private double impliedVolatility;

        

        #region
        public double ImpliedVolatility
        {
            get { return impliedVolatility; }
            set { impliedVolatility = value; }
        }

        public double IRate
        {
            get { return iRate; }
            set { iRate = value; }
        } 

        public double DRate
        {
            get { return dRate; }
            set { dRate = value; }
        }

        public double Vega
        {
            get { return vega; }
            set { vega = value; }
        }

        public double Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        } 


        public double Delta
        {
            get { return delta; }
            set { delta = value; }
        }

        public double NOfD2
        {
            get { return nOfD2; }
            set { nOfD2 = value; }
        }

        public double NOfD1
        {
            get { return nOfD1; }
            set { nOfD1 = value; }
        }


        public bool Call
        {
            get { return call; }
            set { call = value; }
        }

        public double Maturity
        {
            get { return maturity; }
            set { maturity = value; }
        }

        public double Spot
        {
            get { return spot; }
            set { spot = value; }
        }

        public double Strike
        {
            get { return strike; }
            set { strike = value; }
        }

        #endregion

        //constructors
        #region
        public Option()
        {
            spot = 0;
            strike = 0;
            maturity = 0.0;
            call = true;
            nOfD1 = 0;
            nOfD2 = 0;
            delta = 0;
            price = 0;
            gamma = 0;
            vega = 0;
            impliedVolatility = 0; 
        }

        public Option(double s, double k, double t, double p, double q,  double r, bool c)
        {
            maturity = t;
            Spot = s;
            Strike = k;
            Call = c;
            dRate = q;
            Price = p;
            iRate = r;
            
        }
        
        #endregion

        public double CalculateGreeks3_1(double sigma) 
        {
            double d1 = (Math.Log(Spot / Strike) + (iRate - dRate + (sigma * sigma) / 2.0) * Maturity) / (sigma * Math.Sqrt(Maturity));
            double d2 = (Math.Log(Spot / Strike) + (iRate - dRate - (sigma * sigma) / 2.0) * Maturity) / (sigma * Math.Sqrt(Maturity));//d1 - sigma * Math.Sqrt(Maturity);

            this.nOfD1 = NumericalIntegration.CummulativeDistributionOfNormalVariable(d1);
            this.nOfD2 = NumericalIntegration.CummulativeDistributionOfNormalVariable(d2);

            //Console.WriteLine("N(d1) = {0}", Math.Round(NOfD1, 12));
            //Console.WriteLine("N(d2) = {0}", Math.Round(NOfD2, 12));

            SetDelta();
            SetVega(d1, d2);
            SetGamma(d1, d2, sigma);
            double bS = BlackScholes();
            if (this.price == 0)
                this.price = bS;
            //Console.WriteLine("Option's BS value = {0} ", bS);
            return bS; 
        }

        public double CalculateGreeksSimpson(double sigma)
        {
            double d1 = (Math.Log(Spot / Strike) + (iRate - dRate + (sigma * sigma) / 2.0) * Maturity) / (sigma * Math.Sqrt(Maturity));
            double d2 = (Math.Log(Spot / Strike) + (iRate - dRate - (sigma * sigma) / 2.0) * Maturity) / (sigma * Math.Sqrt(Maturity));//d1 - sigma * Math.Sqrt(Maturity);

            this.nOfD1 = NumericalIntegration.Simpsons(NDistFunc, tolerance, 0, d1, n);
            this.nOfD2 = NumericalIntegration.Simpsons(NDistFunc, tolerance, 0, d2, n);
            this.nOfD1 = 0.5 + (this.nOfD1 / (Math.Sqrt(2 * PI)));
            this.nOfD2 = 0.5 + (this.nOfD2 / (Math.Sqrt(2 * PI)));

            //Console.WriteLine("N(d1) = {0}", Math.Round(NOfD1, 12));
            //Console.WriteLine("N(d2) = {0}", Math.Round(NOfD2, 12));

            SetDelta();
            SetVega(d1, d2);
            SetGamma(d1, d2, sigma);
            double bS = BlackScholes();
            if (this.price == 0)
                this.price = bS;
           // Console.WriteLine("Option's BS value = {0} ", bS);
            return bS;
        }

        public double CalculateStrikeSimpson(double k)
        {
            double d1 = (Math.Log(Spot / k) + (iRate - dRate + (impliedVolatility * impliedVolatility) / 2.0) * Maturity) 
                / (impliedVolatility * Math.Sqrt(Maturity));
            double d2 = (Math.Log(Spot / k) + (iRate - dRate - (impliedVolatility * impliedVolatility) / 2.0) * Maturity) 
                / (impliedVolatility * Math.Sqrt(Maturity));//d1 - sigma * Math.Sqrt(Maturity);

            this.nOfD1 = NumericalIntegration.Simpsons(NDistFunc, tolerance, 0, d1, n);
            this.nOfD2 = NumericalIntegration.Simpsons(NDistFunc, tolerance, 0, d2, n);
            this.nOfD1 = 0.5 + (this.nOfD1 / (Math.Sqrt(2 * PI)));
            this.nOfD2 = 0.5 + (this.nOfD2 / (Math.Sqrt(2 * PI)));

            //Console.WriteLine("N(d1) = {0}", Math.Round(NOfD1, 12));
            //Console.WriteLine("N(d2) = {0}", Math.Round(NOfD2, 12));

            SetDelta();
            //SetGamma(d1, d2, impliedVolatility);
            double g = k*(Math.Exp(-dRate * Maturity) / (impliedVolatility * Math.Sqrt(Maturity))) *
                (1.0 / (Math.Sqrt(2 * Math.PI))) * Math.Exp(-d1 * d1 / 2.0);

            return g;
        }

        private void SetVega(double d1, double d2)
        {

            this.vega = (1.0 / (Math.Sqrt(2 * Math.PI))) * (Spot * Math.Exp(-dRate * Maturity)) * Math.Sqrt(Maturity) * Math.Exp(-d1 * d1 / 2.0);
            //Console.WriteLine("Vega = {0}", this.vega);
        }

        private void SetGamma(double d1, double d2, double sigma)
        {
            this.gamma = (Math.Exp(-dRate * Maturity) / (Spot * sigma * Math.Sqrt(Maturity))) * 
                (1.0 / (Math.Sqrt(2 * Math.PI))) * Math.Exp(-d1 * d1 / 2.0);
        }

        private void SetDelta()
        {
            //Console.WriteLine("N(d1)= {0}", nOfD1.ToString());

            if (this.call)
                this.Delta = Math.Exp(-dRate * Maturity) * nOfD1;
            else
                this.Delta = -Math.Exp(-dRate * Maturity) * (1-nOfD1);
            //Console.WriteLine("Delta = {0}", this.Delta);
        }

        public double BlackScholes()
        {
            double impliedVol = 0;

            if (this.Call)
            {
                impliedVol = ((Spot * Math.Exp(-dRate * Maturity)) * this.NOfD1) - ((Strike * Math.Exp(-iRate * Maturity)) * this.nOfD2);// -this.price;
            }
            else
            {
                impliedVol = (Strike * Math.Exp(-iRate * Maturity)) * (1 - nOfD2) - ((Spot * Math.Exp(-dRate * Maturity)) * (1 - NOfD1));// -this.price;
            }

            return impliedVol;
        }

       /* public void SetDeltaUsingSimpsons(double sigma, double q, double r)
        {
            //N(d1)

            double d1 = (Math.Log(Spot / Strike) + (r - q + (sigma * sigma) / 2.0) * Maturity) / (sigma * Math.Sqrt(Maturity));
            double d2 = d1 - sigma * Math.Sqrt(Maturity);
            //Console.WriteLine("d1 = {0}", d1.ToString()); 


            double nOfD1 = NumericalIntegration.ConvergenceMachine(NDistFunc, tolerance, 0, d1, n);
            this.nOfD1 = 0.5 +(nOfD1/(Math.Sqrt(2*Math.PI)));
            //Console.WriteLine("N(d1)= {0}", nOfD1.ToString());
            double nOfD2 = CummulativeDistributionOfNormalVariable(d2);

            if (this.call)
                this.Delta = Math.Exp(-q * Maturity) * this.nOfD1;
            else
                this.Delta = -Math.Exp(-q * Maturity) * (1 - this.nOfD1);
         }*/

       
        public double DeltaHedge(double n)
        {
            double hedge;
            if (Call)
            {
                hedge = n * Math.Exp(-dRate * Maturity) * this.Delta;
            }
            else
            {
                hedge = -n * Math.Exp(-dRate * Maturity) * this.Delta;
            }
            return hedge;
        }


        private static double NDistFunc(double x)
        {
            double r = (double)Math.Exp(-(x * x) /2.0);//  also /2
            return r;
        }


        public void print() 
        {
            Console.WriteLine();
            if (this.Call) 
                Console.WriteLine("European Call") ;
            else 
                Console.WriteLine("European Put"); 

            Console.WriteLine("S = {0}", this.Spot);
            Console.WriteLine("K = {0} ", this.Strike);
            Console.WriteLine("t = {0} ", this.Maturity);
            Console.WriteLine("Vega = {0} ", this.Vega);
            Console.WriteLine("Delta = {0} ", Math.Round(this.Delta, 12));
            Console.WriteLine("Price/BS Value = {0}", Math.Round(this.Price, 12));
           // Console.WriteLine("Volatility/Implied Volatility =" + Math.Round(ImpliedVolatility, 12));
            Console.WriteLine();
        }
       


    }
}

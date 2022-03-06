using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BlackScholesCalculator
{
    class HW_4
    {
        public void Run(){
        // Problem 1
            const double PI = 3.14159265358979323846264338; 
            #region
            Console.WriteLine("------------Problem 1-------------");

            //given
            double t = 0.25;
            double s = 40.0;
            double k = 40.0;
            double q = 0.01;
            double p = 0;
            double r = 0.05;
            bool c = true;
            double sigma = 0.2;


            Option call1 = new Option(s, k, t, p, q, r, c);
            Console.WriteLine("Problem 1. Using routine 3.1:");
            call1.CalculateGreeks3_1(sigma);
            call1.print();

            Console.WriteLine("Problem 1. Using Simpson's Approximation:");
            Option callSimpson = new Option(s, k, t, p, q, r, c);
            callSimpson.CalculateGreeksSimpson(sigma);
            callSimpson.print();
            #endregion

            //Problem 3 
            #region
            Console.WriteLine("----------------Problem 3------------------");
            //given
            t = 5.0/12.0;
            s = 50.0;
            k = 45.0;
            q = 0.01;
            p = 0;
            r = 0.03;
            c = true;
            sigma = 0.30;

            Option call3 = new Option(s, k, t, p, q, r, c);
            //use Simpson's approx
            call3.CalculateGreeksSimpson(sigma);
            call3.print();
            Console.WriteLine("");
            #endregion


            //Problem 6
            double initGuess = 0.5;
            double impliedVol = 0;
            #region
            Console.WriteLine("Homework #4");
            Console.WriteLine("Problem 6");

            t = 0.5;
            s = 30.0;
            k = 30.0;
            q = 0.01;
            p = 2.5;
            r = 0.03;
            c = true; 
            double tolerance = 1e-6;
            Option call6 = new Option(s, k, t, p, q, r, c);
            impliedVol = NumericalApproximation.NewtonsMethod(initGuess, call6, tolerance);
            call6.print();
            Console.WriteLine("IV calculated using Newton's method = " + Math.Round(impliedVol, 12).ToString());

            
            #endregion

            //Problem #7
            #region
            Console.WriteLine("Problem #7");
            Console.WriteLine("Part (i)");
            t = 5.0/12.0;
            s = 40.0;
            k = 40.0;
            q = 0.01;
            p = 2.75;
            r = 0.025;
            c = true;

            double a = 0.0001;
            double b = 1.0;
            Option call_bisec = new Option(s, k, t, p, q, r, c);
            impliedVol = NumericalApproximation.BisectionMethod(a, b, call_bisec, tolerance);
            Console.WriteLine("IV calculated using Bisection method = " + Math.Round(impliedVol, 6).ToString());

            Option call_secant = new Option(s, k, t, p, q, r, c);
            impliedVol = NumericalApproximation.SecantMethod(initGuess, call_secant, tolerance);
            Console.WriteLine("IV calculated using Secant method = " + Math.Round(impliedVol, 6).ToString());

            Option call_newton = new Option(s, k, t, p, q, r, c);
            impliedVol = NumericalApproximation.NewtonsMethod(initGuess, call_newton, tolerance);
            Console.WriteLine("IV calculated using Newton's method = " + Math.Round(impliedVol, 6).ToString());

            Console.WriteLine("Part (ii)");
            double sigmaImp = (Math.Sqrt(2.0 * PI) / (s * Math.Sqrt(t))) * (p - (((r - q) * t) * 0.5) * s) / (1.0 - ((r + q) * t) * 0.5);
            double error = Math.Abs(sigmaImp - impliedVol) / impliedVol;
            Console.WriteLine("Sigma implied = {0}", Math.Round(sigmaImp, 9).ToString());
            Console.WriteLine("Relative error = {0}", Math.Round(error, 9));
            #endregion

            #region
            Console.WriteLine("Problem #8");
            t = 130.0/365.0;
            s = 1193.0;
            k = 800.0;
            q = 0.017;
            p = 10.55;
            r = 0.01;
            c = false;

            Option call8 = new Option(s, k, t, p, q, r, c);
            impliedVol = NumericalApproximation.NewtonsMethod(initGuess, call8, tolerance);
            Console.WriteLine("IV calculated using Newton's method = " + Math.Round(impliedVol, 6).ToString());

            #endregion

            //Book , page 158
            #region
            /*
            t = 0.25;
            s = 30.0;
            k = 30.0;
            q = 0.02;
            p = 2.5;
            r = 0.06;
            c = true;
           
            Option call_secant = new Option(s, k, t, p, q, r, c);
            impliedVol = SecantMethod(initGuess, call_secant);
            Console.WriteLine("IV calculated using Secant method = " + Math.Round(impliedVol, 6).ToString());

            double a = 0.0001;
            double b = 1.0;
            Option call_bisec = new Option(s, k, t, p, q, r, c);
            impliedVol = BisectionMethod(a, b, call_bisec);
            Console.WriteLine("IV calculated using Bisection method = " + Math.Round(impliedVol, 6).ToString());

             Option call_newton = new Option(s, k, t, p, q, r, c);
            impliedVol = NewtonsMethod(initGuess, call_newton);
            Console.WriteLine("IV calculated using Newton's method = " + Math.Round(impliedVol, 6).ToString());


            double sigmaImp = (Math.Sqrt(2.0 * PI) / (s * Math.Sqrt(t)))* (p - (((r - q) * t) * 0.5) * s) / (1.0 - ((r + q) * t) * 0.5);
            double error = Math.Abs(sigmaImp -impliedVol)/impliedVol;

            Console.WriteLine("Relative error = {0}", error);
            */
            
            #endregion

            //Problem 8 
            #region 
            //Load file with options 
            
         /* string file = "Q8_spx.csv";
            string dir = "C:\\Users\\joanna\\Documents\\MyStuff\\School\\Pre_MFE_AC\\HW_4"; 
            string tempFile = dir + "\\Q8_solution.txt";
            TextWriter tw = new StreamWriter(tempFile);

            Option[] spxOptions = read(file, dir);
            for (int i = 0; i < spxOptions.Length; i++) 
            {
                double iv = NewtonsMethod(initGuess, spxOptions[i]);
                spxOptions[i].ImpliedVolatility = iv;
                spxOptions[i].print();
                if (spxOptions[i].Call == true)
                    tw.Write("C" + ",");
                else 
                    tw.Write("P" + ",");
                tw.WriteLine(spxOptions[i].Maturity.ToString() + "," + spxOptions[i].Strike.ToString() + "," + spxOptions[i].Price.ToString() +", " + spxOptions[i].ImpliedVolatility.ToString());
            }
            tw.Flush();
            tw.Close();

            //write to a file
           
            */
            #endregion


    }//end Run

         private static Option[] read(string file, string dir)
        {
            double p;
            double t = 0;
            double r = 0.01;
            double q = 0.017;
            int d = 0;
            double spot = 1193.0;
            bool c = true;
            int i = 0;

            DateTime today = Convert.ToDateTime("11/21/2011");

            string line = "";

            Console.WriteLine("Proseccing file {0}", file);

            Option [] spxOptions = new Option[15];

            TextReader tr = new StreamReader(dir + "\\" + file);
            string[] parsedElements = new string[6];
            tr.ReadLine();
            while ((line = tr.ReadLine()) != null)
            {
                string[] rowElements = line.Split(',');
                
                double strike = Convert.ToDouble(rowElements[1]);
                double last = Convert.ToDouble(rowElements[2]);
                double bid = Convert.ToDouble(rowElements[3]);
                double ask = Convert.ToDouble(rowElements[4]);
                double volume = Convert.ToInt32(rowElements[5]);
                //DateTime expiration = Convert.ToDateTime(rowElements[6]);
                d = Convert.ToInt32(rowElements[6]);
                if (rowElements[7].Equals("C") || rowElements[7].Equals("c"))
                    c = true;
                else
                    c = false;

                p = (bid + ask) / 2.0;
                //d = Convert.ToInt32(expiration - today);
                t = d / 365.00;
                spxOptions[i] = new Option(spot, strike, t, p, q, r, c);
                i++;

            }
            tr.Close();
            return spxOptions;
        }

        public static double AccrueInterest(double r, double c)
        {
            double cash = c * Math.Exp(r / 12.0);
            Console.WriteLine("Cash : {0}", cash.ToString());
            return cash;
        }

       
        public static void PrintPosition(double oP, double aP, double cP)
        {
            Console.WriteLine("Option Position: {0}", oP.ToString());
            Console.WriteLine("Asset Position: {0}", aP.ToString());
            Console.WriteLine("Cash Position: {0}", cP.ToString());

        }

    }
}

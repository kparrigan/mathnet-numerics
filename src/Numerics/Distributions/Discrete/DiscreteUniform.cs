﻿// <copyright file="DiscreteUniform.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://mathnet.opensourcedotnet.info
//
// Copyright (c) 2009 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace MathNet.Numerics.Distributions
{
    using System;
    using System.Collections.Generic;
    using Properties;

    /// <summary>
    /// The discrete uniform distribution is a distribution over integers. The distribution
    /// is parameterized by a lower and upper bound (both inclusive).
    /// </summary>
    /// <remarks><para>The distribution will use the <see cref="System.Random"/> by default. 
    /// Users can set the random number generator by using the <see cref="RandomSource"/> property.</para>
    /// <para>The statistics classes will check all the incoming parameters whether they are in the allowed
    /// range. This might involve heavy computation. Optionally, by setting Control.CheckDistributionParameters
    /// to false, all parameter checks can be turned off.</para></remarks>
    public class DiscreteUniform : IDiscreteDistribution
    {
        /// <summary>
        /// The distribution's lower bound.
        /// </summary>
        private int _lower;

        /// <summary>
        /// The distribution's upper bound.
        /// </summary>
        private int _upper;

        /// <summary>
        /// The distribution's random number generator.
        /// </summary>
        private Random _random;

        /// <summary>
        /// Initializes a new instance of the DiscreteUniform class.
        /// </summary>
        /// <param name="lower">Lower bound.</param>
        /// <param name="upper">Upper bound; must be at least as large as <paramref name="lower"/>.</param>
        public DiscreteUniform(int lower, int upper)
        {            
            SetParameters(lower, upper);
            RandomSource = new System.Random();
        }

        /// <summary>
        /// A string representation of the distribution.
        /// </summary>
        public override string ToString()
        {
            return "DiscreteUniform(Lower = " + _lower + ", Upper = " + _upper + ")";
        }

        /// <summary>
        /// Checks whether the parameters of the distribution are valid. 
        /// </summary>
        /// <param name="lower">Lower bound.</param>
        /// <param name="upper">Upper bound; must be at least as large as <paramref name="lower"/>.</param>
        /// <returns>True when the parameters are valid, false otherwise.</returns>
        private static bool IsValidParameterSet(int lower, int upper)
        {
            if (lower <= upper)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the parameters of the distribution after checking their validity.
        /// </summary>
        /// <param name="lower">Lower bound.</param>
        /// <param name="upper">Upper bound; must be at least as large as <paramref name="lower"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the parameters don't pass the <see cref="IsValidParameterSet"/> function.</exception>
        private void SetParameters(int lower, int upper)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(lower, upper))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            _lower = lower;
            _upper = upper;
        }

        /// <summary>
        /// Gets or sets the lower bound of the probability distribution.
        /// </summary>
        public int LowerBound
        {
            get
            {
                return _lower;
            }

            set
            {
                SetParameters(value, _upper);
            }
        }

        /// <summary>
        /// Gets or sets the upper bound of the probability distribution.
        /// </summary>
        public int UpperBound
        {
            get
            {
                return _upper;
            }

            set
            {
                SetParameters(_lower, value);
            }
        }

        #region IDistribution Members

        /// <summary>
        /// Gets or sets the random number generator which is used to draw random samples.
        /// </summary>
        public Random RandomSource
        {
            get
            {
                return _random;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _random = value;
            }
        }

        /// <summary>
        /// Gets the mean of the distribution.
        /// </summary>
        public double Mean
        {
            get { return (_lower + _upper) / 2.0; }
        }

        /// <summary>
        /// Gets the standard deviation of the distribution.
        /// </summary>
        public double StdDev
        {
            get { return System.Math.Sqrt(((_upper - _lower + 1.0) * (_upper - _lower + 1.0) - 1.0) / 12.0); }
        }

        /// <summary>
        /// Gets the variance of the distribution.
        /// </summary>
        public double Variance
        {
            get { return ((_upper - _lower + 1.0) * (_upper - _lower + 1.0) - 1.0) / 12.0; }
        }

        /// <summary>
        /// Gets the entropy of the distribution.
        /// </summary>
        public double Entropy
        {
            get { return System.Math.Log(_upper - _lower + 1.0); }
        }

        /// <summary>
        /// Gets the skewness of the distribution.
        /// </summary>
        public double Skewness
        {
            get { return 0.0; }
        }

        /// <summary>
        /// Gets the smallest element in the domain of the distributions which can be represented by an integer.
        /// </summary>
        public int Minimum { get { return _lower; } }

        /// <summary>
        /// Gets the largest element in the domain of the distributions which can be represented by an integer.
        /// </summary>
        public int Maximum { get { return _upper; } }

        /// <summary>
        /// Computes the cumulative distribution function of the Bernoulli distribution.
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative density.</param>
        /// <returns>the cumulative density at <paramref name="x"/>.</returns>
        public double CumulativeDistribution(double x)
        {
            if (x < _lower)
            {
                return 0.0;
            }
            else if (x >= _upper)
            {
                return 1.0;
            }

            return Math.Min(1.0, (Math.Floor(x) - _lower + 1) / (_upper - _lower + 1));
        }

        #endregion

        #region IDiscreteDistribution Members

        /// <summary>
        /// The mode of the distribution; since every element in the domain has the same probability this method returns the middle one.
        /// </summary>
        public int Mode
        {
            get { return (int) Math.Floor((_lower + _upper) / 2.0); }
        }

        /// <summary>
        /// The median of the distribution.
        /// </summary>
        public int Median
        {
            get { return (int)Math.Floor((_lower + _upper) / 2.0); }
        }

        /// <summary>
        /// Computes the probability of a specific value.
        /// </summary>
        public double Probability(int val)
        {
            if (val >= _lower && val <= _upper)
            {
                return 1.0 / (_upper - _lower + 1);
            }

            return 0.0;
        }

        /// <summary>
        /// Computes the probability of a specific value.
        /// </summary>
        public double ProbabilityLn(int val)
        {
            if (val >= _lower && val <= _upper)
            {
                return - Math.Log(_upper - _lower + 1);
            }

            return Double.NegativeInfinity;
        }

        /// <summary>
        /// Samples a uniformly distributed random variable.
        /// </summary>
        public int Sample()
        {
            return DoSample(RandomSource, _lower, _upper);
        }

        /// <summary>
        /// Samples an array of uniformly distributed random variables.
        /// </summary>
        /// <returns>a sequence of samples from the distribution.</returns>
        public IEnumerable<int> Samples()
        {
            while (true)
            {
                yield return DoSample(RandomSource, _lower, _upper);
            }
        }

        #endregion

        /// <summary>
        /// Samples a uniformly distributed random variable.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="lower">The lower bound of the uniform random variable.</param>
        /// <param name="upper">The upper bound of the uniform random variable.</param>
        /// <returns>A sample from the discrete uniform distribution.</returns>
        public static int Sample(System.Random rnd, int lower, int upper)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(lower, upper))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            return DoSample(rnd, lower, upper);
        }

        /// <summary>
        /// Samples a sequence of uniformly distributed random variables.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="lower">The lower bound of the uniform random variable.</param>
        /// <param name="upper">The upper bound of the uniform random variable.</param>
        /// <returns>a sequence of samples from the discrete uniform distribution.</returns>
        public static IEnumerable<int> Samples(System.Random rnd, int lower, int upper)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(lower, upper))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            while (true)
            {
                yield return DoSample(rnd, lower, upper);
            }
        }

        /// <summary>
        /// Generates one sample from the discrete uniform distribution. This method does not do any parameter checking.
        /// </summary>
        /// <param name="rnd">The random source to use.</param>
        /// <param name="lower">The lower bound of the uniform random variable.</param>
        /// <param name="upper">The upper bound of the uniform random variable.</param>
        /// <returns>A random sample from the discrete uniform distribution.</returns>
        private static int DoSample(System.Random rnd, int lower, int upper)
        {
            return rnd.Next() % (upper - lower + 1) + lower;
        }
    }
}
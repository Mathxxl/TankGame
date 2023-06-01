using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    /// <summary>
    /// Class that generates random numbers with weighting
    /// </summary>
    [Serializable]
    public class RandomPondered : MonoBehaviour
    {
        #region Attributes

        public List<int> weightings;
        private List<int> _weightingsSummed;
        private int _total;

        #endregion

        #region Properties

        public int Count => weightings.Count;

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Create a RandomPondered Component using values
        /// </summary>
        /// <param name="parent">GameObject receiving the component</param>
        /// <param name="values">Ponderations</param>
        /// <returns></returns>
        public static RandomPondered CreateComponent(GameObject parent, IEnumerable<int> values)
        {
            var rp = parent.AddComponent<RandomPondered>();
            rp.weightings = new List<int>();
            foreach (var v in values)
            {
                rp.weightings.Add(v);
            }
            rp.CalculateValues();
            return rp;
        }
        
        /// <summary>
        /// Create a RandomPondered Component using a base value and a number of elements
        /// </summary>
        /// <param name="parent">GameObject receiving the component</param>
        /// <param name="baseValue">Base ponderation value for each element</param>
        /// <param name="count">Number of elements</param>
        /// <returns></returns>
        public static RandomPondered CreateComponent(GameObject parent, int baseValue, int count)
        {
            var rp = parent.AddComponent<RandomPondered>();
            rp.weightings = new List<int>();
            for(var i = 0; i < count; i++)
            {
                rp.weightings.Add(baseValue);
            }
            rp.CalculateValues();
            return rp;
        }
        
    
        /// <summary>
        /// Return a random int (0, Count) with weighting taken into account
        /// </summary>
        public int GetRandom()
        {
            var trueRandom = Random.Range(0, _total);
            for (var i = 0; i < _weightingsSummed.Count; i++)
            {
                var current = _weightingsSummed[i];
                if (trueRandom < current && current != 0)
                {
                    return i;
                }
            }
            return _weightingsSummed.Count;
        }

        /// <summary>
        /// Return a random int with weighting and exclusion list
        /// </summary>
        /// <param name="mem">List of values that should not be returned</param>
        /// <returns></returns>
        public int GetRandomWithExclusion(List<int> mem)
        {
            var value = GetRandom();
            while (mem.Contains(value))
            {
                value = GetRandom();
            }
            mem.Add(value);
            return value;
        }

        /// <summary>
        /// Add an element to the list of possibilities
        /// </summary>
        /// <param name="newValue">The weighting of the new element. If negative, value will be 0.</param>
        public void Add(int newValue = 0)
        {
            newValue = (newValue < 0) ? 0 : newValue;
        
            weightings.Add(newValue);
            _total += newValue;
            _weightingsSummed.Add(_total);
        }
    
        /// <summary>
        /// Add an element to the list of possibilities at index i.
        /// </summary>
        /// <param name="index">Index (-> value) to add the the possibilities, if greater than the current count zeros will be added before. If less, the value will be modified.</param>
        /// <param name="value">weighting of the new element. If negative, will be 0.</param>
        public void AddAt(int index, int value = 0)
        {
            var count = weightings.Count;
            if (index > count)
            {
                var diff = index - count;
                while (diff > 0)
                {
                    diff--;
                    Add();
                }
            }
            else
            {
                weightings[index] = value;
                CalculateValues();
            }
            Add(value);
        }

        /// <summary>
        /// Add weight to an element of the list.
        /// </summary>
        /// <param name="index">Index of the element. If greater than the number of elements, new element will be created.</param>
        /// <param name="value">Value to add.</param>
        public void AddWeight(int index, int value = 1)
        {
            var count = weightings.Count;
            if (index > count)
            {
                AddAt(index, value);
            }
            else
            {
                weightings[index] += value;
                CalculateValues();
            }
        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            weightings ??= new List<int>();
            CalculateValues();
        }
    
        /// <summary>
        /// Recalculate the whole _weightingSummed and the total.
        /// </summary>
        private void CalculateValues()
        {
            _weightingsSummed = new List<int>();
            var temp = 0;

            foreach (var value in weightings)
            {
                temp += value;
                _weightingsSummed.Add(temp);
            }

            _total = temp;
        }

        #endregion

        #endregion

        #region Testing

        /// <summary>
        /// Test method that makes a given number of random generations
        /// </summary>
        /// <param name="tests"></param>
        public void Testweighting(int tests = 1000)
        {
            var results = new int[Count];
            for (var i = 0; i < tests; i++)
            {
                var value = GetRandom();
                results[value]++;
            }

            var sresult = results.Aggregate("[", (current, v) => current + (v + " "));

            sresult += "]";
            Debug.Log(sresult);
        }

        #endregion
    }
}

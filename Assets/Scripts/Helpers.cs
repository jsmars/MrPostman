using System;
using System.Collections.Generic;
using UnityEngine;

public enum LetterColor { Red, Blue, Yellow }

public static class Helpers
{
	private static readonly int _letterColorCount = Enum.GetValues(typeof(LetterColor)).Length;

	public static Color FromLetterColor(LetterColor color)
	{
		switch (color)
		{
			case LetterColor.Red: return Color.red;
			case LetterColor.Blue: return Color.blue;
			case LetterColor.Yellow: return Color.yellow;
			default:
				Debug.LogError("Missing color for: " + color);
				return Color.black;
		}
	}

	public static void SetStampColor(GameObject obj, LetterColor color)
	{
		var stamp = obj.transform.Find("Stamp");
		if (stamp != null)
		{
			var stampMat = stamp.GetComponent<Renderer>();
            if (stampMat != null)
            {
                var letter = obj.GetComponent<LetterEntity>();
                if (letter == null || letter.LetterType == Assets.Scripts.Enums.LetterTypeEnum.Letter) // no letter means this is a bin
                    stampMat.material.color = Helpers.FromLetterColor(color);
                else
                    stampMat.material.color = Color.white;
            }
		}
	}

	public static int LetterColorCount
	{
		get
		{
			return _letterColorCount;
		}
	}

    /// <summary>
    /// A simple implementation of a list with weights that can give a random element based on weight. 
    /// Fast lookups in exchange for larger memory footprint based on precision. 
    /// Precision must be atleast number of objects in list, but should be higher to allow differences in weight.
    /// Add all elements at the same time as re-weighting may be costly.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeightedList<T>
    {
        public int EnabledElements { get; private set; }
        public int Count { get { return elements.Count; } }
        List<WeightedElement> elements;
        WeightedElement[] table;
        System.Random Random = new System.Random();

        T singleEnabled;

        public WeightedList(int precision = 100)
        {
            table = new WeightedElement[precision];
        }

        public void SetElements(List<WeightedElement> elements)
        {
            this.elements = elements;
            if (elements.Count > table.Length) throw new Exception("Elements contains more objects than current precision can handle!");

            //Normalize weights
            float total = 0;
            foreach (var item in elements)
                total += item.Weight;
            if (total <= 0)
                throw new Exception("No element has any weight!");
            foreach (var item in elements)
                item.Weight /= total;

            total = 0;
            int from, to = 0;
            foreach (var item in elements)
            {
                total += item.Weight;
                from = to;
                to = (int)(total * table.Length);
                for (int i = from; i < to; i++)
                    table[i] = item;
            }
            calcEnabled();
        }

        public void RecalculateWeights()
        {
            SetElements(elements);
        }
        
        public void Add(T element, int weight, bool recalculateWeights = true)
        {
            Add(new WeightedElement(element, weight), recalculateWeights);
        }

        public void Add(WeightedElement element, bool recalculateWeights = true)
        {
            if (elements == null)
                elements = new List<WeightedElement>();
            elements.Add(element);
            if (recalculateWeights)
                SetElements(elements);
        }

        public void Clear()
        {
            if (elements != null)
                elements.Clear();
            EnabledElements = 0;
        }

        public T RandomElement()
        {
            if (EnabledElements == 0) return default(T);
            if (EnabledElements == 1) return singleEnabled;
            if (elements.Count == 1) return elements[0].Value;

            var e = elements[UnityEngine.Random.Range(0, elements.Count)];
            if (e.Enabled)
                return e.Value;
            return RandomElement();
        }

        public void SetEnable(int index, bool value)
        {
            elements[index].Enabled = value;
            calcEnabled();
        }

        private void calcEnabled()
        {
            int count = 0;
            singleEnabled = default(T);
            foreach (var item in elements)
                if (item.Enabled)
                {
                    count++;
                    singleEnabled = item.Value;
                }
            EnabledElements = count;
        }

        public class WeightedElement
        {
            public WeightedElement(T value, float weight = 1)
            {
                Value = value;
                Weight = weight;
            }
            public T Value { get; private set; }
            public float Weight { get; internal set; }
            public bool Enabled = true;
        }
    }
}

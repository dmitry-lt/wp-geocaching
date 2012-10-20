using System;
using System.Collections;
using System.Collections.Generic;

namespace WP_Geocaching.Model
{
    public class HashSet<T> : ICollection<T>
    {
        private Dictionary<T, T> dictionary;

        public HashSet()
        {
            dictionary = new Dictionary<T, T>();
        }

        public void Add(T item)
        {
            dictionary.Add(item, item);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(T item)
        {
            return dictionary.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            return dictionary.Remove(item);
        }

        public IEnumerator GetEnumerator()
        {
            return dictionary.Keys.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return dictionary.Keys.GetEnumerator();
        }

        public int Count
        {         
            get
            {
                return dictionary.Keys.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
    }
}

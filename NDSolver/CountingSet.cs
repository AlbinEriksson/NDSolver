using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class CountingPair<T>
    {
        public T item;
        public int count;
    }

    class CountingSet<T> : ISet<T>
    {
        private List<CountingPair<T>> elements = new List<CountingPair<T>>();
        private int count = 0;

        public int Count => count;

        public bool IsReadOnly => false;

        public int this[T item]
        {
            get
            {
                return Get(item);
            }
        }

        public T this[int index]
        {
            get
            {
                return elements[index].item;
            }
        }

        public CountingSet() { }

        public CountingSet(List<CountingPair<T>> elements)
        {
            this.elements = new List<CountingPair<T>>(elements);
            foreach(var pair in elements)
            {
                count += pair.count;
            }
        }

        public CountingSet(IEnumerable<T> items)
        {
            AddAll(items);
        }

        public bool Add(T item)
        {
            if (TryGet(item, out CountingPair<T> result))
            {
                result.count++;
            }
            else
            {
                elements.Add(new CountingPair<T> { item = item, count = 1 });
            }

            count++;
            return true;
        }

        public void AddAll(IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            elements.Clear();
            count = 0;
        }

        public bool Contains(T item)
        {
            return Get(item) > 0;
        }

        public bool TryGet(T item, out CountingPair<T> result)
        {
            foreach(var pair in elements)
            {
                if(pair.item.Equals(item))
                {
                    result = pair;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach(var pair in elements)
            {
                for(int i = 0; i < pair.count; i++)
                {
                    array[arrayIndex] = pair.item;
                    arrayIndex++;
                }
            }
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            foreach(T item in other)
            {
                Remove(item);
            }
        }

        public int Get(T item)
        {
            foreach(var pair in elements)
            {
                if(pair.item.Equals(item))
                {
                    return pair.count;
                }
            }

            return 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CountingSetEnumerator<T>(elements);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            CountingSet<T> newSet = new CountingSet<T>();

            foreach(T item in other)
            {
                int count = 1;
                if(other is CountingSet<T> set)
                {
                    count = set[item];
                }

                int newCount = Math.Min(count, this[item]);

                for(int i = 0; i < newCount; i++)
                {
                    newSet.Add(item);
                }
            }

            elements = newSet.elements;
            count = newSet.count;
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            CountingSet<T> set = new CountingSet<T>(other);

            foreach(var pair in elements)
            {
                for (int i = 0; i < pair.count; i++)
                {
                    if (!set.Remove(pair.item))
                    {
                        return false;
                    }
                }
            }

            return set.Count > 0;
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            CountingSet<T> set = new CountingSet<T>(other);
            return set.IsProperSubsetOf(this);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            CountingSet<T> set = new CountingSet<T>(other);

            foreach (var pair in elements)
            {
                for (int i = 0; i < pair.count; i++)
                {
                    if (!set.Remove(pair.item))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            CountingSet<T> set = new CountingSet<T>(other);
            return set.IsProperSupersetOf(this);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            foreach(T item in other)
            {
                if(Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Remove(T item)
        {
            if(TryGet(item, out CountingPair<T> pair))
            {
                pair.count--;
                int count = pair.count;
                if(count == 0)
                {
                    elements.Remove(pair);
                }

                this.count--;
                return true;
            }

            return false;
        }

        public void RemoveAll(IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                Remove(item);
            }
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            CountingSet<T> set = new CountingSet<T>(other);

            foreach(var pair in elements)
            {
                if(set.Get(pair.item) != pair.count)
                {
                    return false;
                }
            }

            return true;
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            CountingSet<T> newSet = new CountingSet<T>(elements);
            CountingSet<T> intersection = new CountingSet<T>(elements);
            intersection.IntersectWith(other);
            newSet.UnionWith(other);
            newSet.ExceptWith(intersection);

            elements = newSet.elements;
            count = newSet.count;
        }

        public void UnionWith(IEnumerable<T> other)
        {
            CountingSet<T> set = new CountingSet<T>(other);

            foreach (var pair in set.elements)
            {
                int newCount = Math.Max(count, this[pair.item]);
                if(TryGet(pair.item, out CountingPair<T> myPair))
                {
                    myPair.count = newCount;
                }
                else
                {
                    elements.Add(new CountingPair<T> { item = pair.item, count = pair.count });
                }
            }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CountingSetEnumerator<T>(elements);
        }
    }

    class CountingSetEnumerator<T> : IEnumerator<T>
    {
        private readonly List<CountingPair<T>> elements;
        private List<CountingPair<T>>.Enumerator enumerator;

        public T Current => enumerator.Current.item;

        object IEnumerator.Current => Current;

        public CountingSetEnumerator(List<CountingPair<T>> elements)
        {
            this.elements = elements;
            Reset();
        }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator = elements.GetEnumerator();
        }
    }
}

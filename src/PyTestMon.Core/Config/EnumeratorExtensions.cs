using System.Collections;
using System.Collections.Generic;

namespace PyTestMon.Core.Config
{
    internal static class EnumeratorExtensions
    {
        #region Enumerator for this collection
        internal class Enumerator<T> : IEnumerator<T>
        {
            private readonly IEnumerator _enumerator;

            public Enumerator(IEnumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }

            public T Current
            {
                get { return (T)_enumerator.Current; }
            }

            object IEnumerator.Current
            {
                get { return _enumerator.Current; }
            }
        }
        #endregion

        internal static IEnumerator<T> ToGenericEnumerator<T>(this IEnumerator enumerator)
        {
            return new Enumerator<T>(enumerator);
        }
    }
}
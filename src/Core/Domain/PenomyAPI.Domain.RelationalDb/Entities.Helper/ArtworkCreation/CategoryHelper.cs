using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PenomyAPI.Domain.RelationalDb.Entities.Helper.ArtworkCreation;

public sealed class CategoryHelper
{
    #region Category Equality Comparer section.
    public sealed class CategoryEqualityComparer : IEqualityComparer<Category>
    {
        private static readonly object _lock = new object();
        private static CategoryEqualityComparer _instance;

        /// <summary>
        ///     Private constructor for singleton implementation.
        /// </summary>
        private CategoryEqualityComparer() { }

        public bool Equals(Category x, Category y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Category category)
        {
            return category.GetHashCode();
        }

        public static CategoryEqualityComparer Instance
        {
            get
            {
                lock (_lock)
                {
                    if (Equals(_instance, null))
                    {
                        _instance = new CategoryEqualityComparer();
                    }
                }

                return _instance;
            }
        }
    }

    public static CategoryEqualityComparer GetEqualityComparer()
    {
        return CategoryEqualityComparer.Instance;
    }
    #endregion Category Equality Comparer section.
}

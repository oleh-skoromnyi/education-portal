using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class PagedList<T>
    {
        public int PageCount { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public long TotalItemCount { get; }

        public IEnumerable<T> Items { get; }

        public PagedList(int pageNumber, int pageSize, long totalItemCount, IEnumerable<T> items)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.PageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            this.TotalItemCount = totalItemCount;
            this.Items = items;
        }
    }
}
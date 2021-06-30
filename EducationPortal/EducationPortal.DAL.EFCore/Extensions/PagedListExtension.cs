﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EducationPortal.Core;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.DAL
{
    public static class PagedListExtention
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException($"pageNumber = {pageNumber}. PageNumber cannot be below 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException($"pageSize = {pageSize}. PageSize cannot be less than 1.");
            }

            var subset = new List<T>();
            long totalCount = 0;
            if (query != null)
            {
                totalCount = query.LongCount();
                if (totalCount > 0)
                {
                    query = pageNumber == 1
                                ? query.Take(pageSize)
                                : query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                    subset = query.ToList();
                }
            }

            return new PagedList<T>(pageNumber, pageSize, totalCount, subset);
        }
    }
}

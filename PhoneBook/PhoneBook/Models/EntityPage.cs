﻿namespace PhoneBook.ViewModels
{
    using System.Collections.Generic;

    public class EntityPage<T> : List<T>
    {
        public EntityPage()
        {
        }

        public EntityPage(
            IEnumerable<T> phoneViewModels,
            int page,
            int minPage,
            int maxPage,
            int totalPages,
            int pageSize)
        {
            this.AddRange(phoneViewModels);
            this.Page = page;
            this.MinPage = minPage;
            this.MaxPage = maxPage;
            this.TotalPages = totalPages;
            this.PageSize = pageSize;
        }

        public int Page { get; }

        public int MinPage { get; }

        public int MaxPage { get; }

        public int TotalPages { get; }

        public int PageSize { get; }
    }
}

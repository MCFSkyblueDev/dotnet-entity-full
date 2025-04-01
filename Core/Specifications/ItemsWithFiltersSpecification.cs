using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Params;

namespace Core.Specifications
{
    public class ItemsWithFiltersSpecification : BaseSpecification<ItemEntity>
    {
        public ItemsWithFiltersSpecification(EntitySpecParams itemParams) : base(
            item => (
                string.IsNullOrEmpty(itemParams.Search) || item.Name.ToLower().Contains(itemParams.Search.ToLower())
            )
        )
        {
            // AddInclude(item => item.)
            ApplyPaging(itemParams.PageSize * (itemParams.PageNumber - 1), itemParams.PageSize);
            switch (itemParams.SortBy?.ToLower(), itemParams.SortOrder?.ToLower())
            {
                case ("name", "asc"):
                    Console.WriteLine("Name Asc");
                    AddOrderBy(p => p.Name);
                    break;
                case ("name", "desc"):
                    Console.WriteLine("Name Desc");
                    AddOrderByDescending(p => p.Name);
                    break;
                case ("price", "asc"):
                    AddOrderBy(p => p.Price);
                    break;
                case ("price", "desc"):
                    AddOrderByDescending(p => p.Price);
                    break;
                case ("quantity", "asc"):
                    AddOrderBy(p => p.Quantity);
                    break;
                case ("quantity", "desc"):
                    AddOrderByDescending(p => p.Quantity);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
        }
    }
}
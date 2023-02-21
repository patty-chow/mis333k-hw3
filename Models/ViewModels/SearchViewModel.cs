using NuGet.Protocol.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace Chow_Patty_HW3.Models
{
    public enum SearchType { GreaterThan, LessThan}

    [Microsoft.EntityFrameworkCore.Keyless]
    public class SearchViewModel
    {
        [Display(Name = "Search by Title:")]
        public String Title { get; set; }
        
        [Display(Name = "Search by Description:")]
        public String Description { get; set; }

        [Display(Name = "Search by Category:")]
        public Category? SelectedCategories { get; set; }
        
        [Display(Name = "Search by Language:")]
        public Int32 SelectedLangID { get; set; }

        [Display(Name ="Search by Star Count:")]
        public Decimal? SearchStarCount { get; set; }

        [Display(Name = "Type of Search;")]
        public SearchType? SearchType { get; set; }


        [Display(Name = "Search by Last Updated Date:")]
        [DataType(DataType.Date)]
        public DateTime? SelectedDate { get; set; }
    }

}
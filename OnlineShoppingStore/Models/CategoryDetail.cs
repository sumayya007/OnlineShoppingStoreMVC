using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShoppingStore.Models
{
    public class CategoryDetail
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage="Category name is required")]
        [StringLength(100,ErrorMessage="Minimum 3 and maximum 100 characters are allowed",MinimumLength=100)]
        public string CategoryName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
    public class ProductDetail
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product name name is required")]
        [StringLength(100, ErrorMessage = "Minimum 3 and maximum 100 characters are allowed", MinimumLength = 100)]
        public string ProductName { get; set; }
        [Required]
        [Range(1,50)]
        public Nullable<int> CategoryId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        [Required]
        [Range(typeof(int),"1","500",ErrorMessage="Invalid quantity")]
        public Nullable<int> Quantity { get; set; }
        [Required]
        [Range(typeof(decimal),"1","200000",ErrorMessage="Invalid price")]
        public Nullable<decimal> Price { get; set; }
        public SelectList Categories { get; set; }
    }
}
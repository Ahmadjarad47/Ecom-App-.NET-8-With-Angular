using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities.Product
{
    public class Rating : BaseEntity<int>
    {
        [Range(1, 5)]
        public int Stars { get; set; }
        public string content { get; set; }
        public DateTime Review { get; set; } = DateTime.Now;
        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public virtual AppUser AppUser { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}

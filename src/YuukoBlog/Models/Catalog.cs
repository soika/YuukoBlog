namespace YuukoBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Catalog
    {
        public Guid Id { get; set; }

        [MaxLength(32)]
        public string Url { get; set; }

        public string Title { get; set; }

        public int PRI { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PWANotesApp.Web.Models
{
    public class Note
    {
        public Note()
        {
            Items = new HashSet<NoteItem>();
        }

        [Required]
        public int Id
        {
            get;
            set;
        }

        [Required]
        [StringLength(256)]
        public string Title
        {
            get;
            set;
        }

        [Required]
        [StringLength(450)]
        public string Owner
        {
            get;
            set;
        }

        [Required]
        [StringLength(450)]
        public string CreatedBy
        {
            get;
            set;
        }

        [Required]
        public DateTime CreatedDate
        {
            get;
            set;
        }

        [Required]
        [StringLength(450)]
        public string LastUpdatedBy
        {
            get;
            set;
        }

        [Required]
        public DateTime LastUpdatedDate
        {
            get;
            set;
        }

        public virtual ICollection<NoteItem> Items
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PWANotesApp.Web.Models
{
    public class NoteItem
    {
        [Required]
        public int Id
        {
            get;
            set;
        }

        [Required]
        public int NoteId
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        [Required]
        public NoteItemType Type
        {
            get;
            set;
        }

        [Required]
        public int Order
        {
            get;
            set;
        }

        public virtual Note Note
        {
            get;
            set;
        }
    }
}
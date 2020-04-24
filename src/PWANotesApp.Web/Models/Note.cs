using System;
using System.Collections.Generic;
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

        public int Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Owner
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public string LastUpdatedBy
        {
            get;
            set;
        }

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

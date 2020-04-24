using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWANotesApp.Web.Models
{
    public class NoteItem
    {
        public int Id
        {
            get;
            set;
        }

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

        public NoteItemType Type
        {
            get;
            set;
        }

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
using PWANotesApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWANotesApp.Web.ViewModels
{
    public class NoteItemViewModel
    {
        public int NoteId
        {
            get;
            set;
        }

        public NoteItemType Type
        {
            get;
            set;
        }
    }

    public class NewTextNoteItemViewModel : NoteItemViewModel
    {
        public string TextContent
        {
            get;
            set;
        }
    }

    public class EditTextNoteItemViewModel : NewTextNoteItemViewModel
    {
        public int Id
        {
            get;
            set;
        }
    }

    public class NoteItemDetailsViewModel : NoteItemViewModel
    {
        public int Id
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }
    }
}

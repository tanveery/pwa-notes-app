using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWANotesApp.Web.ViewModels
{
    public class NoteViewModel
    {
        public string Title
        {
            get;
            set;
        }
    }

    public class IndexNoteViewModel : NoteViewModel
    {
        public int Id
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public DateTime LastUpdatedDate
        {
            get;
            set;
        }
    }

    public class NoteDetailsViewModel : IndexNoteViewModel
    {
        public List<NoteItemDetailsViewModel> Items
        {
            get;
            set;
        }
    }

    public class NewNoteViewModel : NoteViewModel
    { }

    public class EditNoteTitleViewModel : NoteViewModel
    {
        public int Id
        {
            get;
            set;
        }
    }
}

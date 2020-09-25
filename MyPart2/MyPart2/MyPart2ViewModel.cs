using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyPart2
{
    public class MyPart2ViewModel : INotifyPropertyChanged
    {
        public IList<DocumentType> DocumentTypes { get; set; }

        DocumentType _selectedDocumentType;

        public DocumentType SelectedDocumentType { get {
                return _selectedDocumentType;
            }
            set {
                if(_selectedDocumentType != value)
                {
                    _selectedDocumentType = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class DocumentTypes
    {
        public List<DocumentType> data { get; set; }
    }

    public class DocumentType
    {
        public int id { get; set; }

        public string name { get; set; }

        public bool active { get; set; }

        public bool has_back_image { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}

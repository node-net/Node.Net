using System.Collections;

namespace Node.Net.Collections
{
    public class Document : HashBase
    {
        public Document() { Update(); }
        public Document(string json) : base(json) { Update(); }
        public Document(System.IO.Stream stream) : base(stream) { Update(); }

        private Traverser traverser = null;
        [System.ComponentModel.Browsable(false)]
        public Traverser Traverser 
        { 
            get 
            { 
                if(object.ReferenceEquals(null,traverser))
                {
                    traverser = new Traverser(this);
                    traverser.SetDocument(this, this);
                }
                return traverser; 
            } 
        }

        public IMetaDataManager MetaData
        {
            get { return traverser.MetaData; }
        }
        public override void Open(System.IO.Stream stream)
        {
            Clear();
            var dictionary = (IDictionary)GetReader().Read(stream);
            Collections.Copier.Copy(dictionary, this);
            Update();
        }

        public override void Update(bool deep=true)
        {
            traverser = null;
            traverser = new Traverser(this);
            traverser.SetDocument(this, this);
        }


    }
}

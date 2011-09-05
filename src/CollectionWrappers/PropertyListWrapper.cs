using System;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.CollectionWrappers
{
    public class PropertyListWrapper<ContainerT, PropertyT> : PropertyCollectionWrapper<ContainerT, PropertyT> , IList<PropertyT>
    {
        protected IList<ContainerT> innerList
        {
            get
            {
                return (IList<ContainerT>)innerCollection;
            }
        }

        public PropertyListWrapper(IList<ContainerT> toWrap, string propertyName) : base(toWrap, propertyName) { }
        public int IndexOf(PropertyT item) {
            throw new NotSupportedException("since the container information is lost, this operation cannot be supported") ;
        }

        public void Insert(int index, PropertyT item) {
            ThrowReadOnlyException();
        }

        public void RemoveAt(int index) {
            ThrowReadOnlyException();
        }

        public PropertyT this[int index] {
            get {
                PropertyEnumeratorWrapper<ContainerT, PropertyT> en = (PropertyEnumeratorWrapper<ContainerT, PropertyT>) GetEnumerator();
                return en.GetPropVal(innerList[index]);
            }
            set { ThrowReadOnlyException(); }
        }
    }
}

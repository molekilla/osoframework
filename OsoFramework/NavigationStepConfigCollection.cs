// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using OsoFramework.Http;

namespace OsoFramework
{
 
    public class NavigationStepConfigCollection : ConfigurationElementCollection
    {
        public NavigationStepConfigCollection()
        {

        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new HttpNavigationStep();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((HttpNavigationStep)element).Name;
        }

        public HttpNavigationStep this[int index]
        {
            get
            {
                return (HttpNavigationStep)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public HttpNavigationStep this[string Name]
        {
            get
            {
                return (HttpNavigationStep)BaseGet(Name);
            }
        }

        public int IndexOf(HttpNavigationStep url)
        {
            return BaseIndexOf(url);
        }

        public void Add(HttpNavigationStep url)
        {
            BaseAdd(url);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(HttpNavigationStep url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }


}

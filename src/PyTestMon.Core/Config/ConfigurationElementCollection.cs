using System;
using System.Collections.Generic;
using System.Configuration;

namespace PyTestMon.Core.Config
{
    public class ConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T> where T : ConfigurationElement, new()
    {
        private readonly string _elementName;
        private readonly Func<T, object> _keyExtractor;

        public ConfigurationElementCollection(string elementName, Func<T, object> keyExtractor)
        {
            _elementName = elementName;
            _keyExtractor = keyExtractor;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return _keyExtractor((T)element);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return _elementName; }
        }

        public new IEnumerator<T> GetEnumerator()
        {
            return base.GetEnumerator().ToGenericEnumerator<T>();
        }
    }
}
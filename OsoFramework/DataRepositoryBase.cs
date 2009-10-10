// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Text;
using SubSonic.Repository;

namespace OsoFramework
{
    public class DataRepositoryBase : IDataRepository
	{
        protected SimpleRepository dataRespository;

        public DataRepositoryBase()
        {
        }
        #region IDataRepository implementation


        public virtual void Update<T>(T data) where T : class, new()
        {
            dataRespository.Update(data);
        }

        public virtual void Delete<T>(T data) where T : class, new()
        {
            dataRespository.Delete<T>(data);
        }
        public virtual void Insert<T>(T data) where T : class, new()
        {
            dataRespository.Add<T>(data);
        }

        public virtual System.Collections.Generic.IEnumerable<T> FindBy<T>(object id) where T : class, IParseData, new()
        {

            List<T> items = new List<T>();
            IList<T> foundItems = dataRespository.Find<T>(x => x.KeyIndex == id.ToString());
            foreach (var item in foundItems)
            {
                items.Add(item);
            }
            return items.ToArray();
        }

        public virtual System.Collections.Generic.IEnumerable<T> FindAll<T>() where T : class, new()
        {
            List<T> items = new List<T>();

            foreach (var item in dataRespository.All<T>())
            {
                items.Add(item);
            }
            return items.ToArray();
        }

        #endregion

	}
}

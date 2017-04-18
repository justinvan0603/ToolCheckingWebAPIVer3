using System.Collections.Generic;
using ChatBot.Infrastructure.Mappings;

namespace ChatBot.ViewModels
{

    public class BaseViewModel<S> where S : class
    {
        public virtual void OnConvert(S source, params object[] values)
        {
            dynamic dest = this;
            PropertyCopy.Copy(source, dest);
        }

    }

    public class ViewModelMapper<D, S>
        where D : BaseViewModel<S>, new()
        where S : class, new()
    {
        public static D Map(S source, params object[] values)
        {
            D dest = new D();
            dest.OnConvert(source, values);
            return dest;
        }
        public static List<D> MapObjects(List<S> sources, params object[] values) 
        {
            List<D> dests = new List<D>();
            foreach (S source in sources)
            {
                D dest = new D();
                dest.OnConvert(source, values);
                dests.Add(dest);
            }
            return dests;
        }

    }



}
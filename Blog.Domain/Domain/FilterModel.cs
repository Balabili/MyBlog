using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class FilterModel
    {
        public FilterModel()
        {
            Pager = new Pager
            {
                Index = 1,
                Size = 10
            };
        }

        public Pager Pager { get; set; }

        public Boolean HasPager { get; set; }

        public String KeyWords { get; set; }

        public String MainTable { get; set; }

        public String Condition { get; set; }

    }

    public class Pager
    {
        private Int32 _index = 1;

        private Int32 _size = 10;

        public Int32 Index
        {
            get
            {
                return this._index;
            }
            set
            {
                _index = value;
            }
        }

        public Int32 Size
        {
            get
            {
                return this._size;
            }
            set
            {
                _size = value;
            }
        }
    }
}

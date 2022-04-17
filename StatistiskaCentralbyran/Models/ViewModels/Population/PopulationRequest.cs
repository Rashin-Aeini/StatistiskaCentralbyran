using System.Collections.Generic;

namespace StatistiskaCentralbyran.Models.ViewModels.Population
{
    public class PopulationRequest
    {
        private int _page;
        private int _size;

        public int Page 
        { 
            get
            {
                if(_page == 0)
                {
                    _page = 1;
                }

                return _page;
            }
            set
            {
                _page = value;
            }
        }

        public int Size
        {
            get
            {
                if (_size == 0)
                {
                    _size = 10;
                }

                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public string Value { get; set; }
    }
}

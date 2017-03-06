using System;
using System.Collections.Generic;
using System.Text;

namespace BucketApp.ViewModels
{
    class ObjectsModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Ower { get; set; }
        public int Rating { get; set; }
        public Location Location { get; set; }
        public List<Floor> Floors { get; set; }
    }
    class Floor
    {

    }
    class Location
    {

    }
}

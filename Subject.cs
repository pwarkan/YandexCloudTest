using System;
using System.Collections.Generic;
using System.Text;

namespace YandexCloudTest
{
    public class Subject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Court> ChildCourts { get; set; } = new List<Court>();
    }
}

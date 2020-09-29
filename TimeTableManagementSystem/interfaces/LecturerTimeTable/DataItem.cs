using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManagementSystem.interfaces.LecturerTimeTable
{
    class DataItem
    {

        public string Time { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }

        public string[] MyDates = new string[7];

        public string this[int index]
        {
            get => MyDates[index];
            set => MyDates[index] = value;
        }


    }
}

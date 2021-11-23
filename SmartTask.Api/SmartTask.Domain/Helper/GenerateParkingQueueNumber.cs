using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTask.Domain.Helper
{
   public class GenerateParkingQueueNumber
    {
        public static StringBuilder GetNumber(int number)
        {
            StringBuilder generatedNumber = new StringBuilder($"{number+1}-{DateTime.Now.DayOfYear}");

            return generatedNumber;
        }
    }
}

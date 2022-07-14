namespace SolarCalculator.Service.Models
{
    //{
    //    "results": {
    //        "sunrise":"6:39:53 AM",
    //        "sunset":"6:48:32 PM",
    //        "solar_noon":"12:44:12 PM",
    //        "day_length":"12:08:38.8300000" },
    //    "status":"OK"
    //}

    public class SolarCalculatorResult
    {
        public ResultValues results { get; set; }
        public string status { get; set; }

        public SolarCalculatorResult()
        {
            results = new ResultValues();
        }
    }

    public class ResultValues
    {
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string solar_noon { get; set; }
        public string day_length { get; set; }
    }

}
namespace DataProcessor.Library.Tests;

public static class TestData
{
    public static List<string> Data = new List<string>()
    {
        "John,Koenig,1975/10/17,6",
        "INVALID RECORD FORMAT",
        "Dylan,Hunt,2000/10/02,8",
        "John,Crichton,1999/03/19,7",
        "Check,Date,0/2//,9",
        "Dave,Lister,1988/02/15,9",
        "BAD RECORD",
        "John,Sheridan,1994/01/26,6",
        "Dante,Montana,2000/11/01,5",
        "Check,Rating,2014/05/03,a",
        "Isaac,Gampu,1977/09/10,4",
    };

    public static List<string> GoodRecord =
        new List<string>() { "John,Koenig,1975/10/17,6" };

    public static List<string> BadRecord =
        new List<string>() { "BAD RECORD" };

    public static List<string> BadStartDate =
        new List<string>() { "Check,Date,0/2//,9" };

    public static List<string> BadRating =
        new List<string>() { "Check,Rating,2014/05/03,a" };

}

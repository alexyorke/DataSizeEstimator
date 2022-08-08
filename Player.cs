using System.ComponentModel.DataAnnotations;

namespace DataSizeEstimator;

public class Player
{
    [CreditCard]
    public string PlayerDescription { get; set; }

    [MaxLength(10)]
    //[StringLength(2)]
    public List<List<string>> Names { get; set; }
}
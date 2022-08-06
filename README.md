# DataSizeEstimator

## What does this do?

Given some code, it automatically creates a valid object that conforms to all `ValidationAttributes`. This means that string lengths are randomized, list lengths randomized, and fake data is auto-generated.

Also works recursively on properties, including generics.

```csharp
public class Player
{
    [CreditCard]
    public string PlayerDescription { get; set; }

    [MaxLength(10)]
    [StringLength(2)]
    public List<List<string>> Names { get; set; }
}
```

Sample output on `Player.Names`:

```
[[[["alCPreMN2C","ie3b","e6ZF9($M","^u0J","TmlM5","ruB^f1UBP8","29fE#","Wq","f","G*Nf)"]],[["ajoxugg5","MvEAFNtw1","9ZRJE","K$","Ua0dE","vBj","2ZY52Xqzyq","Uthf(A4","NQ801#ypn","V"]],[["*b","foPEruDjh","rg","VPgvkhQ@S","Faw9HqLSb#","9zVG","g2DMPgLldK","Gni1ia4dFw","EaJrAjvE4","zNQmn"]],[["n@7","R17^","","oJD","HG","HuaJY)DY","Y3ia8KeDnR","u(kw2k","ZCtvZ4","i2Y*"]],[["LIBm","07f$gPhN","NS(5qV8G9","s7KKBm","9LyKX","n","e#","t4S(GSpfv","^zNKJPpq",""]],[["mp","FS","0JE46PGvVO","7Xn","P9e","l9x","SbA","KFwRmu","dUidLLvJ4",""]],[["j)jV","DxenUR9EH","H(PT","lIWOrXVN","2LpvOzW2y","","8rcJ","r","kwXIem0Yi",""]],[["VTOrGvQbx","9","d#4X","j5Nd51aZP",")mfyw","w)Lls(7CLw","T0zd","VcxU","UN",""]],[["ovSe)WX","eu","8dvUh@n9","H2*D","gU@nhxH","Iq1p","*GQSi","","61eN","x5QZ"]],[["6gDpuevdB","","BAu@z","R)bNge5c","r9EB#i(","pRU*VS4NOH","","QJd","AN2lI","u3Maem"]]]]
```

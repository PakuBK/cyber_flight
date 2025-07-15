using System;

namespace CF.Data {
[Serializable]
public class CurrencyToken
{
    public int FragmentsTier1 = 0;
    public int FragmentsTier2 = 0;
    public int FragmentsTier3 = 0;
    public int FragmentsTier4 = 0;
    public int FragmentsTier5 = 0;

    public int Shards = 0;

    public CurrencyToken(int shards, int f1=0, int f2=0, int f3=0, int f4=0, int f5=0)
    {
        Shards = shards;
        FragmentsTier1 = f1;
        FragmentsTier2 = f2;
        FragmentsTier3 = f3;
        FragmentsTier4 = f4;
        FragmentsTier5 = f5;
    }
}
}

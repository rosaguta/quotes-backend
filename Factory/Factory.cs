using Interface;
using DAL;
namespace Factory;

public static class DalFactory
{
    public static IQuoteDAL Get()
    {
        return new QuoteDAL();
    }

    public static IRizzDAL Getrizz()
    {
        return new RizzDAL();
    }
}
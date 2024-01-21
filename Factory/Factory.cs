using Interface;
using DAL;
namespace Factory;

public static class FactoryQuotaDal
{
    public static IQuoteDAL Get()
    {
        return new QuoteDAL();
    }
}
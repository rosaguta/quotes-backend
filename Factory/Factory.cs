using Interface;
using DAL;
namespace Factory;

public static class DalFactory
{
    public static IQuoteDAL GetQuoteDal()
    {
        return new QuoteDAL();
    }

    public static IRizzDAL GetRizzDal()
    {
        return new RizzDAL();
    }

    public static IUserDAL GetUserDal()
    {
        return new UserDAL();
    }

    public static IInsultDAL GetInsultDal()
    {
        return new InsultDAL();
    }

    public static ILonerDAL GetLonerDal()
    {
        return new LonerDAL();
    }
}
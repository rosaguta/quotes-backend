using DTO;
using Interface;
using Logic.Mapper;

namespace Logic;

public class LonerCollection
{
    readonly ILonerDAL _LonerInterface;

    public LonerCollection()
    {
        _LonerInterface = Factory.DalFactory.GetLonerDal();
    }

    public bool PostTime(Loner loner)
    {
        LonerDTO lonerDto = loner.convertToDto();
        bool posted = _LonerInterface.PostTime(lonerDto);
        return posted;
    }
}
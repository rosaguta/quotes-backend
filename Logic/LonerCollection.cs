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
        loner.CalculateAloneDuration();
        LonerDTO lonerDto = loner.convertToDto();
        bool posted = _LonerInterface.PostTime(lonerDto);
        return posted;
    }

    public List<Loner>? GetAllLoners()
    {
        List<Loner>? loners = new List<Loner>();
        List<LonerDTO>? lonerDtos = _LonerInterface.getAllLoners();
        foreach (LonerDTO? lonerDto in lonerDtos)
        {
            loners.Add(lonerDto.ConvertToLogic());
        }

        return loners;
    }
}
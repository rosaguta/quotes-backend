using DTO;

namespace Interface;

public interface ILonerDAL
{
    bool PostTime(LonerDTO lonerDto);
}
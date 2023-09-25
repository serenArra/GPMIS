using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Authorization.Users.Dto
{
    public interface IGetLoginAttemptsInput: ISortedResultRequest
    {
        string Filter { get; set; }
    }
}
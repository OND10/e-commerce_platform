using Rewards.API.Message;

namespace Rewards.API.Services
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewards);
    }
}

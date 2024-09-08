using Microsoft.EntityFrameworkCore;
using Rewards.API.DataBase;
using Rewards.API.Message;
using Rewards.API.Models;
using System.Text;

namespace Rewards.API.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> _dboptions;

        public RewardService(DbContextOptions<AppDbContext> dboptions)
        {
            _dboptions = dboptions;
        }

        public async Task UpdateRewards(RewardsMessage rewards)
        {
            Reward reward = new()
            {
                UserId = rewards.UserId,
                OrderId = rewards.OrderId,
                RewardDate = DateTime.Now,
                RewardActivity = rewards.RewardsActivity
            };

            await using var _db = new AppDbContext(_dboptions);
            await _db.AddAsync(reward);
            await _db.SaveChangesAsync();
        }
    }
}

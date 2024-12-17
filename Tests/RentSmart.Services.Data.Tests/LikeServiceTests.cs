namespace RentSmart.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MockQueryable;
    using Moq;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using Xunit;

    public class LikeServiceTests
    {
        [Fact]
        public async Task WhenUserLike2TimesOnly1LikeShouldBecounted()
        {
            var list = new List<UserLike>();
            var mockQueryable = list.AsQueryable().BuildMock();
            var mockRepo = new Mock<IRepository<UserLike>>();
            mockRepo.Setup(x => x.All()).Returns(mockQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<UserLike>())).Callback((UserLike userLike) => list.Add(userLike));
            var service = new LikeService(mockRepo.Object);

            await service.SetLikeAsync("1", "1", true);
            await service.SetLikeAsync("1", "1", true);
            await service.SetLikeAsync("1", "1", true);

            Assert.Equal(1, list.Count);
        }

        [Fact]
        public async Task WhenUserDisLike2TimesOnly1LikeShouldBeCountedIfItIsAlreadyLiked()
        {
            var list = new List<UserLike>
            {
                new UserLike { UserId = "1", PropertyId = "1" },
            };

            var mockQueryable = list.AsQueryable().BuildMock();
            var mockRepo = new Mock<IRepository<UserLike>>();
            mockRepo.Setup(x => x.All()).Returns(mockQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<UserLike>())).Callback((UserLike userLike) => list.Add(userLike))
                .Returns(Task.CompletedTask);
            mockRepo.Setup(x => x.Delete(It.IsAny<UserLike>()))
                .Callback((UserLike userLike) => list.Remove(userLike));
            var service = new LikeService(mockRepo.Object);

            await service.SetLikeAsync("1", "1", false);
            await service.SetLikeAsync("1", "1", false);

            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task WhenUserDisLikeNotLikedPropertyNothingHappens()
        {
            var list = new List<UserLike>();
            var mockQueryable = list.AsQueryable().BuildMock();
            var mockRepo = new Mock<IRepository<UserLike>>();
            mockRepo.Setup(x => x.All()).Returns(mockQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<UserLike>())).Callback((UserLike userLike) => list.Add(userLike))
                .Returns(Task.CompletedTask);

            var service = new LikeService(mockRepo.Object);

            await service.SetLikeAsync("1", "1", false);
            Assert.Equal(0, list.Count);
        }
    }
}

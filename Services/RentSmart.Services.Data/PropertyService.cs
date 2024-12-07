﻿namespace RentSmart.Services.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Mapping;
    using RentSmart.Web.ViewModels.Properties.InputModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels;
    using RentSmart.Web.ViewModels.Properties.ViewModels.AllByUser;

    public class PropertyService : IPropertyService
    {
        private const int MaxNumberOfCostumTags = 3;
        private const int MaxNumberOfImages = 5;
        private readonly string[] allowedExtensions = ["jpg", "png", "gif"];
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<District> districtRepository;
        private readonly IDeletableEntityRepository<Tag> tagRepository;
        private readonly IDeletableEntityRepository<Manager> managerRepository;
        private readonly IDeletableEntityRepository<Rental> rentalRepository;
        private readonly IOrderService orderService;

        public PropertyService(
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<District> districtRepository,
            IDeletableEntityRepository<Tag> tagRepository,
            IDeletableEntityRepository<Manager> managerRepository,
            IDeletableEntityRepository<Rental> rentalRepository,
            IOrderService orderService)
        {
            this.propertyRepository = propertyRepository;
            this.districtRepository = districtRepository;
            this.tagRepository = tagRepository;
            this.managerRepository = managerRepository;
            this.rentalRepository = rentalRepository;
            this.orderService = orderService;
        }

        public async Task AddAsync(AddPropertyInputModel input, string userId, string imagePath)
        {
            var property = new Property
            {
                Description = input.Description,
                Floor = input.Floor,
                Size = input.Size,
                PropertyTypeId = input.PropertyTypeId,
                CityId = input.CityId,
                OwnerId = input.OwnerId,
                PricePerMonth = input.PricePerMonth,
            };

            var manager = this.managerRepository.All().FirstOrDefault(x => x.UserId == userId);
            property.Manager = manager;

            var district = this.districtRepository.All().FirstOrDefault(x => x.Name == input.DistrictName);
            district ??= new District { Name = input.DistrictName };

            property.District = district;
            foreach (var tagInput in input.TagIds)
            {
                var tag = this.tagRepository.All().FirstOrDefault(x => x.Id == tagInput);
                property.Tags.Add(new PropertyTag { Tag = tag, Property = property });
            }

            if (input.CustomTags.Count() > MaxNumberOfCostumTags)
            {
                throw new Exception($"You have reached the maximum limit of {MaxNumberOfCostumTags} customtags!");
            }

            foreach (var tagName in input.CustomTags)
            {
                var tag = this.tagRepository.All().FirstOrDefault(x => x.Name == tagName) ?? new Tag { Name = tagName };
                property.Tags.Add(new PropertyTag { Tag = tag, Property = property });
            }

            if (input.Images != null && input.Images.Any())
            {
                if (input.Images.Count() > MaxNumberOfImages)
                {
                    throw new Exception($"You have reached the maximum limit of {MaxNumberOfImages} images!");
                }

                Directory.CreateDirectory($"{imagePath}/properties/");
                foreach (var image in input.Images)
                {
                    var extension = Path.GetExtension(image.FileName).TrimStart('.');
                    if (!this.allowedExtensions.Contains(extension))
                    {
                        throw new Exception($"Invalid image extension {extension}");
                    }

                    var dbImage = new Image
                    {
                        Extension = extension,
                    };
                    property.Images.Add(dbImage);
                    var physicalPath = $"{imagePath}/properties/{dbImage.Id}.{extension}";
                    using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                    await image.CopyToAsync(fileStream);
                }
            }

            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();
            await this.orderService.UsingActiveOrder(property.OwnerId, property.Id);
        }

        public async Task<List<PropertyInListViewModel>> GetAllAvailableAsync<TPropertyInListViewModel>(int page, int propertiesPerPage)
        {
            var properties = await this.propertyRepository.AllAsNoTracking()
                 .OrderByDescending(x => x.CreatedOn)
                 .To<PropertyInListViewModel>()
                 .ToListAsync();

            properties = properties.Where(x => this.IsPropertyAvailable(x.Id)).Skip((page - 1) * propertiesPerPage)
                 .Take(propertiesPerPage).ToList();

            foreach (var property in properties)
            {
                var averageRating = this.AveragePropertyRating(property.Id);
                property.AverageRating = averageRating == 0 ? "No rating yet!" : $"{averageRating:0.0} / 5";
                property.IsAvailable = true;
                property.TotalLikes = this.GetPropertyLikesCount(property.Id);
            }

            return properties;
        }

        public async Task<PropertyDetailsViewModel> GetByIdAsync(string id)
        {
            var property = await this.propertyRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<PropertyDetailsViewModel>()
                .FirstOrDefaultAsync();
            var averageRating = this.AveragePropertyRating(property.Id);
            property.AverageRating = averageRating == 0 ? "No rating yet!" : $"{averageRating:0.0} / 5";
            property.IsAvailable = this.IsPropertyAvailable(property.Id);

            var dbProperty = await this.propertyRepository.AllAsNoTracking()
                .Include(x => x.Images)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            foreach (var tag in dbProperty.Tags)
            {
                property.TagsTagNames.Add(tag.Tag.Name);
            }

            foreach (var image in dbProperty.Images)
            {
                var stringImage = image.RemoteImageUrl != null ? image.RemoteImageUrl.ToString()
                    : "/images/properties/" + image.Id + "." + image.Extension;
                property.ImagesUrls.Add(stringImage);
            }

            if (dbProperty.Images.Count == 0)
            {
                property.ImagesUrls.Add("/images/noimage.jpg");
            }

            property.TotalLikes = this.GetPropertyLikesCount(property.Id);
            return property;
        }

        public double AveragePropertyRating(string propertyId)
        {
            var property = this.propertyRepository.AllAsNoTracking()
                .Include(x => x.Rentals)
                .ThenInclude(x => x.Rating)
                .Where(x => x.Id == propertyId)
                .FirstOrDefault();
            if (property.Rentals.Count == 0)
            {
                return 0;
            }

            var ratings = property.Rentals.Select(x => x.Rating?.AverageRating).Where(x => x.HasValue).ToList();
            return ratings.Count > 0 ? (double)ratings.Average() : 0;
        }

        public bool IsPropertyAvailable(string propertyId)
        {
            var property = this.propertyRepository.AllAsNoTracking()
                .Include(x => x.Rentals)
                .Where(x => x.Id == propertyId)
                .FirstOrDefault();
            if (property.Rentals.Count == 0)
            {
                return true;
            }

            var lastRent = property.Rentals.OrderByDescending(x => x.RentDate).FirstOrDefault();
            return lastRent!.RentDate.AddMonths(lastRent.DurationInMonths) < DateTime.UtcNow;
        }

        public async Task<string> PropertyCurrentRenterId(string propertyId)
        {
            var lastRental = await this.rentalRepository.AllAsNoTracking()
                .Include(x => x.Renter)
                .Where(x => x.PropertyId == propertyId)
                .OrderByDescending(x => x.RentDate)
                .FirstOrDefaultAsync();
            if (!lastRental.IsActive)
            {
                return null;
            }

            return lastRental.Renter.UserId;
        }

        public async Task<UserAllPropertiesViewModel> GetByIdAllProperties(string userId, bool isManager, bool isOwner, bool isRenter, int page, int propertiesPerPage)
        {
            var allProperties = new UserAllPropertiesViewModel();
            allProperties.Id = userId;

            var likedProperties = await this.propertyRepository.AllAsNoTracking()
                .Where(x => x.Likes.Any(x => x.UserId == userId))
                .To<LikedPropertiesViewModel>()
                .ToListAsync();
            allProperties.LikedProperties = likedProperties;

            if (isOwner)
            {
                var ownerProperties = await this.propertyRepository.AllAsNoTracking()
                    .Where(x => x.Owner.UserId == userId)
                    .To<OwnerPropertyInListViewModel>()
                    .ToListAsync();
                foreach (var property in ownerProperties)
                {
                    property.IsAvailable = this.IsPropertyAvailable(property.Id);
                }

                allProperties.OwnedProperties = ownerProperties;
            }

            if (isManager)
            {
                var totalManagerPropertiesCount = await this.propertyRepository.AllAsNoTracking()
                    .Where(x => x.Manager.UserId == userId).CountAsync();
                var managerProperties = await this.propertyRepository.AllAsNoTracking()
                    .Where(x => x.Manager.UserId == userId)
                    .OrderByDescending(x => x.CreatedOn)
                    .Skip((page - 1) * propertiesPerPage)
                    .Take(propertiesPerPage)
                    .To<ManagerPropertyInListViewModel>()
                    .ToListAsync();
                foreach (var property in managerProperties)
                {
                    property.IsAvailable = this.IsPropertyAvailable(property.Id);
                }

                allProperties.ManagedProperties.Properties = managerProperties;
                allProperties.ManagedProperties.ItemsPerPage = propertiesPerPage;
                allProperties.ManagedProperties.CurrentPage = page;
                allProperties.ManagedProperties.ItemsCount = totalManagerPropertiesCount;
            }

            if (isRenter)
            {
                var renterProperties = await this.propertyRepository.AllAsNoTracking()
                    .Where(x => x.Rentals.Any(x => x.Renter.UserId == userId))
                    .To<RenterPropertyInListViewModel>()
                    .ToListAsync();
                foreach (var property in renterProperties)
                {
                    var currentRentalUserId = await this.PropertyCurrentRenterId(property.Id);
                    property.IsCurrentRental = currentRentalUserId == userId ? true : false;
                }

                allProperties.RentedProperties = renterProperties;
            }

            return allProperties;
        }

        public async Task DeleteAsync(string propertyId)
        {
            var property = await this.propertyRepository.All().FirstOrDefaultAsync(x => x.Id == propertyId);
            if (property == null)
            {
                throw new Exception("You are trying to delete an Nonexisting Property!");
            }

            this.propertyRepository.Delete(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(EditPropertyInputModel input)
        {
            var property = await this.propertyRepository.All().FirstOrDefaultAsync(x => x.Id == input.Id);
            property.PricePerMonth = input.PricePerMonth;
            property.CityId = input.CityId;
            property.Description = input.Description;
            property.Floor = input.Floor;
            property.Size = input.Size;
            property.PropertyTypeId = input.PropertyTypeId;

            var district = await this.districtRepository.All().Where(x => x.Name == input.DistrictName).FirstOrDefaultAsync();
            if (district == null)
            {
                district = new District() { Name = input.DistrictName };
            }

            property.District = district;
            await this.propertyRepository.SaveChangesAsync();
        }

        public T GetById<T>(string id)
        {
            return this.propertyRepository.AllAsNoTracking()
                         .Where(x => x.Id == id)
                         .To<T>().FirstOrDefault();
        }

        public int GetCountsAvailable()
        {
            return this.propertyRepository.AllAsNoTracking().ToList().Where(x => this.IsPropertyAvailable(x.Id)).Count();
        }

        public int GetPropertyLikesCount(string propertyId)
        {
           return this.propertyRepository.AllAsNoTracking().Where(x => x.Id == propertyId).Select(x => x.Likes.Count).FirstOrDefault();
        }

        public string GetManagerUserId(string propertyId)
        {
            return this.propertyRepository.AllAsNoTracking().Where(x => x.Id == propertyId).Select(x => x.Manager.UserId).FirstOrDefault();
        }
    }
}

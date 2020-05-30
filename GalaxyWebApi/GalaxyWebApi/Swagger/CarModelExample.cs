using System;
using GalaxyWebApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace GalaxyWebApi.Swagger
{
    public class CarModelExample : IExamplesProvider<Car>
    {
        public Car GetExamples()
        {
            var dnow = DateTime.UtcNow;
            return new Car
            {
                Id = Guid.NewGuid(),
                ModelName = "Toyota",
                CarType = CarType.Hatchback,
                CreatedOn = dnow,
                ModifiedOn = dnow
            };
        }
    }
}

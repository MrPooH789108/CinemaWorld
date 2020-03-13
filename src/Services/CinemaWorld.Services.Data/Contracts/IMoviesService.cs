﻿namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Movies;
    using CinemaWorld.Models.ViewModels.Movies;

    public interface IMoviesService : IBaseDataService
    {
        Task<MovieViewModel> CreateAsync(MovieCreateInputModel movieCreateInputModel);

        Task EditAsync(MovieEditViewModel movieEditViewModel);

        Task<IEnumerable<T>> GetAllMoviesAsync<T>();
    }
}

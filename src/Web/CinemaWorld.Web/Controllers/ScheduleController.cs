﻿namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Cinemas;
    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Models.ViewModels.Schedule;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ScheduleController : Controller
    {
        private const int SchedulePageSize = 5;
        private const int LatestMoviesCount = 6;

        private readonly IMovieProjectionsService movieProjectionsService;
        private readonly IMoviesService moviesService;
        private readonly ICinemasService cinemasService;

        public ScheduleController(
            IMovieProjectionsService movieProjectionsService,
            IMoviesService moviesService,
            ICinemasService cinemasService)
        {
            this.movieProjectionsService = movieProjectionsService;
            this.moviesService = moviesService;
            this.cinemasService = cinemasService;
        }

        public async Task<IActionResult> Index(int? pageNumber, string cinemaName)
        {
            this.ViewData["CurrentFilter"] = cinemaName;

            var movieProjections = await Task.Run(
                    () => this.movieProjectionsService
                        .GetAllMovieProjectionsByCinemaAsQueryeable<MovieProjectionDetailsViewModel>(cinemaName));

            var movieProjectionsPaginated = await PaginatedList<MovieProjectionDetailsViewModel>
                    .CreateAsync(movieProjections, pageNumber ?? 1, SchedulePageSize);

            var latestMovies = await this.moviesService
                .GetRecentlyAddedMoviesAsync<RecentlyAddedMovieDetailsViewModel>(LatestMoviesCount);

            var cinemas = await this.cinemasService
                .GetAllCinemasAsync<CinemaDetailsViewModel>();

            var viewModel = new ScheduleDetailsViewModel
            {
                MovieProjections = movieProjectionsPaginated,
                LatestMovies = latestMovies,
                Cinemas = cinemas,
            };

            return this.View(viewModel);
        }
    }
}

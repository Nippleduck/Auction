﻿using Auction.Data.ImageStore;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        public ImagesController(ImageService imageService) => this.imageService = imageService;

        private readonly ImageService imageService;

        private const string ContentType = "image/jpeg";

        [HttpGet("lot/{id}/large")]
        public async Task<ActionResult> GetFullSizeAsync(int id, CancellationToken ct) =>
            File(await imageService.GetFullSizeImageAsync(id, ct), ContentType);

        [HttpGet("lot/{id}/thumbnail")]
        public async Task<ActionResult> GetThumbnailAsync(int id, CancellationToken ct) =>
            File(await imageService.GetThumbnailImageAsync(id, ct), ContentType);
    }
}

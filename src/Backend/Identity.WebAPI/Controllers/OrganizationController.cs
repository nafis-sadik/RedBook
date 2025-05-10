﻿using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;

namespace Identity.WebAPI.Controllers
{
    /// <summary>
    /// Organization Module
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController(IOrganizationService organizationService) : ControllerBase
    {
        private readonly IOrganizationService _organizationService = organizationService;

        /// <summary>
        /// Get organization details by organization Id
        /// </summary>
        /// <param name="orgId">Organization Id or unique identifier which is the primary key of organization table</param>
        [HttpGet]
        [Route("{orgId}")]
        public async Task<IActionResult> Get(int orgId) => Ok(await _organizationService.GetOrganizationAsync(orgId));

        /// <summary>
        /// Get all organizations owned by user
        /// </summary>
        [HttpGet]
        [Route("Owned")]
        public async Task<IActionResult> GetAll() => Ok(await _organizationService.GetUserOwnedOrgsAsync());

        /// <summary>
        /// Add new organization with details
        /// </summary>
        /// <param name="orgInfo">Organization details object</param>
        [HttpPost]
        public async Task<IActionResult> Add(OrganizationModel orgInfo) => Ok(await _organizationService.AddOrganizationAsync(orgInfo));

        /// <summary>
        /// Update existing organization with details
        /// </summary>
        /// <param name="orgInfo">Organization details object</param>
        [HttpPut]
        public async Task<IActionResult> Update(OrganizationModel orgInfo) => Ok(await _organizationService.UpdateOrganizationAsync(orgInfo));

        /// <summary>
        /// Get Paginated collection of organizations
        /// </summary>
        /// <param name="orgPagedCollection">Paginated Organization list for paging table</param>
        [HttpGet]
        [Route("PagedOrganizations")]
        public async Task<IActionResult> GetPagedAsync([FromQuery] PagedModel<OrganizationModel> orgPagedCollection) => Ok(await _organizationService.GetPagedOrganizationsAsync(orgPagedCollection));

        /// <summary>
        /// Delete existing organization with details
        /// </summary>
        /// <param name="orgId">Organization Id or unique identifier which is the primary key of organization table</param>
        [HttpDelete]
        [Route("{orgId}")]
        public async Task<IActionResult> Delete(int orgId)
        {
            await _organizationService.DeleteOrganizationAsync(orgId);
            return Ok();
        }
    }
}

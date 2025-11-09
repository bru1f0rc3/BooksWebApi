using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO.Branch;
using WebApplication2.Interfaces;

namespace WebApplication2.Controllers.Book
{
    /// <summary>
    /// Контроллер для управления филиалами библиотеки
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        /// <summary>
        /// Конструктор контроллера филиалов
        /// </summary>
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BranchDTO>>> GetAllBranches()
        {
            var branches = await _branchService.GetAllBranches();
            return Ok(branches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDTO>> GetBranchById(int id)
        {
            var branch = await _branchService.GetBranchById(id);
            if (branch == null)
                return NotFound();
            return Ok(branch);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BranchDTO>> CreateBranch(CreateBranchDTO branch)
        {
            var createdBranch = await _branchService.CreateBranch(branch);
            return CreatedAtAction(nameof(GetBranchById), new { createdBranch.id }, createdBranch);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BranchDTO>> UpdateBranch(int id, UpdateBranchDTO branch)
        {
            var updatedBranch = await _branchService.UpdateBranch(id, branch);
            if (updatedBranch == null)
                return NotFound();
            return Ok(updatedBranch);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBranch(int id)
        {
            var result = await _branchService.DeleteBranch(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
} 
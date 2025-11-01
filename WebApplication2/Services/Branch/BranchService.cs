using WebApplication2.Connection;
using WebApplication2.DTO.Branch;
using WebApplication2.Interfaces;

namespace WebApplication2.Services.Branch
{
    /// <summary>
    /// Сервис для работы с филиалами библиотеки
    /// </summary>
    public class BranchService : IBranchService
    {
        public async Task<List<BranchDTO>> GetAllBranches()
        {
            const string sql = "SELECT * FROM branches ORDER BY name";
            var branches = await DbConnect.QueryAsync<BranchDTO>(sql);
            return branches.ToList();
        }

        public async Task<BranchDTO?> GetBranchById(int id)
        {
            const string sql = "SELECT * FROM branches WHERE id = @id";
            return await DbConnect.QueryFirstOrDefaultAsync<BranchDTO>(sql, new { id = id });
        }

        public async Task<BranchDTO> CreateBranch(CreateBranchDTO branch)
        {
            const string sql = @"
                INSERT INTO branches (name)
                VALUES (@name)
                RETURNING *";
            
            return await DbConnect.QueryFirstOrDefaultAsync<BranchDTO>(sql, new { Name = branch.name });
        }

        public async Task<BranchDTO?> UpdateBranch(int id, UpdateBranchDTO branch)
        {
            const string sql = @"
                UPDATE branches
                SET name = @name
                WHERE id = @id
                RETURNING *";
            
            return await DbConnect.QueryFirstOrDefaultAsync<BranchDTO>(sql, new { id = id, Name = branch.name });
        }

        public async Task<bool> DeleteBranch(int id)
        {
            const string sql = "DELETE FROM branches WHERE id = @id";
            var rowsAffected = await DbConnect.ExecuteAsync(sql, new { id = id });
            return rowsAffected > 0;
        }
    }
} 
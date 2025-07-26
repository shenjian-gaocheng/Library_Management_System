using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services.ReaderService;
using backend.DTOs;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        private readonly ReaderService _readerService;

        /**
         * 构造函数
         * @param readerService Reader 服务依赖
         * @return 无
         */
        public ReaderController(ReaderService readerService)
        {
            _readerService = readerService;
        }

        /**
         * 获取所有 Reader
         * @return Reader 列表
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reader>>> GetAllReaders()
        {
            var readers = await _readerService.GetAllReadersAsync();
            return Ok(readers);
        }

        /**
         * 根据 ID 获取 Reader
         * @param id ReaderID
         * @return Reader 对象
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<Reader>> GetReaderByID(string id)
        {
            var reader = await _readerService.GetReaderByIDAsync(id);
            if (reader == null) return NotFound();
            return Ok(reader);
        }

        /**
         * 添加一个 Reader
         * @param dto ReaderDto
         * @return 结果状态
         */
        [HttpPost]
        public async Task<ActionResult> AddReader([FromBody] ReaderDto dto)
        {
            var reader = new Reader
            {
                ReaderID = dto.ReaderID,
                Password = dto.Password,
                Name = dto.Name,
                CreditScore = dto.CreditScore,
                ReaderType = dto.ReaderType,
                AccountStatus = dto.AccountStatus,
                Permission = dto.Permission
            };

            var result = await _readerService.AddReaderAsync(reader);
            return result > 0 ? Ok() : BadRequest();
        }

        /**
         * 更新一个 Reader
         * @param dto ReaderDto
         * @return 结果状态
         */
        [HttpPut]
        public async Task<ActionResult> UpdateReader([FromBody] ReaderDto dto)
        {
            var reader = new Reader
            {
                ReaderID = dto.ReaderID,
                Password = dto.Password,
                Name = dto.Name,
                CreditScore = dto.CreditScore,
                ReaderType = dto.ReaderType,
                AccountStatus = dto.AccountStatus,
                Permission = dto.Permission
            };

            var result = await _readerService.UpdateReaderAsync(reader);
            return result > 0 ? Ok() : NotFound();
        }

        /**
         * 删除一个 Reader
         * @param id ReaderID
         * @return 结果状态
         */
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReader(string id)
        {
            var result = await _readerService.DeleteReaderAsync(id);
            return result > 0 ? Ok() : NotFound();
        }
    }
}

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
         * ���캯��
         * @param readerService Reader ��������
         * @return ��
         */
        public ReaderController(ReaderService readerService)
        {
            _readerService = readerService;
        }

        /**
         * ��ȡ���� Reader
         * @return Reader �б�
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reader>>> GetAllReaders()
        {
            var readers = await _readerService.GetAllReadersAsync();
            return Ok(readers);
        }

        /**
         * ���� ID ��ȡ Reader
         * @param id ReaderID
         * @return Reader ����
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<Reader>> GetReaderByID(string id)
        {
            var reader = await _readerService.GetReaderByIDAsync(id);
            if (reader == null) return NotFound();
            return Ok(reader);
        }

        /**
         * ���һ�� Reader
         * @param dto ReaderDto
         * @return ���״̬
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
         * ����һ�� Reader
         * @param dto ReaderDto
         * @return ���״̬
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
         * ɾ��һ�� Reader
         * @param id ReaderID
         * @return ���״̬
         */
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReader(string id)
        {
            var result = await _readerService.DeleteReaderAsync(id);
            return result > 0 ? Ok() : NotFound();
        }
    }
}

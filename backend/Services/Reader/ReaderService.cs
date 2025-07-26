using backend.DTOs.Web;
using backend.Models;
using backend.Repositories.ReaderRepository;

namespace backend.Services.ReaderService
{
    /**
     * ReaderService 提供对 ReaderRepository 的业务逻辑封装
     */
    public class ReaderService
    {
        private readonly ReaderRepository _readerRepository;

        /**
         * 构造函数
         * @param readerRepository Reader 仓储依赖
         * @return 无
         */
        public ReaderService(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        /**
         * 根据 ReaderID 获取 Reader 对象
         * @param readerID 读者 ID
         * @return Reader 对象或 null
         */
        public async Task<Reader> GetReaderByIDAsync(string readerID)
        {
            return await _readerRepository.GetByIDAsync(readerID);
        }

        /**
         * 获取所有 Reader 对象
         * @return Reader 对象列表
         */
        public async Task<IEnumerable<Reader>> GetAllReadersAsync()
        {
            return await _readerRepository.GetAllAsync();
        }

        /**
         * 添加一个新的 Reader
         * @param reader Reader 实体
         * @return 受影响的行数
         */
        public async Task<int> AddReaderAsync(Reader reader)
        {
            return await _readerRepository.AddAsync(reader);
        }

        /**
         * 更新一个 Reader
         * @param reader Reader 实体
         * @return 受影响的行数
         */
        public async Task<int> UpdateReaderAsync(Reader reader)
        {
            return await _readerRepository.UpdateAsync(reader);
        }

        /**
         * 删除一个 Reader
         * @param readerID ReaderID
         * @return 受影响的行数
         */
        public async Task<int> DeleteReaderAsync(string readerID)
        {
            return await _readerRepository.DeleteAsync(readerID);
        }

        public async Task<bool> RegisterReaderAsync(RegisterDto registerDto)
        {
            Reader reader = new Reader
            {
                ReaderID = registerDto.UserName,
                Password = registerDto.Password
            };
            return await _readerRepository.AddAsync(reader) > 0;
        }
    }
}

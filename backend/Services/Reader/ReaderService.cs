using backend.Common.Constants;
using backend.DTOs.Reader;
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
        public async Task<Reader> GetReaderByReaderIDAsync(long readerID)
        {
            return await _readerRepository.GetByReaderIDAsync(readerID);
        }

        /**
         * 根据 UserName 获取 Reader 对象
         * @param UserName 用户名,通常是学号
         * @return Reader 对象或 null
         */
        public async Task<Reader> GetReaderByUserNameAsync(string userName)
        {
            return await _readerRepository.GetByUserNameAsync(userName);
        }

        /**
         * 获取所有 Reader 对象
         * @return Reader 对象列表
         */
        public async Task<IEnumerable<Reader>> GetAllReadersAsync()
        {
            return await _readerRepository.GetAllReadersAsync();
        }

        /**
         * 添加一个新的 Reader
         * @param reader Reader 实体
         * @return 受影响的行数
         */
        public async Task<int> InsertReaderAsync(Reader reader)
        {
            return await _readerRepository.InsertReaderAsync(reader);
        }

        /**
         * 更新一个 Reader
         * @param reader Reader 实体
         * @return 受影响的行数
         */
        public async Task<int> UpdateReaderAsync(Reader reader)
        {
            return await _readerRepository.UpdateReaderAsync(reader);
        }

        /**
         * 删除一个 Reader
         * @param readerID ReaderID
         * @return 受影响的行数
         */
        public async Task<int> DeleteReaderAsync(long readerID)
        {
            return await _readerRepository.DeleteReaderAsync(readerID);
        }

        public async Task<bool> RegisterReaderAsync(ReaderRegisterDto registerDto)
        {

            string userName = registerDto.UserName;
            string password = registerDto.Password;

            string msg;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("用户名和密码不能为空。");
            }
            else if (userName.Length < UserConstants.UsernameMinLength || userName.Length > UserConstants.UsernameMaxLength)
            {
                throw new ArgumentException($"用户名长度必须在{UserConstants.UsernameMinLength}到{UserConstants.UsernameMaxLength}之间。");
            }
            else if (password.Length < UserConstants.PasswordMinLength || password.Length > UserConstants.PasswordMaxLength)
            {
                throw new ArgumentException($"密码长度必须在{UserConstants.PasswordMinLength}到{UserConstants.PasswordMaxLength}之间。");
            }
            else if (IsUserNameExistsAsync(userName).Result)
            {
                throw new ArgumentException("用户名已存在，请选择其他用户名。");
            }

            Reader reader = new Reader
            {
                UserName = registerDto.UserName,
                Password = registerDto.Password
            };

            bool res = await _readerRepository.InsertReaderAsync(reader) > 0;
            if (!res)
            {
                throw new Exception("注册失败，请稍后再试。");
            }

            return res;
        }

        /**
         * 检查用户名是否已存在
         * @param userName 用户名
         * @return true 如果用户名已存在，否则 false
         */
        public async Task<bool> IsUserNameExistsAsync(string userName)
        {
            return await _readerRepository.IsUserNameExistsAsync(userName);
        }

        /**
         * 
         * 重置密码
         * 
         */
        public async Task<bool> ResetPasswordAsync(string userName, string newPassword)
        {
            return await _readerRepository.ResetPasswordAsync(userName, newPassword) > 0;
        }


        /**
         * 
         * 更新头像
         * 
         */
        public async Task<bool> UpdateAvatarAsync(long readerID, string avatarUrl)
        {
            return await _readerRepository.UpdateAvatarAsync(readerID, avatarUrl) > 0;
        }

        /**
         * 
         * 更新Reader的Profile字段
         * 
         */
        public async Task<bool> UpdateProfileAsync(Reader reader)
        {
            return await _readerRepository.UpdateProfileAsync(reader) > 0;
        }

    }
}

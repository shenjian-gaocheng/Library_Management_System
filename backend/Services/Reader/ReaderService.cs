using backend.Common.Constants;
using backend.DTOs.Reader;
using backend.Models;
using backend.Repositories.ReaderRepository;

namespace backend.Services.ReaderService
{
    /**
     * ReaderService ???? ReaderRepository ???????????
     */
    public class ReaderService
    {
        private readonly ReaderRepository _readerRepository;

        /**
         * ??????
         * @param readerRepository Reader ???????
         * @return ??
         */
        public ReaderService(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        /**
         * ???? ReaderID ??? Reader ????
         * @param readerID ???? ID
         * @return Reader ??????null
         */
        public async Task<Reader> GetReaderByReaderIDAsync(long readerID)
        {
            return await _readerRepository.GetByReaderIDAsync(readerID);
        }

        /**
         * ???? UserName ??? Reader ????
         * @param UserName ?????,????????
         * @return Reader ????? null
         */
        public async Task<Reader> GetReaderByUserNameAsync(string userName)
        {
            return await _readerRepository.GetByUserNameAsync(userName);
        }

        /**
         * ??????? Reader ????
         * @return Reader ?????งา?
         */
        public async Task<IEnumerable<Reader>> GetAllReadersAsync()
        {
            return await _readerRepository.GetAllReadersAsync();
        }

        /**
         * ?????????? Reader
         * @param reader Reader ???
         * @return ???????????
         */
        public async Task<int> InsertReaderAsync(Reader reader)
        {
            return await _readerRepository.InsertReaderAsync(reader);
        }

        /**
         * ??????? Reader
         * @param reader Reader ???
         * @return ???????????
         */
        public async Task<int> UpdateReaderAsync(Reader reader)
        {
            return await _readerRepository.UpdateReaderAsync(reader);
        }

        /**
         * ?????? Reader
         * @param readerID ReaderID
         * @return ???????????
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
                throw new ArgumentException("?????????????????");
            }
            else if (userName.Length < UserConstants.UsernameMinLength || userName.Length > UserConstants.UsernameMaxLength)
            {
                throw new ArgumentException($"??????????????{UserConstants.UsernameMinLength}??{UserConstants.UsernameMaxLength}???");
            }
            else if (password.Length < UserConstants.PasswordMinLength || password.Length > UserConstants.PasswordMaxLength)
            {
                throw new ArgumentException($"???????????{UserConstants.PasswordMinLength}??{UserConstants.PasswordMaxLength}???");
            }
            else if (IsUserNameExistsAsync(userName).Result)
            {
                throw new ArgumentException("???????????????????????????");
            }

            Reader reader = new Reader
            {
                UserName = registerDto.UserName,
                Password = registerDto.Password
            };

            bool res = await _readerRepository.InsertReaderAsync(reader) > 0;
            if (!res)
            {
                throw new Exception("?????????????????");
            }

            return res;
        }

        /**
         * ????????????????
         * @param userName ?????
         * @return true ?????????????????? false
         */
        public async Task<bool> IsUserNameExistsAsync(string userName)
        {
            return await _readerRepository.IsUserNameExistsAsync(userName);
        }

        /**
         * 
         * ????????
         * 
         */
        public async Task<bool> ResetPasswordAsync(string userName, string newPassword)
        {
            return await _readerRepository.ResetPasswordAsync(userName, newPassword) > 0;
        }


        /**
         * 
         * ???????
         * 
         */
        public async Task<bool> UpdateAvatarAsync(long readerID, string avatarUrl)
        {
            return await _readerRepository.UpdateAvatarAsync(readerID, avatarUrl) > 0;
        }

        /**
         * 
         * ????Reader??Profile???
         * 
         */
        public async Task<bool> UpdateProfileAsync(Reader reader)
        {
            return await _readerRepository.UpdateProfileAsync(reader) > 0;
        }

    }
}

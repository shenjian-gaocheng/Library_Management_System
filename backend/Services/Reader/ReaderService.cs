using backend.Common.Constants;
using backend.DTOs.Reader;
using backend.Models;
using backend.Repositories.ReaderRepository;

namespace backend.Services.ReaderService
{
    /**
     * ReaderService ï¿½á¹©ï¿½ï¿½ ReaderRepository ï¿½ï¿½Òµï¿½ï¿½ï¿½ß¼ï¿½ï¿½ï¿½×°
     */
    public class ReaderService
    {
        private readonly ReaderRepository _readerRepository;

        /**
         * ï¿½ï¿½ï¿½ìº¯ï¿½ï¿½
         * @param readerRepository Reader ï¿½Ö´ï¿½ï¿½ï¿½ï¿½ï¿½
         * @return ï¿½ï¿½
         */
        public ReaderService(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        /**
         * ï¿½ï¿½ï¿½ï¿½ ReaderID ï¿½ï¿½È¡ Reader ï¿½ï¿½ï¿½ï¿½
         * @param readerID ï¿½ï¿½ï¿½ï¿½ ID
         * @return Reader ï¿½ï¿½ï¿½ï¿½ï¿½ null
         */
        public async Task<Reader> GetReaderByReaderIDAsync(long readerID)
        {
            return await _readerRepository.GetByReaderIDAsync(readerID);
        }

        /**
         * ¸ù¾Ý UserName »ñÈ¡ Reader ¶ÔÏó
         * @param UserName ÓÃ»§Ãû,Í¨³£ÊÇÑ§ºÅ
         * @return Reader ¶ÔÏó»ò null
         */
        public async Task<Reader> GetReaderByUserNameAsync(string userName)
        {
            return await _readerRepository.GetByUserNameAsync(userName);
        }

        /**
         * ï¿½ï¿½È¡ï¿½ï¿½ï¿½ï¿½ Reader ï¿½ï¿½ï¿½ï¿½
         * @return Reader ï¿½ï¿½ï¿½ï¿½ï¿½Ð±ï¿½
         */
        public async Task<IEnumerable<Reader>> GetAllReadersAsync()
        {
            return await _readerRepository.GetAllReadersAsync();
        }

        /**
         * ï¿½ï¿½ï¿½ï¿½Ò»ï¿½ï¿½ï¿½Âµï¿½ Reader
         * @param reader Reader Êµï¿½ï¿½
         * @return ï¿½ï¿½Ó°ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
         */
        public async Task<int> InsertReaderAsync(Reader reader)
        {
            return await _readerRepository.InsertReaderAsync(reader);
        }

        /**
         * ï¿½ï¿½ï¿½ï¿½Ò»ï¿½ï¿½ Reader
         * @param reader Reader Êµï¿½ï¿½
         * @return ï¿½ï¿½Ó°ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
         */
        public async Task<int> UpdateReaderAsync(Reader reader)
        {
            return await _readerRepository.UpdateReaderAsync(reader);
        }

        /**
         * É¾ï¿½ï¿½Ò»ï¿½ï¿½ Reader
         * @param readerID ReaderID
         * @return ï¿½ï¿½Ó°ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
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
                throw new ArgumentException("ÓÃ»§ÃûºÍÃÜÂë²»ÄÜÎª¿Õ¡£");
            }
            else if (userName.Length < UserConstants.UsernameMinLength || userName.Length > UserConstants.UsernameMaxLength)
            {
                throw new ArgumentException($"ÓÃ»§Ãû³¤¶È±ØÐëÔÚ{UserConstants.UsernameMinLength}µ½{UserConstants.UsernameMaxLength}Ö®¼ä¡£");
            }
            else if (password.Length < UserConstants.PasswordMinLength || password.Length > UserConstants.PasswordMaxLength)
            {
                throw new ArgumentException($"ÃÜÂë³¤¶È±ØÐëÔÚ{UserConstants.PasswordMinLength}µ½{UserConstants.PasswordMaxLength}Ö®¼ä¡£");
            }
            else if (IsUserNameExistsAsync(userName).Result)
            {
                throw new ArgumentException("ÓÃ»§ÃûÒÑ´æÔÚ£¬ÇëÑ¡ÔñÆäËûÓÃ»§Ãû¡£");
            }

            Reader reader = new Reader
            {
                UserName = registerDto.UserName,
                Password = registerDto.Password
            };

            bool res = await _readerRepository.InsertReaderAsync(reader) > 0;
            if (!res)
            {
                throw new Exception("×¢²áÊ§°Ü£¬ÇëÉÔºóÔÙÊÔ¡£");
            }

            return res;
        }

        /**
         * ¼ì²éÓÃ»§ÃûÊÇ·ñÒÑ´æÔÚ
         * @param userName ÓÃ»§Ãû
         * @return true Èç¹ûÓÃ»§ÃûÒÑ´æÔÚ£¬·ñÔò false
         */
        public async Task<bool> IsUserNameExistsAsync(string userName)
        {
            return await _readerRepository.IsUserNameExistsAsync(userName);
        }

        /**
         * 
         * ÖØÖÃÃÜÂë
         * 
         */
        public async Task<bool> ResetPasswordAsync(string userName, string newPassword)
        {
            return await _readerRepository.ResetPasswordAsync(userName, newPassword) > 0;
        }


        /**
         * 
         * ¸üÐÂÍ·Ïñ
         * 
         */
        public async Task<bool> UpdateAvatarAsync(long readerID, string avatarUrl)
        {
            return await _readerRepository.UpdateAvatarAsync(readerID, avatarUrl) > 0;
        }

        /**
         * 
         * ¸üÐÂReaderµÄProfile×Ö¶Î
         * 
         */
        public async Task<bool> UpdateProfileAsync(Reader reader)
        {
            return await _readerRepository.UpdateProfileAsync(reader) > 0;
        }

    }
}

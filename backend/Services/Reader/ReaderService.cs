using backend.Common.Constants;
using backend.DTOs.Reader;
using backend.Models;
using backend.Repositories.ReaderRepository;

namespace backend.Services.ReaderService
{
    /**
     * ReaderService �ṩ�� ReaderRepository ��ҵ���߼���װ
     */
    public class ReaderService
    {
        private readonly ReaderRepository _readerRepository;

        /**
         * ���캯��
         * @param readerRepository Reader �ִ�����
         * @return ��
         */
        public ReaderService(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        /**
         * ���� ReaderID ��ȡ Reader ����
         * @param readerID ���� ID
         * @return Reader �����?null
         */
        public async Task<Reader> GetReaderByReaderIDAsync(long readerID)
        {
            return await _readerRepository.GetByReaderIDAsync(readerID);
        }

        /**
         * ���� UserName ��ȡ Reader ����
         * @param UserName �û���,ͨ����ѧ��
         * @return Reader ����� null
         */
        public async Task<Reader> GetReaderByUserNameAsync(string userName)
        {
            return await _readerRepository.GetByUserNameAsync(userName);
        }

        /**
         * ��ȡ���� Reader ����
         * @return Reader �����б�
         */
        public async Task<IEnumerable<Reader>> GetAllReadersAsync()
        {
            return await _readerRepository.GetAllReadersAsync();
        }

        /**
         * ����һ���µ� Reader
         * @param reader Reader ʵ��
         * @return ��Ӱ�������?
         */
        public async Task<int> InsertReaderAsync(Reader reader)
        {
            return await _readerRepository.InsertReaderAsync(reader);
        }

        /**
         * ����һ�� Reader
         * @param reader Reader ʵ��
         * @return ��Ӱ�������?
         */
        public async Task<int> UpdateReaderAsync(Reader reader)
        {
            return await _readerRepository.UpdateReaderAsync(reader);
        }

        /**
         * ɾ��һ�� Reader
         * @param readerID ReaderID
         * @return ��Ӱ�������?
         */
        public async Task<int> DeleteReaderAsync(long readerID)
        {
            return await _readerRepository.DeleteReaderAsync(readerID);
        }

        public async Task<bool> RegisterReaderAsync(ReaderRegisterDto registerDto)
        {

            string userName = registerDto.UserName;
            string password = registerDto.Password;

            // string msg; // 未使用的变量，已注释

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("�û��������벻��Ϊ�ա�");
            }
            else if (userName.Length < UserConstants.UsernameMinLength || userName.Length > UserConstants.UsernameMaxLength)
            {
                throw new ArgumentException($"�û������ȱ�����{UserConstants.UsernameMinLength}��{UserConstants.UsernameMaxLength}֮�䡣");
            }
            else if (password.Length < UserConstants.PasswordMinLength || password.Length > UserConstants.PasswordMaxLength)
            {
                throw new ArgumentException($"���볤�ȱ�����{UserConstants.PasswordMinLength}��{UserConstants.PasswordMaxLength}֮�䡣");
            }
            else if (IsUserNameExistsAsync(userName).Result)
            {
                throw new ArgumentException("�û����Ѵ��ڣ���ѡ�������û�����");
            }

            Reader reader = new Reader
            {
                UserName = registerDto.UserName,
                Password = registerDto.Password
            };

            bool res = await _readerRepository.InsertReaderAsync(reader) > 0;
            if (!res)
            {
                throw new Exception("ע��ʧ�ܣ����Ժ����ԡ�");
            }

            return res;
        }

        /**
         * ����û����Ƿ��Ѵ���
         * @param userName �û���
         * @return true ����û����Ѵ��ڣ����� false
         */
        public async Task<bool> IsUserNameExistsAsync(string userName)
        {
            return await _readerRepository.IsUserNameExistsAsync(userName);
        }

        /**
         * 
         * ��������
         * 
         */
        public async Task<bool> ResetPasswordAsync(string userName, string newPassword)
        {
            return await _readerRepository.ResetPasswordAsync(userName, newPassword) > 0;
        }


        /**
         * 
         * ����ͷ��
         * 
         */
        public async Task<bool> UpdateAvatarAsync(long readerID, string avatarUrl)
        {
            return await _readerRepository.UpdateAvatarAsync(readerID, avatarUrl) > 0;
        }

        /**
         * 
         * ����Reader��Profile�ֶ�
         * 
         */
        public async Task<bool> UpdateProfileAsync(Reader reader)
        {
            return await _readerRepository.UpdateProfileAsync(reader) > 0;
        }

    }
}

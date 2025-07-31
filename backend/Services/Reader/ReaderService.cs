using backend.DTOs.Web;
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
         * @return Reader ����� null
         */
        public async Task<Reader> GetReaderByIDAsync(string readerID)
        {
            return await _readerRepository.GetByIDAsync(readerID);
        }

        /**
         * ��ȡ���� Reader ����
         * @return Reader �����б�
         */
        public async Task<IEnumerable<Reader>> GetAllReadersAsync()
        {
            return await _readerRepository.GetAllAsync();
        }

        /**
         * ����һ���µ� Reader
         * @param reader Reader ʵ��
         * @return ��Ӱ�������
         */
        public async Task<int> AddReaderAsync(Reader reader)
        {
            return await _readerRepository.AddAsync(reader);
        }

        /**
         * ����һ�� Reader
         * @param reader Reader ʵ��
         * @return ��Ӱ�������
         */
        public async Task<int> UpdateReaderAsync(Reader reader)
        {
            return await _readerRepository.UpdateAsync(reader);
        }

        /**
         * ɾ��һ�� Reader
         * @param readerID ReaderID
         * @return ��Ӱ�������
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

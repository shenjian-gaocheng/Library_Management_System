using backend.DTOs;
using backend.Models;
using backend.Repositories.BorrowRecordRepository;

namespace backend.Services.BorrowingService
{
    public class BorrowingService
    {
        private readonly BorrowRecordRepository _borrowRecordRepository;

        // 构造函数
        public BorrowingService(BorrowRecordRepository borrowRecordRepository)
        {
            _borrowRecordRepository = borrowRecordRepository;
        }

        // 异步方法，通过借阅记录ID获取借阅记录
        public async Task<BorrowRecord> GetBorrowRecordByIDAsync(int borrowRecordID)
        {
            return await _borrowRecordRepository.GetByIDAsync(borrowRecordID);
        }

        // 异步方法，通过读者ID获取该读者的所有借阅记录
        public async Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByReaderIDAsync(string readerID)
        {
            return await _borrowRecordRepository.GetByReaderIDAsync(readerID);
        }

        // 异步方法，通过图书ID获取所有借阅该图书的记录
        public async Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByBookIDAsync(string bookID)
        {
            return await _borrowRecordRepository.GetByBookIDAsync(bookID);
        }

        // 异步方法，获取所有的借阅记录
        public async Task<IEnumerable<BorrowRecord>> GetAllBorrowRecordsAsync()
        {
            return await _borrowRecordRepository.GetAllAsync();
        }

        // 异步方法，添加新的借阅记录
        public async Task<int> AddBorrowRecordAsync(BorrowRecord borrowRecord)
        {
            return await _borrowRecordRepository.AddAsync(borrowRecord);
        }

        // 异步方法，更新借阅记录
        public async Task<int> UpdateBorrowRecordAsync(BorrowRecord borrowRecord)
        {
            return await _borrowRecordRepository.UpdateAsync(borrowRecord);
        }

        // 异步方法，通过借阅记录ID删除借阅记录
        public async Task<int> DeleteBorrowRecordAsync(string borrowRecordID)
        {
            return await _borrowRecordRepository.DeleteAsync(borrowRecordID);
        }
    }
}
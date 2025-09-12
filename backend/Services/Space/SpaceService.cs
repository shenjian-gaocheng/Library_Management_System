using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs.Reader;
using backend.Repositories.Space;

namespace backend.Services.Space
{
    public class SpaceService
    {
        private readonly SpaceRepository _repository;
        public SpaceService(SpaceRepository repository) { _repository = repository; }

        public Task<IEnumerable<SeatDto>> GetSeatLayoutAsync(int buildingId, int floor)
        {
            return _repository.GetSeatLayoutAsync(buildingId, floor);
        }

        public async Task CreateSeatReservationAsync(CreateSeatReservationDto dto, long readerId)
        {
            // 规则1: 结束时间必须在开始时间之后
            if (dto.EndTime <= dto.StartTime)
                throw new InvalidOperationException("结束时间必须晚于开始时间。");

            // 规则2: 预约时长不能超过4小时
            if ((dto.EndTime - dto.StartTime).TotalHours > 4)
                throw new InvalidOperationException("单次预约最长为4小时。");
                
            // 规则3: 检查读者是否已有未完成的预约
            var activeReservations = await _repository.GetActiveReservationsCountAsync(readerId);
            if (activeReservations > 0)
                throw new InvalidOperationException("您已有未完成的预约，无法再次预约。");

            // 规则4: 检查该座位在该时间段是否已被占用
            var isAvailable = await _repository.IsSeatAvailableAsync(dto.SeatID, dto.StartTime, dto.EndTime);
            if (!isAvailable)
                throw new InvalidOperationException("该座位在此时间段已被预约，请选择其他时间。");

            await _repository.CreateSeatReservationAsync(dto.SeatID, readerId, dto.StartTime, dto.EndTime);
        }

        public Task<IEnumerable<MyReservationDto>> GetMyReservationsAsync(long readerId)
        {
            return _repository.GetMyReservationsAsync(readerId);
        }

        public Task<bool> CancelReservationAsync(int reservationId, long readerId)
        {
            return _repository.CancelReservationAsync(reservationId, readerId);
        }
    }
}
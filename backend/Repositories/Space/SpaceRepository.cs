using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs.Reader;
using System;

namespace backend.Repositories.Space
{
    public class SpaceRepository
    {
        private readonly string _connectionString;
        public SpaceRepository(string connectionString) { _connectionString = connectionString; }

        public async Task<IEnumerable<SeatDto>> GetSeatLayoutAsync(int buildingId, int floor)
        {
            var sql = "SELECT * FROM V_Seat_Status WHERE BuildingID = :BuildingID AND Floor = :Floor ORDER BY SeatNumber";
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<SeatDto>(sql, new { BuildingID = buildingId, Floor = floor });
        }

        public async Task<bool> IsSeatAvailableAsync(int seatId, DateTime startTime, DateTime endTime)
        {
            // 核心逻辑：检查在该时间段内，是否存在任何与目标时间段重叠的、未完成的预约
            var sql = @"
                SELECT COUNT(*) 
                FROM Reserve_Seat 
                WHERE SeatID = :SeatID 
                AND Status = '未完成' 
                AND StartTime < :EndTime AND EndTime > :StartTime"; // 标准的重叠时间段检查
                
            using var connection = new OracleConnection(_connectionString);
            var conflictingReservations = await connection.ExecuteScalarAsync<int>(sql, new { SeatID = seatId, StartTime = startTime, EndTime = endTime });
            
            return conflictingReservations == 0;
        }
        public async Task<int> GetActiveReservationsCountAsync(long readerId)
        {
            var sql = "SELECT COUNT(*) FROM Reserve_Seat WHERE ReaderID = :ReaderID AND Status = '未完成'";
            using var connection = new OracleConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(sql, new { ReaderID = readerId });
        }
        
        public async Task CreateSeatReservationAsync(int seatId, long readerId, DateTime startTime, DateTime endTime)
        {
            var sql = @"
                INSERT INTO Reserve_Seat (ReaderID, SeatID, StartTime, EndTime, Status) 
                VALUES (:ReaderID, :SeatID, :StartTime, :EndTime, '未完成')";
            using var connection = new OracleConnection(_connectionString);
            await connection.ExecuteAsync(sql, new { ReaderID = readerId, SeatID = seatId, StartTime = startTime, EndTime = endTime });
        }

        public async Task<IEnumerable<MyReservationDto>> GetMyReservationsAsync(long readerId)
        {
            var sql = @"
                SELECT r.ReservationID, s.SeatID, b.BuildingName, s.Floor, s.SeatNumber, r.StartTime, r.EndTime, r.Status
                FROM Reserve_Seat r
                JOIN Seat s ON r.SeatID = s.SeatID
                JOIN Building b ON s.BuildingID = b.BuildingID
                WHERE r.ReaderID = :ReaderID
                ORDER BY r.StartTime DESC";
            using var connection = new OracleConnection(_connectionString);
            return await connection.QueryAsync<MyReservationDto>(sql, new { ReaderID = readerId });
        }

        public async Task<bool> CancelReservationAsync(int reservationId, long readerId)
        {
            var sql = "UPDATE Reserve_Seat SET Status = '取消' WHERE ReservationID = :ReservationID AND ReaderID = :ReaderID AND Status = '未完成'";
            using var connection = new OracleConnection(_connectionString);
            var affectedRows = await connection.ExecuteAsync(sql, new { ReservationID = reservationId, ReaderID = readerId });
            return affectedRows > 0;
        }
    }
}
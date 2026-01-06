using Microsoft.EntityFrameworkCore;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Queries;
using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Helpers
{
    internal static class RecordFilterHelper
    {
        public static IQueryable<User> Filter(this IQueryable<User> users, UserPagedQuery query)
        {
            if(query == null)
            {
                return users;
            }
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var normalized = query.Search.ToUpper();
                users = users.Where(u => u.FullName.ToUpper().Contains(normalized) || 
                    (u.NormalizedEmail != null && u.NormalizedEmail.Contains(normalized)));
            }
            if (query.DepartmentId.HasValue)
            {
                users = users.Where(u => u.DepartmentId ==  query.DepartmentId.Value);
            }
            return users;
        }

        public static IQueryable<UserDto> MapToDto(this IQueryable<User> query)
        {
            return query.Select(u => Map(u));
        }

        public static async Task<PagedResponse<T>> Paginate<T>(this IQueryable<T> source, int page, int size)
        {
            var count = source.Count();
            var data = await source
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PagedResponse<T>(data, page, size, count);
        }

        private static UserDto Map(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                PhotoUrl = user.PhotoUrl,
                Department = user.Department?.Name
            };
        }
    }
}

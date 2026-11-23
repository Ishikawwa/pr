namespace WebApi.Dtos.AttestationCommissionMembers;

public sealed record AttestationCommissionMemberDto(int Id, int CommissionId, string FullName, string? Position, string Role, int SortOrder);
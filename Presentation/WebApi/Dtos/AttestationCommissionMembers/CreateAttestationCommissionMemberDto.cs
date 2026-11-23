namespace WebApi.Dtos.AttestationCommissionMembers;

public sealed record CreateAttestationCommissionMemberDto(int CommissionId, string FullName, string? Position, string Role, int SortOrder);
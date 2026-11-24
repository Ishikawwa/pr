namespace WebApi.Dtos.AttestationCommissionMembers;

public sealed record UpdateAttestationCommissionMemberDto(int CommissionId, string FullName, string? Position, string Role, int SortOrder);
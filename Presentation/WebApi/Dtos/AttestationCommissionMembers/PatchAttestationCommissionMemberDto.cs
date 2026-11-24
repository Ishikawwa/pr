namespace WebApi.Dtos.AttestationCommissionMembers;

public sealed record PatchAttestationCommissionMemberDto(int? CommissionId, string? FullName, string? Position, string? Role, int? SortOrder);
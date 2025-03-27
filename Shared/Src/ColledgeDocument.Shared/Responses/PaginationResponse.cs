namespace ColledgeDocument.Shared.Responses;

public record PaginationResponse<TResponse>(
    int TotalCount,
    List<TResponse> Data,
    bool IsHaveNextPage,
    bool IsHavePrevPage);
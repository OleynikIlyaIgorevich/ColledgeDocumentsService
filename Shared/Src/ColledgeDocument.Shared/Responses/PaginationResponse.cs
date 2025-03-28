﻿namespace ColledgeDocument.Shared.Responses;

public record PaginationResponse<TResponse>(
    [property: JsonPropertyName("total_count")]
    int TotalCount,
    [property: JsonPropertyName("data")]
    List<TResponse> Data,
    [property: JsonPropertyName("is_have_next_page")]
    bool IsHaveNextPage,
    [property: JsonPropertyName("is_have_prev_page")]
    bool IsHavePrevPage);
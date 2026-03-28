namespace BlogSimples.Common.Application;

public record PaginadoResponse<T>(
    IEnumerable<T> Items,
    DateTime? UltimaDataCriacao,
    string? UltimoId,
    bool PossuiProximaPagina
);

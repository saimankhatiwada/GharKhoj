namespace GharKhoj.Api.Models.Common;

public interface ICollectionResponseDto<TModel>
{
    List<TModel> Items { get; init; }
}

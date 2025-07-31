public class CommentService
{
    private readonly CommentRepository _repository;

    public CommentService(CommentRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CommentDetailDto>> SearchCommentAsync(string ISBN)
    {
        return _repository.SearchCommentAsync(ISBN);
    }
}
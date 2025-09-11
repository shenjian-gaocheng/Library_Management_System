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
    
    public Task<IEnumerable<CommentDetailDto>> SearchCommentAsyncByCommentID(decimal comment_id)
    {
        return _repository.SearchCommentAsyncByCommentID(comment_id);
    }

    public async Task<int> AddCommentAsync(CommentDetailDto commentDto)
    {
        return await _repository.AddCommentAsync(commentDto);
    }
}


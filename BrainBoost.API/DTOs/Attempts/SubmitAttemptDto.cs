namespace BrainBoost.API.DTOs.Attempts;

public class SubmitAttemptDto
{
    public List<SubmitAnswerDto> Answers { get; set; } = new();
}